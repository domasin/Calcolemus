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

    val renamepair:
      fm1: formula<fol> * fm2: formula<fol> ->
        formula<fol> * formula<fol>

    val listcases:
      fn: ('a -> ('b -> 'a -> 'c) -> 'd list) ->
        rfn: ('b -> 'a list -> 'c) -> lis: 'a list -> acc: 'd list -> 'd list

    val overlaps:
      l: term * r: term ->
        tm: term ->
        rfn: (func<string,term> -> term -> 'a) -> 'a list

    val crit1:
      formula<fol> ->
        formula<fol> -> formula<fol> list

    val critical_pairs:
      fma: formula<fol> ->
        fmb: formula<fol> -> formula<fol> list

    val normalize_and_orient:
      ord: (term -> term -> bool) ->
        eqs: formula<fol> list ->
        formula<fol> -> term * term

    val status:
      eqs: 'a list * def: 'b list * crs: 'c list -> eqs0: 'a list -> unit
        when 'a: equality

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