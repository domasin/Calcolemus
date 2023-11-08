#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Meson
open Formulas
open Fol
open Unif
open Calculemus.Prop
open Calculemus.Prolog
open Calculemus.Skolem

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

type transformation = 
    | Trivial
    | NonTrivial
    | Cyclic

let rec istriv3 env x t =
    match t with
    | Var y when y = x -> Trivial
    | Var y when not (defined env y) -> NonTrivial
    | Var y -> istriv3 env x (apply env y) 
    | Fn(f,args) -> 
        match 
            args
            |> List.exists (fun a -> 
                istriv3 env x a = Trivial
            )
        with 
        | true -> Cyclic
        | false -> NonTrivial

// istriv3 undefined "x" !!!"x"
// istriv3 (("x" |-> !!!"0")undefined) "x" !!!"y"

// istriv3 undefined "x" !!!"y"
// istriv3 undefined "y" !!!"f(y)"
// istriv3 (("y" |-> !!!"x")undefined) "x" !!!"f(y)"

let rec unify3 success failure
    (env : func<string, term>) eqs =
    match eqs with
    | [] -> success env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify3 success failure env (List.zip fargs gargs @ oth)
        else
            failure "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify3 success failure env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match istriv3 env x t with
            // if there is a malicious cycle
            | Cyclic -> failure "Cyclic"
            // If there is a benign cycle in env, env is unchanged
            | Trivial -> unify3 success failure env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | _ -> unify3 success failure ((x |-> t) env) oth

let rec unify_literals3 success failure env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify3 success failure env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals3 success failure  env (p, q)
    | False, False -> success env
    | _ -> failure "Impossible unification"

let unifiable_literals = 
    unify_literals3 (fun _ -> true) (fun _ -> false)

let mutable anc = 0
let mutable rules = 0

let unify_literals_sf = 
    unify_literals3 (fun x -> x,1,1) (fun _ -> failwith "unify_literals3")

let add x = x + 1

add 
|> fun x -> x 2


let rec mexpand_basic3 rules ancestors g cont (env, n, k) 
    : func<string,term> * int * int =
    if n < 0 then failwith "Too deep"
    else
        try 
            // ancestor unification
            ancestors
            |> Lib.Search.tryfind (fun a -> 
                cont (Tableaux.unify_literals env (g, negate a), n, k)
            )
        with _ -> 
            rules
            |> Lib.Search.tryfind (fun rule -> 
                let (asm,c),k' = renamerule k rule
                
                (asm, cont)
                ||> List.foldBack (fun subgoal cont -> 
                    mexpand_basic3 rules (g::ancestors) subgoal cont
                )
                |> fun f -> 
                    f (Tableaux.unify_literals env (g, c), n - List.length asm, k')
            )
                    
            

// let rec mexpand_basic3 rules ancestors g cont (env, n, k) 
//     : func<string,term> * int * int =
//     if n < 0 then failwith "Too deep"
//     else
//         let ancestor = 
//             ancestors
//             |> List.tryFind (fun a -> 
//                 // anc <- anc + 1
//                 // printfn "anc: %i" anc
//                 unifiable_literals env (g, negate a)
//             )

//         // ancestor unification
//         match ancestor with
//         | Some a ->
//             cont (unify_literals3 id (fun _ -> failwith "unify_literals3") env (g, negate a)
//                 ,n, k)
//         | _ -> 
//             // Prolog-style extension
//             rules
//             |> List.tryFind (fun rule ->
//                 let (asm, c) ,k' = renamerule k rule

//                 (Tableaux.unify_literals env (g, c), n - List.length asm, k')
//                 |> List.foldBack (fun subgoal -> 
//                     mexpand_basic3 rules (g :: ancestors) subgoal
//                 ) asm cont
//             ) |> Option.get

            // rules
            // |> Lib.Search.tryfind (fun rule ->
            //     let (asm, c) ,k' = renamerule k rule

            //     (Tableaux.unify_literals env (g, c), n - List.length asm, k')
            //     |> List.foldBack (fun subgoal -> 
            //         mexpand_basic rules (g :: ancestors) subgoal
            //     ) asm cont
            // )

let puremeson_basic3 fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    Tableaux.deepen (fun n ->
        mexpand_basic3 rules [] False id (undefined, n, 0)
        |> ignore
        n) 0

let meson_basic3 fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic3 << list_conj) (simpdnf fm1)

!! @"((forall x. P1(x) ==> P0(x)) /\ (exists x. P1(x))) /\
     ((forall x. P2(x) ==> P0(x)) /\ (exists x. P2(x))) /\
     ((forall x. P3(x) ==> P0(x)) /\ (exists x. P3(x))) /\
     ((forall x. P4(x) ==> P0(x)) /\ (exists x. P4(x))) /\
     ((forall x. P5(x) ==> P0(x)) /\ (exists x. P5(x))) /\
     ((exists x. Q1(x)) /\ (forall x. Q1(x) ==> Q0(x))) /\
     (forall x. P0(x)
                ==> (forall y. Q0(y) ==> R(x,y)) \/
                ((forall y. P0(y) /\ S0(y,x) /\
                    (exists z. Q0(z) /\ R(y,z))
                    ==> R(x,y)))) /\
     (forall x y. P3(y) /\ (P5(x) \/ P4(x)) ==> S0(x,y)) /\
     (forall x y. P3(x) /\ P2(y) ==> S0(x,y)) /\
     (forall x y. P2(x) /\ P1(y) ==> S0(x,y)) /\
     (forall x y. P1(x) /\ (P2(y) \/ Q1(y)) ==> ~(R(x,y))) /\
     (forall x y. P3(x) /\ P4(y) ==> R(x,y)) /\
     (forall x y. P3(x) /\ P5(y) ==> ~(R(x,y))) /\
     (forall x. (P4(x) \/ P5(x)) ==> exists y. Q0(y) /\ R(x,y))
     ==> exists x y. P0(x) /\ P0(y) /\
     exists z. Q1(z) /\ R(y,z) /\ R(x,y)"
|> meson_basic3

// Meson.meson_basic 1' 24
// meson_basic3 1' 24
// Meson.meson 1' 26

type successFailure<'a,'b,'c,'d> = ('a -> 'b) -> ('c -> 'b) -> 'd -> 'b

let isEven success failure n = 
    if (n % 2 = 0)
    then n |> success
    else n |> failure

let rec tryfindIsEven success failure l =
    match l with
    | [] -> failure "tryfind"
    | h :: t -> 
        isEven id (fun x -> tryfindIsEven success failure t) h

let rec tryUnifyLit success failure env l =
    match l with
    | [] -> failure "tryfind"
    | h :: t -> 
        unify_literals3 success (fun x -> tryUnifyLit success failure env t) env h

[
    (!!"P(y)", !!"P(f(y))")
    (!!"P(x)", !!"P(0)")
    (!!"P(x)", !!"P(1)")
]
|> tryUnifyLit id (fun x -> failwith "pippo") undefined

[1;2;3] 
|> tryfindIsEven id (fun x -> failwith "pippo")

[1;3] 
|> tryfindIsEven id (fun x -> failwith "pippo")

let rec backchain2 success failure rules n k env goals =
    match goals with
    | [] -> env
    | g :: gs ->
        if n = 0 then 
            failure "Too deep" 
        else
            let rec tryUnifyLit success failure env l =
                match l with
                | [] -> failure "tryfind"
                | rule :: t -> 
                    let (a, c), k' = renamerule k rule
                    backchain2 success failure rules (n - 1) k' 
                        (unify_literals3 success (fun x -> tryUnifyLit success failure env t) env (c, g)) 
                        (a@gs)
            rules
            |> tryUnifyLit success failure env

!!>["S(x) <= S(S(x))"] 
|> backchain2 id (fun x -> failwith "pippo")
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 undefined
|> graph

let rec mexpand_basic3 success failure rules ancestors g 
    cont
    (env, n, k) =
    
    if n < 0 then 
        failure "Too deep"
    else
        // ancestor unification
        let rec unifyAnc success failure env l =
            match l with
            | [] -> failure "unifyAnc"
            | a :: t -> 
                cont (unify_literals3 
                    success
                    (fun x -> unifyAnc success failure env t) env (g, negate a), n, k)
        try
            ancestors
            |> unifyAnc success (fun err -> failwith err) env  
        with _ -> 
            // Prolog-style extension
            let rec tryRules success failure env l =
                match l with
                | [] -> failure "tryAncestors"
                | rule :: t -> 
                    let (asm, c) ,k' = renamerule k rule

                    (unify_literals3 
                        success
                        (fun x -> tryRules success failure env t)  
                        env (g, c), n - List.length asm, k')
                    |> List.foldBack (fun subGoal -> 
                        
                        mexpand_basic3 
                            success 
                            failure rules (g :: ancestors)
                            subGoal
                        ) asm cont

            rules
            |> tryRules success failure env  

mexpand_basic3 id (fun x -> failwith "pippo")
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ]
   [] False (fun (x,_,_) -> x) (undefined,1,0)