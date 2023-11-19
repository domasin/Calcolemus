// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Lib.Fpf
open Lib.Partition
open Formulas
open Fol

/// <summary>
/// Congruence closure.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Cong = 

    /// <summary>
    /// Returns all subterms of a term.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The list of subterms of the input term.
    /// </returns>
    /// 
    /// <example id="subterms-1">
    /// <code lang="fsharp">
    /// subterms !!!"f(g(x),y)"
    /// </code>
    /// Evaluates to <c>[``x``; ``y``; ``f(g(x),y)``; ``g(x)``]</c>.
    /// </example>
    val subterms: tm: term -> term list

    /// <summary>
    /// Checks if <c>s</c> and <c>t</c> should be equated by a 1-step 
    /// congruence.
    /// </summary>
    /// 
    /// <param name="eqv">The input congruence.</param>
    /// <param name="s">The first input term.</param>
    /// <param name="t">The second input term.</param>
    /// 
    /// <returns>
    /// true, if all the the input terms immediate subterms are already 
    /// equivalent; otherwise, false.
    /// </returns>
    /// 
    /// <example id="congruent-1">
    /// <code lang="fsharp">
    /// congruent 
    ///     (equate (!!!"2",!!!"4")unequal) 
    ///     (!!!"f(4)", !!!"f(2)")
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="congruent-1">
    /// <code lang="fsharp">
    /// congruent 
    ///     (equate (!!!"2",!!!"4")unequal) 
    ///     (!!!"f(4)", !!!"f(3)")
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val congruent:
      eqv: partition<term> ->
        s: term * t: term -> bool

    /// <summary>
    /// Extends the terms congruence relation <c>eqv</c> with the new 
    /// congruence <c>s ~ t</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The algorithm maintains a `predecessor function' <c>pfn</c> mapping 
    /// each canonical representative s of an equivalence class C to the 
    /// set of terms of which some s' ? C is an immediate subterm.
    /// </remarks>
    /// 
    /// <param name="s">The first input term.</param>
    /// <param name="t">The second input term.</param>
    /// <param name="eqv">The input congruence.</param>
    /// <param name="pfn">The input `predecessor function'.</param>
    /// 
    /// <returns>
    /// The extended congruence relation together with the updated predecessor 
    /// function.
    /// </returns>
    val emerge:
      s: term * t: term ->
        eqv: partition<term> *
        pfn: func<term,term list> ->
          partition<term> *
          func<term,term list>

    val predecessors:
      t: term ->
        pfn: func<term,term list> ->
        func<term,term list>

    val ccsatisfiable: fms: formula<fol> list -> bool

    val ccvalid: fm: formula<fol> -> bool