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
    val qelim:
      bfn: (formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>

    val lift_qelim:
      afn: (string list -> formula<fol> -> formula<fol>)  ->
        nfn: (formula<fol> -> formula<fol>) ->
        qfn: (string list -> formula<fol> -> formula< fol>) ->
        (formula<fol> -> formula<fol>)

    /// <summary>
    /// Returns a negation normal form of the input formula applying a given 
    /// `literal modification' function to the literals.
    /// </summary>
    val cnnf:
      lfn: (formula<fol> -> formula<fol>) ->
        fm: formula<fol> -> formula<fol>

    val lfn_dlo: fm: formula<fol> -> formula<fol>

    val dlobasic: fm: formula<fol> -> formula<fol>

    val afn_dlo:
      vars: 'a -> fm: formula<fol> -> formula<fol>

    val quelim_dlo: (formula<fol> -> formula<fol>)