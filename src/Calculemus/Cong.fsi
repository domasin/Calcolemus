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
    /// <example id="congruent-2">
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
    /// equivalence <c>s ~ t</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The algorithm maintains a `predecessor function' <c>pfn</c> mapping 
    /// each canonical representative \(s\) of an equivalence class \(C\) to 
    /// the set of terms of which some \(s' \in C\) is an immediate subterm.
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
    /// 
    /// <example id="emerge-1">
    /// <code lang="fsharp">
    /// (unequal,undefined)
    /// |> emerge (!!!"0",!!!"1") 
    /// |> fun (Partition f,pfn) -> graph f, graph pfn
    /// </code>
    /// Evaluates to <c>([(``0``, Nonterminal ``1``); (``1``, Terminal (``1``, 2))], [(``1``, [])])</c>.
    /// </example>
    val emerge:
      s: term * t: term ->
        eqv: partition<term> *
        pfn: func<term,term list> ->
          partition<term> *
          func<term,term list>

    /// <summary>
    /// Updates a predecessor function with a new mapping for each immediate
    /// subterm of a term.
    /// </summary>
    /// 
    /// <param name="t">The input term.</param>
    /// <param name="pfn">The input `predecessor function'.</param>
    /// 
    /// <returns>
    /// The `predecessor function' updated with a new mapping for each immediate
    /// subterm of the term.
    /// </returns>
    /// 
    /// <example id="predecessors-1">
    /// <code lang="fsharp">
    /// predecessors !!!"f(0,g(1,0))" undefined
    /// |> graph
    /// </code>
    /// Evaluates to <c>[(``0``, [``f(0,g(1,0))``]); (``g(1,0)``, [``f(0,g(1,0))``])]</c>.
    /// </example>
    val predecessors:
      t: term ->
        pfn: func<term,term list> ->
        func<term,term list>

    /// <summary>
    /// Tests if a list of ground equations and inequations 
    /// is satisfiable.
    /// </summary>
    /// 
    /// <param name="fms">The input list of equations/inequations.</param>
    /// 
    /// <returns>
    /// true, if the input list of equations/inequations is satisfiable; 
    /// otherwise, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_eq: not an equation</c> when one of the input formulas is not an equation/inequation.</exception>
    /// 
    /// <example id="ccsatisfiable-1">
    /// <code lang="fsharp">
    /// !!>["m(0,1)=1";"~(m(0,1)=0)"]
    /// |> ccsatisfiable
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="ccsatisfiable-2">
    /// <code lang="fsharp">
    /// !!>["m(0,1)=1";"~(m(0,1)=1)"]
    /// |> ccsatisfiable
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="ccsatisfiable-3">
    /// <code lang="fsharp">
    /// !!>["P(0)"]
    /// |> ccsatisfiable
    /// </code>
    /// Throws <c>System.Exception: dest_eq: not an equation</c>.
    /// </example>
    val ccsatisfiable: fms: formula<fol> list -> bool

    /// <summary>
    /// Tests if an equation/inequation is valid
    /// </summary>
    /// 
    /// <param name="fm">The input equation/inequation.</param>
    /// 
    /// <returns>
    /// true, if the input equation/inequation is valid; otherwise, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_eq: not an equation</c> when the input formulas is not an equation or inequation.</exception>
    /// 
    /// <example id="ccvalid-1">
    /// <code lang="fsharp">
    /// !! @"f(f(f(f(f(c))))) = c /\ f(f(f(c))) = c
    /// ==> f(c) = c \/ f(g(c)) = g(f(c))"
    /// |> ccvalid
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="ccvalid-2">
    /// <code lang="fsharp">
    /// !! @"f(f(f(f(c)))) = c /\ f(f(c)) = c ==> f(c) = c"
    /// ==> f(c) = c \/ f(g(c)) = g(f(c))"
    /// |> ccvalid
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="ccvalid-3">
    /// <code lang="fsharp">
    /// !!"P(0)"
    /// |> ccvalid
    /// </code>
    /// Throws <c>System.Exception: dest_eq: not an equation</c>.
    /// </example>
    val ccvalid: fm: formula<fol> -> bool