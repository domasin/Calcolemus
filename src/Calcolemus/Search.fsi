// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

///<summary>
/// Searching functions.
/// </summary>
/// 
/// <category index="3">Sort and search algorithms</category>
module Search = 

    /// <summary>
    /// Returns the result of the first successful application of a function to 
    /// the elements of a list.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>tryfind f [x1;...;xn]</c> returns <c>f xi</c> for the first 
    /// <c>xi</c> in the list for which application of <c>f</c> succeeds.
    /// 
    /// <p>Fails with tryfind if the application of the function fails for all 
    /// elements in the list. This will always be the case if the list is empty.
    /// </p>
    /// </remarks>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The first successful application of the function to an element of the 
    /// list.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the application of the function fails for all elements in the list.</exception>
    /// 
    /// <example id="tryfind-1">
    /// <code lang="fsharp">
    /// [1;2;3] 
    /// |> tryfind (fun n -> if n % 2 = 0 then string n else failwith "f")
    /// </code>
    /// Evaluates to <c>"2"</c>.
    /// </example>
    /// 
    /// <example id="tryfind-2">
    /// <code lang="fsharp">
    /// [1;2;3] 
    /// |> tryfind (fun n -> if n > 3 then string n else failwith "f")
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <note>
    /// Perhaps it could be replaced with the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.TryPick``2'/>.
    /// </note>
    val tryfind: f: ('a -> 'b) -> l: 'a list -> 'b
    
    /// <summary>
    /// Applies a function to every element of a list, returning a list of 
    /// results for those elements for which application succeeds.
    /// </summary>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The list of successful application's results.
    /// </returns>
    /// 
    /// <example id="mapfilter-1">
    /// <code lang="fsharp">
    /// [1;2;3;4] 
    /// |> mapfilter (fun n -> if n % 2 = 0 then string n else failwith "f")
    /// </code>
    /// Evaluates to <c>["2"; "4"]</c>.
    /// </example>
    /// 
    /// <example id="mapfilter-2">
    /// <code lang="fsharp">
    /// [1;2;3] 
    /// |> mapfilter (fun n -> if n > 3 then string n else failwith "f")
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    val mapfilter: f: ('a -> 'b) -> l: 'a list -> 'b list
    
    /// <summary>
    /// Finds the element of a list <c>l</c> that maximizes or minimizes 
    /// (based on the given <c>ord</c>) a function <c>f</c>. 
    /// </summary>
    /// 
    /// <remarks>
    /// Used to define maximize and minimize.
    /// </remarks>
    /// 
    /// <param name="ord">The order that defines whether to maximize or minimize.</param>
    /// <param name="f">The input function.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The element of the list that optimizes the function.
    /// </returns>
    /// 
    /// <example id="optimize-1">
    /// <code lang="fsharp">
    /// optimize (&lt;) (( * ) -1) [-1;2;3]
    /// </code>
    /// Evaluates to <c>-1</c>.
    /// </example>
    /// 
    /// <example id="optimize-2">
    /// <code lang="fsharp">
    /// optimize (&gt;) (( * ) -1) [-1;2;3]
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    val optimize: ord: ('a -> 'a -> bool) -> f: ('b -> 'a) -> l: 'b list -> 'b
    
    /// <summary>
    /// Finds the element of a list <c>l</c> that maximizes a function 
    /// <c>f</c>. 
    /// </summary>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The element of the list that maximises the function.
    /// </returns>
    /// 
    /// <example id="maximize-example">
    /// <code lang="fsharp">
    /// maximize (( * ) -1) [-1;2;3]
    /// </code>
    /// Evaluates to <c>-1</c>.
    /// </example>
    val maximize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison
    
    /// <summary>
    /// Finds the element of a list <c>l</c> that minimizes a function 
    /// <c>f</c>. 
    /// </summary>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The element of the list that minimizes the function.
    /// </returns>
    /// 
    /// <example id="minimizes-example">
    /// <code lang="fsharp">
    /// minimize (( * ) -1) [-1;2;3]
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    val minimize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison