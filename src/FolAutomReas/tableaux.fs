// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Tableaux, seen as an optimized version of a Prawitz-like procedure.
module FolAutomReas.Tableaux

open FolAutomReas.Lib

open Formulas
open Prop
open Fol
open Skolem
open Herbrand
open Unif

/// Unifies an input pair of litterals.
/// 
/// It uses `env` as an accumulator of the environment of the variable 
/// assignments, maintained globally and represented as a cycle-free finite 
/// partial function just as in `unify`.
/// 
/// It also handles the degenerated case `False,False` because it will 
/// be used later.
let rec unify_literals env tmp =
    match tmp with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals env (p, q)
    | False, False -> env
    | _ -> failwith "Can't unify literals"

/// Unifies complementary literals `(p, q)`.   
let unify_complements env (p, q) =
    unify_literals env (p, negate q)

/// Unifies and refutes a list of disjuncts `dsj`, each member of which 
/// being a list of implicitly conjoined litterals.
let rec unify_refute djs (acc : func<string, term>) : func<string, term> =
    match djs with
    | [] -> acc
    | d::odjs -> 
        // separate d into positive and negative literals.
        let pos, neg = List.partition positive d
        // try to unify them as complementary literals and solve 
        // the remaining problem with the resulting instantiation
        tryfind 
            (unify_refute odjs << unify_complements acc) 
            (allpairs (fun p q -> (p, q)) pos neg)

/// <summary>
/// Main loop for prawitz procedure.
/// </summary>
/// <param name="djs0">The initial formula in DNF uninstantiated.</param>
/// <param name="fvs">The set of free variables in the initial formula.</param>
/// <param name="djs">Accumulator for the substitution instances.</param>
/// <param name="n">A counter to generate fresh variable names.</param>
/// <returns>
/// The final instantiation together with the number of instances tried.
/// </returns>
let rec prawitz_loop djs0 fvs djs n =
    let l = List.length fvs
    // create new variables.
    let newvars = List.map (fun k -> "_" + string (n * l + k)) (1--l)
    // create the new instantiation.
    let inst = fpf fvs (List.map (fun x -> Var x) newvars)
    // incorporate the new instantiation in the previous substitution instances.
    let djs1 = distrib (image (image (subst inst)) djs0) djs
    // try to refute the new DNF accumulated and return if succeeds.
    try unify_refute djs1 undefined,(n + 1) with 
    // otherwise try with a larger conjunction.
    | Failure _ -> prawitz_loop djs0 fvs djs1 (n + 1)

/// Tests an input fol formula `fm` for validity based on a Prawitz-like 
/// procedure.
let prawitz fm =
    let fm0 = skolemize (Not (generalize fm))
    snd <| prawitz_loop (simpdnf fm0) (fv fm0) [[]] 0

// pg. 177
// ------------------------------------------------------------------------- //
// More standard tableau procedure, effectively doing DNF incrementally.     //
// ------------------------------------------------------------------------- //

let rec tableau (fms, lits, n) cont (env, k) =
    if n < 0 then failwith "no proof at this level" 
    else
        match fms with
        | [] -> failwith "tableau: no proof"
        | And (p, q) :: unexp ->
            tableau (p :: q :: unexp, lits, n) cont (env, k)
        | Or (p, q) :: unexp ->
            tableau (p :: unexp, lits, n) (tableau (q :: unexp, lits, n) cont) (env, k)
        | Forall (x, p) :: unexp ->
            let y = Var ("_" + string k)
            let p' = subst (x |=> y) p
            tableau (p' :: unexp @ [Forall (x, p)], lits, n - 1) cont (env, k + 1)
        | fm :: unexp ->
            try
                lits
                |> tryfind (fun l ->
                    cont (unify_complements env (fm, l), k))
            with _ ->
                tableau (unexp, fm :: lits, n) cont (env, k)

let rec deepen f n =
    try printf "Searching with depth limit "
        printfn "%d" n
        f n
    with _ ->
        deepen f (n + 1)
        
let tabrefute fms =
    deepen (fun n ->
        tableau (fms, [], n) id (undefined, 0)
        |> ignore
        n) 0

let tab fm =
    let sfm = askolemize (Not (generalize fm))
    if sfm = False then 0 else tabrefute [sfm]

// pg. 178
// ------------------------------------------------------------------------- //
// Try to split up the initial formula first; often a big improvement.       //
// ------------------------------------------------------------------------- //

let splittab fm =
    generalize fm
    |> Not
    |> askolemize
    |> simpdnf
    |> List.map tabrefute

