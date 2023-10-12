// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.Fpf

open Formulas
open Prop

/// <summary>
/// The Davis-Putnam and the Davis-Putnam-Loveland-Logemann procedures.
/// </summary>
/// 
/// <category index="3">Propositional logic</category>
module DP =

    /// <summary>
    /// Checks if <c>clauses</c> contain one litterals.
    /// </summary>
    val containOneLitterals: clauses: 'a list list -> bool

    val one_literal_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    val containPureLitterals:
      clauses: formula<'a> list list -> bool when 'a: comparison

    val affirmative_negative_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    val resolve_on:
      p: formula<'a> ->
        clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    val resolution_blowup:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    val resolution_rule:
      clauses: formula<'a> list list -> formula<'a> list list
        when 'a: comparison

    val dp: clauses: formula<'a> list list -> bool when 'a: comparison

    val dpsat: fm: formula<prop> -> bool

    val dptaut: fm: formula<prop> -> bool

    val posneg_count:
      cls: formula<'a> list list -> l: formula<'a> -> int
        when 'a: equality

    val dpll: clauses: formula<'a> list list -> bool when 'a: comparison

    val dpllsat: fm: formula<prop> -> bool

    val dplltaut: fm: formula<prop> -> bool

    type trailmix =
        | Guessed
        | Deduced

    val unassigned:
      (formula<'a> list list ->
         (formula<'a> * 'b) list -> formula<'a> list)
        when 'a: comparison

    val unit_subpropagate:
      cls: formula<'a> list list *
      fn: func<formula<'a>,unit> *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * func<formula<'a>,unit> *
        (formula<'a> * trailmix) list when 'a: comparison

    val unit_propagate:
      cls: formula<'a> list list *
      trail: (formula<'a> * trailmix) list ->
        formula<'a> list list * (formula<'a> * trailmix) list
        when 'a: comparison

    val backtrack: trail: ('a * trailmix) list -> ('a * trailmix) list

    val dpli:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    val dplisat: fm: formula<prop> -> bool

    val dplitaut: fm: formula<prop> -> bool

    val backjump:
      cls: formula<'a> list list ->
        p: formula<'a> ->
        trail: (formula<'a> * trailmix) list ->
        (formula<'a> * trailmix) list when 'a: comparison

    val dplb:
      cls: formula<'a> list list ->
        trail: (formula<'a> * trailmix) list -> bool when 'a: comparison

    val dplbsat: fm: formula<prop> -> bool

    val dplbtaut: fm: formula<prop> -> bool