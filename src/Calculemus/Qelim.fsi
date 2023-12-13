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

    val qelim:
      bfn: (formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>

    val lift_qelim:
      afn: (string list -> formula<fol> -> formula<fol>)  ->
        nfn: (formula<fol> -> formula<fol>) ->
        qfn: (string list -> formula<fol> -> formula< fol>) ->
        (formula<fol> -> formula<fol>)

    val cnnf:
      lfn: (formula<fol> -> formula<fol>) ->
        (formula<fol> -> formula<fol>)

    val lfn_dlo: fm: formula<fol> -> formula<fol>

    val dlobasic: fm: formula<fol> -> formula<fol>

    val afn_dlo:
      vars: 'a -> fm: formula<fol> -> formula<fol>

    val quelim_dlo: (formula<fol> -> formula<fol>)