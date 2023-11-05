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
/// Backchaining procedure for Horn clauses, and toy Prolog implementation.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Prolog = 

    /// <summary>
    /// Renames the variables in a rule schematically starting with <c>_k</c>.
    /// </summary>
    /// 
    /// <param name="k">The counter for variables renaming.</param>
    /// <param name="asm">The assumptions of the input rule.</param>
    /// <param name="c">The conclusion of the input rule.</param>
    /// 
    /// <returns>
    /// The input rule with the variables renamed schematically starting with 
    /// <c>_k</c>
    /// </returns>
    /// 
    /// <example id="renamerule-1">
    /// <code lang="fsharp">
    /// renamerule 0 (!!>["P(x)";"Q(y)"],!!"P(f(x))")
    /// |> graph
    /// </code>
    /// Evaluates to <c>(([`P(_0)`; `Q(_1)`], `P(f(_0))`), 2)</c>.
    /// </example>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val renamerule:
      k: int ->
        asm: formula<fol> list * c: formula<fol> ->
          (formula<fol> list * formula<fol>) * int

    /// <summary>
    /// Basic prover for Horn clauses.
    /// </summary>
    /// 
    /// <param name="rules">The list of input rules.</param>
    /// <param name="n">The limit on the maximum number of rule applications.</param>
    /// <param name="k">The counter for variables renaming.</param>
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. .</param>
    /// <param name="goals">The input goals.</param>
    /// 
    /// <returns>
    /// The current instantiation <c>env</c>, if the list of <c>goals</c> is 
    /// empty; otherwise, recursively searches through the <c>rules</c> one 
    /// whose consequent can be unified with the current goal and such that the 
    /// new subgoals together with the original ones can be solved under that 
    /// instantiation.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Too deep</c> when <c>n = 0</c>.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goals are not solvable, at least at the current limit.</exception>
    /// 
    /// <example id="backchain-1">
    /// <code lang="fsharp">
    /// !!>["S(x) &lt;= S(S(x))"] 
    /// |> backchain 
    ///     [
    ///         ([], !!"0 &lt;= x"); 
    ///         ([!!"x &lt;= y"], !!"S(x) &lt;= S(y)")
    ///     ] 2 0 undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("_0", ``x``); ("_1", ``S(x)``); ("_2", ``_1``); ("x", ``0``)]</c>.
    /// </example>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val backchain:
      rules: (formula<fol> list * formula<fol>) list ->
        n: int ->
        k: int ->
        env: func<string,term> ->
        goals: formula<fol> list -> func<string,term>

    /// <summary>
    /// Converts a raw Horn clause into a rule.
    /// </summary>
    /// 
    /// <param name="cls">The input Horn clause.</param>
    /// 
    /// <returns>
    /// The equivalent in rule format, if it exists.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>non-Horn clause</c> when <c>cls</c> is not an Horn clause.</exception>
    /// 
    /// <example id="hornify-1">
    /// <code lang="fsharp">
    /// !!>["~P(x)";"Q(y)";"~T(x)"]
    /// |> hornify
    /// </code>
    /// Evaluates to <c>([`P(x)`; `T(x)`], `Q(y)`)</c>.
    /// </example>
    /// 
    /// <example id="hornify-2">
    /// <code lang="fsharp">
    /// !!>["P(x)";"Q(y)";"~T(x)"]
    /// |> hornify
    /// </code>
    /// Throws <c>System.Exception: non-Horn clause</c>.
    /// </example>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val hornify:
      cls: formula<'a> list ->
        formula<'a> list * formula<'a> when 'a: equality

    /// <summary>
    /// Tests the validity of a formula convertible in Horn clauses.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The instantiation resulting from the backchain procedure and the level 
    /// at which it succeeds, if the formula is valid and Horn clause 
    /// convertible.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>non-Horn clause</c> when <c>fm</c> is not convertible into a set of Horn clauses.</exception>
    /// 
    /// <note>
    /// Prints to the <c>stdout</c> the depth limits at which the search was 
    /// carried out.
    /// </note>
    /// 
    /// <example id="hornprove-1">
    /// <code lang="fsharp">
    /// Pelletier.p32
    /// |> hornprove
    /// |> fun (inst,level) -> 
    ///     graph inst, level
    /// </code>
    /// Evaluates to <c>([("_0", ``c_x``); ("_1", ``_0``); ("_2", ``_0``); ("_3", ``_2``)], 8)</c>.
    /// </example>
    /// 
    /// <example id="hornprove-2">
    /// <code lang="fsharp">
    /// !! @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) 
    ///         ==> ~(~q \/ ~q)"
    /// |> hornprove
    /// </code>
    /// Throws <c>System.Exception: non-Horn clause</c>.
    /// </example>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val hornprove:
      fm: formula<fol> -> func<string,term> * int

    /// <summary>
    /// Parses rules in a Prolog-like syntax.
    /// </summary>
    /// 
    /// <param name="s">The input string to be parsed.</param>
    /// 
    /// <returns>
    /// The rule corresponding to the input string, if this is syntactically 
    /// valid.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Extra material after rule</c> when the input string isn't syntactically valid.</exception>
    /// 
    /// <example id="parserule-1">
    /// <code lang="fsharp">
    /// "S(X) &lt;= S(Y) :- X &lt;= Y"
    /// |> parserule
    /// </code>
    /// Evaluates to <c>([`X &lt;= Y`], `S(X) &lt;= S(Y)`)</c>.
    /// </example>
    /// 
    /// <example id="parserule-2">
    /// <code lang="fsharp">
    /// "S(X) >"
    /// |> parserule
    /// </code>
    /// Throws <c>System.Exception: Extra material after rule</c>.
    /// </example>
    /// 
    /// <category index="2">Prolog</category>
    val parserule:
      s: string -> formula<fol> list * formula<fol>

    /// <summary>
    /// Prolog interpreter without clear variable binding output.
    /// </summary>
    /// 
    /// <param name="rules">The input rules.</param>
    /// <param name="gl">The input goal.</param>
    /// 
    /// <returns>
    /// The instantiation resulting from backchaining, if the goal is solvable 
    /// from the rules.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goal is not solvable from the rules.</exception>
    /// 
    /// <example id="simpleprolog-1">
    /// <code lang="fsharp">
    /// let lerules = ["0 &lt;= X"; "S(X) &lt;= S(Y) :- X &lt;= Y"]
    /// 
    /// simpleprolog lerules "S(S(0)) &lt;= S(S(S(0)))"
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("_0", ``S(0)``); ("_1", ``S(S(0))``); ("_2", ``0``); ("_3", ``S(0)``); ("_4", ``_3``)]</c>.
    /// </example>
    /// 
    /// <example id="simpleprolog-2">
    /// <code lang="fsharp">
    /// simpleprolog lerules "S(0) &lt;= 0"
    /// |> graph
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <category index="2">Prolog</category>
    val simpleprolog:
      rules: string list -> gl: string -> func<string,term>

    /// <summary>
    /// Prolog interpreter.
    /// </summary>
    /// 
    /// <param name="rules">The input rules.</param>
    /// <param name="gl">The input goal.</param>
    /// 
    /// <returns>
    /// A successful goal's variables instantiation, if the goal is solvable 
    /// from the rules.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goal is not solvable from the rules.</exception>
    /// 
    /// <example id="prolog-1">
    /// <code lang="fsharp">
    /// let lerules = ["0 &lt;= X"; "S(X) &lt;= S(Y) :- X &lt;= Y"]
    /// 
    /// prolog lerules "S(S(x)) &lt;= S(S(S(0)))"
    /// </code>
    /// Evaluates to <c>[`x = 0`]</c>.
    /// </example>
    /// 
    /// <example id="prolog-2">
    /// <code lang="fsharp">
    /// prolog lerules "S(0) &lt;= 0"
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <category index="2">Prolog</category>
    val prolog: rules: string list -> gl: string -> formula<fol> list