// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace FolAutomReas

open FolAutomReas.Lib
open Formulas
open Fol

/// <summary>Tableaux, seen as an optimized version of a Prawitz-like procedure.</summary>
module Tableaux = 

    /// <summary>Unifies an input pair of litterals.</summary>
    /// <example>
    /// An example of successful unification: \(x \mapsto f(y)\)
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x)",!!"P(f(y))")
    /// </code>
    /// An example of cyclic assignment:
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(y)",!!"P(f(y))")
    /// </code>
    /// An example of impossible unification:
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
    /// </code>
    /// </example>
    /// <param name="env">An accumulator of the environment of the variable assignments, maintained globally and represented as a cycle-free finite partial function.</param>
    /// <param name="tmp">The pair of litterals to be unified. It also handles the pair <c>False,False</c>.</param>
    /// <returns>A variable-term mappings that unify <c>tmp</c>, if the unification is possible and there is no cycle. If the input pair is already unified or the input mappings already unify it, <c>unify_literals</c> just returns the input mappings, otherwise it augments the input mappings with the term-variable assignments needed for the unification.</returns>
    /// <exception cref="T:System.Exception">
    /// <ul>
    /// <li>with message 'cyclic' when there is a cyclic assignment;</li>
    /// <li>with message 'impossible unification' when the unification is not 
    /// possible.</li>
    /// <li>with message 'Can't unify literals' when the input <c>tmp</c> is neither a pair of literal or the degenerated case <c>False,False</c>.</li>
    /// </ul>
    /// </exception>
    val unify_literals:
      env: func<string,term> ->
        tmp: (formula<fol> * formula<fol>) ->
          func<string,term>

    // /// Unifies complementary literals <c>(p, q)</c>.   
    val unify_complements:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    // /// Unifies and refutes a list of disjuncts <c>dsj</c>, each member of which 
    // /// being a list of implicitly conjoined litterals.
    // val unify_refute:
    //   djs: formula<fol> list list ->
    //     acc: func<string,term> -> func<string,term>

    // /// <summary>Main loop for prawitz procedure.</summary>
    // /// <param name="djs0">The initial formula in DNF uninstantiated.</param>
    // /// <param name="fvs">The set of free variables in the initial formula.</param>
    // /// <param name="djs">Accumulator for the substitution instances.</param>
    // /// <param name="n">A counter to generate fresh variable names.</param>
    // /// <returns>
    // /// The final instantiation together with the number of instances tried.
    // /// </returns>
    val prawitz_loop:
      djs0: formula<fol> list list ->
        fvs: string list ->
        djs: formula<fol> list list ->
        n: int -> func<string,term> * int

    // /// Tests an input fol formula <c>fm</c> for validity based on a Prawitz-like 
    // /// procedure.
    val prawitz: fm: formula<fol> -> int

    val tableau:
      fms: formula<fol> list * lits: formula<fol> list *
      n: int ->
        cont: (func<string,term> * int -> 'a) ->
        env: func<string,term> * k: int -> 'a

    val deepen: f: (int -> 'a) -> n: int -> 'a

    val tabrefute: fms: formula<fol> list -> int

    val tab: fm: formula<fol> -> int

    val splittab: fm: formula<fol> -> int list