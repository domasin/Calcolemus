// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Formulas
open Fol

/// <summary>
/// Skolemizing a set of 
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Skolems = 

    /// <summary>
    /// Renames all the function symbols in the term.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The term with an <c>old_</c> prefix added to each function symbols.
    /// </return>
    /// 
    /// 
    val rename_term: tm: term -> term

    val rename_form: (formula<fol> -> formula<fol>)

    val skolems:
      fms: formula<fol> list ->
        corr: string list -> formula<fol> list * string list

    val skolemizes:
      fms: formula<fol> list -> formula<fol> list