// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Calcolemus.Lib.Function
open Calcolemus.Lib.List
open Calcolemus.Lib.Search
open Calcolemus.Lib.Set
open Calcolemus.Lib.Fpf

open Formulas
open Prop
open Defcnf 

module DP = 

    let (!>>) xs = 
        xs |> List.map (List.map (!>))

    // ====================================================================== //
    // The Davis-Putnam and Davis-Putnam-Loveland-Logemann procedures.        //
    // ====================================================================== //

    // ---------------------------------------------------------------------- //
    // The DP procedure.                                                      //
    //  --------------------------------------------------------------------- //

    let hasUnitClause (clauses: formula<'a> list list) = 
        clauses |> List.exists (List.length >> (=) 1)

    let one_literal_rule clauses =
        let u = List.head (List.find (fun cl -> List.length cl = 1) clauses)
        let u' = negate u
        let clauses1 = List.filter (fun cl -> not (mem u cl)) clauses
        image (fun cl -> subtract cl [u']) clauses1

    let pureLiterals clauses = 
        let neg',pos = List.partition negative (unions clauses)
        let neg = image negate neg'
        let pos_only = subtract pos neg 
        let neg_only = subtract neg pos
        union pos_only (image negate neg_only)

    let hasPureLiteral clauses = 
        clauses
        |> pureLiterals
        |> List.length > 0

    let affirmative_negative_rule clauses =
        match clauses |> pureLiterals with 
        | [] -> failwith "affirmative_negative_rule" 
        | pureLits -> 
            clauses
            |> List.filter (fun cl -> intersect cl pureLits = [])

    let resolve_on p clauses =
        let p' = negate p 
        let pos, notpos = List.partition (mem p) clauses
        let neg, other = List.partition (mem p') notpos
        let res0 =
            let pos' = image (List.filter (fun l -> l <> p)) pos
            let neg' = image (List.filter (fun l -> l <> p')) neg
            allpairs union pos' neg'
        let clauses' = union other (List.filter (non trivial) res0)
        clauses'

    let resolution_blowup cls l =
        let m = List.length (List.filter (mem l) cls)
        let n = List.length (List.filter (mem (negate l)) cls)
        m * n - m - n

    let resolution_rule clauses =
        let pvs = List.filter positive (unions clauses)
        let p = minimize (resolution_blowup clauses) pvs
        resolve_on p clauses

    // ---------------------------------------------------------------------- //
    // Overall procedure.                                                     //
    // ---------------------------------------------------------------------- //

    let rec dp clauses =
        if clauses = [] then 
            true 
        else if mem [] clauses then 
            false 
        else if clauses |> hasUnitClause then 
            dp (one_literal_rule clauses)
        else if clauses |> hasPureLiteral then
            dp (affirmative_negative_rule clauses)
        else
            dp (resolution_rule clauses)

    // ---------------------------------------------------------------------- //
    // Davis-Putnam satisfiability tester and tautology checker.              //
    // ---------------------------------------------------------------------- //

    let dpsat fm = dp (defcnfs fm)

    let dptaut fm = not (dpsat (Not fm))

    // ---------------------------------------------------------------------- //
    // The same thing but with the DPLL procedure.                            //
    // ---------------------------------------------------------------------- //

    let posneg_count cls l =
        let m = List.length (List.filter (mem l) cls)                 
        let n = List.length (List.filter (mem (negate l)) cls)
        m + n                                  

    let rec dpll clauses =
        if clauses = [] then 
            true 
        else if mem [] clauses then 
            false 
        else if clauses |> hasUnitClause then 
            dpll (one_literal_rule clauses)
        else if clauses |> hasPureLiteral then
            dpll (affirmative_negative_rule clauses)
        else
            let pvs = List.filter positive (unions clauses)
            let p = maximize (posneg_count clauses) pvs
            dpll (insert [p] clauses) || dpll (insert [negate p] clauses)

    let dpllsat fm = dpll (defcnfs fm)

    let dplltaut fm = not (dpllsat (Not fm))                   

    // ---------------------------------------------------------------------- //
    // Iterative implementation with explicit trail instead of recursion.     //
    // ---------------------------------------------------------------------- //

    type trailmix = Guessed | Deduced

    let unassigned clauses trail =
        let litabs p = 
            match p with
            | Not q -> q
            | _ -> p

        subtract (unions (image (image litabs) clauses))
            (image (litabs << fst) trail)

    let rec unit_subpropagate (cls, fn, trail) =
        // remove the contrary of deduced or guessed literals
        let cls' = 
            List.map (List.filter (not << defined fn << negate)) cls
        // find unit clauses
        let uu = function
            | [c] when not (defined fn c) -> [c]
            | _ -> failwith ""
        let newunits = unions (mapfilter uu cls')
        // if there aren't, we are finished
        if newunits = [] then
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
            // reapply unit propagation on new clauses, fpf and trail
            unit_subpropagate (cls', fn', trail')

    let unit_propagate (cls, trail) = 
        // put in the fpf all literals in trail both Deduced or Guessed
        let fn = 
            undefined
            |> List.foldBack (fun (x, _) -> x |-> ()) trail 
        let cls', fn', trail' = unit_subpropagate (cls, fn, trail)
        cls', trail'

    let rec backtrack trail =
        match trail with
        | (p, Deduced) :: tt ->
            backtrack tt
        | _ -> trail

    let rec dpli cls trail =
        // apply unit propagation
        let cls', trail' = unit_propagate (cls, trail)
        // if there is a conflict:
        if mem [] cls' then
            match backtrack trail with
            // if we are in one half of a case split,
            | (p, Guessed) :: tt ->
                // test the other half with the decision literal negated;
                dpli cls ((negate p, Deduced) :: tt)
            // otherwise, we are finished: clauses are unsatisfiable;
            | _ -> false
        // if there is no conflict:
        else
            match unassigned cls trail' with
            // if all literals have already been tested,
            // we are finished: clauses are satisfiable;
            | [] -> 
                true
            // otherwise, make a new case split.
            | ps ->
                let p = maximize (posneg_count cls') ps
                dpli cls ((p, Guessed) :: trail')

    let dplisat fm = dpli (defcnfs fm) []

    let dplitaut fm = not (dplisat (Not fm))

    // ---------------------------------------------------------------------- //
    // With simple non-chronological backjumping and learning.                //
    // ---------------------------------------------------------------------- //

    let rec backjump cls p trail =
        // go back to the last case split start
        match backtrack trail with
        // if there are
        | (q, Guessed) :: tt ->
            // unit propagate assuming p
            let cls', trail' = unit_propagate (cls, (p, Guessed) :: tt)
            // if there is still a conflict
            if mem [] cls' then 
                // try backjump further
                backjump cls p tt 
            else 
                trail
        // otherwise, return the trail unchanged
        | _ -> trail

    let rec dplb cls trail =
        let cls', trail' = unit_propagate (cls, trail)
        if mem [] cls' then
            match backtrack trail with
            | (p, Guessed) :: tt ->
                let trail' = backjump cls p tt
                let declits = List.filter (fun (_, d) -> d = Guessed) trail'
                let conflict = insert (negate p) (image (negate << fst) declits)
                dplb (conflict :: cls) ((negate p, Deduced) :: trail')
            | _ -> false
        else
            match unassigned cls trail' with
            | [] -> true
            | ps ->
                let p = maximize (posneg_count cls') ps
                dplb cls ((p, Guessed) :: trail')

    let dplbsat fm = dplb (defcnfs fm) []

    let dplbtaut fm = not (dplbsat (Not fm))
