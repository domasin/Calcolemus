// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Knuth-Bendix completion.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Completion = 

    /// <summary>
    /// Replaces the variables in a pair of two given formulas by schematic 
    /// variables of the form <c>xn</c>.
    /// </summary>
    /// 
    /// <param name="fm1">The first input formula.</param>
    /// <param name="fm2">The second input formula.</param>
    /// 
    /// <returns>
    /// The pair of the two input formulas with the variables renamed by 
    /// schematic variables of the form <c>xn</c>.
    /// </returns>
    /// 
    /// <example id="renamepair-1">
    /// <code lang="fsharp">
    /// (!!"i(x) * y = z", !!"z * t = y")
    /// |> renamepair
    /// </code>
    /// Evaluates to <c>(`i(x0) * x1 = x2`, `x5 * x3 = x4`)</c>.
    /// </example>
    /// 
    /// <category index="1">Critical pairs</category>
    val renamepair:
      fm1: formula<fol> * fm2: formula<fol> ->
        formula<fol> * formula<fol>

    /// <summary>
    /// Auxiliary function used to define 
    /// <see cref='M:Calculemus.Completion.overlaps``1'/>.
    /// </summary>
    /// 
    /// <category index="1">Critical pairs</category>
    val listcases:
      fn: ('a -> ('b -> 'a -> 'c) -> 'd list) ->
        rfn: ('b -> 'a list -> 'c) -> lis: 'a list -> acc: 'd list -> 'd list

    /// <summary>
    /// Finds all possibile `overlaps' of an equation with a term.
    /// </summary>
    /// 
    /// <remarks>
    /// Defines all ways of overlapping an equation <c>l = r</c> with another 
    /// term <c>tm</c>, where the additional argument <c>rfn</c> is used to 
    /// create each overall critical pair from an instantiation <c>i</c>.
    /// </remarks>
    /// 
    /// <param name="l">The LHS of the input equation.</param>
    /// <param name="r">The RHS of the input equation.</param>
    /// <param name="tm">The input term.</param>
    /// <param name="rfn">The function that applied to the resulting instantiation gives the critical pair arising from that overlap.</param>
    /// 
    /// <returns>
    /// All possibile overlaps of the equation <c>l = r</c> with the <c>tm</c>.
    /// </returns>
    /// 
    /// <example id="overlaps-1">
    /// <code lang="fsharp">
    /// overlaps 
    ///   (!!!"f(f(x))",!!!"g(x)") !!!"f(f(y))" 
    ///   (fun i t -> subst i (mk_eq t !!!"g(y)"))
    /// </code>
    /// Evaluates to <c>[`f(g(x)) = g(f(x))`; `g(y) = g(y)`]</c>.
    /// </example>
    /// 
    /// <category index="1">Critical pairs</category>
    val overlaps:
      l: term * r: term ->
        tm: term ->
        rfn: (func<string,term> -> term -> 'a) -> 'a list

    /// <summary>
    /// Auxiliary function to define 
    /// <see cref='M:Calculemus.Completion.critical_pairs'/>.
    /// </summary>
    /// 
    /// <param name="eq1">The first input equation.</param>
    /// <param name="eq2">The second input equation.</param>
    /// 
    /// <returns>
    /// The list of all the critical pairs of the input equations.
    /// </returns>
    /// 
    /// <category index="1">Critical pairs</category>
    val crit1:
      eq1: formula<fol> ->
        eq2: formula<fol> -> formula<fol> list

    /// <summary>
    /// Generate all critical pairs between two equations.
    /// </summary>
    /// 
    /// <param name="eq1">The first input equation.</param>
    /// <param name="eq2">The second input equation.</param>
    /// 
    /// <returns>
    /// The list of all the critical pairs of the input equations.
    /// </returns>
    /// 
    /// <example id="critical_pairs-1">
    /// <code lang="fsharp">
    /// let eq = !!"f(f(x)) = g(x)" 
    /// critical_pairs eq eq
    /// </code>
    /// Evaluates to <c>[`f(g(x0)) = g(f(x0))`; `g(x1) = g(x1)`]</c>.
    /// </example>
    /// 
    /// <category index="1">Critical pairs</category>
    val critical_pairs:
      eq1: formula<fol> ->
        eq2: formula<fol> -> formula<fol> list

    /// <summary>
    /// Normalizes and orients a given equation w.r.t a given set of equations 
    /// based on the given ordering.
    /// </summary>
    /// 
    /// <param name="ord">The given ordering.</param>
    /// <param name="eqs">The given set of equations.</param>
    /// <param name="atm">The input equation to normalize and orient.</param>
    /// 
    /// <returns>
    /// The pair of LHS and RHS of the normalized and oriented equation.
    /// </returns>
    /// 
    /// <example id="normalize_and_orient-1">
    /// <code lang="fsharp">
    /// let eqs = !!>[
    ///         "1 * x = x"; 
    ///         "i(x) * x = 1"; 
    ///         "(x * y) * z = x * y * z"
    /// ]
    /// 
    /// let ord = lpo_ge (weight ["1"; "*"; "i"])
    /// 
    /// !!"i(y) * i(x) = i(x * (1 * y))"
    /// |> normalize_and_orient ord eqs 
    /// </code>
    /// Evaluates to <c>(``i(x * y)``, ``i(y) * i(x)``)</c>.
    /// </example>
    /// 
    /// <category index="2">Completion</category>
    val normalize_and_orient:
      ord: (term -> term -> bool) ->
        eqs: formula<fol> list ->
        atm: formula<fol> -> term * term

    /// <summary>
    /// Auxiliary function to print the current status of the completion 
    /// process.
    /// </summary>
    /// 
    /// <param name="eqs">The current set of equations.</param>
    /// <param name="def">The current list of deferred critical pairs.</param>
    /// <param name="crs">The current list of critical pairs.</param>
    /// <param name="eqs0">The initial set of equations.</param>
    /// 
    /// <category index="2">Completion</category>
    val status:
      eqs: 'a list * def: 'b list * crs: 'c list -> eqs0: 'a list -> unit
        when 'a: equality

    /// <summary>
    /// Completes a set of equations transforming it in a confluent term 
    /// rewriting system, if the procedure has success.
    /// </summary>
    /// 
    /// <remarks>
    /// It is a semi-decision algorithm.
    /// </remarks>
    /// 
    /// <param name="ord">The given ordering: actually an LPO's reflective version is expected.</param>
    /// <param name="eqs">The given set of equations.</param>
    /// <param name="def">The accumulator list of deferred critical pairs initially empty.</param>
    /// <param name="crits">The critical pairs, initially from the <c>eqs</c> equations.</param>
    /// 
    /// <returns>
    /// The completed set of equations that defines a confluent term 
    /// rewriting system, if the procedure has success.
    /// </returns>
    /// 
    /// <note>
    /// Prints to the <c>stdout</c> diagnostic informations of the current 
    /// completion status.
    /// </note>
    /// 
    /// <example id="complete-1">
    /// <code lang="fsharp">
    /// let eqs = !!>[
    ///         "1 * x = x"; 
    ///         "i(x) * x = 1"; 
    ///         "(x * y) * z = x * y * z"
    /// ]
    /// 
    /// let ord = lpo_ge (weight ["1"; "*"; "i"])
    /// 
    /// (eqs,[],unions(allpairs critical_pairs eqs eqs))
    /// |> complete ord
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// ["`i(x4 * x5) = i(x5) * i(x4)`"; "`x1 * i(x5 * x1) = i(x5)`";
    ///  "`i(x4) * x1 * i(x3 * x1) = i(x4) * i(x3)`";
    ///  "`x1 * i(i(x4) * i(x3) * x1) = x3 * x4`";
    ///  "`i(x3 * x5) * x0 = i(x5) * i(x3) * x0`";
    ///  "`i(x4 * x5 * x6 * x3) * x0 = i(x3) * i(x4 * x5 * x6) * x0`";
    ///  "`i(x0 * i(x1)) = x1 * i(x0)`"; "`i(i(x2 * x1) * x2) = x1`";
    ///  "`i(i(x4) * x2) * x0 = i(x2) * x4 * x0`"; "`x1 * i(x2 * x1) * x2 = 1`";
    ///  "`x1 * i(i(x4 * x5) * x1) * x3 = x4 * x5 * x3`";
    ///  "`i(x3 * i(x1 * x2)) = x1 * x2 * i(x3)`";
    ///  "`i(i(x3 * i(x1 * x2)) * i(x5 * x6)) * x1 * x2 * x0 = x5 * x6 * x3 * x0`";
    ///  "`x1 * x2 * i(x1 * x2) = 1`"; "`x2 * x3 * i(x2 * x3) * x1 = x1`";
    ///  "`i(x3 * x4) * x3 * x1 = i(x4) * x1`";
    ///  "`i(x1 * x3 * x4) * x1 * x3 * x4 * x0 = x0`";
    ///  "`i(x1 * i(x3)) * x1 * x4 = x3 * x4`";
    ///  "`i(i(x5 * x2) * x5) * x0 = x2 * x0`";
    ///  "`i(x4 * i(x1 * x2)) * x4 * x0 = x1 * x2 * x0`"; "`i(i(x1)) = x1`";
    ///  "`i(1) = 1`"; "`x0 * i(x0) = 1`"; "`x0 * i(x0) * x3 = x3`";
    ///  "`i(x2 * x3) * x2 * x3 * x1 = x1`"; "`x1 * 1 = x1`"; "`i(1) * x1 = x1`";
    ///  "`i(i(x0)) * x1 = x0 * x1`"; "`i(x1) * x1 * x2 = x2`"; "`1 * x = x`";
    ///  "`i(x) * x = 1`"; "`(x * y) * z = x * y * z`"]
    /// </code>
    /// and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 4 equations and 8 pending critical pairs + 0 deferred
    /// 5 equations and 12 pending critical pairs + 0 deferred
    /// ...
    /// 32 equations and 0 pending critical pairs + 1 deferred
    /// 32 equations and 0 pending critical pairs + 0 deferred
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Completion</category>
    val complete:
      ord: (term -> term -> bool) ->
        eqs: formula<fol> list * def: formula<fol> list *
        crits: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Applies the interreduction refinement to an input set of equations.
    /// </summary>
    /// 
    /// <remarks>
    /// <ul>
    /// <li>
    /// discards any equation whose LHS is reducible by any of the others 
    /// (excluding itself);
    /// </li>
    /// <li>
    /// reduces the RHS of any equation with all the equations (including 
    /// itself).
    /// </li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="dun">The accumulator, initially empty, of the equations to be reduced</param>
    /// <param name="eqs">The input set of equations.</param>
    /// 
    /// <returns>
    /// The set of equations interreduced.
    /// </returns>
    /// 
    /// <example id="complete-1">
    /// <code lang="fsharp">
    /// !!>[
    ///    "i(x4 * x5) = i(x5) * i(x4)"; "x1 * i(x5 * x1) = i(x5)";
    ///    "i(x4) * x1 * i(x3 * x1) = i(x4) * i(x3)";
    ///    "x1 * i(i(x4) * i(x3) * x1) = x3 * x4";
    ///    "i(x3 * x5) * x0 = i(x5) * i(x3) * x0";
    ///    "i(x4 * x5 * x6 * x3) * x0 = i(x3) * i(x4 * x5 * x6) * x0";
    ///    "i(x0 * i(x1)) = x1 * i(x0)"; "i(i(x2 * x1) * x2) = x1";
    ///    "i(i(x4) * x2) * x0 = i(x2) * x4 * x0"; "x1 * i(x2 * x1) * x2 = 1";
    ///    "x1 * i(i(x4 * x5) * x1) * x3 = x4 * x5 * x3";
    ///    "i(x3 * i(x1 * x2)) = x1 * x2 * i(x3)";
    ///    "i(i(x3 * i(x1 * x2)) * i(x5 * x6)) * x1 * x2 * x0 = x5 * x6 * x3 * x0";
    ///    "x1 * x2 * i(x1 * x2) = 1"; "x2 * x3 * i(x2 * x3) * x1 = x1";
    ///    "i(x3 * x4) * x3 * x1 = i(x4) * x1";
    ///    "i(x1 * x3 * x4) * x1 * x3 * x4 * x0 = x0";
    ///    "i(x1 * i(x3)) * x1 * x4 = x3 * x4";
    ///    "i(i(x5 * x2) * x5) * x0 = x2 * x0";
    ///    "i(x4 * i(x1 * x2)) * x4 * x0 = x1 * x2 * x0"; "i(i(x1)) = x1";
    ///    "i(1) = 1"; "x0 * i(x0) = 1"; "x0 * i(x0) * x3 = x3";
    ///    "i(x2 * x3) * x2 * x3 * x1 = x1"; "x1 * 1 = x1"; "i(1) * x1 = x1";
    ///    "i(i(x0)) * x1 = x0 * x1"; "i(x1) * x1 * x2 = x2"; "1 * x = x";
    ///    "i(x) * x = 1"; "(x * y) * z = x * y * z"
    ///]
    ///|> interreduce []
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [
    ///   "`i(x4 * x5) = i(x5) * i(x4)`"; 
    ///   "`i(i(x1)) = x1`"; 
    ///   "`i(1) = 1`"; 
    ///   "`x0 * i(x0) = 1`";
    ///   "`x0 * i(x0) * x3 = x3`"; 
    ///   "`x1 * 1 = x1`"; 
    ///   "`i(x1) * x1 * x2 = x2`"; 
    ///   "`1 * x = x`"; 
    ///   "`i(x) * x = 1`"; 
    ///   "`(x * y) * z = x * y * z`""
    /// ]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Interreduction</category>
    val interreduce:
      dun: formula<fol> list ->
        eqs: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Completes a set of equations transforming it in a confluent term 
    /// rewriting system, if the procedure has success. It also simplifies the 
    /// result applying interreduction.
    /// </summary>
    /// 
    /// <param name="wts">The list defining the function symbols' ordering.</param>
    /// <param name="eqs">The given set of equations.</param>
    /// 
    /// <returns>
    /// The completed and interreduced set of equations, if the procedure has 
    /// success.
    /// </returns>
    /// 
    /// <note>
    /// Prints to the <c>stdout</c> diagnostic informations of the current 
    /// completion status.
    /// </note>
    /// 
    /// <example id="complete_and_simplify-1">
    /// <code lang="fsharp">
    /// [!!"i(a) * (a * b) = b"]
    /// |> complete_and_simplify ["1"; "*"; "i"]
    /// </code>
    /// Evaluates to 
    /// <c>[`x0 * i(x0) * x3 = x3`; `i(i(x0)) * x1 = x0 * x1`; `i(a) * a * b = b`]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 2 equations and 4 pending critical pairs + 0 deferred
    /// 3 equations and 9 pending critical pairs + 0 deferred
    /// 3 equations and 0 pending critical pairs + 0 deferred
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Interreduction</category>
    val complete_and_simplify:
      wts: string list ->
        eqs: formula<fol> list -> formula<fol> list
