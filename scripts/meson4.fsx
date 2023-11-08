#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus.Lib.Search
open Calculemus.Tableaux
open Calculemus.Prop
open Calculemus.Lib.Fpf
open Calculemus.Formulas
open Calculemus.Fol
open Calculemus.Prolog
open Calculemus.Skolem
open Calculemus.Meson

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

type transformation = 
    | Trivial
    | NonTrivial
    | Cyclic

let rec check env x t =
    match t with
    | Var y when y = x -> Trivial
    | Var y when not (defined env y) -> NonTrivial
    | Var y -> check env x (apply env y) 
    | Fn(f,args) -> 
        match 
            args
            |> List.exists (fun a -> 
                check env x a = Trivial
            )
        with 
        | true -> Cyclic
        | false -> NonTrivial

let rec unify_sf success failure
    (env : func<string, term>) eqs =
    match eqs with
    | [] -> success env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify_sf success failure env (List.zip fargs gargs @ oth)
        else
            failure "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify_sf success failure env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match check env x t with
            // if there is a malicious cycle
            | Cyclic -> failure "Cyclic"
            // If there is a benign cycle in env, env is unchanged
            | Trivial -> unify_sf success failure env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | _ -> unify_sf success failure ((x |-> t) env) oth

let rec unify_literals_sf success failure env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify_sf success failure env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals_sf success failure  env (p, q)
    | False, False -> success env
    | _ -> failure "Impossible unification"

let rec mexpand_basic rules ancestors g 
    cont
    (env, n, k) 
    : func<string,term> * int * int =
    if n < 0 then failwith "Too deep"
    else
        match
            ancestors 
            |> List.tryPick (fun a -> 
                unify_literals_sf Some (fun _ -> None) env (g, negate a)
            )
        with
            // ancestor unification
            | Some env -> cont (env, n, k)
            | None -> 
                // Prolog-style extension
                rules
                |> tryfind (fun rule ->
                    let (asm, c) ,k' = renamerule k rule
                    
                    let env = unify_literals env (g, c)
                    
                    // update teh instantiation so that it solves all assumptions
                    let cont = 
                        (asm, cont) 
                        ||> List.foldBack (fun subgoal cont -> 
                            mexpand_basic rules (g::ancestors) subgoal cont
                        ) 
                    
                    cont (env, n - List.length asm, k')
                )

let puremeson_basic fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen (fun n ->
        mexpand_basic rules [] False id (undefined, n, 0)
        |> ignore
        n) 0

let meson_basic fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic << list_conj) (simpdnf fm1)

let davis_putnam_example = 
    !! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"

// meson_basic davis_putnam_example

let steamroller = 
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

// meson_basic steamroller