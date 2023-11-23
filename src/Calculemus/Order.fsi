// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Formulas
open Fol

/// <summary>
/// Simple term orderings including LPO.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Order = 

    val termsize: tm: term -> int

    val lexord:
      ord: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> bool
        when 'a: equality

    val lpo_gt:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    val lpo_ge:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    val weight:
      lis: 'a list -> f: 'a * n: 'b -> g: 'a * m: 'b -> bool
        when 'a: comparison and 'b: comparison