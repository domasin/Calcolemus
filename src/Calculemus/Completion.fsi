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
    /// <category index="2">Completion</category>
    val status:
      eqs: 'a list * def: 'b list * crs: 'c list -> eqs0: 'a list -> unit
        when 'a: equality

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">Completion</category>
    val complete:
      ord: (term -> term -> bool) ->
        eqs: formula<fol> list * def: formula<fol> list *
        crits: formula<fol> list -> formula<fol> list

    val interreduce:
      dun: formula<fol> list ->
        eqs: formula<fol> list -> formula<fol> list

    val complete_and_simplify:
      wts: string list ->
        eqs: formula<fol> list -> formula<fol> list

    val eqs: formula<fol> list

    val wts: string list

    val ord: (term -> term -> bool)

    val def: 'a list

    val crits: formula<fol> list

    val complete1:
      ord: (term -> term -> bool) ->
        eqs: formula<fol> list * def: formula<fol> list *
        crits: formula<fol> list ->
          formula<fol> list * formula<fol> list *
          formula<fol> list