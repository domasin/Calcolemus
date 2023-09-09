// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Functions for sorting
[<AutoOpen>]
module FolAutomReas.Lib.Sorting

// ------------------------------------------------------------------------- //
// Merging of sorted lists (maintaining repetitions).                        //
// ------------------------------------------------------------------------- //

/// Merges together two sorted lists with respect to a given ordering.
/// 
/// If two lists l1 and l2 are sorted with respect to the given ordering 
/// comparer, then merge ord l1 l2 will merge them into a sorted list of all 
/// the elements. The merge keeps any duplicates; it is not a set operation.
/// merge (<) [1;3;7] [2;4;5;6] returns [1;2;3;4;5;6;7]
/// 
/// Never fails, but if the lists are not appropriately sorted the results 
/// will not in general be correct.
let rec merge comparer l1 l2 =
    match l1, l2 with
    | [], x
    | x, [] -> x
    | h1 :: t1, h2 :: t2 ->
        if comparer h1 h2 then
            h1 :: (merge comparer t1 l2)
        else
            h2 :: (merge comparer l1 t2)

// ------------------------------------------------------------------------- //
// Bottom-up mergesort.                                                      //
// ------------------------------------------------------------------------- //

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
let sort ord =
    let rec mergepairs l1 l2 =
        match l1, l2 with
        | [s],[] -> s
        | l,[] ->
            mergepairs [] l
        | l,[s1] ->
            mergepairs (s1::l) []
        | l, s1 :: s2 :: ss ->
            mergepairs ((merge ord s1 s2)::l) ss
    fun l -> 
        if l = [] then [] 
        else mergepairs [] (List.map (fun x -> [x]) l)


// ------------------------------------------------------------------------- //
// Common measure predicates to use with "sort".                             //
// ------------------------------------------------------------------------- //

/// increasing predicate to use with "sort"
/// 
/// increasing List.length [1] [1;2] returns true`
let increasing f x y =
    compare (f x) (f y) < 0
    
/// decreasing predicate to use with "sort"
/// 
/// decreasing List.length [1;2] [1] returns true
let decreasing f x y =
    compare (f x) (f y) > 0