#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Prop
open Calcolemus.DP
open Calcolemus.Lib.Set
open Calcolemus.Lib.Search
open Calcolemus.Propexamples
open Calcolemus.Formulas
open Calcolemus.Lib.Fpf
open Calcolemus.Defcnf

// fsi.AddPrinter sprint_prop_formula

let ppToStr = List.map sprint_prop_formula

let cToStr = List.map ppToStr

let fToStr f = 
     graph f
     |> List.map (fun (a,v) -> 
          sprintf "%s |-> %A" (a |> sprint_prop_formula) v
     )

let tToStr = 
    List.map (fun (ff,flag:trailmix) -> 
        (ff |> sprint_prop_formula,flag)
    )

let rec unit_subpropagate (cls, fn, trail) =
    // printfn "unit_subpropagate: %A %A %A" (cToStr cls) (fToStr fn) (tToStr trail)
    // remove the contrary of deduced or guessed literals
    let cls' = 
        List.map (List.filter (not << defined fn << negate)) cls
    // printfn "contrary of deduced or guessed removed: %A %A %A" (cToStr cls') (fToStr fn) (tToStr trail)
    // find unit clauses
    let uu = function
        | [c] when not (defined fn c) -> [c]
        | _ -> failwith ""
    let newunits = unions (mapfilter uu cls')
    // printfn "unit clauses: %A" (ppToStr newunits)
    // if there aren't, we are finished
    if newunits = [] then
        // printfn "...finished unit_subpropagate." 
        cls', fn, trail
    // otherwise,
    else
        // update the trail with the new unit clauses
        // (marking the literal as Deduced)
        let trail' = 
            trail
            |> List.foldBack (fun p t -> (p, Deduced) :: t) newunits 
        // and update the fpf with the new unit clauses
        let fn' = 
            fn
            |> List.foldBack (fun u -> u |-> ()) newunits 
        // printfn "fpf and trail updated" 
        // reapply unit propagation on new clauses, fpf and trail
        unit_subpropagate (cls', fn', trail')

let unit_propagate (cls, trail) = 
     // printfn "unit_propagate: %A %A" (cToStr cls) (tToStr trail)
     // put in the fpf all literals in trail both Deduced or Guessed
     let fn = 
          undefined
          |> List.foldBack (fun (x, _) -> x |-> ()) trail 
     // printfn "fpf generated" 
     let cls', fn', trail' = unit_subpropagate (cls, fn, trail)
     // printfn "...finished unit_propagate."
     cls', trail'

let rec backtrack trail =
     printfn "backtrack: %A" (tToStr trail)
     match trail with
     | (p, Deduced) :: tt ->
          backtrack tt
     | _ -> trail

let rec dpli cls trail =
     
     printfn "dpli: %A %A" (cToStr cls) (tToStr trail)

     // apply unit propagation
     // printfn "unit propagation:" 
     let cls', trail' = unit_propagate (cls, trail)
     printfn "unit propagation result: %A %A" (cToStr cls') (tToStr trail')
     // if there is a conflict:
     if mem [] cls' then
        printfn "conflict"
        match backtrack trail with
        // if we are in one half of a case split,
        | (p, Guessed) :: tt ->
            // test the other half with the decision literal negated;
            printfn "conflict: try %s: " (Not p |> sprint_prop_formula)
            dpli cls ((negate p, Deduced) :: tt)
        // otherwise, we are finished: clauses are unsatisfiable;
        | _ -> false
     // if there is no conflict:
     else
          // printfn "no conflicts"
          match unassigned cls trail' with
          // if all literals have already been tested,
          // we are finished: clauses are satisfiable;
          | [] -> 
               // printfn "no literal unassigned: dpli finished."
               true
          // otherwise, make a new case split.
          | ps ->
               let p = maximize (posneg_count cls') ps
               printfn "case split on %s: " (p |> sprint_prop_formula)
               dpli cls ((p, Guessed) :: trail')

dpli !>>[["~p1"; "~p10"; "p11"]; ["~p1"; "~p10"; "~p11"]; ["p1"; "p10"];
   ["p1"; "~p10"];["~p1"; "p10"]] []

// let rec literals fm =
//     match fm with
//     | Atom a -> [fm]
//     | Not p -> [fm]
//     | And (p, q)
//     | Or (p, q)
//     | Imp (p, q)
//     | Iff (p, q) ->
//         (literals p)@(literals q)
//     | Forall (x, p)
//     | Exists (x, p) -> literals p

// let rec toClauses = function
//     | (And (f1,f2)) -> (literals f1)::(toClauses f2)
//     | fm -> [literals fm]

// !> @"(~p1 \/ ~p10 \/ p11) /\ (~p1 \/ ~p10 \/ ~p11) /\ (p1 \/ p10) /\ (p1 \/ ~p10) /\ (~p1 \/ p10)"
// |> toClauses

// !> @"(~p1 \/ ~p10 \/ p11) /\ (~p1 \/ ~p10 \/ ~p11) /\ (p1 \/ p10) /\ (p1 \/ ~p10) /\ (~p1 \/ p10)"
// |> satisfiable



// let dplisat fm = dpli (defcnfs fm) []

// !> @"(~p1 \/ ~p10 \/ p11) /\ (~p1 \/ ~p10 \/ ~p11) /\ (p1 \/ p10) /\ (p1 \/ ~p10) /\ (~p1 \/ p10)"
// |> dplisat

backjump !>>[["~p";"q"];["~q"]] !>"a"
    [
        !>"c", Deduced; 
        !>"b", Deduced; 
        !>"~a", Deduced
        !>"e", Guessed; 
        !>"p", Deduced; 
        !>"d", Guessed
    ]

dplb !>>[["~p";"q"];["~q"]] 
    [!>"p", Deduced; !>"~q", Deduced]

dplb !>>[["~p";"q"];["~q"]] []

#time
dplitaut(prime 101)
#time
// evaluates to Real: 00:02:14.842, CPU: 00:02:13.449, GC gen0: 1504, gen1: 26, gen2: 2

#time
dplbtaut(prime 101)
// Real: 00:00:40.079, CPU: 00:00:39.350, GC gen0: 435, gen1: 5, gen2: 0
#time

open Calcolemus.Lib.Time

time dplbtaut (prime 101)
// Evaluates to:
// CPU time (user): 36.981689
// val it: bool = true

time dplitaut (prime 101)
// Evaluates to:
// CPU time (user): 36.981689
// val it: bool = true

