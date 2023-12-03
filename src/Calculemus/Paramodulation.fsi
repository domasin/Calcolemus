// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Paramodulation.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Paramodulation = 

    val overlapl:
      l: term * r: term ->
        fm: formula<fol> ->
        rfn: (func<string,term> -> formula<fol> -> 'a) ->
        'a list

    val overlapc:
      l: term * r: term ->
        cl: formula<fol> list ->
        rfn: (func<string,term> -> formula<fol> list -> 'a) ->
        acc: 'a list -> 'a list

    val paramodulate:
      pcl: formula<fol> list ->
        ocl: formula<fol> list -> formula<fol> list list

    val para_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    val paraloop:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    val pure_paramodulation: fm: formula<fol> -> bool

    val paramodulation: fm: formula<fol> -> bool list