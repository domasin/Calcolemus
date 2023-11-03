// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

/// <summary>
/// Handy functions for list manipulation.
/// </summary>
/// 
/// <category index="2">Collections (lists and sets)</category>
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
    /// <exception cref="T:System.Exception">Thrown with message <c>last</c> when the input does not have any elements.</exception>
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
    /// <exception cref="T:System.Exception">Thrown with message <c>butlast</c> when the input does not have any elements.</exception>
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
    /// <exception cref="T:System.Exception">Thrown with message <c>chop_list</c> when chop index is negative or exceeds the number of elements in the list.</exception>
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

    /// <summary>
    /// Return a new list with a new item inserted before the given index.
    /// </summary>
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

    /// <summary>
    /// Returns the index of the first element in the list <c>xs</c> that 
    /// equals <c>x</c>. Raises <c>KeyNotFoundException</c> if no such element 
    /// exists. 
    /// </summary>
    /// 
    /// <param name="x">The element to search.</param>
    /// <param name="xs">The input list.</param>
    /// 
    /// <returns>
    /// The index of the first element that equals <c>x</c>.
    /// </returns>
    /// 
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">Thrown when no element in the list equals <c>x</c>.</exception>
    /// 
    /// <example id="index-1">
    /// <code lang="fsharp">
    /// index 2 [0;1;3;3;2;3]
    /// </code>
    /// Evaluates to <c>4</c>.
    /// </example>
    /// 
    /// <example id="index-2">
    /// <code lang="fsharp">
    /// index 5 [0;1;3;3;2;3]
    /// </code>
    /// Throws <c>System.Collections.Generic.KeyNotFoundException: An index satisfying the predicate was not found in the collection.</c>.
    /// </example>
    val inline index: x: 'a -> xs: 'a list -> int when 'a: equality

    /// <summary>
    /// Checks if <c>x</c> comes earlier than <c>y</c> in list <c>l</c>.
    /// </summary>
    /// 
    /// <param name="l">The input list.</param>
    /// <param name="x">The element to search.</param>
    /// <param name="y">The element to compare with <c>x</c>.</param>
    /// 
    /// <returns>
    /// true, if <c>x</c> comes earlier than <c>y</c> in <c>l</c>, 
    /// or <c>x</c> is in <c>l</c> but not <c>y</c>. Otherwise, false.
    /// </returns>
    /// 
    /// <example id="earlier-1">
    /// <code lang="fsharp">
    /// earlier [0;1;2;3] 2 3
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="earlier-2">
    /// <code lang="fsharp">
    /// earlier [0;1;2;3] 3 4
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="earlier-3">
    /// <code lang="fsharp">
    /// earlier [0;1;2;3] 3 2
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="earlier-4">
    /// <code lang="fsharp">
    /// earlier [0;1;3] 2 3
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="earlier-5">
    /// <code lang="fsharp">
    /// earlier [0;1;2;3] 4 5
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val earlier: l: 'a list -> x: 'a -> y: 'a -> bool when 'a: comparison

    /// <summary>
    /// Searches a list of pairs <c>l</c> for a pair whose first component 
    /// equals a specified value <c>a</c>, failing if no matching is found.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>assoc a [(a1,b1);...;(an,bn)]</c> returns the first <c>bi</c> in the 
    /// list such that <c>ai</c> equals <c>a</c>.
    /// </remarks>
    /// 
    /// <param name="l">The input list.</param>
    /// <param name="a">The value to search.</param>
    /// 
    /// <returns>
    /// The second component of the pair, if a matching for the first is 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>find</c> when no matching is found.</exception>
    /// 
    /// <example id="assoc-1">
    /// <code lang="fsharp">
    /// assoc 2 [(1,2);(2,3)]
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    /// 
    /// <example id="assoc-2">
    /// <code lang="fsharp">
    /// assoc 2 [(1,2);(2,3);(2,4)]
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    /// 
    /// <example id="assoc-3">
    /// <code lang="fsharp">
    /// assoc 3 [(1,2);(2,3)]
    /// </code>
    /// Throws <c>System.Exception: find</c>.
    /// </example>
    val assoc: a: 'a -> l: ('a * 'b) list -> 'b when 'a: comparison

    /// <summary>
    /// Searches a list of pairs <c>l</c> for a pair whose second component 
    /// equals a specified value <c>b</c>, failing if no matching is found.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>rev_assoc b [(a1,b1);...;(an,bn)]</c> returns the first <c>ai</c> in 
    /// the list such that <c>bi</c> equals <c>b</c>.
    /// </remarks>
    /// 
    /// <param name="l">The input list.</param>
    /// <param name="b">The value to search.</param>
    /// 
    /// <returns>
    /// The first component of the pair, if a matching for the second is 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>find</c> when no matching is found.</exception>
    /// 
    /// <example id="rev_assoc-1">
    /// <code lang="fsharp">
    /// rev_assoc 2 [(1,2);(2,3)]
    /// </code>
    /// Evaluates to <c>1</c>.
    /// </example>
    /// 
    /// <example id="rev_assoc-2">
    /// <code lang="fsharp">
    /// rev_assoc 2 [(1,2);(2,2);(2,3)]
    /// </code>
    /// Evaluates to <c>1</c>.
    /// </example>
    /// 
    /// <example id="rev_assoc-3">
    /// <code lang="fsharp">
    /// rev_assoc 1 [(1,2);(2,3)]
    /// </code>
    /// Throws <c>System.Exception: find</c>.
    /// </example>
    val rev_assoc: b: 'b -> l: ('a * 'b) list -> 'a when 'b: comparison