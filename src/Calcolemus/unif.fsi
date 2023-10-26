namespace Calcolemus

open Calcolemus.Lib.Fpf

open Fol

/// <summary>
/// Unification for first order terms.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Unif = 

    /// <summary>
    /// Checks if the assignment \(x \mapsto t\), added to <c>env</c>, 
    /// is trivial or cyclic.
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
    /// <ul>
    /// <li>true on trivial assignment \(x = t\);</li>
    /// <li>false on acyclic assignment.</li>
    /// </ul>
    /// </returns>
    /// <exception cref="T:System.Exception">with message 'cyclic', on cyclic but not trivial assignment.</exception>
    val istriv:
        env: func<string,term> -> x: string -> t: term -> bool

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
    /// A variable-term mappings that unify <c>eqs</c>, if the unification is possible and there is no cycle.
    /// </returns>
    /// <remarks>
    /// It applies some transformations to <c>eqs</c> and incorporates the 
    /// resulting variable-term mappings into <c>env</c>.
    /// 
    /// <c>env</c> might contain mappings that could map a variable to a term 
    /// containing other variables that are themselves assigned: for example 
    /// \(x \mapsto y\) and \(y \mapsto z\) instead of just \(x \mapsto z\) 
    /// directly. The call to <see cref='M:Calcolemus.Unif.istriv'/> guarantees 
    /// that there is no cycle or detects it and stops immediately the unification 
    /// process with a failure.
    /// </remarks>
    /// <exception cref="T:System.Exception">
    /// <ul>
    /// <li>with message 'cyclic' when there is a cyclic assignment;</li>
    /// <li>with message 'impossible unification' when the unification is not 
    /// possible.</li>
    /// </ul>
    /// </exception>
    val unify:
        env: func<string,term> ->
        eqs: (term * term) list -> func<string,term>

    /// <summary>Removes useless mappings from an environment of variable-term 
    /// mappings <c>env</c> returned by <see cref='M:Calcolemus.Unif.unify'/>, 
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
    /// <ul>
    /// <li>a new environment of variable-term mappings that is an MGU of 
    /// <c>env</c>, if there are useless mappings to be removed;</li>
    /// <li>the same input <c>env</c>, if it is already an MGU.</li>
    /// </ul>
    /// </returns>
    /// <remarks>The function never fails. However, it would cause a StackOverflow 
    /// crash if called on a cyclic <c>env</c>. <see cref='M:Calcolemus.Unif.unify'/> 
    /// is specifically designed not to produce cyclic mappings.</remarks>
    val solve: env: func<string,term> -> func<string,term>

    /// <summary>
    /// Returns an MGU for the input list of term-term pairs <c>eqs</c>, if it is 
    /// actually unifiable.
    /// </summary>
    /// <example>
    /// The following returns \(x \mapsto 0\).
    /// <code lang="fsharp">
    /// fullunify [Var "x", Fn("0",[])]
    /// </code>
    /// The following (\(f(x,g(y)),f(y,x)\)) fails with 'cyclic'.
    /// <code lang="fsharp">
    /// fullunify [Fn ("f",[Var "x"; Fn("g",[Var "y"])]), Fn ("f",[Var "y"; Var "x"])]
    /// </code>
    /// The following fails with 'impossible unification'.
    /// <code lang="fsharp">
    /// fullunify [Fn ("0",[]), Fn("1",[])]
    /// </code>
    /// </example>
    /// <param name="eqs">The list of term-term pairs to be unified.</param>
    /// <returns>
    /// A variable-term mappings (represented as a finite partial 
    /// function) that is an MGU for <c>eqs</c>, if it is actually unifiable.
    /// </returns>
    /// <exception cref="T:System.Exception">
    /// <ul>
    /// <li>with message 'cyclic' when there is a cyclic assignment;</li>
    /// <li>with message 'impossible unification' when the unification is not 
    /// possible.</li>
    /// </ul>
    /// </exception>
    val fullunify: eqs: (term * term) list -> func<string,term>

    /// <summary>
    /// Finds an MGU for a list of term-term pairs <c>eqs</c>, if it is 
    /// unifiable, and applies the instantiation to give the unified result.
    /// </summary>
    /// <example>
    /// The following returns a successful unification: \((x,0) \longrightarrow (0,0)\)
    /// <code lang="fsharp">
    /// unify_and_apply [Var "x", Fn("0",[])
    /// </code>
    /// The following (\(f(x,g(y)),f(y,x)\)) fails with 'cyclic'.
    /// <code lang="fsharp">
    /// unify_and_apply [Fn ("f",[Var "x"; Fn("g",[Var "y"])]), Fn ("f",[Var "y"; Var "x"])]
    /// </code>
    /// The following fails with 'impossible unification'.
    /// <code lang="fsharp">
    /// unify_and_apply [Fn ("0",[]), Fn("1",[])]
    /// </code>
    /// </example>
    /// <param name="eqs">The list of term-term pairs to be unified.</param>
    /// <returns>The unified result, if it exists.</returns>
    /// <exception cref="T:System.Exception">
    /// <ul>
    /// <li>with message 'cyclic' when there is a cyclic assignment;</li>
    /// <li>with message 'impossible unification' when the unification is not 
    /// possible.</li>
    /// </ul>
    /// </exception>
    val unify_and_apply:
        eqs: (term * term) list -> (term * term) list