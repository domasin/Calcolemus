// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Unification for first order terms. 
module FolAutomReas.Unif

open FolAutomReas.Lib

open Fol

/// Checks if the assignment `x |-> t` is cyclic and in this case fails 
/// unless `x` = `t` in which case it returns true, indicating that the 
/// assignment is 'trivial' 
let rec istriv env x t =
    match t with
    | Var y -> 
        y = x
        || defined env y
        && istriv env x (apply env y)
    | Fn(f,args) ->
        List.exists (istriv env x) args 
        && failwith "cyclic"
        
/// <summary>
/// Main unification procedure.
/// </summary>
/// <example>
/// This example shows how it works.
/// <code lang="fsharp">
/// unify undefined [!!!"f(x,y)",!!!"f(y,x)"]
/// </code>
/// returns:
/// <code lang="fsharp">
/// val it: func&lt;string,term&gt; = Leaf (..., [("x", &lt;&lt;|y|&gt;&gt;)])
/// </code>
/// </example>
/// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms used as an accumulator for the final result of the unification procedure.</param>
/// <param name="eqs">The list of term-term pairs to be unified.</param>
/// <returns>
/// The final resulting variable-term mappings that unify `eqs`.
/// </returns>
/// <remarks>
/// It applies some transformations to `eqs` and incorporates the 
/// resulting variable-term mappings into `env`
/// 
/// `env` might contain mappings that could map a variable to a term 
/// containing other variables that are themselves assigned: for example 
/// \(x \mapsto y\) and \(y \mapsto z\) instead of just \(x \mapsto z\) 
/// directly. The call to `istriv` guarantees that there is no cycle 
/// or detects it and stops immediately the unification process with a failure.
/// </remarks>
let rec unify (env : func<string, term>) eqs =
    match eqs with
    | [] -> env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify env (List.zip fargs gargs @ oth)
        else
            failwith "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            unify 
                (
                    // If there is a benign cycle in env, env is unchanged; 
                    // while if there is a malicious one, the unification 
                    // will fail.
                    if istriv env x t then env
                    // Otherwise, x |-> t is incorporated into env for the 
                    // next recursive call.
                    else (x |-> t) env
                ) oth

/// Returns one Most General Unifier (MGU) of an `env` result from `unify` 
/// to reach a 'fully solved form.
let rec solve env =
    let env' = mapf (tsubst env) env
    if env' = env then env 
    else solve env'

/// Unification reaching a final solved form (often this isn't needed).
/// 
/// If the input list of term pairs `eqs` is unifiable, it returns
/// one instantiation that is an MGU for it. Otherwise, it fails.
let fullunify eqs = solve (unify undefined eqs)

/// Unifier for a pair of terms
let unify_and_apply eqs =
    let i = fullunify eqs
    let apply (t1, t2) =
        tsubst i t1, tsubst i t2
    List.map apply eqs
