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
/// Model elimination procedure (MESON version, based on Stickel's PTTP).
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Meson = 

    /// <summary>
    /// Converts a clause in the set of all its contrapositives.
    /// </summary>
    /// 
    /// <remarks>
    /// If the clause is completely negative, is also added the rule with 
    /// <c>false</c> as conclusion.
    /// </remarks>
    /// 
    /// <param name="cls">The input clause.</param>
    /// 
    /// <returns>
    /// The set of contrapositives of the input clause.
    /// </returns>
    /// 
    /// <example id="contrapositives-1">
    /// <code lang="fsharp">
    /// contrapositives !!>["P";"Q";"~R"]
    /// </code>
    /// Evaluates to <c>[([`~Q`; `R`], `P`); ([`~P`; `R`], `Q`); ([`~P`; `~Q`], `~R`)]</c>.
    /// </example>
    /// 
    /// <example id="contrapositives-2">
    /// <code lang="fsharp">
    /// contrapositives !!>["~P";"~Q";"~R"]
    /// </code>
    /// Evaluates to <c>[([`P`; `Q`; `R`], `false`); ([`Q`; `R`], `~P`); ([`P`; `R`], `~Q`); ([`P`; `Q`], `~R`)]</c>.
    /// </example>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val contrapositives:
      cls: formula<'a> list ->
        (formula<'a> list * formula<'a>) list when 'a: comparison

    /// <summary>
    /// The core of MESON: ancestor unification or Prolog-style extension.
    /// </summary>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val mexpand_basic:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> 'a) ->
        env: func<string,term> * n: int * k: int -> 'a

    /// <summary>
    /// Full MESON procedure.
    /// </summary>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val puremeson_basic: fm: formula<fol> -> int

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="1">Basic MESON procedure</category>
    val meson_basic: fm: formula<fol> -> int list

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val equal:
      env: func<string,term> ->
        fm1: formula<fol> -> fm2: formula<fol> -> bool

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val expand:
      expfn: ('a -> ('b * int * 'c -> 'd) -> 'b * int * 'c -> 'd) ->
        goals1: 'a ->
        n1: int ->
        goals2: 'a ->
        n2: int -> n3: int -> cont: ('b * int * 'c -> 'd) -> env: 'b -> k: 'c -> 'd

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val mexpand:
      rules: (formula<fol> list * formula<fol>) list ->
        ancestors: formula<fol> list ->
        g: formula<fol> ->
        cont: (func<string,term> * int * int -> 'a) ->
        env: func<string,term> * n: int * k: int -> 'a

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val puremeson: fm: formula<fol> -> int

    /// <summary>
    /// TODO
    /// </summary>
    /// 
    /// <category index="2">MESON procedure optimized</category>
    val meson: fm: formula<fol> -> int list