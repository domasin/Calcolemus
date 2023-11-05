// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Resolution.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Resolution = 

    /// <summary>
    /// Returns an MGU for a set of literals.
    /// </summary>
    /// 
    /// <remarks>
    /// The difference with 
    /// <seealso cref='M:Calculemus.Tableaux.unify_literals'/> is that this 
    /// function can be applied to a list of literals instead of a pair and 
    /// also that it returns an MGU and not a simple unifier.
    /// </remarks>
    /// 
    /// <param name="l">The input list of literals.</param>
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure.</param>
    /// 
    /// <returns>
    /// An MGU, if the unification is possible and there are no 
    /// cycles. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>cyclic</c> when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>impossible unification</c> when the unification is not possible.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Can't unify literals</c> when the input <c>eq</c> is neither a pair of literal or the degenerated case <c>False,False</c></exception>
    /// 
    /// <example id="mgu-1">
    /// Successful unification
    /// <code lang="fsharp">
    /// mgu !!>["P(x)";"P(f(y))"] undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="mgu-2">
    /// Degenerated case
    /// <code lang="fsharp">
    /// mgu !!>["false";"false"] undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    /// 
    /// <example id="mgu-3">
    /// Cyclic assignment
    /// <code lang="fsharp">
    /// mgu !!>["P(x)";"P(f(x))"] undefined
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="mgu-4">
    /// Impossible unification
    /// <code lang="fsharp">
    /// mgu !!>["P(0)";"P(f(y))"] undefined
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    /// 
    /// <example id="mgu-5">
    /// Invalid input
    /// <code lang="fsharp">
    /// mgu !!>["P(x) /\ Q(x)";"P(f(y)) /\ Q(f(y))"] undefined
    /// </code>
    /// Throws <c>System.Exception: Can't unify literals</c>.
    /// </example>
    /// 
    /// <note>
    /// See also
    /// <seealso cref='M:Calculemus.Tableaux.unify_literals'/>, 
    /// <seealso cref='M:Calculemus.Unif.solve'/>
    /// </note>
    /// 
    /// <category index="1">Unification of literals</category>
    val mgu:
      l: formula<fol> list ->
        env: func<string,term> -> func<string,term>

    /// <summary>
    /// Tests if two literals are unifiable.
    /// </summary>
    /// 
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>
    /// true, if the input are literals and unifiable; otherwise, false.
    /// </returns>
    /// 
    /// <example id="unifiable-1">
    /// <code lang="fsharp">
    /// unifiable !!"P(x)" !!"P(f(y))"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="unifiable-2">
    /// <code lang="fsharp">
    /// unifiable !!"P(x)" !!"P(f(x))"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="1">Unification of literals</category>
    val unifiable:
      p: formula<fol> -> q: formula<fol> -> bool

    /// <summary>
    /// Renames the free variables in a list of formulas by adding the given 
    /// prefix.
    /// </summary>
    /// 
    /// <param name="pfx">The first input literal.</param>
    /// <param name="cls">The list of formulas.</param>
    /// 
    /// <returns>
    /// The list of formulas with the free variables with the given prefix.
    /// </returns>
    /// 
    /// <example id="rename-1">
    /// <code lang="fsharp">
    /// rename "old_" !!>["P(x)";"Q(y)"]
    /// </code>
    /// Evaluates to <c>[`P(old_x)`; `Q(old_y)`]</c>.
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val rename:
      pfx: string ->
        cls: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Returns all resolvents of two clauses on a given literal.
    /// </summary>
    /// 
    /// <param name="cl1">The first input clause.</param>
    /// <param name="cl2">The second input clause.</param>
    /// <param name="p">The given literal.</param>
    /// <param name="acc">The supporting accumulator.</param>
    /// 
    /// <returns>
    /// The result of resolving <c>cl1</c> and <c>cl2</c> on the literal 
    /// <c>p</c>.
    /// </returns>
    /// 
    /// <example id="resolvents-1">
    /// <code lang="fsharp">
    /// resolvents 
    ///   !!>["P(x)";"~R(x,y)";"Q(x)";"P(0)"]
    ///   !!>["~P(f(y))";"T(x,y,z)";"~P(z)"]
    ///   !!"P(x)"
    ///   []
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[`P(0)`; `Q(z)`; `T(z,y,z)`; `~P(f(y))`; `~R(z,y)`];
    ///  [`P(0)`; `Q(f(y))`; `T(f(y),y,z)`; `~P(z)`; `~R(f(y),y)`];
    ///  [`P(0)`; `Q(f(y))`; `T(f(y),y,f(y))`; `~R(f(y),y)`];
    ///  [`Q(0)`; `T(0,y,0)`; `~P(f(y))`; `~R(0,y)`]]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val resolvents:
      cl1: formula<fol> list ->
        cl2: formula<fol> list ->
        p: formula<fol> ->
        acc: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Returns all resolvents of two clauses.
    /// </summary>
    /// 
    /// <param name="cl1">The first input clause.</param>
    /// <param name="cl2">The second input clause.</param>
    /// 
    /// <returns>
    /// All the resolvents of <c>cl1</c> and <c>cl2</c>.
    /// </returns>
    /// 
    /// <example id="resolve_clauses-1">
    /// <code lang="fsharp">
    /// resolve_clauses 
    ///   !!>["P(x)";"Q(x)";"P(0)"]
    ///   !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[`P(0)`; `Q(yz)`; `~P(f(yy))`; `~Q(yz)`];
    ///  [`P(0)`; `Q(f(yy))`; `~P(yz)`; `~Q(yz)`];
    ///  [`P(0)`; `Q(f(yy))`; `~Q(f(yy))`];
    ///  [`Q(0)`; `~P(f(yy))`; `~Q(0)`];
    ///  [`P(yz)`; `P(0)`; `~P(yz)`; `~P(f(yy))`];
    ///  [`P(xx)`; `Q(xx)`; `~P(f(yy))`; `~Q(0)`];
    ///  [`Q(0)`; `~P(f(yy))`; `~Q(0)`]]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val resolve_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Basic resolution loop.
    /// </summary>
    /// 
    /// <remarks>
    /// Keeps generating resolvents till the empty clause is derived.
    /// </remarks>
    /// 
    /// <param name="used">The working list of clauses, initially empty.</param>
    /// <param name="unused">The input list of clauses to be refuted.</param>
    /// 
    /// <returns>
    /// true, if a refutation for <c>unused</c> is found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found.</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>
    /// </note>
    /// 
    /// <example id="basic_resloop-1">
    /// <code lang="fsharp">
    /// basic_resloop ([],!!>>[["P(x)"];["~P(x)"]])
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="basic_resloop-2">
    /// <code lang="fsharp">
    /// basic_resloop ([],!!>>[["P(x)"]])
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val basic_resloop:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the unsatisfiability of a formula using the basic resolution 
    /// procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found (and this is always the case if the formula is either valid or satisfiable).</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <example id="pure_basic_resolution-1">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> pure_basic_resolution
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_basic_resolution-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> pure_basic_resolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_basic_resolution-3">
    /// Valid formula
    /// <code lang="fsharp">
    /// !!"""P(x) \/ ~P(x)"""
    /// |> pure_basic_resolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val pure_basic_resolution: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting in subproblems and then 
    /// testing them with a basic resolution procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the results of 
    /// <see cref='M:Calculemus.Resolution.pure_basic_resolution'/> on each 
    /// subproblems, if the formula is valid and a proof could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no proof could be found.</exception>
    /// 
    /// <example id="basic_resolution-1">
    /// Valid formula (example from Davis and Putnam (1960))
    /// <code lang="fsharp">
    /// !! @"exists x. exists y. forall z.
    ///     (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ///     ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    /// |> basic_resolution
    /// </code>
    /// Evaluates to <c>[true]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 3 unused.
    /// 1 used; 2 unused.
    /// ...
    /// 82 used; 478 unused.
    /// 83 used; 483 unused.
    /// 84 used; 488 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="basic_resolution-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> basic_resolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <example id="basic_resolution-3">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> basic_resolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="2">Basic resolution</category>
    val basic_resolution: fm: formula<fol> -> bool list

    /// <summary>
    /// Returns the matchings of a list of term-term pairs.
    /// </summary>
    /// 
    /// <remarks>
    /// Matching is a cut-down version of unification in which the 
    /// instantiation of variables is allowed only in the first term.
    /// </remarks>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure.</param>
    /// <param name="eqs">The input list of term-term pairs to be matched.</param>
    /// 
    /// <returns>The resulting matchings, if there are.</returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>term_match</c> when there are no matchings.</exception>
    /// 
    /// <example id="term_match-1">
    /// <code lang="fsharp">
    /// [!!!"x",!!!"f(y)"]
    /// |> term_match undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="term_match-2">
    /// <code lang="fsharp">
    /// [!!!"f(y)",!!!"x"]
    /// |> term_match undefined
    /// </code>
    /// Throws <c>System.Exception: term_match</c>.
    /// </example>
    /// 
    /// <category index="3">Matching</category>
    val term_match:
      env: func<string,term> ->
        eqs: (term * term) list -> func<string,term>

    /// <summary>
    /// Returns the matchings of a pair of literals.
    /// </summary>
    /// 
    /// <remarks>
    /// Matching is a cut-down version of unification in which the 
    /// instantiation of variables is allowed only in the first term.
    /// </remarks>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure.</param>
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>The resulting matching, if there is.</returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>term_match</c> when there are no matchings.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>match_literals</c> when there input formulas are not literals.</exception>
    /// 
    /// <example id="match_literals-1">
    /// <code lang="fsharp">
    /// (!!"P(x)",!!"P(f(y))")
    /// |> match_literals undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="match_literals-2">
    /// <code lang="fsharp">
    /// (!!"P(f(y))",!!"P(x)")
    /// |> match_literals undefined
    /// </code>
    /// Throws <c>System.Exception: term_match</c>.
    /// </example>
    /// 
    /// <example id="match_literals-3">
    /// <code lang="fsharp">
    /// (!!"P(x) /\ Q(x)",!!"P(f(y)) /\ Q(f(y))")
    /// |> match_literals undefined
    /// </code>
    /// Throws <c>System.Exception: match_literals</c>.
    /// </example>
    /// 
    /// <category index="3">Matching</category>
    val match_literals:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    /// <summary>
    /// Tests if the first clause subsumes the second.
    /// </summary>
    /// 
    /// <param name="cls1">The first input clause.</param>
    /// <param name="cls2">The second input clause.</param>
    /// 
    /// <returns>
    /// true, if the first clause subsumes the second; otherwise, false.
    /// </returns>
    /// 
    /// <example id="subsumes_clause-1">
    /// <code lang="fsharp">
    /// subsumes_clause !!>["P(x)"] !!>["Q(0)";"P(f(y))"]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="subsumes_clause-2">
    /// <code lang="fsharp">
    /// subsumes_clause !!>["Q(0)";"P(f(y))"] !!>["P(x)"]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="4">Subsumption</category>
    val subsumes_clause:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> bool

    /// <summary>
    /// Replaces each clause in a list with the given one, if the latter 
    /// subsumes the first.
    /// </summary>
    /// 
    /// <param name="cl">The clause that could subsume.</param>
    /// <param name="lis">The input list of clauses.</param>
    /// 
    /// <returns>
    /// The list obtained by <c>lis</c> replacing with <c>cl</c> each of its 
    /// element that are subsumed by it.
    /// </returns>
    /// 
    /// <example id="replace-1">
    /// <code lang="fsharp">
    /// !!>>[["Q(0)";"P(f(y))"];["P(x)";"~P(x)"]]
    /// |> replace !!>["P(x)"]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`P(x)`; `~P(x)`]]</c>.
    /// </example>
    /// 
    /// <category index="4">Subsumption</category>
    val replace:
      cl: formula<fol> list ->
        lis: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Inserts a newly generated clause <c>cl</c> by first removing the 
    /// tautological and replacing the subsumed ones.
    /// </summary>
    /// 
    /// <param name="gcl">The given clause.</param>
    /// <param name="cl">The clause to be inserted after checking.</param>
    /// <param name="unused">The input list fo clauses.</param>
    /// 
    /// <returns>
    /// The input list <c>unused</c> incremented by <c>cl</c>, if this latter 
    /// passes the check; otherwise, the input list <c>unused</c> unchanged.
    /// </returns>
    /// 
    /// <example id="incorporate-1">
    /// Inserted since neither tautological nor subsumed
    /// <code lang="fsharp">
    /// !!>>[["P(x)"];["Q(y)"]]
    /// |> incorporate [!!"R(0)"] [!!"R(f(z))"]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`Q(y)`]; [`R(f(z))`]]</c>.
    /// </example>
    /// 
    /// <example id="incorporate-2">
    /// Not inserted since subsumed by gcl
    /// <code lang="fsharp">
    /// !!>>[["P(x)"];["Q(y)"]]
    /// |> incorporate [!!"R(w)"] [!!"R(f(z))"]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`Q(y)`]] </c>.
    /// </example>
    /// 
    /// <example id="incorporate-3">
    /// Not inserted since subsumed by another clause in the list
    /// <code lang="fsharp">
    /// !!>>[["P(x)"];["Q(y)"]]
    /// |> incorporate [!!"R(0)"] [!!"P(f(z))"]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`Q(y)`]] </c>.
    /// </example>
    /// 
    /// <example id="incorporate-4">
    /// Not inserted since tautological
    /// <code lang="fsharp">
    /// !!>>[["P(x)"];["Q(y)"]]
    /// |> incorporate [!!"R(0)"] !!>["R(f(z))";"~R(f(z))"]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`Q(y)`]] </c>.
    /// </example>
    /// 
    /// <category index="4">Subsumption</category>
    val incorporate:
      gcl: formula<fol> list ->
        cl: formula<fol> list ->
        unused: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Resolution loop with subsumption and replacement.
    /// </summary>
    /// 
    /// <param name="used">The working list of clauses, initially empty.</param>
    /// <param name="unused">The input list of clauses to be refuted.</param>
    /// 
    /// <returns>
    /// true, if a refutation for <c>unused</c> is found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found.</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>
    /// </note>
    /// 
    /// <example id="resloop_wsubs-1">
    /// <code lang="fsharp">
    /// resloop_wsubs ([],!!>>[["P(x)"];["~P(x)"]])
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="resloop_wsubs-2">
    /// <code lang="fsharp">
    /// resloop_wsubs ([],!!>>[["P(x)"]])
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="5">Resolution with subsumption and replacement</category>
    val resloop_wsubs:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the unsatisfiability of a formula using a resolution with 
    /// subsumption and replacement.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found (and this is always the case if the formula is either valid or satisfiable).</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <example id="pure_resolution_wsubs-1">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> pure_resolution_wsubs
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_resolution_wsubs-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> pure_resolution_wsubs
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_resolution_wsubs-3">
    /// Valid formula
    /// <code lang="fsharp">
    /// !!"""P(x) \/ ~P(x)"""
    /// |> pure_resolution_wsubs
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Resolution with subsumption and replacement</category>
    val pure_resolution_wsubs: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with a resolution procedure that 
    /// handles subsumption and replacement.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the results of 
    /// <see cref='M:Calculemus.Resolution.pure_resolution_wsubs'/> on each 
    /// subproblems, if the formula is valid and a proof could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no proof could be found.</exception>
    /// 
    /// <example id="resolution_wsubs-1">
    /// Valid formula (example from Davis and Putnam (1960))
    /// <code lang="fsharp">
    /// !! @"exists x. exists y. forall z.
    ///     (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ///     ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    /// |> resolution_wsubs
    /// </code>
    /// Evaluates to <c>[true]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// ...
    /// 6 used; 3 unused.
    /// 7 used; 2 unused.
    /// </code>
    /// Note how much the number of clauses generated has been reduced compared 
    /// to <see cref='M:Calculemus.Resolution.basic_resolution'/>.
    /// </example>
    /// 
    /// <example id="resolution_wsubs-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> resolution_wsubs
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <example id="resolution_wsubs-3">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> resolution_wsubs
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="5">Resolution with subsumption and replacement</category>
    val resolution_wsubs: fm: formula<fol> -> bool list

    /// <summary>
    /// Returns all resolvents of two clauses if at least one of them contains 
    /// only positive literals.
    /// </summary>
    /// 
    /// <param name="cl1">The first input clause.</param>
    /// <param name="cl2">The second input clause.</param>
    /// 
    /// <returns>
    /// All the resolvents of <c>cl1</c> and <c>cl2</c>, if at least one of 
    /// them is completely positive; otherwise, an empty list.
    /// </returns>
    /// 
    /// <example id="presolve_clauses-1">
    /// <code lang="fsharp">
    /// presolve_clauses 
    ///   !!>["P(x)";"Q(x)";"P(0)"]
    ///   !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[`P(0)`; `Q(yz)`; `~P(f(yy))`; `~Q(yz)`];
    ///  [`P(0)`; `Q(f(yy))`; `~P(yz)`; `~Q(yz)`];
    ///  [`P(0)`; `Q(f(yy))`; `~Q(f(yy))`];
    ///  [`Q(0)`; `~P(f(yy))`; `~Q(0)`];
    ///  [`P(yz)`; `P(0)`; `~P(yz)`; `~P(f(yy))`];
    ///  [`P(xx)`; `Q(xx)`; `~P(f(yy))`; `~Q(0)`];
    ///  [`Q(0)`; `~P(f(yy))`; `~Q(0)`]]
    /// </code>
    /// </example>
    /// 
    /// <example id="presolve_clauses-2">
    /// <code lang="fsharp">
    /// presolve_clauses 
    ///   !!>["P(x)";"Q(x)";"P(0)";"~A"]
    ///   !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    /// 
    /// <category index="6">Positive resolution</category>
    val presolve_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Positive resolution loop.
    /// </summary>
    /// 
    /// <param name="used">The working list of clauses, initially empty.</param>
    /// <param name="unused">The input list of clauses to be refuted.</param>
    /// 
    /// <returns>
    /// true, if a refutation for <c>unused</c> is found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found.</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>
    /// </note>
    /// 
    /// <example id="presloop-1">
    /// <code lang="fsharp">
    /// presloop ([],!!>>[["P(x)"];["~P(x)"]])
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="presloop-2">
    /// <code lang="fsharp">
    /// presloop ([],!!>>[["P(x)"]])
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="6">Positive resolution</category>
    val presloop:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the unsatisfiability of a formula using a positive resolution 
    /// procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found (and this is always the case if the formula is either valid or satisfiable).</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <example id="pure_presolution-1">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> pure_presolution
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_presolution-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> pure_presolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_presolution-3">
    /// Valid formula
    /// <code lang="fsharp">
    /// !!"""P(x) \/ ~P(x)"""
    /// |> pure_presolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <category index="6">Positive resolution</category>
    val pure_presolution: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with the positive resolution procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the results of 
    /// <see cref='M:Calculemus.Resolution.pure_presolution'/> on each 
    /// subproblems, if the formula is valid and a proof could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no proof could be found.</exception>
    /// 
    /// <example id="presolution-1">
    /// Valid formula (&#321;o&#347; example)
    /// <code lang="fsharp">
    /// !! @"(forall x y z. P(x,y) /\ P(y,z) ==> P(x,z)) /\
    ///      (forall x y z. Q(x,y) /\ Q(y,z) ==> Q(x,z)) /\
    ///      (forall x y. Q(x,y) ==> Q(y,x)) /\
    ///      (forall x y. P(x,y) \/ Q(x,y))
    ///      ==> (forall x y. P(x,y)) \/ (forall x y. Q(x,y))"
    /// |> presolution
    /// </code>
    /// Evaluates to <c>[true]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 6 unused.
    /// 1 used; 5 unused.
    /// ...
    /// 34 used; 42 unused.
    /// 35 used; 41 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="presolution-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> presolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <example id="presolution-3">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> presolution
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="6">Positive resolution</category>
    val presolution: fm: formula<fol> -> bool list

    /// <summary>
    /// Tests the unsatisfiability of a formula using a resolution procedure 
    /// with subsumption and replacement and the set-of-support restriction.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found (and this is always the case if the formula is either valid or satisfiable).</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <example id="pure_resolution_wsos-1">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> pure_resolution_wsos
    /// </code>
    /// Evaluates to <c>true</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 2 unused.
    /// 1 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_resolution_wsos-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> pure_resolution_wsos
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="pure_resolution_wsos-3">
    /// Valid formula
    /// <code lang="fsharp">
    /// !!"""P(x) \/ ~P(x)"""
    /// |> pure_resolution_wsos
    /// </code>
    /// Throws <c>System.Exception: No proof found</c> and prints to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 1 unused.
    /// </code>
    /// </example>
    /// 
    /// <category index="7">Set-of-support restriction</category>
    val pure_resolution_wsos: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them using a resolution procedure with subsumption and 
    /// replacement and the set-of-support restriction.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the results of 
    /// <see cref='M:Calculemus.Resolution.pure_resolution_wsos'/> on each 
    /// subproblems, if the formula is valid and a proof could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no proof could be found.</exception>
    /// 
    /// <example id="resolution_wsos-1">
    /// Valid formula (&#321;o&#347; example)
    /// <code lang="fsharp">
    /// !! @"exists x. exists y. forall z.
    ///    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ///    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    /// |> resolution_wsos
    /// </code>
    /// Evaluates to <c>[true]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 6 unused.
    /// 1 used; 5 unused.
    /// ...
    /// 34 used; 42 unused.
    /// 35 used; 41 unused.
    /// </code>
    /// </example>
    /// 
    /// <example id="resolution_wsos-2">
    /// Satisfiable (not valid) formula
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> resolution_wsos
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <example id="resolution_wsos-3">
    /// Unsatisfiable formula
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> resolution_wsos
    /// </code>
    /// Throws <c>System.Exception: No proof found</c>.
    /// </example>
    /// 
    /// <category index="7">Set-of-support restriction</category>
    val resolution_wsos: fm: formula<fol> -> bool list
