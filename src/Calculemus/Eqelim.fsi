// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Equality elimination: Brand transform etc.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Eqelim = 

    val modify_S:
      cl: formula<fol> list -> formula<fol> list list

    val modify_T:
      cl: formula<fol> list -> formula<fol> list

    val is_nonvar: _arg1: term -> bool

    val find_nestnonvar: tm: term -> term

    val find_nvsubterm: fm: formula<fol> -> term

    val replacet: rfn: func<term,term> -> tm: term -> term

    val replace:
      rfn: func<term,term> ->
        (formula<fol> -> formula<fol>)

    val emodify:
      fvs: string list ->
        cls: formula<fol> list -> formula<fol> list

    val modify_E:
      cls: formula<fol> list -> formula<fol> list

    val brand:
      cls: formula<fol> list list ->
        formula<fol> list list

    val bpuremeson: fm: formula<fol> -> int

    val bmeson: fm: formula<fol> -> int list

    val emeson: fm: formula<fol> -> int list