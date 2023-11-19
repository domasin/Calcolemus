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

    val subterms: tm: term -> term list

    val congruent:
      eqv: partition<term> ->
        s: term * t: term -> bool

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