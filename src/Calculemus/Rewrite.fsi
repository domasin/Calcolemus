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
/// Rewriting.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Rewrite = 

    val rewrite1: eqs: formula<fol> list -> t: term -> term

    val rewrite: eqs: formula<fol> list -> tm: term -> term