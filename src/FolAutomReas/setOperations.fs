// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib.fs)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Set operations on ordered lists.
[<AutoOpen>]
module FolAutomReas.Lib.SetOperations

open Sort

// ------------------------------------------------------------------------- //
// Set operations on ordered lists                                           //
// ------------------------------------------------------------------------- //

/// Removes repeated elements from a list. Makes a list into a ‘set’.
/// 
/// setify l removes repeated elements from l, leaving the last occurrence of 
/// each duplicate in the list.
/// 
/// <example>
/// <code>setify [1;2;3;1;4;3]</code> returns <code>[1;2;3;4]</code>
/// </example>
/// 
/// Never fails
let setify =
    let rec canonical lis =
        match lis with
        | x :: (y :: _ as rest) ->
            compare x y < 0
            && canonical rest
        | _ -> true
    fun l -> 
        if canonical l then 
            l
        else // try List.sortWith compare instead
            uniq (sort (fun x y -> compare x y <= 0) l)

/// Computes the union of two ‘sets’.
/// 
/// union l1 l2 returns a list consisting of the elements of l1 not already in 
/// l2 concatenated with l2. If l1 and l2 are initially free from duplicates, 
/// this gives a set-theoretic union operation. union [1;2;3] [1;5;4;3] returns 
/// [1;2;3;4;5]; union [1;1;1] [1;2;3;2] returns [1;2;3].
/// 
/// Never fails.
let union =
    let rec union l1 l2 =
        match l1, l2 with
        | [], l2 -> l2
        | l1, [] -> l1
        | (h1 :: t1 as l1), (h2 :: t2 as l2) ->
            if h1 = h2 then
                h1 :: (union t1 t2)
            elif h1 < h2 then
                h1 :: (union t1 l2)
            else
                h2 :: (union l1 t2)
    fun s1 s2 ->
        union (setify s1) (setify s2)
        
/// Computes the intersection of two ‘sets’.
/// 
/// intersect l1 l2 returns a list consisting of those elements of l1 that also 
/// appear in l2. If both sets are free of repetitions, this can be considered 
/// a set-theoretic intersection operation. intersect [1;2;3] [3;5;4;1] returns 
/// [1;3]; intersect [1;2;4;1] [1;2;3;2] returns [1;2].
/// 
/// Never fails.
let intersect =
    let rec intersect l1 l2 =
        match l1, l2 with
        | [], l2 -> []
        | l1, [] -> []
        | (h1 :: t1 as l1), (h2 :: t2 as l2) ->
            if h1 = h2 then
                h1 :: (intersect t1 t2)
            elif h1 < h2 then
                intersect t1 l2
            else
                intersect l1 t2
    fun s1 s2 ->
        intersect (setify s1) (setify s2)
        
/// Computes the set-theoretic difference of two ‘sets’
/// 
/// subtract l1 l2 returns a list consisting of those elements of l1 that do 
/// not appear in l2. If both lists are initially free of repetitions, this can 
/// be considered a set difference operation. subtract [1;2;3] [3;5;4;1] 
/// returns [2]; subtract [1;2;4;1] [4;5] returns [1;2]
/// 
/// Never fails.
let subtract =
    let rec subtract l1 l2 =
        match l1, l2 with
        | [], l2 -> []
        | l1, [] -> l1
        | (h1 :: t1 as l1), (h2 :: t2 as l2) ->
            if h1 = h2 then
                subtract t1 t2
            elif h1 < h2 then
                h1 :: (subtract t1 l2)
            else
                subtract l1 t2
    fun s1 s2 ->
        subtract (setify s1) (setify s2)
        
// pg. 620
let subset,psubset =
    let rec subset l1 l2 =
        match l1, l2 with
        | [], l2 -> true
        | l1, [] -> false
        | h1 :: t1, h2 :: t2 ->
            if h1 = h2 then subset t1 t2
            elif h1 < h2 then false
            else subset l1 t2
    and psubset l1 l2 =
        match l1, l2 with
        | l1, [] -> false
        | [], l2 -> true
        | h1 :: t1, h2 :: t2 ->
            if h1 = h2 then psubset t1 t2
            elif h1 < h2 then false
            else subset l1 t2
    (fun s1 s2 -> subset (setify s1) (setify s2)),
    (fun s1 s2 -> psubset (setify s1) (setify s2))

/// Tests two ‘sets’ for equality.
/// 
/// set_eq l1 l2 returns true if every element of l1 appears in l2 and every 
/// element of l2 appears in l1. Otherwise it returns false. In other words, it 
/// tests if the lists are the same considered as sets, i.e. ignoring 
/// duplicates. set_eq [1;2] [2;1;2] returns true; set_eq [1;2] [1;3] returns 
/// false.
/// 
/// Never fails.
let rec set_eq s1 s2 =
    setify s1 = setify s2
    
/// inserts one new element into a set
/// 
/// insert 4 [2;3;3;5] returns [2;3;4;5] 
let insert x s =
    union [x] s
    
// pg. 620
let image f s =
    setify (List.map f s)

// ------------------------------------------------------------------------- //
// Union of a family of sets.                                                //
// ------------------------------------------------------------------------- //

// pg. 620
let unions s =
    List.foldBack (@) s []
    |> setify

// ------------------------------------------------------------------------- //
// List membership. This does *not* assume the list is a set.                //
// ------------------------------------------------------------------------- //

// pg. 620
let rec mem x lis =
    match lis with
    | [] -> false
    | hd :: tl ->
        hd = x
        || mem x tl

// ------------------------------------------------------------------------- //
// Finding all subsets or all subsets of a given size.                       //
// ------------------------------------------------------------------------- //

// pg. 620
let rec allsets m l =
    if m = 0 then [[]]
    else
        match l with
        | [] -> []
        | h :: t ->
            union (image (fun g -> h :: g) (allsets (m - 1) t)) (allsets m t)
        
// pg. 620
let rec allsubsets s =
    match s with
    | [] -> [[]]
    | a :: t ->
        let res = allsubsets t
        union (image (fun b -> a :: b) res) res
                    
// pg. 620
let allnonemptysubsets s =
    subtract (allsubsets s) [[]]