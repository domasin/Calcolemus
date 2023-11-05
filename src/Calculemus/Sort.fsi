// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus.Lib

///<summary>
/// Sorting functions.
/// </summary>
/// 
/// <category index="3">Sort and search algorithms</category>
module Sort = 

    /// <summary>
    /// Merges together two sorted lists with respect to a given ordering.
    /// </summary>
    /// 
    /// <remarks>
    /// If two lists <c>l1</c> and <c>l2</c> are sorted with respect to the 
    /// given ordering <c>ord</c>, then <c>merge ord l1 l2</c> will merge 
    /// them into a sorted list of all the elements. The merge keeps any 
    /// duplicates; it is not a set operation.
    /// </remarks>
    /// 
    /// <param name="ord">The input ordering.</param>
    /// <param name="l1">The first list to be merged.</param>
    /// <param name="l2">The second list to be merged.</param>
    /// 
    /// <returns>The merged list.</returns>
    /// 
    /// <note>
    /// It never fails, but if the lists are not appropriately sorted the 
    /// results will not in general be correct.
    /// </note>
    /// 
    /// <example id="merge-1">
    /// <code lang="fsharp">
    /// merge (&lt;) [1;3;7] [2;4;5;6]
    /// </code>
    /// Evaluates to <c>[1;2;3;4;5;6;7]</c>.
    /// </example>
    /// 
    /// <example id="merge-2">
    /// <code lang="fsharp">
    /// merge (&lt;) [1;3;7] [4;6;5;2;]
    /// </code>
    /// Evaluates to <c>[1; 3; 4; 6; 5; 2; 7]</c>.
    /// <p>Note that since the second input list was not sorted, the result is 
    /// not sorted either.</p>
    /// </example>
    val merge: ord: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> 'a list
    
    /// <summary>
    /// Sorts a list using a given transitive 'ordering' relation.
    /// </summary>
    /// 
    /// <remarks>
    /// The call <c>sort op list</c> where <c>op</c> is a transitive relation 
    /// on the elements of <c>list</c>, will topologically sort the list, i.e. 
    /// will permute it such that if <c>x op y</c> but <c>not y op x</c> then 
    /// <c>x</c> will occur to the left of <c>y</c> in the sorted list. 
    /// In particular if <c>op</c> is a total order, the list will be sorted in 
    /// the usual sense of the word.
    /// </remarks>
    /// 
    /// <param name="ord">The ordering relation.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>The sorted list.</returns>
    /// 
    /// <example id="sort-1">
    /// <code lang="fsharp">
    /// sort (&lt;) [3;1;4;1;5;9;2;6;5;3;5]
    /// </code>
    /// Evaluates to <c>[1;1;2;3;3;4;5;5;5;6;9]</c>.
    /// </example>
    val sort: ord: ('a -> 'a -> bool) -> l: 'a list -> 'a list when 'a: equality
    
    /// <summary>
    /// Checks whether <c>x</c> is less than <c>y</c> based on <c>f</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// increasing predicate to use with 
    /// <see cref='M:Calculemus.Lib.Sort.sort``1'/>.
    /// </remarks>
    /// 
    /// <param name="f">The function based on which to compare <c>x</c> and <c>y</c>.</param>
    /// <param name="x">The supposed smaller element.</param>
    /// <param name="y">The supposed greater element.</param>
    /// 
    /// <returns>
    /// true if <c>x</c> is less than <c>y</c> based on <c>f</c>, otherwise 
    /// false.
    /// </returns>
    /// 
    /// <example id="increasing-1">
    /// <code lang="fsharp">
    /// increasing List.length [1] [1;2]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    val increasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
    
    /// <summary>
    /// Checks whether <c>x</c> is greater than <c>y</c> based on <c>f</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// decreasing predicate to use with 
    /// <see cref='M:Calculemus.Lib.Sort.sort``1'/>.
    /// </remarks>
    /// 
    /// <param name="f">The function based on which to compare <c>x</c> and <c>y</c>.</param>
    /// <param name="x">The supposed greater element.</param>
    /// <param name="y">The supposed smaller element.</param>
    /// 
    /// <returns>
    /// true if <c>x</c> is greater than <c>y</c> based on <c>f</c>, otherwise 
    /// false.
    /// </returns>
    /// 
    /// <example id="decreasing-1">
    /// <code lang="fsharp">
    /// decreasing List.length [1;2] [1]
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    val decreasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
