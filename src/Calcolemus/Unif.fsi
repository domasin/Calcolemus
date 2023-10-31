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
    /// Checks the new assignment <c>x |-> t</c> against a given environment of 
    /// already existing assignments <c>env</c>.
    /// </summary>
    /// 
    /// <param name="env">The environment of mappings (represented as a finite partial function) from variables to terms in which to check the new assignment.</param>
    /// <param name="x">The variable to be assigned.</param>
    /// <param name="t">The term assigned to the variable.</param>
    /// 
    /// <returns>
    /// true, if the assignment is trivial (<c>t</c> = <c>x</c>); otherwise, if 
    /// there are no cycle, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic', when the new assignment would generate a cycle.</exception>
    /// 
    /// <example id="istriv-1">
    /// Trivial assignment
    /// <code lang="fsharp">
    /// istriv undefined "x" !!!"x"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="istriv-2">
    /// Acyclic nontrivial assignment
    /// <code lang="fsharp">
    /// istriv undefined "x" !!!"y"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="istriv-3">
    /// Cyclic nontrivial assignment
    /// <code lang="fsharp">
    /// istriv (("y" |-> !!!"x")undefined) "x" !!!"f(y)"
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    val istriv:
        env: func<string,term> -> x: string -> t: term -> bool

    /// <summary>
    /// Returns a unifier for a list of term-term pairs <c>eqs</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It applies some transformations to <c>eqs</c> and incorporates the 
    /// resulting variable-term mappings into <c>env</c>.
    /// 
    /// <c>env</c> might contain mappings that could map a variable to a term 
    /// containing other variables that are themselves assigned: for example 
    /// \(x \mapsto y\) and \(y \mapsto z\) instead of just \(x \mapsto z\) 
    /// directly. The call to <see cref='M:Calcolemus.Unif.istriv'/> guarantees 
    /// that there are no cycle or detects it and stops immediately the unification 
    /// process with a failure.
    /// </remarks>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure.</param>
    /// <param name="eqs">The list of term-term pairs to be unified.</param>
    /// 
    /// <returns>
    /// A variable-term mappings that unify <c>eqs</c>, if the unification is 
    /// possible and there are no cycles.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <example id="unify-1">
    /// No previous assignments
    /// <code lang="fsharp">
    /// unify undefined [!!!"x", !!!"0"]
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``0``)]</c>.
    /// </example>
    /// 
    /// <example id="unify-2">
    /// Augmented assignment
    /// <code lang="fsharp">
    /// unify (("x" |-> !!!"y")undefined) [!!!"x", !!!"0"]
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``y``); ("y", ``0``)]</c>.
    /// </example>
    /// 
    /// <example id="unify-3">
    /// Direct cycle (\(y \mapsto f(y)\))
    /// <code lang="fsharp">
    /// unify undefined [!!!"y", !!!"f(y)"]
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify-4">
    /// Derived cycle (\(x \mapsto y, y \mapsto f(x)\))
    /// <code lang="fsharp">
    /// unify (("x" |-> !!!"y")undefined) [!!!"y", !!!"f(x)"]
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify-5">
    /// Impossible unification
    /// <code lang="fsharp">
    /// unify undefined [!!!"0", !!!"1"]
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    val unify:
        env: func<string,term> ->
        eqs: (term * term) list -> func<string,term>

    /// <summary>
    /// Reduces an input unifier <c>env</c> to an MGU.
    /// </summary>
    /// 
    /// <remarks>
    /// Removes useless mappings from the input variable-term 
    /// mappings <c>env</c> returned by <see cref='M:Calcolemus.Unif.unify'/>, 
    /// giving a most general unifier (MGU).
    /// </remarks>
    /// 
    /// <param name="env">The environment of mappings to be reduced.</param>
    /// 
    /// <returns>
    /// An MGU from <c>env</c>.
    /// </returns>
    /// 
    /// <example id="solve-1">
    /// The following reduces \(x \mapsto y, x \mapsto 0\) to \(x \mapsto 0\).
    /// <code lang="fsharp">
    /// solve (("x" |-> !!!"0")(("x" |-> !!!"y")undefined))
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``0``)]</c>.
    /// </example>
    /// 
    /// <example id="solve-2">
    /// The following returns the input unchanged \(x \mapsto 1, y \mapsto 0\) 
    /// since the input is already an MGU.
    /// <code lang="fsharp">
    /// solve (("y" |-> !!!"0")(("x" |-> !!!"1")undefined))
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``1``); ("y", ``0``)]</c>.
    /// </example>
    /// 
    /// <example id="solve-3">
    /// The following would cause a StackOverflow crash.
    /// <code lang="fsharp">
    /// solve (("y" |-> !!!"f(y)")undefined)
    /// </code>
    /// </example>
    /// 
    /// <note>
    /// The function never fails. However, it would cause a StackOverflow 
    /// crash if called on a cyclic <c>env</c>. 
    /// <see cref='M:Calcolemus.Unif.unify'/> is specifically designed not to 
    /// produce cyclic mappings.
    /// </note>
    val solve: env: func<string,term> -> func<string,term>

    /// <summary>
    /// Returns an MGU for a list of term-term pairs <c>eqs</c>.
    /// </summary>
    /// 
    /// <param name="eqs">The list of term-term pairs to be unified.</param>
    /// 
    /// <returns>
    /// A variable-term mappings (represented as a finite partial function) 
    /// that is an MGU for <c>eqs</c>, if it is actually unifiable.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible 
    /// unification' when the unification is not possible.</exception>
    /// 
    /// <example id="fullunify-1">
    /// <code lang="fsharp">
    /// fullunify [(!!!"x", !!!"0"); (!!!"x", !!!"y")]
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``0``); ("y", ``0``)]</c>.
    /// </example>
    /// 
    /// <example id="fullunify-2">
    /// <code lang="fsharp">
    /// fullunify [!!!"f(x,g(y))", !!!"f(y,x)"]
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="fullunify-3">
    /// <code lang="fsharp">
    /// fullunify [!!!"0",!!!"1"]
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    val fullunify: eqs: (term * term) list -> func<string,term>

    /// <summary>
    /// Finds and apply an MGU to a list of term-term pairs <c>eqs</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Finds an MGU for a list of term-term pairs <c>eqs</c>, if it is 
    /// unifiable, and applies the instantiation to give the unified result.
    /// </remarks>
    /// 
    /// <param name="eqs">The list of term-term pairs to be unified.</param>
    /// 
    /// <returns>The unified result, if it exists.</returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <example id="unify_and_apply-1">
    /// <code lang="fsharp">
    /// unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"]
    /// </code>
    /// Evaluates to <c>[(``f(f(z),g(y))``, ``f(f(z),g(y))``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_and_apply-2">
    /// <code lang="fsharp">
    /// unify_and_apply [!!!"f(x,g(y))", !!!"f(y,x)"]
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify_and_apply-3">
    /// <code lang="fsharp">
    /// unify_and_apply [!!!"0",!!!"1"]
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    val unify_and_apply:
        eqs: (term * term) list -> (term * term) list