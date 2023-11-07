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
/// Model elimination procedure (MESON version, based on Stickel's PTTP).
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Meson = 

    /// <summary>
    /// Converts a clause into the set of all its contrapositives.
    /// </summary>
    /// 
    /// <remarks>
    /// If the clause is completely negative, is also added the rule with 
    /// <c>false</c> as conclusion.
    /// </remarks>
    /// 
    /// <param name="cls">The input clause.</param>
    /// 
    /// <returns>
    /// The set of contrapositives of the input clause.
    /// </returns>
    /// 
    /// <example id="contrapositives-1">
    /// <code lang="fsharp">
    /// contrapositives !!>["P";"Q";"~R"]
    /// </code>
    /// Evaluates to <c>[([`~Q`; `R`], `P`); ([`~P`; `R`], `Q`); ([`~P`; `~Q`], `~R`)]</c>.
    /// </example>
    /// 
    /// <example id="contrapositives-2">
    /// <code lang="fsharp">
    /// contrapositives !!>["~P";"~Q";"~R"]
    /// </code>
    /// Evaluates to <c>[([`P`; `Q`; `R`], `false`); ([`Q`; `R`], `~P`); ([`P`; `R`], `~Q`); ([`P`; `Q`], `~R`)]</c>.
    /// </example>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val contrapositives:
      cls: formula<'a> list ->
        (formula<'a> list * formula<'a>) list when 'a: comparison

    /// <summary>
    /// Core MESON engine.
    /// </summary>
    /// 
    /// <remarks>
    /// It tries to solve the input goal <c>g</c> according to the <c>rules</c> 
    /// with the following steps:
    /// <ul>
    /// <li>
    /// If the current size bound has been exceeded, fail;
    /// </li>
    /// <li>
    /// otherwise, try to unify the current goal with the negation of one of 
    /// its ancestors (not renaming variables) and call <c>cont</c> to do the 
    /// same for the remaining goals under the new instantiation;
    /// </li>
    /// <li>
    /// if this fails, try a normal Prolog-style extension with one of the 
    /// rules, first unifying with a renamed rule and then iterating recursive 
    /// calls over the list of subgoals, with the environment modified 
    /// according to the results of unification, the permissible number of new 
    /// nodes decreased by the number of new subgoals created, 
    /// and the variable renaming counter increased.
    /// </li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="rules">The input list of rules.</param>
    /// <param name="ancestors">The accumulator for the ancestors.</param>
    /// <param name="g">The goal to be solved from the rules.</param>
    /// <param name="cont">The continuation function to solve the subgoals.</param>
    /// <param name="env">The given instantiation under which trying solving.</param>
    /// <param name="n">The maximum number of rule applications permitted.</param>
    /// <param name="k">The counter for variable renaming.</param>
    /// 
    /// <returns>
    /// The triple with the current instantiation, the depth reached and the 
    /// number of variables renamed.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Too deep</c> when <c>n &lt; 0</c>.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goals are not solvable, at least at the current limit.</exception>
    /// 
    /// <note>
    /// See also 
    /// <seealso cref='M:Calculemus.Prolog.backchain'/>,
    /// <seealso cref='M:Calculemus.Tableaux.tableau'/> 
    /// </note>
    /// 
    /// <example id="mexpand_basic-1">
    /// <code lang="fsharp">
    /// mexpand_basic 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,1,0)
    /// |> fun (env,n,k) -> (graph env,n,k)
    /// </code>
    /// Evaluates to <c>([("_0", ``_1``)], 0, 2)</c>.
    /// </example>
    /// 
    /// <example id="mexpand_basic-2">
    /// <code lang="fsharp">
    /// mexpand_basic 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,0,0)
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <example id="mexpand_basic-3">
    /// <code lang="fsharp">
    /// mexpand_basic 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,-1,0)
    /// </code>
    /// Throws <c>System.Exception: Too deep</c>.
    /// </example>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val mexpand_basic:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> func<string,term> * int * int) ->
        env: func<string,term> * n: int * k: int -> func<string,term> * int * int

    /// <summary>
    /// Tests the unsatisfiability of a formula using the core MESON 
    /// procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the number of depth limit reached trying refuting the formula with 
    /// MESON, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="puremeson_basic-1">
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> puremeson_basic
    /// </code>
    /// Evaluates to <c>1</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// </code>
    /// </example>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val puremeson_basic: fm: formula<fol> -> int

    /// <summary>
    /// Tests the validity of a formula by negating it and splitting in 
    /// subproblems to be refuted with the core MESON procedure. 
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the list of depth limits reached trying to refute the subproblems, if 
    /// the formula is valid.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="meson_basic-1">
    /// <code lang="fsharp">
    /// !! @"exists x. exists y. forall z.
    ///     (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ///     ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    /// |> meson_basic
    /// </code>
    /// Evaluates to <c>[8]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// Searching with depth limit 3
    /// Searching with depth limit 4
    /// Searching with depth limit 5
    /// Searching with depth limit 6
    /// Searching with depth limit 7
    /// Searching with depth limit 8
    /// </code>
    /// </example>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val meson_basic: fm: formula<fol> -> int list

    /// <summary>
    /// Tests if two literals are identical under the given instantiation.
    /// </summary>
    /// 
    /// <param name="env">The given instantiation.</param>
    /// <param name="fm1">The first input literal.</param>
    /// <param name="fm2">The second input literal.</param>
    /// 
    /// <returns>
    /// true, if the input formulas are literals and identical under the give 
    /// instantiation; otherwise, false (this is the case also if the input 
    /// formulas are identical but not literals).
    /// </returns>
    /// 
    /// <example id="equal-1">
    /// Identical literals
    /// <code lang="fsharp">
    /// equal 
    ///   (("x" |-> !!!"f(y)")undefined) 
    ///   !!"P(x)" !!"P(f(y))"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="equal-2">
    /// Different literals
    /// <code lang="fsharp">
    /// equal 
    ///   (("x" |-> !!!"f(z)")undefined) 
    ///   !!"P(x)" !!"P(f(y))"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="equal-3">
    /// Identical formulas that are not literals
    /// <code lang="fsharp">
    /// equal 
    ///   (("x" |-> !!!"f(y)")undefined) 
    ///   !!"P(x) /\ P(x)" !!"P(f(y)) /\ P(f(y))"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val equal:
      env: func<string,term> ->
        fm1: formula<fol> -> fm2: formula<fol> -> bool

    /// <summary>
    /// Optimizes the depth limit used by a basic expansion.
    /// </summary>
    /// 
    /// <remarks>
    /// Applies <c>expfn</c> to <c>goals1</c> with size limit <c>n1</c>, 
    /// then attempts <c>goals2</c> with whatever is left over from 
    /// <c>goals1</c> plus an additional <c>n2</c>, yet forces the 
    /// continuation to fail unless the second takes more than <c>n3</c>.
    /// </remarks>
    /// 
    /// <param name="expfn">The input basic expansion.</param>
    /// <param name="goals1">The first list of subgoals.</param>
    /// <param name="goals2">The second list of subgoals.</param>
    /// <param name="n1">The depth limit for <c>goals1</c>.</param>
    /// <param name="n2">The depth limit for <c>goals2</c> that will be added to what is left from <c>goals1</c>.</param>
    /// <param name="n3">The minimum depth limit allowed for <c>goals2</c>.</param>
    /// <param name="cont">The supporting continuation function.</param>
    /// <param name="env">The given instantiation under which trying solving.</param>param>
    /// <param name="k">The counter for variable renaming.</param>
    /// 
    /// <returns>
    /// The triple with the current instantiation, the depth reached and the 
    /// number of variables renamed. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>pair</c> when... TODO</exception>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val expand2:
      expfn : (list<formula<fol>> -> (func<string,term> * int * int -> func<string,term> * int * int) -> func<string,term> * int * int -> func<string,term> * int * int) ->
      goals1: list<formula<fol>> ->
      n1    : int ->
      goals2: list<formula<fol>> ->
      n2    : int ->
      n3    : int ->
      cont  : (func<string,term> * int * int -> func<string,term> * int * int) ->
      env   : func<string,term> ->
      k     : int
            -> func<string,term> * int * int

    /// <summary>
    /// Core MESON engine optimized with 
    /// <see cref='M:Calculemus.Meson.expand2'/>.
    /// </summary>
    /// 
    /// <remarks>
    /// ... TODO
    /// </remarks>
    /// 
    /// <param name="rules">The input list of rules.</param>
    /// <param name="ancestors">The accumulator for the ancestors.</param>
    /// <param name="g">The goal to be solved from the rules.</param>
    /// <param name="cont">The continuation function to solve the subgoals.</param>
    /// <param name="env">The given instantiation under which trying solving.</param>
    /// <param name="n">The maximum number of rule applications permitted.</param>
    /// <param name="k">The counter for variable renaming.</param>
    /// 
    /// <returns>
    /// The triple with the current instantiation, the depth reached and the 
    /// number of variables renamed.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Too deep</c> when <c>n &lt; 0</c>.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goals are not solvable, at least at the current limit.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>repetition</c> when... TODO</exception>
    /// 
    /// <example id="mexpand-1">
    /// <code lang="fsharp">
    /// mexpand 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,1,0)
    /// |> fun (env,n,k) -> (graph env,n,k)
    /// </code>
    /// Evaluates to <c>([("_0", ``_1``)], 0, 2)</c>.
    /// </example>
    /// 
    /// <example id="mexpand-2">
    /// <code lang="fsharp">
    /// mexpand 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,0,0)
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <example id="mexpand-3">
    /// <code lang="fsharp">
    /// mexpand 
    ///   [
    ///       ([], !!"P(x)"); 
    ///       ([!!"P(x)"], False);
    ///   ]
    ///   [] False id (undefined,-1,0)
    /// </code>
    /// Throws <c>System.Exception: Too deep</c>.
    /// </example>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val mexpand:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> func<string,term> * int * int ) ->
        env: func<string,term> * 
        n: int * 
        k: int -> func<string,term> * int * int 

    /// <summary>
    /// Tests the unsatisfiability of a formula using the core MESON 
    /// procedure optimized.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the number of depth limit reached trying refuting the formula with 
    /// MESON, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="puremeson-1">
    /// <code lang="fsharp">
    /// !!"P(x) /\ ~P(x)"
    /// |> puremeson
    /// </code>
    /// Evaluates to <c>1</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// </code>
    /// </example>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val puremeson: fm: formula<fol> -> int

    /// <summary>
    /// Tests the validity of a formula by negating it and splitting in 
    /// subproblems to be refuted with the core MESON procedure optimized. 
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the list of depth limits reached trying to refute the subproblems, if 
    /// the formula is valid.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="meson-1">
    /// <code lang="fsharp">
    /// !! @"exists x. exists y. forall z.
    ///     (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ///     ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    /// |> meson
    /// </code>
    /// Evaluates to <c>[8]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// Searching with depth limit 3
    /// Searching with depth limit 4
    /// Searching with depth limit 5
    /// Searching with depth limit 6
    /// Searching with depth limit 7
    /// Searching with depth limit 8
    /// </code>
    /// </example>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val meson: fm: formula<fol> -> int list