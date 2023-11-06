// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Model Elimination.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Meson = 

    val contrapositives:
      cls: formula<'a> list ->
        (formula<'a> list * formula<'a>) list when 'a: comparison

    val mexpand001:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> 'a) ->
        env: func<string,term> * n: int * k: int -> 'a

    val puremeson001: fm: formula<fol> -> int

    val meson001: fm: formula<fol> -> int list

    val equal:
      env: func<string,term> ->
        fm1: formula<fol> -> fm2: formula<fol> -> bool

    val expand:
      expfn: ('a -> ('b * int * 'c -> 'd) -> 'b * int * 'c -> 'd) ->
        goals1: 'a ->
        n1: int ->
        goals2: 'a ->
        n2: int -> n3: int -> cont: ('b * int * 'c -> 'd) -> env: 'b -> k: 'c -> 'd

    val mexpand:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> 'a) ->
        env: func<string,term> * n: int * k: int -> 'a

    val puremeson: fm: formula<fol> -> int

    val meson: fm: formula<fol> -> int list