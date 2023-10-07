// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

module Sort = 

    /// Merges together two sorted lists with respect to a given ordering.
    /// 
    /// If two lists l1 and l2 are sorted with respect to the given ordering 
    /// comparer, then merge ord l1 l2 will merge them into a sorted list of all 
    /// the elements. The merge keeps any duplicates; it is not a set operation.
    /// merge (<) [1;3;7] [2;4;5;6] returns [1;2;3;4;5;6;7]
    /// 
    /// Never fails, but if the lists are not appropriately sorted the results 
    /// will not in general be correct.
    val merge: comparer: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> 'a list
    
    /// <summary>
    /// Sorts a list using a given transitive ‘ordering’ relation.
    /// 
    /// The call sort op list where op is a transitive relation on the elements of 
    /// list, will topologically sort the list, i.e. will permute it such that if x 
    /// op y but not y op x then x will occur to the left of y in the sorted list. 
    /// In particular if op is a total order, the list will be sorted in the usual 
    /// sense of the word
    /// 
    /// Never fails
    /// </summary>
    /// <remarks>
    /// This function uses the Quicksort algorithm internally, which has good 
    /// typical-case performance and will sort topologically. However, its 
    /// worst-case performance is quadratic. By contrast mergesort gives a good 
    /// worst-case performance but requires a total order. Note that any 
    /// comparison-based topological sorting function must have quadratic behaviour 
    /// in the worst case. For an n-element list, there are n(n-1)/2 pairs. For any 
    /// topological sorting algorithm, we can make sure the first n(n-1)/2-1 pairs 
    /// compared are unrelated in either direction, while still leaving the option 
    /// of choosing for the last pair (a,b) either 
    /// </remarks>
    val sort: ord: ('a -> 'a -> bool) -> ('a list -> 'a list) when 'a: equality
    
    /// increasing predicate to use with "sort"
    /// 
    /// increasing List.length [1] [1;2] returns true`
    val increasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
    
    /// decreasing predicate to use with "sort"
    /// 
    /// decreasing List.length [1;2] [1] returns true
    val decreasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison
