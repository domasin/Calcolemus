// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf
open Lib.Partition

open Formulas
open Prop

/// <summary>
/// Stalmarck method.
/// </summary>
/// 
/// <category index="3">Propositional logic</category>
module Stal = 

    val triplicate:
      fm: formula<prop> ->
        formula<prop> * formula<prop> list

    val atom: lit: formula<'a> -> formula<'a>

    val align:
      p: formula<'a> * q: formula<'a> ->
        formula<'a> * formula<'a> when 'a: comparison

    val equate2:
      p: formula<'a> * q: formula<'a> ->
        eqv: partition<formula<'a>> ->
        partition<formula<'a>> when 'a: comparison

    val irredundant:
      rel: partition<formula<'a>> ->
        eqs: (formula<'a> * formula<'a>) list ->
        (formula<'a> * formula<'a>) list when 'a: comparison

    val consequences:
      formula<'a> * formula<'a> ->
        fm: formula<'a> ->
        eqs: (formula<'a> * formula<'a>) list ->
        (formula<'a> * formula<'a>) list when 'a: comparison

    val triggers:
      fm: formula<'a> ->
        ((formula<'a> * formula<'a>) *
         (formula<'a> * formula<'a>) list) list
        when 'a: comparison

    val trigger:
      (formula<prop> ->
         ((formula<prop> * formula<prop>) *
          (formula<prop> * formula<prop>) list) list)

    val relevance:
      trigs: (('a * 'a) * 'b) list -> func<'a,(('a * 'a) * 'b) list>
        when 'a: comparison and 'b: comparison

    val equatecons:
      p0: formula<'a> * q0: formula<'a> ->
        partition<formula<'a>> *
        func<formula<'a>,('b * 'c list) list> ->
          'c list *
          (partition<formula<'a>> *
           func<formula<'a>,('b * 'c list) list>)
            when 'a: comparison and 'b: comparison and 'c: comparison

    val zero_saturate:
      partition<formula<'a>> *
      func<formula<'a>,
                   ('b * (formula<'a> * formula<'a>) list) list>  ->
        assigs: (formula<'a> * formula<'a>) list ->
        partition<formula<'a>> *
        func<formula<'a>,
                     ('b * (formula<'a> * formula<'a>) list) list>
            when 'a: comparison and 'b: comparison

    val zero_saturate_and_check:
      partition<formula<'a>> *
      func<formula<'a>,
                   ('b * (formula<'a> * formula<'a>) list) list>  ->
        trigs: (formula<'a> * formula<'a>) list ->
        partition<formula<'a>> *
        func<formula<'a>,
                     ('b * (formula<'a> * formula<'a>) list) list>
            when 'a: comparison and 'b: comparison

    val truefalse:
      pfn: partition<formula<'a>> -> bool
        when 'a: comparison

    val equateset:
      s0: formula<'a> list ->
        partition<formula<'a>> *
        func<formula<'a>,('b * 'c list) list> ->
          partition<formula<'a>> *
          func<formula<'a>,('b * 'c list) list>
            when 'a: comparison and 'b: comparison and 'c: comparison

    val inter:
      els: formula<'a> list ->
        partition<formula<'a>> * 'b ->
          partition<formula<'a>> * 'c ->
            rev1: func<formula<'a>,formula<'a> list> ->
            rev2: func<formula<'a>,formula<'a> list> ->
            partition<formula<'a>> *
            func<formula<'a>,('d * 'e list) list> ->
              partition<formula<'a>> *
              func<formula<'a>,('d * 'e list) list>
                when 'a: comparison and 'd: comparison and 'e: comparison

    val reverseq:
      domain: 'a list ->
        eqv: partition<'a> -> func<'a,'a list>
        when 'a: comparison

    val stal_intersect:
      partition<formula<'a>> *
      func<formula<'a>,('b * 'c list) list> ->
        partition<formula<'a>> *
        func<formula<'a>,('b * 'c list) list> ->
          partition<formula<'a>> *
          func<formula<'a>,('b * 'c list) list> ->
            partition<formula<'a>> *
            func<formula<'a>,('b * 'c list) list>
                when 'a: comparison and 'b: comparison and 'c: comparison

    val saturate:
      n: int ->
        partition<formula<'a>> *
        func<formula<'a>,
                     ('b * (formula<'a> * formula<'a>) list)  list> ->
          assigs: (formula<'a> * formula<'a>) list ->
          allvars: formula<'a> list ->
          partition<formula<'a>> *
          func<formula<'a>,
                       ('b * (formula<'a> * formula<'a>) list)    list>
            when 'a: comparison and 'b: comparison

    val splits:
      n: int ->
        partition<formula<'a>> *
        func<formula<'a>,
                     ('b * (formula<'a> * formula<'a>) list)  list> ->
          allvars: formula<'a> list ->
          vars: formula<'a> list ->
          partition<formula<'a>> *
          func<formula<'a>,
                       ('b * (formula<'a> * formula<'a>) list)    list>
            when 'a: comparison and 'b: comparison

    val saturate_upto:
      vars: formula<'a> list ->
        n: int ->
        m: int ->
        trigs: ((formula<'a> * formula<'a>) *
                (formula<'a> * formula<'a>) list) list ->
        assigs: (formula<'a> * formula<'a>) list -> bool
        when 'a: comparison

    val stalmarck: fm: formula<prop> -> bool