// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Formulas
open Fol

/// <summary>
/// Quantifier elimination basics.
/// </summary>
/// 
/// <category index="6">Decidable subsets and theories</category>
module Qelim = 

    /// <summary>
    /// Accepts the core quantifier elimination procedure and generalizes it 
    /// slightly to work for \(\exists x.\ p\) where \(p\) is any conjunction 
    /// of literals, some perhaps not involving \(x\).
    /// </summary>
    /// 
    /// <param name="bfn">The core quantifier elimination procedure.</param>
    /// <param name="x">The existentially quantified variable.</param>
    /// <param name="p">The body of the existential formula.</param>ù
    /// 
    /// <returns>
    /// The equivalent formula of \(\exists x.\ p\) with the existential 
    /// quantifier applied only to the conjuncts in \(p\) that involve \(x\).
    /// </returns>
    /// 
    /// <category index="1">Quantifier elimination</category>
    val qelim:
      bfn: (formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>

    /// <summary>
    /// Main quantifier elimination function
    /// </summary>
    /// 
    /// <param name="afn">A function to convert inequalities not included in the core language.</param>
    /// <param name="nfn">A disjunctive normal form function.</param>
    /// <param name="qfn">The core quantifier elimination function.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <category index="1">Quantifier elimination</category>
    val lift_qelim:
      afn: (string list -> formula<fol> -> formula<fol>)  ->
        nfn: (formula<fol> -> formula<fol>) ->
        qfn: (string list -> formula<fol> -> formula< fol>) ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Returns a disjunctive normal form of the input formula applying a given 
    /// `literal modification' function to the literals.
    /// </summary>
    /// 
    /// <param name="lfn">The literal modification function.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A formula in disjunctive normal form equivalent to the input formula 
    /// and with literals modified based on the literal modification function.
    /// </returns>
    /// 
    /// <example id="cnnf-1">
    /// <code lang="fsharp">
    /// !!"~(s = t) /\ ~(s &lt; t)"
    /// |> cnnf lfn_dlo
    /// </code>
    /// Evaluates to <c>`(s &lt; t \/ t &lt; s) /\ (s = t \/ t &lt; s)`</c>.
    /// </example>
    /// 
    /// <category index="1">Quantifier elimination</category>
    val cnnf:
      lfn: (formula<fol> -> formula<fol>) ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Literal modification function to remove negated dlo literals.
    /// </summary>
    /// 
    /// <param name="fm">The input literal.</param>
    /// <returns>
    /// A formula equivalent to the input literal without negation, if the 
    /// input formula is of the form <c>~(s &lt; t)</c> or <c>~(s = t)</c>; 
    /// otherwise, the input formula unchanged.
    /// </returns>
    /// 
    /// <example id="lfn_dlo-1">
    /// <code lang="fsharp">
    /// lfn_dlo !!"~s &lt; t"
    /// </code>
    /// Evaluates to <c>`s = t \/ t &lt; s`</c>.
    /// </example>
    /// 
    /// <example id="lfn_dlo-2">
    /// <code lang="fsharp">
    /// lfn_dlo !!"~s = t"
    /// </code>
    /// Evaluates to <c>`s &lt; t \/ t &lt; s`</c>.
    /// </example>
    /// 
    /// <example id="lfn_dlo-3">
    /// <code lang="fsharp">
    /// lfn_dlo !!"~s = t /\ ~s &lt; t"
    /// </code>
    /// Evaluates to <c>`~s = t /\ ~s &lt; t`</c>.
    /// </example>
    /// 
    /// <category index="2">Example: dense linear orders</category>
    val lfn_dlo: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Basic quantifier elimination function for dense linear orders.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <category index="2">Example: dense linear orders</category>
    val dlobasic: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Converts inequality relations not included in the core language into 
    /// equivalent admitted relations.
    /// </summary>
    /// 
    /// <param name="vars">Unused params required by <see cref='M:Calculemus.Qelim.lift_qelim'/> signature.</param>
    /// <param name="fm">The input literal.</param>
    /// 
    /// <returns>
    /// A formula equivalent to the input literal with inequality relations not 
    /// included in the language converted into admitted ones.
    /// </returns>
    /// 
    /// <example id="afn_dlo-1">
    /// <code lang="fsharp">
    /// afn_dlo [] !!"s &lt;= t"
    /// </code>
    /// Evaluates to <c>`~t &lt; s`</c>.
    /// </example>
    /// 
    /// <category index="2">Example: dense linear orders</category>
    val afn_dlo:
      vars: 'a -> fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Quantifier elimination function for dense linear orders formulas.
    /// </summary>
    /// 
    /// <param name="fm">The input dlo formula.</param>
    /// <returns>
    /// A quantifier free formula equivalent to input one (provided this is a 
    /// dlo formula).
    /// </returns>
    /// 
    /// <example id="quelim_dlo-1">
    /// <code lang="fsharp">
    /// quelim_dlo !!"forall x y. exists z. z &lt; x /\ z &lt; y"
    /// </code>
    /// Evaluates to <c>`true`</c>.
    /// </example>
    /// 
    /// <example id="quelim_dlo-2">
    /// <code lang="fsharp">
    /// quelim_dlo !!"exists z. z &lt; x /\ z &lt; y"
    /// </code>
    /// Evaluates to <c>`true`</c>.
    /// </example>
    /// 
    /// <example id="quelim_dlo-3">
    /// <code lang="fsharp">
    /// quelim_dlo !!"exists z. x &lt; z /\ z &lt; y"
    /// </code>
    /// Evaluates to <c>`x &lt; y`</c>.
    /// </example>
    /// 
    /// <example id="quelim_dlo-4">
    /// <code lang="fsharp">
    /// quelim_dlo !!"(forall x. x &lt; a ==> x &lt; b)"
    /// </code>
    /// Evaluates to <c>`~(b &lt; a \/ b &lt; a)`</c>.
    /// </example>
    /// 
    /// <category index="2">Example: dense linear orders</category>
    val quelim_dlo: fm: formula<fol> -> formula<fol>