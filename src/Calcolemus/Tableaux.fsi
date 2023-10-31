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
    /// <example id="unify_literals-4">
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
    /// <example id="unify_literals-4">
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
    /// Unifies and refutes a list of disjuncts <c>dsj</c>, each member of 
    /// which being a list of implicitly conjoined literals.
    /// </summary>
    /// 
    /// <category index="2">Refutation via unification</category>
    val unify_refute:
      djs: formula<fol> list list ->
        acc: func<string,term> -> func<string,term>

    /// <summary>Main loop for prawitz procedure.</summary>
    /// 
    /// <param name="djs0">The initial formula in DNF uninstantiated.</param>
    /// <param name="fvs">The set of free variables in the initial formula.</param>
    /// <param name="djs">Accumulator for the substitution instances.</param>
    /// <param name="n">A counter to generate fresh variable names.</param>
    /// <returns>
    /// The final instantiation together with the number of instances tried.
    /// </returns>
    /// 
    /// <category index="3">Prawitz procedure</category>
    val prawitz_loop:
      djs0: formula<fol> list list ->
        fvs: string list ->
        djs: formula<fol> list list ->
        n: int -> func<string,term> * int

    /// <summary>
    /// Tests an input fol formula <c>fm</c> for validity based on a 
    /// Prawitz-like procedure.
    /// </summary>
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