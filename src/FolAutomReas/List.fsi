// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// <summary>
/// Handy functions for list manipulation.
/// </summary>
module List = 

    /// <summary>
    /// Returns the last element of the list. 
    /// </summary>
    /// 
    /// <note>
    /// It should be replaced with the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.Last``1'/>.
    /// </note>
    /// 
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>The last element of the list.</returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'last' when the input does not have any elements.</exception>
    /// 
    /// <example id="last-1">
    /// <code lang="fsharp">
    /// last [1;2;3;4;5]
    /// </code>
    /// Evaluates to <c>5</c>.
    /// </example>
    /// 
    /// <example id="last-2">
    /// <code lang="fsharp">
    /// last ([]:int list)
    /// </code>
    /// Throws <c>System.Exception: last</c>.
    /// </example>
    val last: l: 'a list -> 'a

    /// <summary>
    /// Computes the sub-list of a list consisting of all but the last element.
    /// </summary>
    /// 
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>All the elements of the input list but the last one.</returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'butlast' when the input does not have any elements.</exception>
    /// 
    /// <example id="butlast-1">
    /// <code lang="fsharp">
    /// butlast [1;2;3;4;5]
    /// </code>
    /// Evaluates to <c>[1;2;3;4]</c>.
    /// </example>
    /// 
    /// <example id="butlast-2">
    /// <code lang="fsharp">
    /// butlast ([]:int list)
    /// </code>
    /// Throws <c>System.Exception: butlast</c>.
    /// </example>
    val butlast: l: 'a list -> 'a list

    /// <summary>
    /// Computes the list of all results from applying the function <c>f</c> to 
    /// pairs from the two input lists <c>l1</c> and <c>l2</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>allpairs f [x1;...;xm] [y1;...;yn]</c> returns the list of results 
    /// <c>[f x1 y1; f x1 y2; ... ; f x1 yn; f x2 y1 ; f x2 y2 ...; f xm y1; 
    /// f xm y2 ...; f xm yn]</c>
    /// </remarks>
    /// 
    /// <param name="f">The function to apply to the pairs.</param>
    /// <param name="l1">The list of first elements of the pairs.</param>
    /// <param name="l2">The list of second elements of the pairs.</param>
    /// 
    /// <returns>
    /// The list of results of applying <c>f</c> to all the pairs formed 
    /// from <c>l1</c> and <c>l2</c>.
    /// </returns>
    /// 
    /// <example id="allpairs-example">
    /// <code lang="fsharp">
    /// allpairs (+) [1;2;3] [1;2]
    /// // [1+1; 1+2; 2+1; 2+2; 3+1; 3+2]
    /// </code>
    /// Evaluates to <c>[2; 3; 3; 4; 4; 5]</c>.
    /// </example>
    val allpairs: f: ('a -> 'b -> 'c) -> l1: 'a list -> l2: 'b list -> 'c list

    /// <summary>
    /// Produces all pairs of distinct elements from a single list.
    /// </summary>
    /// 
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// All pairs of distinct elements from the input list <c>l</c>.
    /// </returns>
    /// 
    /// <example id="allpairs-example">
    /// <code lang="fsharp">
    /// distinctpairs [1;2;3;4]
    /// </code>
    /// Evaluates to <c>[(1, 2); (1, 3); (1, 4); (2, 3); (2, 4); (3, 4)]</c>.
    /// </example>
    val distinctpairs: l: 'a list -> ('a * 'a) list

    /// <summary>
    /// Chops a list <c>l</c> into two parts at the specified index <c>n</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>chop_list i [x1;...;xn]</c> returns 
    /// <c>([x0;...;xi-1],[xi;...;xn])</c>.
    /// </remarks>
    /// 
    /// <param name="n">The index at which the list is chopped.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// The two chopped lists. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'chop_list' when chop index is negative or exceeds the number of elements in the list.</exception>
    /// 
    /// <example id="chop_list-1">
    /// <code lang="fsharp">
    /// chop_list 3 [1;2;3;4;5;6]
    /// </code>
    /// Evaluates to <c>([1;2;3], [4;5;6])</c>.
    /// </example>
    /// 
    /// <example id="chop_list-2">
    /// <code lang="fsharp">
    /// ['a';'b';'c';'d'] |> chop_list -1
    /// </code>
    /// Throws <c>System.Exception: chop_list</c>.
    /// </example>
    /// 
    /// <example id="chop_list-3">
    /// <code lang="fsharp">
    /// ['a';'b';'c';'d'] |> chop_list 5
    /// </code>
    /// Throws <c>System.Exception: chop_list</c>.
    /// </example>
    /// 
    /// <note>
    /// It should be replaced with the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.SplitAt``1'/>.
    /// </note>
    val chop_list: n: int -> l: 'a list -> 'a list * 'a list

    /// <summary>Return a new list with a new item inserted before the given index.</summary>
    ///
    /// <param name="i">The index where the item should be inserted.</param>
    /// <param name="x">The value to insert.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>The result list.</returns>
    ///
    /// <exception cref="T:System.Exception">Thrown when index is below 0 or greater than <c>l.Length</c>.</exception>
    ///
    /// <example id="insertat-1">
    /// <code lang="fsharp">
    /// [ 0; 1; 2 ] |> insertat 1 9
    /// </code>
    /// Evaluates to <c>[0; 9; 1; 2]</c>.
    /// </example>
    /// 
    /// <example id="insertat-2">
    /// <code lang="fsharp">
    /// [ 0; 1; 2 ] |> insertat -1 9
    /// </code>
    /// Throws <c>System.Exception: insertat: list too short for position to 
    /// exist</c>.
    /// </example>
    /// 
    /// <example id="insertat-3">
    /// <code lang="fsharp">
    /// [ 0; 1; 2 ] |> insertat -1 4
    /// </code>
    /// Throws <c>System.Exception: insertat: list too short for position to 
    /// exist</c>.
    /// </example>
    /// 
    /// <note>
    /// It should be replaced with the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.InsertAt``1'/>.
    /// </note>
    val insertat: i: int -> x: 'a -> l: 'a list -> 'a list

    /// Returns position of given element in list.
    /// 
    /// The call index x l where l is a list returns the position number of the 
    /// first instance of x in the list, failing if there is none. The indices 
    /// start at zero, corresponding to el.
    val inline index: x: 'a -> xs: 'a list -> int when 'a: equality

    /// Checks if x comes earlier than y in list l
    /// 
    /// earlier [0;1;2;3] 2 3 return true, earlier [0;1;2;3] 3 2 false.
    /// earlier returns false also if x or y is not in the list.
    val earlier: l: 'a list -> x: 'a -> y: 'a -> bool when 'a: comparison