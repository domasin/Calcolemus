// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Calcolemus.Lib.Fpf

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
    /// A simplistic heuristic to predict the best literal to resolve on.
    /// </summary>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="p">The literal on which to resolve.</param>
    /// <returns>
    /// m * n - m - n where m and m are the number of clauses in which <c>l</c> 
    /// occurs respectively positively and negatively in <c>cls</c>.
    /// </returns>
    /// 
    /// <example id="resolution_blowup-1">
    /// <code lang="fsharp">
    /// let cls = !>> [
    ///      ["p";"c"];["~p";"d"]
    ///      ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    /// ]
    /// 
    /// resolution_blowup cls !>"c" // evaluates to -1
    /// resolution_blowup cls !>"d" // evaluates to -1
    /// resolution_blowup cls !>"e" // evaluates to -1
    /// resolution_blowup cls !>"p" // evaluates to -1
    /// resolution_blowup cls !>"q" // evaluates to 1
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Resolution rule</category>
    val resolution_blowup:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    /// <summary>
    /// Resolves the input <c>clauses</c> on the literal which minimizes <see cref='M:Calcolemus.DP.resolution_blowup``1'/>.
    /// </summary>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// 
    /// <returns>
    /// The result of resolving the input <c>clauses</c> on the literal which minimizes <see cref='M:Calcolemus.DP.resolution_blowup``1'/>.
    /// </returns>
    /// 
    /// <example id="resolution_rule-1">
    /// <code lang="fsharp">
    /// !>> [
    ///      ["p";"c"];["~p";"d"]
    ///      ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    /// ]
    /// |>  resolution_rule
    /// </code>
    /// Evaluates to <c>[[`c`; `d`]; [`q`; `~c`]; [`q`; `~d`]; [`q`; `~e`];[`~q`; `e`]; [`~q`; `~d`]]</c>.
    /// </example>
    /// 
    /// <category index="4">Resolution rule</category>
    val resolution_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    /// <summary>
    /// Tests <c>clauses</c> (propositional) satisfiability with the 
    /// Davis-Putnam procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// Tests the (propositional) satisfiability of a list of clauses (to be 
    /// understood as an iterated conjunction of disjunctions), using the 
    /// Davis-Putnam procedure which consists in applying recursively the rules 
    /// <see cref='M:Calcolemus.DP.one_literal_rule``1'/>, 
    /// <see cref='M:Calcolemus.DP.affirmative_negative_rule``1'/> and 
    /// <see cref='M:Calcolemus.DP.resolution_rule``1'/>
    /// </remarks>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// 
    /// <returns>
    /// true, if the recursive application of the rules leads to an empty set 
    /// of clauses (meaning that the input is satisfiable); otherwise, when the 
    /// rules lead to a set that contains an empty clause, false (meaning that 
    /// the input is unsatisfiable).
    /// </returns>
    /// 
    /// <example id="dp-1">
    /// <code lang="fsharp">
    /// dp !>> [["p"]]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dp-2">
    /// <code lang="fsharp">
    /// dp !>> [["p"];["~p"]]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dp: clauses: formula<'a> list list -> bool when 'a: comparison

    /// <summary>
    /// Tests <c>fm</c> (propositional) satisfiability with the Davis-Putnam 
    /// procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is satisfiable: otherwise false.
    /// </returns>
    /// 
    /// <example id="dpsat-1">
    /// <code lang="fsharp">
    /// dpsat !> "p"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dpsat-2">
    /// <code lang="fsharp">
    /// dpsat !> "p /\ ~p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dpsat: fm: formula<prop> -> bool

    /// <summary>
    /// Tests <c>fm</c> (propositional) validity with the Davis-Putnam 
    /// procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is a tautology: otherwise false.
    /// </returns>
    /// 
    /// <example id="dptaut-1">
    /// <code lang="fsharp">
    /// dptaut !> "p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dptaut-2">
    /// <code lang="fsharp">
    /// dptaut (prime 11)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="5">Davis-Putnam Procedure</category>
    val dptaut: fm: formula<prop> -> bool

    /// <summary>
    /// Counts the number of occurrences (both positively or negatively) of the 
    /// literal <c>l</c> in <c>cls</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It is used as an heuristic to chose the literal for the splitting rule 
    /// in the Davis-Putnam-Loveland-Logemann procedure.
    /// </remarks>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="l">The input literal.</param>
    /// <returns>
    /// The number of <c>l</c>'s occurrences (both positively or negatively) in 
    /// <c>cls</c>.
    /// </returns>
    /// 
    /// <example id="posneg_count-1">
    /// <code lang="fsharp">
    /// posneg_count !>> [
    ///      ["p";"c"];["~p";"d"]
    ///      ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    /// ] !>"q"
    /// </code>
    /// Evaluates to <c>5</c>.
    /// </example>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val posneg_count:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    /// <summary>
    /// Tests <c>clauses</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// Tests the (propositional) satisfiability of a list of clauses (to be 
    /// understood as an iterated conjunction of disjunctions), using the 
    /// Davis-Putnam-Loveland-Logemann procedure which consists in applying 
    /// recursively the rules 
    /// <see cref='M:Calcolemus.DP.one_literal_rule``1'/>, 
    /// <see cref='M:Calcolemus.DP.affirmative_negative_rule``1'/> and, if 
    /// neither of these is applicable (instead of 
    /// <see cref='M:Calcolemus.DP.resolution_rule``1'/>) the <em>splitting 
    /// rule</em>.
    /// <p></p>
    /// The splitting rule consists in choosing some literal and testing, 
    /// separately, the satisfiability of the union of the input with this 
    /// literal and its negation, respectively: if one of these is satisfiable 
    /// so the original input is.
    /// <p></p>
    /// The literal that maximizes 
    /// <see cref='M:Calcolemus.DP.posneg_count``1'/> is chosen for the 
    /// splitting rule.
    /// </remarks>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// 
    /// <returns>
    /// true, if the recursive application of the rules leads to an empty set 
    /// of clauses (meaning that the input is satisfiable); otherwise, when the 
    /// rules lead to a set that contains an empty clause, false (meaning that 
    /// the input is unsatisfiable).
    /// </returns>
    /// 
    /// <example id="dpll-1">
    /// <code lang="fsharp">
    /// dpll !>> [["p"]]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dpll-2">
    /// <code lang="fsharp">
    /// dpll !>> [["p"];["~p"]]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dpll: clauses: formula<'a> list list -> bool when 'a: comparison

    /// <summary>
    /// Tests <c>fm</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is satisfiable: otherwise false.
    /// </returns>
    /// 
    /// <example id="dpllsat-1">
    /// <code lang="fsharp">
    /// dpllsat !> "p"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dpllsat-2">
    /// <code lang="fsharp">
    /// dpllsat !> "p /\ ~p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dpllsat: fm: formula<prop> -> bool

    /// <summary>
    /// Tests <c>fm</c> (propositional) validity with the 
    /// Davis-Putnam-Loveland-Logemann procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is a tautology: otherwise false.
    /// </returns>
    /// 
    /// <example id="dplltaut-1">
    /// <code lang="fsharp">
    /// dplltaut !> "p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dplltaut-2">
    /// <code lang="fsharp">
    /// dplltaut (prime 11)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="6">Davis-Putnam-Loveland-Logemann Procedure</category>
    val dplltaut: fm: formula<prop> -> bool

    /// <summary>
    /// Flags to mark literals in the case split <em>trail</em> used in the 
    /// DPLL iterative implementation. 
    /// 
    /// <seealso cref='M:Calcolemus.DP.dpli``1'/>
    /// </summary>
    type trailmix =
        /// <summary>
        /// Literal just assumed as the fst half of a case-split.
        /// </summary>
        | Guessed
        /// <summary>
        /// Literal deduced by unit propagation.
        /// </summary>
        | Deduced

    /// <summary>
    /// Returns <c>clauses</c>' literals that are not yet assigned in 
    /// <c>trail</c>.
    /// </summary>
    /// 
    /// <param name="clauses">The input clauses.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// The list of literals in <c>clauses</c> that are not in <c>trail</c>.
    /// </returns>
    /// 
    /// <example id="unassigned-1">
    /// <code lang="fsharp">
    /// let trail = [!>"p", Deduced;!>"q", Guessed]
    /// 
    /// unassigned !>> [
    ///      ["p";"c"];["~p";"d"]
    ///      ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    /// ] trail
    /// </code>
    /// Evaluates to <c>[`c`; `d`; `e`]</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unassigned:
      clauses: formula<'a> list list ->
         trail: (formula<'a> * 'b) list -> formula<'a> list
        when 'a: comparison

    /// <summary>
    /// Performs unit propagation exhaustively modifying internally the clauses 
    /// <c>cls</c> and processing the <c>trail</c> into a finite partial 
    /// function <c>fn</c> for more efficient lookup.
    /// </summary>
    /// 
    /// <remarks>
    /// The clauses are updated only from the point of view of the removing of 
    /// unit clauses' complementary literals, if there are.
    /// </remarks>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="fn">The fpf to process the trail.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// The triple of the clauses, fpf and trail updated with the result of 
    /// unit propagation.
    /// </returns>
    /// 
    /// <example id="unit_subpropagate-1">
    /// <code lang="fsharp">
    /// ((!>> [["p"];["p";"q"]]), undefined,[])
    /// |> unit_subpropagate 
    /// |> fun (cls,fpf,trail) -> (cls,fpf |> graph,trail)
    /// </code>
    /// Evaluates to 
    /// <c>([[`p`]; [`p`; `q`]], [(`p`, ())], [(`p`, Deduced)])</c>.
    /// </example>
    /// 
    /// <example id="unit_subpropagate-2">
    /// <code lang="fsharp">
    /// ((!>> [["p"];["~p";"q"]]), undefined,[])
    /// |> unit_subpropagate 
    /// |> fun (cls,fpf,trail) -> (cls,fpf |> graph,trail)
    /// </code>
    /// Evaluates to 
    /// <c>([[`p`]; [`q`]], [(`p`, ()); (`q`, ())], [(`q`, Deduced); (`p`, Deduced)])</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unit_subpropagate:
      cls: formula<'a> list list *
      fn: func<formula<'a>,unit> *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * func<formula<'a>,unit> *
        (formula<'a> * trailmix) list when 'a: comparison

    /// <summary>
    /// Performs unit propagation modifying internally the clauses <c>cls</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The clauses are updated only from the point of view of the removing of 
    /// unit clauses' complementary literals, if there are.
    /// </remarks>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// The tuple of the clauses and trail updated with the 
    /// result of unit propagation.
    /// </returns>
    /// 
    /// <example id="unit_propagate-1">
    /// <code lang="fsharp">
    /// ((!>> [["p"];["p";"q"]]), [])
    /// |> unit_propagate 
    /// </code>
    /// Evaluates to 
    /// <c>([[`p`]; [`p`; `q`]], [(`p`, Deduced)])</c>.
    /// </example>
    /// 
    /// <example id="unit_propagate-2">
    /// <code lang="fsharp">
    /// ((!>> [["p"];["~p";"q"]]), [])
    /// |> unit_propagate 
    /// </code>
    /// Evaluates to 
    /// <c>([[`p`]; [`q`]], [(`q`, Deduced); (`p`, Deduced)])</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val unit_propagate:
      cls: formula<'a> list list *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * (formula<'a> * trailmix) list
        when 'a: comparison

    /// <summary>
    /// Removes items from the trail until the most recent decision literal is 
    /// reached or there are no one left in the trail.
    /// </summary>
    /// 
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// The literals in the trail starting from the first guessed one.
    /// </returns>
    /// 
    /// <example id="backtrack-1">
    /// <code lang="fsharp">
    /// [
    ///      !>"c", Deduced; 
    ///      !>"b", Deduced; 
    ///      !>"a", Guessed
    /// 
    ///      !>"e", Deduced; 
    ///      !>"d", Guessed
    /// ]
    /// |> backtrack
    /// </code>
    /// Evaluates to 
    /// <c>[(`a`, Guessed); (`e`, Deduced); (`d`, Guessed)]</c>.
    /// </example>
    /// 
    /// <example id="backtrack-2">
    /// <code lang="fsharp">
    /// [
    ///      !>"c", Deduced; 
    ///      !>"b", Deduced; 
    ///      !>"e", Deduced; 
    /// ]
    /// |> backtrack
    /// </code>
    /// Evaluates to 
    /// <c>[]</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val backtrack: trail: ('a * trailmix) list -> ('a * trailmix) list

    /// <summary>
    /// Tests <c>clauses</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure with an iterative 
    /// implementation.
    /// </summary>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// true, if the input (based on trail) is satisfiable; otherwise, false.
    /// </returns>
    /// 
    /// <example id="dpli-1">
    /// <code lang="fsharp">
    /// dpli !>>[["~p";"q"];["~q"]] [!>"p", Deduced; !>"~q", Deduced]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dpli-2">
    /// <code lang="fsharp">
    /// dpli !>>[["~p";"q"];["~q"]] []
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dpli:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    /// <summary>
    /// Tests <c>fm</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure with the iterative 
    /// implementation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is satisfiable: otherwise false.
    /// </returns>
    /// 
    /// <example id="dplisat-1">
    /// <code lang="fsharp">
    /// dplisat !> "p"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dplisat-2">
    /// <code lang="fsharp">
    /// dplisat !> "p /\ ~p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dplisat: fm: formula<prop> -> bool

    /// <summary>
    /// Tests <c>fm</c> (propositional) validity with the 
    /// Davis-Putnam-Loveland-Logemann procedure with iterative implementation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is a tautology: otherwise false.
    /// </returns>
    /// 
    /// <example id="dplitaut-1">
    /// <code lang="fsharp">
    /// dplitaut !> "p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dplitaut-2">
    /// <code lang="fsharp">
    /// dplitaut (prime 11)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="7">DPLL iterative implementation</category>
    val dplitaut: fm: formula<prop> -> bool

    /// <summary>
    /// Goes back through the trail as far as possible while <c>p</c> still 
    /// leads to a conflict.
    /// </summary>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="p">The literal to check.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// true, if the input formula is a tautology: otherwise false.
    /// </returns>
    /// 
    /// <example id="backjump-1">
    /// <code lang="fsharp">
    /// backjump !>>[["~p";"q"];["~q"]] !>"a"
    /// [
    ///     !>"c", Deduced; 
    ///     !>"b", Deduced; 
    ///     !>"~a", Deduced
    ///     !>"e", Guessed; 
    ///     !>"p", Deduced; 
    ///     !>"d", Guessed
    /// ]
    /// </code>
    /// Evaluates to <c>[(`p`, Deduced); (`d`, Guessed)]</c>.
    /// </example>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val backjump:
      cls: formula<'a> list list ->
        p: formula<'a> ->
        trail: (formula<'a> * trailmix) list ->
        (formula<'a> * trailmix) list when 'a: comparison

    /// <summary>
    /// Tests <c>clauses</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure with an iterative 
    /// implementation and backjumping and learning optimizations.
    /// </summary>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <param name="trail">The input trail of assigned literals.</param>
    /// 
    /// <returns>
    /// true, if the input (based on trail) is satisfiable; otherwise, false.
    /// </returns>
    /// 
    /// <example id="dplb-1">
    /// <code lang="fsharp">
    /// dplb !>>[["~p";"q"];["~q"]] [!>"p", Deduced; !>"~q", Deduced]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dplb-2">
    /// <code lang="fsharp">
    /// dplb !>>[["~p";"q"];["~q"]] []
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplb:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    /// <summary>
    /// Tests <c>fm</c> (propositional) satisfiability with the 
    /// Davis-Putnam-Loveland-Logemann procedure with the iterative 
    /// implementation and backjumping and learning optimizations.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is satisfiable: otherwise false.
    /// </returns>
    /// 
    /// <example id="dplbsat-1">
    /// <code lang="fsharp">
    /// dplbsat !> "p"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dplbsat-2">
    /// <code lang="fsharp">
    /// dplbsat !> "p /\ ~p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplbsat: fm: formula<prop> -> bool

    /// <summary>
    /// Tests <c>fm</c> (propositional) validity with the 
    /// Davis-Putnam-Loveland-Logemann procedure with iterative implementation 
    /// and backjumping and learning optimizations.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is a tautology: otherwise false.
    /// </returns>
    /// 
    /// <example id="dplbtaut-1">
    /// <code lang="fsharp">
    /// dplbtaut !> "p"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="dplbtaut-2">
    /// <code lang="fsharp">
    /// dplbtaut (prime 11)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="dplbtaut-3">
    /// dplbtaut is 4X more faster than dplitaut:
    /// <code lang="fsharp">
    /// time dplbtaut (prime 101)
    /// // Evaluates to:
    /// // CPU time (user): 36.981689
    /// // val it: bool = true
    /// 
    /// time dplitaut (prime 101)
    /// // Evaluates to:
    /// // CPU time (user): 130.045742
    /// // val it: bool = true
    /// </code>
    /// </example>
    /// 
    /// <category index="8">DPLL with backjumping and learning</category>
    val dplbtaut: fm: formula<prop> -> bool