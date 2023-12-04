// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Some decidable subsets of first-order logic.
/// </summary>
/// 
/// <category index="6">Decidable subsets and theories</category>
module Decidable = 

    /// <summary>
    /// Tests the validity of a formula in the AE fragment.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input AE formula is valid; otherwise, if the input AE 
    /// formula is invalid, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Not decidable</c> when the input formula doesn't belong to the AE fragment.</exception>
    /// 
    /// <example id="aedecide-1">
    /// <code lang="fsharp">
    /// !! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
    /// (forall u v w x y z.
    /// P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) &lt;=&gt; P(u,z,v)))
    /// ==> forall a b c. P(a,b,c) ==> P(b,a,c)"
    /// |> aedecide
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="aedecide-2">
    /// <code lang="fsharp">
    /// !! @"forall x. f(x) = 0"
    /// |> aedecide
    /// </code>
    /// Throws <c>System.Exception: Not decidable</c>.
    /// </example>
    /// 
    /// <category index="1">The AE fragment</category>
    val aedecide: fm: formula<fol> -> bool

    val separate:
      x: string -> cjs: formula<fol> list -> formula<fol>

    val pushquant:
      x: string -> p: formula<fol> -> formula<fol>

    val miniscope: fm: formula<fol> -> formula<fol>

    val wang: fm: formula<fol> -> bool

    val atom: p: string -> x: string -> formula<fol>

    val premiss_A: p: string * q: string -> formula<fol>

    val premiss_E: p: string * q: string -> formula<fol>

    val premiss_I: p: string * q: string -> formula<fol>

    val premiss_O: p: string * q: string -> formula<fol>

    val anglicize_premiss: fm: formula<fol> -> string

    val anglicize_syllogism: formula<fol> -> string

    val all_possible_syllogisms: formula<fol> list

    val all_possible_syllogisms': formula<fol> list

    val alltuples: n: int -> l: 'a list -> 'a list list

    val allmappings:
      dom: 'a list -> ran: 'b list -> ('a -> 'b) list when 'a: equality

    val alldepmappings:
      dom: ('a * 'b) list -> ran: ('b -> 'c list) -> ('a -> 'c) list
        when 'a: equality

    val allfunctions:
      dom: 'a list -> n: int -> ('a list -> 'a) list when 'a: equality

    val allpredicates:
      dom: 'a list -> n: int -> ('a list -> bool) list when 'a: equality

    val decide_finite: n: int -> fm: formula<fol> -> bool

    val limmeson:
      n: int ->
        fm: formula<fol> -> func<string,term> * int * int

    val limited_meson:
      n: int ->
        fm: formula<fol> ->
        (func<string,term> * int * int) list

    val decide_fmp: fm: formula<fol> -> bool

    val decide_monadic: fm: formula<fol> -> bool