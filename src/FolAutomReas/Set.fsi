// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

///<summary>
/// Set represented as ordered lists and related operations.
/// </summary>
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
    /// <example id="union-1">
    /// <code lang="union">
    /// union [1;2;3] [1;5;4;3]
    /// </code>
    /// Evaluates to <c>[1;2;3;4;5]</c>.
    /// </example>
    /// 
    /// <example id="union-1">
    /// <code lang="union">
    /// union [1;1;1] [1;2;3;2] 
    /// </code>
    /// Evaluates to <c>[1;2;3]</c>.
    /// </example>
    val union: l1: 'a list -> l2: 'a list -> 'a list when 'a: comparison

    /// Computes the intersection of two ‘sets’.
    /// 
    /// intersect l1 l2 returns a list consisting of those elements of l1 that also 
    /// appear in l2. If both sets are free of repetitions, this can be considered 
    /// a set-theoretic intersection operation. intersect [1;2;3] [3;5;4;1] returns 
    /// [1;3]; intersect [1;2;4;1] [1;2;3;2] returns [1;2].
    /// 
    /// Never fails.
    val intersect: ('a list -> 'a list -> 'a list) when 'a: comparison

    /// Computes the set-theoretic difference of two ‘sets’
    /// 
    /// subtract l1 l2 returns a list consisting of those elements of l1 that do 
    /// not appear in l2. If both lists are initially free of repetitions, this can 
    /// be considered a set difference operation. subtract [1;2;3] [3;5;4;1] 
    /// returns [2]; subtract [1;2;4;1] [4;5] returns [1;2]
    /// 
    /// Never fails.
    val subtract: ('a list -> 'a list -> 'a list) when 'a: comparison

    val subset: ('a list -> 'a list -> bool) when 'a: comparison

    val psubset: ('b list -> 'b list -> bool) when 'b: comparison

    /// Tests two ‘sets’ for equality.
    /// 
    /// set_eq l1 l2 returns true if every element of l1 appears in l2 and every 
    /// element of l2 appears in l1. Otherwise it returns false. In other words, it 
    /// tests if the lists are the same considered as sets, i.e. ignoring 
    /// duplicates. set_eq [1;2] [2;1;2] returns true; set_eq [1;2] [1;3] returns 
    /// false.
    /// 
    /// Never fails.
    val set_eq: s1: 'a list -> s2: 'a list -> bool when 'a: comparison

    /// inserts one new element into a set
    /// 
    /// insert 4 [2;3;3;5] returns [2;3;4;5] 
    val insert: x: 'a -> s: 'a list -> 'a list when 'a: comparison

    val image: f: ('a -> 'b) -> s: 'a list -> 'b list when 'b: comparison

    val unions: s: 'a list list -> 'a list when 'a: comparison

    val mem: x: 'a -> lis: 'a list -> bool when 'a: equality

    val allsets: m: int -> l: 'a list -> 'a list list when 'a: comparison

    val allsubsets: s: 'a list -> 'a list list when 'a: comparison

    val allnonemptysubsets: s: 'a list -> 'a list list when 'a: comparison