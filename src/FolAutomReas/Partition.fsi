// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

open FolAutomReas.Lib.Fpf

/// <summary>
/// Implements Union-Find Algorithm. 
/// </summary>
module Partition =

    type pnode<'a> =
      | Nonterminal of 'a
      | Terminal of 'a * int

    type partition<'a> = 
      | Partition of func<'a,pnode<'a>>

    val terminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

    val tryterminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

    val canonize: ptn: partition<'a> -> a: 'a -> 'a when 'a: comparison

    val equivalent: eqv: partition<'a> -> a: 'a -> b: 'a -> bool when 'a: comparison

    val equate:
      a: 'a * b: 'a -> ptn: partition<'a> -> partition<'a> when 'a: comparison

    val unequal: partition<'a>

    val equated: partition<'a> -> 'a list when 'a: comparison

    