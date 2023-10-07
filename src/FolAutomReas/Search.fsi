// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

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
    /// <exception cref="T:System.Exception">Thrown with message 'tryfind' when the application of the function fails for all elements in the list.</exception>
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
    
    /// finds the element of a list l that maximizes or minimizes a function f 
    /// based on the given ord.
    /// 
    /// Support function for use with maximize and minimize.
    val optimize: ord: ('a -> 'a -> bool) -> f: ('b -> 'a) -> lst: 'b list -> 'b
    
    /// finds the element of a list l that maximizes a function f
    /// 
    /// maximize ((*) -1) [-1;2;3] returns -1
    val maximize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison
    
    /// finds the element of a list l that minimizes a function f
    /// 
    /// minimize ((*) -1) [-1;2;3] returns 3
    val minimize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison