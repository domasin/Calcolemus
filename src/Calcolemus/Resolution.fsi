// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calcolemus

open Calcolemus

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
    /// <category index="1">Unification of literals</category>
    val mgu:
      l: formula<fol> list ->
        env: func<string,term> -> func<string,term>

    /// <summary>
    /// Tests if two literals are unifiable.
    /// </summary>
    /// 
    /// <category index="1">Unification of literals</category>
    val unifiable:
      p: formula<fol> -> q: formula<fol> -> bool

    /// <summary>
    /// Renames variables in a set of clauses by adding a prefix.
    /// </summary>
    /// 
    /// <category index="2">Base resolution</category>
    val rename:
      pfx: string ->
        cls: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Returns all resolvents of two clauses on a given literal.
    /// </summary>
    /// 
    /// <category index="2">Base resolution</category>
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
    /// <category index="2">Base resolution</category>
    val resolve_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Base resolution loop.
    /// </summary>
    /// 
    /// <category index="2">Base resolution</category>
    val resloop_base:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the validity of a formula using a base resolution procedure.
    /// </summary>
    /// 
    /// <category index="2">Base resolution</category>
    val pure_resolution_base: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with a base resolution procedure.
    /// </summary>
    /// 
    /// <category index="2">Base resolution</category>
    val resolution_base: fm: formula<fol> -> bool list

    /// <summary>
    /// Matches the first element of each terms pair in a with the second 
    /// element of the pair.
    /// </summary>
    /// 
    /// <remarks>
    /// Matching is a cut-down version of unification in which the 
    /// instantiation of variables is allowed only in the first term.
    /// </remarks>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val term_match:
      env: func<string,term> ->
        eqs: (term * term) list -> func<string,term>

    /// <summary>
    /// Tries to match a pair of literals.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val match_literals:
      env: func<string,term> ->
        formula<fol> * formula<fol> ->
          func<string,term>

    /// <summary>
    /// Tests if the first clause subsumes the second.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val subsumes_clause:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> bool

    /// <summary>
    /// Replaces each clause in a list with the given one, if the latter 
    /// subsumes the first.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val replace:
      cl: formula<fol> list ->
        lis: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Inserts a newly generated clause <c>cl</c> by first removing the 
    /// tautological and replacing the subsumed ones.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val incorporate:
      gcl: formula<fol> list ->
        cl: formula<fol> list ->
        unused: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Resolution loop with subsumption and replacement.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val resloop_subs:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the validity of a formula using a resolution procedure that 
    /// handles subsumption and replacement.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val pure_resolution_subs: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with a resolution procedure that 
    /// handles subsumption and replacement.
    /// </summary>
    /// 
    /// <category index="3">Subsumption and replacement</category>
    val resolution_subs: fm: formula<fol> -> bool list

    /// <summary>
    /// Returns all resolvents of two clauses if at least one of them contains 
    /// only positive literals.
    /// </summary>
    /// 
    /// <category index="4">Positive resolution</category>
    val presolve_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Positive resolution loop.
    /// </summary>
    /// 
    /// <category index="4">Positive resolution</category>
    val presloop:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the validity of a formula using a positive resolution procedure.
    /// </summary>
    /// 
    /// <category index="4">Positive resolution</category>
    val pure_presolution: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with the positive resolution procedure.
    /// </summary>
    /// 
    /// <category index="4">Positive resolution</category>
    val presolution: fm: formula<fol> -> bool list

    /// <summary>
    /// Tests the validity of a formula using a resolution procedure with 
    /// set-of-support restriction.
    /// </summary>
    /// 
    /// <category index="5">Set-of-support restriction</category>
    val pure_sosresolution: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them using a resolution procedure with set-of-support restriction.
    /// </summary>
    /// 
    /// <category index="5">Set-of-support restriction</category>
    val sosresolution: fm: formula<fol> -> bool list
