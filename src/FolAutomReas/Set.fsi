// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

///<summary>
/// Set represented as ordered lists and related operations.
/// </summary>
/// 
/// <note>
/// Try to change the functions that uses this module to use the standard 
/// <see cref='T:Microsoft.FSharp.Collections.SetModule'/>.
/// 
/// <p>
/// This is a point where performance could possibly be greatly improved.
/// </p>
/// </note>
module Set = 

    /// <summary>
    /// Removes repeated elements from a list, making a list into a 'set'.
    /// </summary>
    /// 
    /// <remarks>
    /// Returns a sorted list that contains no duplicate entries according to 
    /// generic hash and equality comparisons on the entries. If an element 
    /// occurs multiple times in the list then the later occurrences are 
    /// discarded. 
    /// </remarks>
    /// 
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>The result list.</returns>
    /// 
    /// <example id="setify-1">
    /// <code lang="fsharp">
    /// setify [1;2;3;1;4;3]
    /// </code>
    /// Evaluates to <c>[1;2;3;4]</c>.
    /// </example>
    val setify: l: 'a list -> 'a list when 'a: comparison

    /// <summary>
    /// Computes the union of two `sets'.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>union l1 l2</c> returns a list consisting of the elements of 
    /// <c>l1</c> not already in <c>l2</c> concatenated with <c>l2</c> . 
    /// If <c>l1</c>  and <c>l2</c>  are initially free from duplicates, 
    /// this gives a set-theoretic union operation. 
    /// </remarks>
    /// 
    /// <param name="l1">The first input list.</param>
    /// <param name="l2">The second input list.</param>
    /// 
    /// <returns>The union of the two lists.</returns>
    /// 
    /// <example id="union-1">
    /// <code lang="fsharp">
    /// union [1;2;3] [1;5;4;3]
    /// </code>
    /// Evaluates to <c>[1;2;3;4;5]</c>.
    /// </example>
    /// 
    /// <example id="union-1">
    /// <code lang="fsharp">
    /// union [1;1;1] [1;2;3;2] 
    /// </code>
    /// Evaluates to <c>[1;2;3]</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.Union``1'/>.
    /// </note>
    val union: l1: 'a list -> l2: 'a list -> 'a list when 'a: comparison

    /// <summary>
    /// Computes the intersection of two 'sets'.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>intersect l1 l2</c> returns a list consisting of those elements of 
    /// <c>l1</c> that also appear in <c>l1</c>. If both sets are free of 
    /// repetitions, this can be considered a set-theoretic intersection 
    /// operation. 
    /// </remarks>
    /// 
    /// <param name="l1">The first input list.</param>
    /// <param name="l2">The second input list.</param>
    /// 
    /// <returns>The intersection of the two lists.</returns>
    /// 
    /// <example id="intersect-1">
    /// <code lang="fsharp">
    /// intersect [1;2;3] [3;5;4;1]
    /// </code>
    /// Evaluates to <c>[1;3]</c>.
    /// </example>
    /// 
    /// <example id="intersect-1">
    /// <code lang="fsharp">
    /// intersect [1;2;4;1] [1;2;3;2]
    /// </code>
    /// Evaluates to <c>[1;2]</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.Intersect``1'/>.
    /// </note>
    val intersect: l1: 'a list -> l2: 'a list -> 'a list when 'a: comparison

    /// <summary>
    /// Computes the set-theoretic difference of two 'sets'.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>subtract l1 l2</c> returns a list consisting of those elements of 
    /// <c>l1</c> that do not appear in <c>l2</c>. If both lists are initially 
    /// free of repetitions, this can be considered a set difference operation.
    /// </remarks>
    /// 
    /// <param name="l1">The first input list.</param>
    /// <param name="l2">The list whose elements will be removed from <c>l1</c>. </param>
    /// 
    /// <returns>
    /// The list with the elements of <c>l2</c> removed from <c>l1</c>.
    /// </returns>
    /// 
    /// <example id="subtract-1">
    /// <code lang="fsharp">
    /// subtract [1;2;3] [3;5;4;1]
    /// </code>
    /// Evaluates to <c>[2]</c>.
    /// </example>
    /// 
    /// <example id="subtract-1">
    /// <code lang="fsharp">
    /// subtract [1;2;4;1] [4;5]
    /// </code>
    /// Evaluates to <c>[1;2]</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.Difference``1'/>.
    /// </note>
    val subtract: l1: 'a list -> l2: 'a list -> 'a list when 'a: comparison

    /// <summary>
    /// Evaluates to <c>true</c> if all elements of the first list are in the second.
    /// </summary>
    ///
    /// <param name="l1">The potential subset.</param>
    /// <param name="l2">The set to test against.</param>
    ///
    /// <returns>true if <c>l1</c> is a subset of <c>l2</c>.</returns>
    ///
    /// <example id="subset-1">
    /// <code lang="fsharp">
    /// subset [1;2;3] [1;4;3;2] 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="subset-2">
    /// <code lang="fsharp">
    /// subset [1;2;3] [2;3;1] 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="subset-3">
    /// <code lang="fsharp">
    /// subset [1;2;3;4] [2;3;1] 
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.IsSubset``1'/>.
    /// </note>
    val subset: l1: 'a list -> l2: 'a list -> bool when 'a: comparison

    /// <summary>
    /// Evaluates to <c>true</c> if all elements of the first list are in the 
    /// second, and at least one element of the second is not in the first.
    /// </summary>
    ///
    /// <param name="l1">The potential subset.</param>
    /// <param name="l2">The set to test against.</param>
    ///
    /// <returns>
    /// true if <c>l1</c> is a proper subset of <c>l2</c>.
    /// </returns>
    ///
    /// <example id="psubset-1">
    /// <code lang="fsharp">
    /// psubset [1;2;3] [1;4;3;2] 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="psubset-2">
    /// <code lang="fsharp">
    /// psubset [1;2;3] [2;3;1] 
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="psubset-3">
    /// <code lang="fsharp">
    /// psubset [1;2;3;4] [2;3;1] 
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.IsProperSubset``1'/>.</note>
    val psubset: l1: 'b list -> l2: 'b list -> bool when 'b: comparison

    /// <summary>
    /// Tests two 'sets' for equality.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>set_eq l1 l2</c> returns <c>true</c> if every element of <c>l1</c> 
    /// appears in <c>l2</c> and every element of <c>l2</c> appears in 
    /// <c>l1</c>. Otherwise it returns false. 
    /// 
    /// <p>In other words, it tests if the lists are the same considered as 
    /// sets, i.e. ignoring duplicates.</p>
    /// </remarks>
    /// 
    /// <param name="l1">The first list.</param>
    /// <param name="l2">The second list.</param>
    /// 
    /// <returns>
    /// true if <c>l1</c> and <c>l2</c> are equal seen as sets.
    /// </returns>
    /// 
    /// <example id="set_eq-1">
    /// <code lang="fsharp">
    /// set_eq [1;2] [2;1;2]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="set_eq-2">
    /// <code lang="fsharp">
    /// set_eq [1;2] [1;3]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val set_eq: l1: 'a list -> l2: 'a list -> bool when 'a: comparison

    /// <summary>
    /// Returns a new set with an element added to the set. No exception is 
    /// raised if the set already contains the given element.
    /// </summary>
    ///
    /// <param name="x">The value to add.</param>
    /// <param name="l">The input 'set'.</param>
    ///
    /// <returns>A new 'set' containing <c>x</c>.</returns>
    ///
    /// <example id="insert-1">
    /// <code lang="fsharp">
    /// insert 4 [2;3;3;5]
    /// </code>
    /// Evaluates to <c>[2;3;4;5]</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.Add``1'/>.
    /// </note>
    val insert: x: 'a -> l: 'a list -> 'a list when 'a: comparison

    /// <summary>
    /// The image of <c>s</c> under <c>f</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Returns a new collection containing the results of applying the
    /// given function to each element of the input 'set'.
    /// </remarks>
    ///
    /// <param name="f">The function to transform elements of the input 'set'.</param>
    /// <param name="s">The input 'set'.</param>
    ///
    /// <returns>A 'set' containing the transformed elements.</returns>
    ///
    /// <example id="set-map">
    /// <code lang="fsharp">
    /// [1;2;3] |> image (fun x -> x * 2)
    /// </code>
    /// Evaluates to <c>[2; 4; 6]</c>
    /// </example>
    val image: f: ('a -> 'b) -> s: 'a list -> 'b list when 'b: comparison

    /// <summary>Computes the union of a sequence of 'sets'.</summary>
    ///
    /// <param name="s">The sequence of 'sets' to union.</param>
    ///
    /// <returns>The union of the input 'sets'.</returns>
    ///
    /// <example id="unions-1">
    /// <code lang="fsharp">
    /// unions [[1;2;3]; [4;3;2;6;2]; [5;3;1;3]]
    /// </code>
    /// Evaluates to <c>[1; 2; 3; 4; 5; 6]</c>.
    /// </example>
    /// 
    /// <note>
    /// Corresponds to the standard 
    /// <see cref='M:Microsoft.FSharp.Collections.SetModule.UnionMany``1'/>.
    /// </note>
    val unions: s: 'a list list -> 'a list when 'a: comparison

    /// <summary>Tests if the list contains the specified element.</summary>
    ///
    /// <param name="value">The value to locate in the input list.</param>
    /// <param name="source">The input list.</param>
    ///
    /// <returns>
    /// True if the input list contains the specified element; false otherwise.
    /// </returns>
    /// 
    /// <example id="mem-1">
    /// <code lang="fsharp">
    /// [1..9] |> mem 0
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="mem-2">
    /// <code lang="fsharp">
    /// [1..9] |> mem 3
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="mem-3">
    /// <code lang="fsharp">
    /// [1, "SpongeBob"; 2, "Patrick"; 3, "Squidward"; 4, "Mr. Krabs"] 
    /// |> mem (2, "Patrick")
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="mem-4">
    /// <code lang="fsharp">
    /// [1, "SpongeBob"; 2, "Patrick"; 3, "Squidward"; 4, "Mr. Krabs"] 
    /// |> mem (22, "Patrick")
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <note>
    /// It's just an alias for 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.Contains``1'/>.
    /// </note>
    val mem: value: 'a -> source: 'a list -> bool when 'a: equality

    /// <summary>
    /// Returns all subsets of the given size.
    /// </summary>
    /// 
    /// <param name="m">The size of the subsets to be returned.</param>
    /// <param name="l">The input 'set'.</param>
    /// 
    /// <returns>All the subsets of the given size.</returns>
    /// 
    /// <example id="allsets-1">
    /// <code lang="fsharp">
    /// allsets 2 [1;2;3]
    /// </code>
    /// Evaluates to <c>[[1; 2]; [1; 3]; [2; 3]]</c>.
    /// </example>
    val allsets: m: int -> l: 'a list -> 'a list list when 'a: comparison

    /// <summary>
    /// Returns all the subsets of the input 'set'.
    /// </summary>
    /// 
    /// <param name="s">The input 'set'.</param>
    /// 
    /// <returns>All the subsets of the input 'set'.</returns>
    /// 
    /// <example id="allsubsets-1">
    /// <code lang="fsharp">
    /// allsubsets [1;2;3]
    /// </code>
    /// Evaluates to 
    /// <c>[[]; [1]; [1; 2]; [1; 2; 3]; [1; 3]; [2]; [2; 3]; [3]]</c>.
    /// </example>
    val allsubsets: s: 'a list -> 'a list list when 'a: comparison

    /// <summary>
    /// Returns all nonempty subsets of the input 'set'.
    /// </summary>
    /// 
    /// <param name="s">The input 'set'.</param>
    /// 
    /// <returns>All nonempty subsets of the input 'set'.</returns>
    /// 
    /// <example id="allnonemptysubsets-1">
    /// <code lang="fsharp">
    /// allsubsets [1;2;3]
    /// </code>
    /// Evaluates to 
    /// <c>[[1]; [1; 2]; [1; 2; 3]; [1; 3]; [2]; [2; 3]; [3]]</c>.
    /// </example>
    val allnonemptysubsets: s: 'a list -> 'a list list when 'a: comparison