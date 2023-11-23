// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Formulas
open Fol

/// <summary>
/// Simple term orderings including LPO.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Order = 

    /// <summary>
    /// Calculates the size of a term as the sum of the number of its variables 
    /// and function symbols.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The sum of the number of variables and function symbols in the term.
    /// </returns>
    /// 
    /// <example id="termsize-1">
    /// <code lang="fsharp">
    /// termsize !!!"f(f(f(x)))"
    /// </code>
    /// Evaluates to <c>4</c>.
    /// </example>
    val termsize: tm: term -> int

    /// <summary>
    /// Returns the general lexicographic extension of an arbitrary relation 
    /// <c>ord</c>.
    /// </summary>
    /// 
    /// <param name="ord">The input relation.</param>
    /// <param name="l1">The first input items to be compared.</param>
    /// <param name="l2">The second input items to be compared.</param>
    /// 
    /// <returns>
    /// true, if the input list are equally long and reading from 
    /// left to right the items in the first list are not less then the 
    /// second's and there is at leas one greater;otherwise false.
    /// </returns>
    /// 
    /// <example id="lexord-1">
    /// <code lang="fsharp">
    /// lexord (>) [1;1;1;2] [1;1;1;1]  // true
    /// lexord (>) [1;1;2;1] [1;1;1;1]  // true
    /// lexord (>) [1;0;2;1] [1;1;1;1]  // false
    /// lexord (>) [1;1;1;1] [1;1;1;1]  // false
    /// lexord (>) [2;2;2;2] [1;1;1]    // false
    /// </code>
    /// </example>
    val lexord:
      ord: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> bool
        when 'a: equality

    /// <summary>
    /// Test if a term is greater than an other based on a irreflexive 
    /// lexicographic path order.
    /// </summary>
    /// 
    /// <param name="w">The function that `weights' the function symbols.</param>
    /// <param name="s">The first input term supposed greater than the second.</param>
    /// <param name="t">The second input term to be compared.</param>
    /// 
    /// <returns>
    /// true, if the first input term is greater than the second based on the 
    /// irreflexive lexicographic path order parametrized by <c>w</c>.
    /// </returns>
    val lpo_gt:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    /// <summary>
    /// Test if a term is greater than an other based on a reflexive 
    /// lexicographic path order.
    /// </summary>
    /// 
    /// <param name="w">The function that `weights' the function symbols.</param>
    /// <param name="s">The first input term supposed greater than the second.</param>
    /// <param name="t">The second input term to be compared.</param>
    /// 
    /// <returns>
    /// true, if the first input term is greater than the second based on the 
    /// reflexive lexicographic path order parametrized by <c>w</c>.
    /// </returns>
    val lpo_ge:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    val weight:
      lis: 'a list -> f: 'a * n: 'b -> g: 'a * m: 'b -> bool
        when 'a: comparison and 'b: comparison