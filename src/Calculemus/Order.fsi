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
    /// second's and there is at least one greater;otherwise false.
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
    /// Tests if a term is greater than an other based on a irreflexive 
    /// lexicographic path order.
    /// </summary>
    /// 
    /// <remarks>
    /// <ul>
    /// <li>
    /// If \(t\) is a variable \(t\), \(s > t\) if \(s \neq t\) and 
    /// \(x \in \text{FVT}(s)\).
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > f(t_1,\ldots,t_m)\) if the sequence 
    /// \(s_1,\ldots,s_n\) is lexicographically greater than 
    /// \(t_1,\ldots,t_n\), i.e if \(s_i = t_i\) for all 
    /// \(i &lt; k \leq m\) and \(s_k > t_k\) under the same ordering;
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_n) > t\) when some \(s_i \geq t\);
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > g(t_1,\ldots,t_n)\) according to the 
    /// specified precedence ordering of the function symbols, without further 
    /// analysis of the \(s_i\) and \(t_i\);
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > g(t_1,\ldots,t_n)\) (whether or not \(f = g\)) 
    /// only if in addition \(f(s_1,\ldots,s_m) > t_i\) for each \(1 \leq i 
    /// \leq n\).
    /// </li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="w">The function that `weights' the function symbols.</param>
    /// <param name="s">The first input term supposed greater than the second.</param>
    /// <param name="t">The second input term to be compared.</param>
    /// 
    /// <returns>
    /// true, if the first input term is greater than the second based on the 
    /// irreflexive lexicographic path order parametrized by <c>w</c>.
    /// </returns>
    /// 
    /// <example id="lpo_gt-1">
    /// The second term is a variable contained in the free variables of the 
    /// first.
    /// <code lang="fsharp">
    /// lpo_gt (weight []) !!!"f(x)" !!!"x"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-2">
    /// The second term is a variable not contained in the free variables of 
    /// the first.
    /// <code lang="fsharp">
    /// lpo_gt (weight []) !!!"f(y)" !!!"x"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-3">
    /// The inputs are function terms with the same function symbol but the 
    /// arguments sequence of the first is greater than that of the second:
    /// <code lang="fsharp">
    /// lpo_gt (weight ["0"; "1"]) !!!"f(0,1)" !!!"f(0,0)"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-4">
    /// The first term is a function term and at least one of its arguments is 
    /// greater or equal than the second term
    /// <code lang="fsharp">
    /// lpo_gt (weight ["0"; "1"]) !!!"h(0,1)" !!!"1"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-5">
    /// The inputs are function terms and the function symbol of the first is 
    /// greater than the second based on the precedence defined by the 
    /// weighting function
    /// <code lang="fsharp">
    /// lpo_gt (weight ["g";"f"]) !!!"f(1)" !!!"g(1)" 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-6">
    /// The input terms are function terms and all arguments of the first are 
    /// greater than the whole second term.
    /// <code lang="fsharp">
    /// lpo_gt (weight ["0";"1";"g";"f"]) !!!"g(f(1))" !!!"f(0)"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_gt-7">
    /// The input terms are the same
    /// <code lang="fsharp">
    /// lpo_gt (weight []) !!!"f(1)" !!!"f(1)"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val lpo_gt:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    /// <summary>
    /// Tests if a term is greater than an other based on a reflexive 
    /// lexicographic path order.
    /// </summary>
    /// 
    /// <remarks>
    /// <ul>
    /// <li>
    /// If \(t\) is a variable \(t\), \(s > t\) if \(s \neq t\) and 
    /// \(x \in \text{FVT}(s)\).
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > f(t_1,\ldots,t_m)\) if the sequence 
    /// \(s_1,\ldots,s_n\) is lexicographically greater than 
    /// \(t_1,\ldots,t_n\), i.e if \(s_i = t_i\) for all 
    /// \(i &lt; k \leq m\) and \(s_k > t_k\) under the same ordering;
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_n) > t\) when some \(s_i \geq t\);
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > g(t_1,\ldots,t_n)\) according to the 
    /// specified precedence ordering of the function symbols, without further 
    /// analysis of the \(s_i\) and \(t_i\);
    /// </li>
    /// <li>
    /// \(f(s_1,\ldots,s_m) > g(t_1,\ldots,t_n)\) (whether or not \(f = g\)) 
    /// only if in addition \(f(s_1,\ldots,s_m) > t_i\) for each \(1 \leq i 
    /// \leq n\).
    /// </li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="w">The function that `weights' the function symbols.</param>
    /// <param name="s">The first input term supposed greater than the second.</param>
    /// <param name="t">The second input term to be compared.</param>
    /// 
    /// <returns>
    /// true, if the first input term is greater than the second based on the 
    /// reflexive lexicographic path order parametrized by <c>w</c>.
    /// </returns>
    /// 
    /// <example id="lpo_ge-1">
    /// The second term is a variable contained in the free variables of the 
    /// first.
    /// <code lang="fsharp">
    /// lpo_ge (weight []) !!!"f(x)" !!!"x"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-2">
    /// The second term is a variable not contained in the free variables of 
    /// the first.
    /// <code lang="fsharp">
    /// lpo_ge (weight []) !!!"f(y)" !!!"x"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-3">
    /// The inputs are function terms with the same function symbol but the 
    /// arguments sequence of the first is greater than that of the second:
    /// <code lang="fsharp">
    /// lpo_ge (weight ["0"; "1"]) !!!"f(0,1)" !!!"f(0,0)"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-4">
    /// The first term is a function term and at least one of its arguments is 
    /// greater or equal than the second term
    /// <code lang="fsharp">
    /// lpo_ge (weight ["0"; "1"]) !!!"h(0,1)" !!!"1"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-5">
    /// The inputs are function terms and the function symbol of the first is 
    /// greater than the second based on the precedence defined by the 
    /// weighting function
    /// <code lang="fsharp">
    /// lpo_ge (weight ["g";"f"]) !!!"f(1)" !!!"g(1)" 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-6">
    /// The input terms are function terms and all arguments of the first are 
    /// greater than the whole second term.
    /// <code lang="fsharp">
    /// lpo_ge (weight ["0";"1";"g";"f"]) !!!"g(f(1))" !!!"f(0)"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="lpo_ge-7">
    /// The input terms are the same
    /// <code lang="fsharp">
    /// lpo_ge (weight []) !!!"f(1)" !!!"f(1)"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    val lpo_ge:
      w: (string * int -> string * int -> bool) ->
        s: term -> t: term -> bool

    /// <summary>
    /// Tests if the function symbol <c>f,n</c> is `bigger' than <c>g,n</c> 
    /// based on a precedence defined in the <c>lis</c> argument or on their 
    /// arities if they are the same symbol.
    /// </summary>
    /// 
    /// <param name="lis">The list that defines the precedence of function symbols.</param>
    /// <param name="f">The first input function symbol, supposed bigger than the second.</param>
    /// <param name="n">The arity of the first function symbol.</param>
    /// <param name="g">The second input function symbol to be compared.</param>
    /// <param name="m">The arity of the second function symbol.</param>
    /// 
    /// <returns>
    /// true, if <c>f</c> comes after <c>g</c> in <c>lis</c>, or <c>f</c> and 
    /// <c>g</c> are the same symbol but <c>n > m</c>; otherwise, false.
    /// </returns>
    /// 
    /// <example id="weight-1">
    /// <code lang="fsharp">
    /// weight ["g";"f"] ("f",1) ("g",2) // true
    /// weight ["f";"f"] ("f",2) ("f",1) // true
    /// weight ["f";"g"] ("f",1) ("g",2) // false
    /// weight ["f";"f"] ("f",1) ("f",2) // false
    /// </code>
    /// </example>
    val weight:
      lis: 'a list -> f: 'a * n: 'b -> g: 'a * m: 'b -> bool
        when 'a: comparison and 'b: comparison