// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calcolemus

open Calcolemus.Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Tableaux, seen as an optimized version of a Prawitz-like procedure.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Tableaux = 

    /// <summary>Returns a unifier for a pair of literals.</summary>
    /// 
    /// <remarks>
    /// It also handles the pair <c>False,False</c>.
    /// </remarks>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>
    /// A variable-term mappings that unify <c>eq</c>, if the unification is 
    /// possible and there are no cycles. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Can't unify literals' when the input <c>eq</c> is neither a pair of literal or the degenerated case <c>False,False</c></exception>
    /// 
    /// <example id="unify_literals-1">
    /// Successful unification
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x)",!!"P(f(y))")
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-2">
    /// Degenerated case
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"false",!!"false")
    /// |> graph
    /// </code>
    /// Evaluates to <c>Empty</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-3">
    /// Cyclic assignment
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(y)",!!"P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-4">
    /// Impossible unification
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-5">
    /// Invalid input
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: Can't unify literals</c>.
    /// </example>
    /// 
    /// <category index="1">Unification of literals</category>
    val unify_literals:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    /// <summary>
    /// Returns a unifier for a complementary pair of literals.
    /// </summary>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>
    /// A variable-term mappings that unify <c>p</c> and <c>q</c>, if the 
    /// unification is possible and there are no cycles. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Can't unify literals' when the input <c>eq</c> is neither a pair of literal or the degenerated case <c>False,False</c></exception>
    /// 
    /// <example id="unify_complements-1">
    /// Successful unification
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(x)",!!"~P(f(y))")
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-2">
    /// Cyclic assignment
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(y)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-3">
    /// Impossible unification
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(g(x))",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-4">
    /// Invalid input
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x) /\ Q(x)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: Can't unify literals</c>.
    /// </example>
    /// 
    /// <category index="1">Unification of literals</category>
    val unify_complements:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    /// <summary>
    /// Returns a unifier that causes each clause in the input list to contain 
    /// complementary literals.
    /// </summary>
    /// 
    /// <param name="djs">The input DNF list of clauses</param>
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// 
    /// <returns>
    /// The variable-term mappings that causes each clause to contain 
    /// complementary literals, if this mapping exists.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'tryfind' when there isn't any such mapping.</exception>
    /// 
    /// <example id="unify_refute-1">
    /// Successful refutation
    /// <code lang="fsharp">
    /// undefined
    /// |> unify_refute !!>>[
    ///         ["P(x)";"~P(f(y))";"R(x,y)"];
    ///         ["Q(x)";"~Q(x)"]
    /// ]
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_refute-2">
    /// Failing refutation
    /// <code lang="fsharp">
    /// undefined
    /// |> unify_refute !!>>[["P(c)"];["Q(c)"]]
    /// </code>
    /// Throws to <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <category index="2">Refutation via unification</category>
    val unify_refute:
      djs: formula<fol> list list ->
        env: func<string,term> -> func<string,term>

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses with a Prawitz-like 
    /// procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The input set of clauses <c>djs0</c> is intended to represent a DNF 
    /// formula and <c>fvs</c> are supposed to be the free variables of the 
    /// formula.
    /// </remarks>
    /// 
    /// <param name="djs0">The input set of clauses.</param>
    /// <param name="fvs">The free variables to unify.</param>
    /// <param name="djs">The accumulator for the substitution instances.</param>
    /// <param name="n">A counter to generate fresh variable names.</param>
    /// 
    /// <returns>
    /// The final instantiation together with the number of instances tried.
    /// </returns>
    /// 
    /// <example id="prawitz_loop-1">
    /// Successful refutation
    /// <code lang="fsharp">
    /// prawitz_loop !!>>[
    ///         ["P(x)";"~P(f(y))";"R(x,y)"];
    ///         ["Q(x)";"~Q(x)"]
    /// ] ["x";"y"] [[]] 0
    /// |> fun (env,nr) -> env |> graph, nr
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <category index="3">Prawitz procedure</category>
    val prawitz_loop:
      djs0: formula<fol> list list ->
        fvs: string list ->
        djs: formula<fol> list list ->
        n: int -> func<string,term> * int

    /// <summary>
    /// Tests the validity of a formula with a Prawitz-like procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The number of instance tried.
    /// </returns>
    /// 
    /// <example id="prawitz_loop-1">
    /// <code lang="fsharp">
    /// prawitz Pelletier.p20
    /// </code>
    /// Evaluates to <c>2</c>.
    /// </example>
    /// 
    /// <category index="3">Prawitz procedure</category>
    val prawitz: fm: formula<fol> -> int

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tableau:
      fms: formula<fol> list * lits: formula<fol> list *
      n: int ->
        cont: (func<string,term> * int -> 'a) ->
        env: func<string,term> * k: int -> 'a

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val deepen: f: (int -> 'a) -> n: int -> 'a

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tabrefute: fms: formula<fol> list -> int

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tab: fm: formula<fol> -> int

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val splittab: fm: formula<fol> -> int list