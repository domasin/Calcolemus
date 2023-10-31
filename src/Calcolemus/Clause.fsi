// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Formulas
open Fol

/// <summary>
/// Useful functions for handling clauses.
/// </summary>
/// 
/// <remarks>
/// A clause is a list of literals that, depending on the context, is to be 
/// considered as an iterated conjunction or an iterated disjunction of its 
/// literals. 
/// <p></p>
/// Lists of clauses are used to represent formulas in disjunctive or 
/// conjunctive normal form and in the two contexts they are interpreted, 
/// respectively, as an iterated disjunction of conjunctions (each clause in 
/// this case is a conjunction) or as an iterated conjunction of disjunctions 
/// (and in this case, therefore, each clause is to be considered as the 
/// disjunction of its literals).
/// </remarks>
/// 
/// <note>
/// This module is not part of the original book code and is added here for 
/// convenience.
/// </note>
/// 
/// <category index="4">First order logic</category>
module Clause = 

    /// <summary>
    /// Returns the literals in a formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of literals in the formula.
    /// </returns>
    /// 
    /// <example id="literals-1">
    /// <code lang="fsharp">
    /// literals !!"P(x) ==> Q(x)"
    /// </code>
    /// Evaluates to <c>["`P(x)`";"`Q(x)`"]</c>.
    /// </example>
    /// 
    /// <category index="1">Literals</category>
    val literals:
        fm: formula<'a>
            -> list<formula<'a>>

    /// <summary>
    /// Returns literals that occur both positively and negatively in 
    /// <c>lits</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input list of literals.</param>
    /// 
    /// <returns>
    /// The list of literals that occur both positively and negatively in 
    /// <c>lits</c>.
    /// </returns>
    /// 
    /// <example id="opposites-1">
    /// <code lang="fsharp">
    /// List.map (!!) ["P(x)";"Q(x)";"~P(x)"]
    /// |> opposites
    /// </code>
    /// Evaluates to <c>["`P(x)`"]</c>.
    /// </example>
    /// 
    /// <category index="1">Literals</category>
    val opposites:
        lits: list<formula<'a>>
            -> list<formula<'a>> 
            when 'a : comparison

    /// <summary>
    /// Parses a list of string lists into a list of formula lists.
    /// Useful when dealing with clauses.
    /// </summary>
    /// 
    /// <param name="xs">The input list of string lists.</param>
    /// <returns>The list of clauses.</returns>
    /// 
    /// <example id="exclamation-exclamation-greater-greater-1">
    /// <code lang="fsharp">
    /// !!>> [["P(x)"];["P(x)";"~Q(x)"]]
    /// </code>
    /// Evaluates to <c>[[`P(x)`]; [`P(x)`; `~Q(x)`]]</c>.
    /// </example>
    /// 
    /// <note>
    /// This operator is not part of the original code and it was added here 
    /// for convenience.
    /// </note>
    /// 
    /// <category index="2">Parsing clauses</category>
    val (!!>>): xs: string list list -> formula<fol> list list

    /// <summary>
    /// Prints the list of clauses rendering literals in the concrete string 
    /// representation.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val sprint_clauses: 
        clauses: list<list<formula<fol>>> -> list<list<string>>

    /// <summary>
    /// Converts a DNF formula into a list fo clauses.
    /// </summary>
    /// 
    /// <param name="fm">The input DNF formula.</param>
    /// <returns>The corresponding list of clauses.</returns>
    /// 
    /// <example id="djsToClauses-1">
    /// <code lang="fsharp">
    /// !! @"(Q(x) /\ ~R(x,y) /\ P(f(z)) \/ (~P(x) /\ Q(x)))"
    /// |> djsToClauses
    /// </code>
    /// Evaluates to <c>[[`Q(x)`; `~R(x,y)`; `P(f(z))`]; [`~P(x)`; `Q(x)`]]</c>.
    /// </example>
    /// 
    /// <category index="5">Formula to clauses</category>
    val djsToClauses:
        fm: formula<'a>
            -> list<list<formula<'a>>>

    /// <summary>
    /// Converts a CNF formula into a list fo clauses.
    /// </summary>
    /// 
    /// <param name="fm">The input CNF formula.</param>
    /// <returns>The corresponding list of clauses.</returns>
    /// 
    /// <example id="cjsToClauses-1">
    /// <code lang="fsharp">
    /// !! @"(Q(x) \/ ~R(x,y) \/ P(f(z)) /\ (~P(x) \/ Q(x)))"
    /// |> cjsToClauses
    /// </code>
    /// Evaluates to <c>[[`Q(x)`; `~R(x,y)`; `P(f(z))`]; [`~P(x)`; `Q(x)`]]</c>.
    /// </example>
    /// 
    /// <category index="5">Formula to clauses</category>
    val cjsToClauses:
        fm: formula<'a>
            -> list<list<formula<'a>>>

    /// <summary>
    /// Converts a list of clauses into a DNF formula.
    /// </summary>
    /// 
    /// <remarks>
    /// It is supposed that DNF is the intended representation.
    /// </remarks>
    /// 
    /// <param name="cls">The input clauses.</param>
    /// <returns>The corresponding DNF formula.</returns>
    /// 
    /// <example id="clausesToDnf-1">
    /// <code lang="fsharp">
    /// !!>>[["Q(x)"; "~R(x,y)"; "P(f(z))"]; ["~P(x)"; "Q(x)"]]
    /// |> clausesToDnf
    /// </code>
    /// Evaluates to <c>`Q(x) /\ ~R(x,y) /\ P(f(z)) \/ ~P(x) /\ Q(x)`</c>.
    /// </example>
    /// 
    /// <category index="6">Clauses to formula</category>
    val clausesToDnf:
        cls: list<list<formula<'a>>>
            -> formula<'a> when 'a : equality