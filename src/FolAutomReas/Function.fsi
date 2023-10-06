// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// <namespacedoc><summary>
/// Misc library functions to set up a nice environment. 
/// </summary></namespacedoc>
/// 
/// <summary>Functions over predicates and functions.</summary>
module Function = 

    /// <summary>
    /// Applies the inverse of a predicate <c>p</c> to an argument <c>x</c>.
    /// </summary>
    /// 
    /// <param name="p">The predicate to reverse.</param>
    /// <param name="x">The element to apply the inverse to.</param>
    /// 
    /// <returns>
    /// true if <c>p x</c> returns false, otherwise false.
    /// </returns>
    /// 
    /// <example id="non-1">
    /// <code lang="fsharp">
    /// 4 |> non ((=) 2)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="non-2">
    /// <code lang="fsharp">
    /// 2 |> non ((=) 2)
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val inline non: p: ('a -> bool) -> x: 'a -> bool

    /// <summary>
    /// Checks if the value <c>x</c> satisfies the predicate <c>p</c>.
    /// </summary>
    /// 
    /// <param name="p">The input predicate.</param>
    /// <param name="x">The element that should satisfy the predicate.</param>
    /// 
    /// <returns>
    /// The input element <c>x</c> itself, if it satisfies <c>p</c>.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'check', when
    /// <c>x</c> does not satisfy <c>p</c>.</exception>
    /// 
    /// <example id="check-1">
    /// <code lang="fsharp">
    /// check ((=) 2) 2
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="check-2">
    /// <code lang="fsharp">
    /// check ((=) 2) 4
    /// </code>
    /// Throws <c>System.Exception: check</c>.
    /// </example>
    val check: p: ('a -> bool) -> x: 'a -> 'a

    /// <summary>
    /// Iterates the application of a function <c>f</c> to an argument <c>x</c> 
    /// a fixed number <c>n</c> of times.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>funpow n f x</c> applies <c>f</c> to <c>x</c> for <c>n</c> times, 
    /// giving the result <c>f (f ... (f x)...)</c> where the number of 
    /// <c>f</c>'s is <c>n</c>. <c>funpow 0 f x</c> returns <c>x</c>. If 
    /// <c>n</c> is negative, it is treated as <c>0</c>. It fails, if any of 
    /// the <c>n</c> applications of <c>f</c> fail.
    /// </remarks>
    /// 
    /// <param name="n">The number of times to apply the function.</param>
    /// <param name="f">The function to apply.</param>
    /// <param name="x">The element to apply the function to.</param>
    /// 
    /// <returns>
    /// The result of applying <c>f</c> to <c>x</c> for <c>n</c> times, if 
    /// <c>n</c> is >= 0. Otherwise, the input argument <c>x</c> unchanged.
    /// </returns>
    /// 
    /// <example id="funpow-1">
    /// <code lang="fsharp">
    /// 2. |> funpow 2 (fun x -> x ** 2.)
    /// </code>
    /// Evaluates to <c>16.</c>
    /// </example>
    /// 
    /// <example id="funpow-2">
    /// <code lang="fsharp">
    /// 2. |> funpow 0 (fun x -> x ** 2.)
    /// </code>
    /// Evaluates to <c>2.</c>
    /// </example>
    /// 
    /// <example id="funpow-3">
    /// <code lang="fsharp">
    /// 2. |> funpow -1 (fun x -> x ** 2.)
    /// </code>
    /// Evaluates to <c>2.</c>
    /// </example>
    /// 
    /// <example id="funpow-4">
    /// <code lang="fsharp">
    /// 2. |> funpow 2 ((/) 0)
    /// </code>
    /// Throws <c>System.DivideByZeroException: 
    /// Attempted to divide by zero.</c>
    /// </example>
    val funpow: n: int -> f: ('a -> 'a) -> x: 'a -> 'a

    /// <summary>
    /// Tests if an input function <c>f</c> can be applied to an argument 
    /// <c>x</c> without failing.
    /// </summary>
    /// 
    /// <param name="f">The function to test.</param>
    /// <param name="x">The argument on which to test the function.</param>
    /// 
    /// <returns>
    /// true, if <c>f x</c> succeeds; false, otherwise.
    /// </returns>
    /// 
    /// <example id="can-1">
    /// <code lang="fsharp">
    /// can List.head [1;2]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="can-2">
    /// <code lang="fsharp">
    /// can List.head []
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val can: f: ('a -> 'b) -> x: 'a -> bool

    /// <summary>
    /// Repeats the application of a function <c>f</c> to an argument <c>x</c> 
    /// until it fails.
    /// </summary>
    /// 
    /// <param name="f">The function to apply.</param>
    /// <param name="x">The element to apply the function to.</param>
    /// 
    /// <returns>
    /// <c>x</c>, if the application of <c>f x</c> fails.
    /// </returns>
    /// 
    /// <example id="repeat-example">
    /// <code lang="fsharp">
    /// repeat (List.removeAt 0) [1;2;3;4;5]
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    val repeat: f: ('a -> 'a) -> x: 'a -> 'a
