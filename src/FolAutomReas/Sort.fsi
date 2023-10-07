// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

///<summary>
/// Functions for sorting.
/// </summary>
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
    /// <example id="merge-1">
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
    /// <note>
    /// This function uses the Quicksort algorithm internally, which has good 
    /// typical-case performance and will sort topologically. However, its 
    /// worst-case performance is quadratic. By contrast mergesort gives a good 
    /// worst-case performance but requires a total order. Note that any 
    /// comparison-based topological sorting function must have quadratic 
    /// behaviour in the worst case. For an n-element list, there are n(n-1)/2 
    /// pairs. For any topological sorting algorithm, we can make sure the 
    /// first n(n-1)/2-1 pairs compared are unrelated in either direction, 
    /// while still leaving the option of choosing for the last pair (a,b) 
    /// either.
    /// </note>
    val sort: ord: ('a -> 'a -> bool) -> ('a list -> 'a list) when 'a: equality
    
    /// increasing predicate to use with "sort"
    /// 
    /// increasing List.length [1] [1;2] returns true`
    val increasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
    
    /// decreasing predicate to use with "sort"
    /// 
    /// decreasing List.length [1;2] [1] returns true
    val decreasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
