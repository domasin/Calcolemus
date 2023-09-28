// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Unification for first order terms. 
module FolAutomReas.Unif

open FolAutomReas.Lib

open Fol

/// <summary>
/// Checks if the assignment \(x \mapsto t\), added to `env`, is trivial or 
/// cyclic.
/// </summary>
/// <example>
/// Trivial assignment returns true:
/// <code lang="fsharp">
/// istriv undefined "x" (Var "x")
/// </code>
/// Acyclic assignment returns false:
/// <code lang="fsharp">
/// istriv undefined "x" (Var "y")
/// </code>
/// Cyclic nontrivial assignment throws System.Exception 'cyclic':
/// <code lang="fsharp">
/// istriv (("y" |-> (Var "x"))undefined) "x" (Fn("f",[Var "y"]))
/// </code>
/// </example>
/// <param name="env">The environment of mappings (represented as a finite partial function) from variables to terms in which to check the new assignment.</param>
/// <param name="x">The variable to be assigned.</param>
/// <param name="t">The term to be assigned</param>
/// <returns>
/// * true on trivial assignment \(x = t\); 
/// * false on acyclic assignment.</returns>
/// <exception cref="T:System.Exception">with message 'cyclic', on cyclic but not trivial assignment.</exception>
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
/// Success with no previous assignment: \(x \mapsto 0\)
/// <code lang="fsharp">
/// unify undefined [Var "x", Fn("0",[])]
/// </code>
/// Success with augmented assignment \(x \mapsto y, y \mapsto 0\)
/// <code lang="fsharp">
/// unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("0",[])]
/// </code>
/// Failure with direct cycle: \(y \mapsto f(y)\)
/// <code lang="fsharp">
/// unify undefined [Var "y", Fn("f",[Var "y"])]
/// </code>
/// Failure with derived cycle: \(x \mapsto y, y \mapsto f(y)\)
/// <code lang="fsharp">
/// unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("f",[Var "y"])]
/// </code>
/// Impossible unification:
/// <code lang="fsharp">
/// unify undefined [Fn ("0",[]), Fn("1",[])]
/// </code>
/// </example>
/// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure.</param>
/// <param name="eqs">The list of term-term pairs to be unified.</param>
/// <returns>
/// A variable-term mappings that unify `eqs`, if the unification is possible and there is no cycle.
/// </returns>
/// <remarks>
/// It applies some transformations to `eqs` and incorporates the 
/// resulting variable-term mappings into `env`.
/// 
/// `env` might contain mappings that could map a variable to a term 
/// containing other variables that are themselves assigned: for example 
/// \(x \mapsto y\) and \(y \mapsto z\) instead of just \(x \mapsto z\) 
/// directly. The call to <see cref='M:FolAutomReas.Unif.istriv'/> guarantees 
/// that there is no cycle or detects it and stops immediately the unification 
/// process with a failure.
/// </remarks>
/// <exception cref="T:System.Exception">
/// * with message 'cyclic' when there is a cyclic assignment; 
/// * with message 'impossible unification' when the unification is not possible.</exception>
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
        // Otherwise we know that condition x |-> s is not in env,
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

/// <summary>Removes useless mappings from an environment of variable-term 
/// mappings `env` returned by <see cref='M:FolAutomReas.Unif.unify'/>, 
/// giving a most general unifier (MGU).
/// </summary>
/// <example>
/// The following reduces \(x \mapsto y, x \mapsto 0\) to \(x \mapsto 0\).
/// <code lang="fsharp">
/// solve (("x" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
/// </code>
/// The following returns the input unchanged \(x \mapsto y, y \mapsto 0\)
/// <code lang="fsharp">
/// solve (("y" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
/// </code>
/// The following would cause a StackOverflow crash.
/// <code lang="fsharp">
/// solve (("y" |-> Fn("f",[Var "y"]))undefined)
/// </code>
/// </example>
/// <param name="env">The environment of mappings to be reduced.</param>
/// <returns>
/// * a new environment of variable-term mappings that is an MGU of `env`, 
/// if there are useless mappings to be removed;
/// * the same input `env`, if it is already an MGU.</returns>
/// <remarks>The function never fails. However, it would cause a StackOverflow 
/// crash if called on a cyclic `env`. <see cref='M:FolAutomReas.Unif.unify'/> 
/// is specifically designed not to produce cyclic mappings.</remarks>
let rec solve env =
    let env' = mapf (tsubst env) env
    if env' = env then env 
    else solve env'

/// <summary>
/// Returns an MGU for the input list of term-term pairs `eqs`, if it is 
/// actually unifiable.
/// </summary>
/// <example>
/// The following returns \(x \mapsto 0\).
/// <code lang="fsharp">
/// fullunify [Var "x", Fn("0",[])]
/// </code>
/// The following fails with 'impossible unification'.
/// <code lang="fsharp">
/// fullunify [Fn ("0",[]), Fn("1",[])]
/// </code>
/// </example>
/// <param name="eqs">The list of term-term pairs to be unified.</param>
/// <returns>
/// A variable-term mappings (represented as a finite partial 
/// function) that is an MGU for `eqs`, if it is actually unifiable.
/// </returns>
/// <exception cref="T:System.Exception">With message 'impossible unification' when the unification is not possible.</exception>
let fullunify eqs = solve (unify undefined eqs)

/// <summary>
/// Finds an MGU for a list of term-term pairs `eqs`, if it is 
/// unifiable, and applies the instantiation to give the unified result.
/// </summary>
/// <param name="eqs">The list of term-term pairs to be unified.</param>
let unify_and_apply eqs =
    let i = fullunify eqs
    let apply (t1, t2) =
        tsubst i t1, tsubst i t2
    List.map apply eqs
