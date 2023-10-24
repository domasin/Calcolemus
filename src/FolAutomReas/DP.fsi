// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.Fpf

open Formulas
open Prop

/// <summary>
/// The Davis-Putnam and the Davis-Putnam-Loveland-Logemann procedures.
/// </summary>
/// 
/// <category index="3">Propositional logic</category>
module DP =

    /// <summary>
    /// Parses a list of string lists into a list of clauses.
    /// A clause is list of literals.
    /// </summary>
    /// 
    /// <param name="xs">The input list of string lists.</param>
    /// <returns>The list of clauses.</returns>
    /// 
    /// <example id="exclamation-exclamation-greater-1">
    /// <code lang="fsharp">
    /// !>> [["p"];["p";"~q"]]
    /// </code>
    /// Evaluates to <c>[[`p`]; [`p`; `~q`]]</c>.
    /// </example>
    /// 
    /// <note>
    /// This operator is not part of the original code and it was added here 
    /// for convenience.
    /// </note>
    /// 
    /// <category index="1">Parsing clauses</category>
    val (!>>): xs: string list list -> formula<prop> list list

    /// <summary>
    /// <c>clauses</c> has unit clauses.
    /// </summary>
    /// 
    /// <param name="clauses">The input clauses to be checked.</param>
    /// <returns>
    /// true, if <c>clauses</c> contain unit clauses; otherwise, false.
    /// </returns>
    /// 
    /// <example id="hasUnitClause-1">
    /// <code lang="fsharp">
    /// !>> [["p"];["p";"~q"]] 
    /// |> hasUnitClause
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="hasUnitClause-2">
    /// <code lang="fsharp">
    /// !>> [["p";"~q"]] 
    /// |> hasUnitClause
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <note>
    /// This function is not part of the original code: it has been added to 
    /// remove the use of exceptions as control flow (which causes performance 
    /// degradation in F#) in the implementations of the DP and DPLL procedures.
    /// </note>
    /// 
    /// <category index="2">Unit propagation rule</category>
    val hasUnitClause: clauses: formula<'a> list list -> bool

    /// <summary>
    /// Applies the 1-literal rule to the input clauses.
    /// </summary>
    /// 
    /// <remarks>
    /// For the <em>first</em> unit clause <c>p</c> in <c>clauses</c>:
    /// <ul>
    /// <li>removes any instance of <c>~p</c> from the other clauses.</li>
    /// <li>removes <c>p</c> and any clauses containing it.</li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// <returns>
    /// The result of 1-literal rule application, if there are unit clauses.
    /// </returns>
    /// 
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">Thrown when there aren't unit clauses.</exception>
    /// 
    /// <example id="one_literal_rule-1">
    /// Removes the <em>first</em> unit clause present.
    /// <code lang="fsharp">
    /// !>> [["p"];["s";"t"];["q"]] 
    /// |> one_literal_rule
    /// </code>
    /// Evaluates to <c>[[`q`; `p /\ q`]]</c>.
    /// </example>
    /// 
    /// <example id="one_literal_rule-2">
    /// Removes also complements of the literal from other clauses.
    /// <code lang="fsharp">
    /// !>> [["p"];["s";"~p"];["~p";"t"]]
    /// |> one_literal_rule
    /// </code>
    /// Evaluates to <c>[[`s`]; [`t`]]</c>.
    /// </example>
    /// 
    /// <example id="one_literal_rule-3">
    /// Removes all clauses containing the literal of the unit clause.
    /// <code lang="fsharp">
    /// !>> [["p"];["s";"p"];["q";"t"]] 
    /// |> one_literal_rule
    /// </code>
    /// Evaluates to <c>[[`q`; `t`]]</c>.
    /// </example>
    /// 
    /// <example id="one_literal_rule-4">
    /// Fails if there aren't unit clauses.
    /// <code lang="fsharp">
    /// !>> [["s";"p"];["q";"t"]] 
    /// |> one_literal_rule
    /// </code>
    /// Throws <c>System.Collections.Generic.KeyNotFoundException: An index satisfying the predicate was not found in the collection.</c>.
    /// </example>
    /// 
    /// <category index="2">Unit propagation rule</category>
    val one_literal_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    /// <summary>
    /// Pure literals in <c>clauses</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is said to be 'pure' if it occurs <em>only positively</em> or 
    /// <em>only negatively</em> in the list of clauses.
    /// </remarks>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// <returns>
    /// The list of literals that occur pure in <c>clauses</c>.
    /// </returns>
    /// 
    /// <example id="pureLiterals-1">
    /// <code lang="fsharp">
    /// !>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
    /// |> pureLiterals
    /// </code>
    /// Evaluates to <c>[`q`]</c>.
    /// </example>
    /// 
    /// <example id="pureLiterals-2">
    /// <code lang="fsharp">
    /// !>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]]
    /// |> pureLiterals
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    /// 
    /// <note>
    /// This function is not part of the original code: it has been added to 
    /// remove the use of exceptions as control flow (which causes performance 
    /// degradation in F#) in the implementations of the DP and DPLL procedures.
    /// </note>
    /// 
    /// <category index="3">Pure literal rule</category>
    val pureLiterals:
      clauses: formula<'a> list list -> formula<'a> list when 'a: comparison

    /// <summary>
    /// <c>clauses</c> has pure literals.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is said to be 'pure' if it occurs <em>only positively</em> or 
    /// <em>only negatively</em> in the list of clauses.
    /// </remarks>
    /// 
    /// <param name="clauses">The input clauses to be checked.</param>
    /// <returns>
    /// true, if <c>clauses</c> contain pure literals; otherwise, false.
    /// </returns>
    /// 
    /// <example id="hasPureLiteral-1">
    /// <code lang="fsharp">
    /// !>> [["p";"q"];["~p";"~q"]]
    /// |> hasPureLiteral
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="hasPureLiteral-2">
    /// <code lang="fsharp">
    /// !>> [["p";"q"];["~p";"q"]]
    /// |> hasPureLiteral
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <note>
    /// This function is not part of the original code: it has been added to 
    /// remove the use of exceptions as control flow (which causes performance 
    /// degradation in F#) in the implementations of the DP and DPLL procedures.
    /// </note>
    /// 
    /// <category index="3">Pure literal rule</category>
    val hasPureLiteral:
      clauses: formula<'a> list list -> bool when 'a: comparison

    /// <summary>
    /// Removes all clauses that contain pure literals.
    /// </summary>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// 
    /// <returns>
    /// The clauses cleaned of those containing pure literals, if the latter 
    /// were present in the input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'affirmative_negative_rule' when there aren't pure literals.</exception>
    /// 
    /// <example id="affirmative_negative_rule-1">
    /// <code lang="fsharp">
    /// !>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
    /// |> affirmative_negative_rule
    /// </code>
    /// Evaluates to <c>[[`p`; `t`]]</c>.
    /// </example>
    /// 
    /// <example id="affirmative_negative_rule-2">
    /// <code lang="fsharp">
    /// !>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]]
    /// |> affirmative_negative_rule
    /// </code>
    /// Throws <c>System.Exception: affirmative_negative_rule</c>.
    /// </example>
    /// 
    /// <category index="3">Pure literal rule</category>
    val affirmative_negative_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    /// <summary>
    /// Resolves <c>clauses</c> on the literal <c>p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// <ul>
    /// <li>separates <c>clauses</c> in those containing <c>p</c> only 
    /// positively \(C_i\) and those only negative \(D_j\) ,</li>
    /// <li>creates new clauses from all the combinations \(C_i \lor D_j\),</li>
    /// <li>removes all clauses containing <c>p</c> or its negation.</li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="p">The literal on which to resolve.</param>
    /// <param name="clauses">The input clauses.</param>
    /// 
    /// <returns>
    /// The result of resolving the input <c>clauses</c> on <c>p</c>, if the 
    /// rule is applicable; otherwise the input unchanged.
    /// </returns>
    /// 
    /// <example id="resolve_on-1">
    /// <code lang="fsharp">
    /// !>> [["p";"c1";"c2"];
    ///      ["~p";"d1";"d2";"d3";"d4"];
    ///      ["q";"t"];
    ///      ["p";"e1";"e2"]]
    /// |> resolve_on !>"p"
    /// </code>
    /// Evaluates to <c>[[`c1`; `c2`; `d1`; `d2`; `d3`; `d4`]; [`d1`; `d2`;`d3`; `d4`; `e1`; `e2`];[`q`; `t`]]</c>.
    /// </example>
    /// 
    /// <example id="resolve_on-2">
    /// <code lang="fsharp">
    /// !>> [["a"];["b"]]
    /// |> resolve_on !>"p"
    /// </code>
    /// Evaluates to <c>[[`a`]; [`b`]]</c>.
    /// </example>
    /// 
    /// <note>
    /// This rule can increase the clauses' size.
    /// </note>
    /// 
    /// <category index="4">Resolution rule</category>
    val resolve_on:
      p: formula<'a> ->
        clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="4">Resolution rule</category>
    val resolution_blowup:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="4">Resolution rule</category>
    val resolution_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dp: clauses: formula<'a> list list -> bool when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dpsat: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dptaut: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val posneg_count:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dpll: clauses: formula<'a> list list -> bool when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dpllsat: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dplltaut: fm: formula<prop> -> bool

    type trailmix =
        | Guessed
        | Deduced

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unassigned:
      (formula<'a> list list ->
         (formula<'a> * 'b) list -> formula<'a> list)
        when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unit_subpropagate:
      cls: formula<'a> list list *
      fn: func<formula<'a>,unit> *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * func<formula<'a>,unit> *
        (formula<'a> * trailmix) list when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unit_propagate:
      cls: formula<'a> list list *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * (formula<'a> * trailmix) list
        when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val backtrack: trail: ('a * trailmix) list -> ('a * trailmix) list

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dpli:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dplisat: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dplitaut: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val backjump:
      cls: formula<'a> list list ->
        p: formula<'a> ->
        trail: (formula<'a> * trailmix) list ->
        (formula<'a> * trailmix) list when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplb:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplbsat: fm: formula<prop> -> bool

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplbtaut: fm: formula<prop> -> bool