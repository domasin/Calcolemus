// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Handy functions for list manipulation
[<AutoOpen>]
module FolAutomReas.Lib.ListFunctions

open FolAutomReas.Lib.Num

// --------------------------------------------------------------------//
// List operations.                                                    //
// --------------------------------------------------------------------//

/// Gives a finite list of integers between the given bounds.
/// 
/// The call m--n returns the list of consecutive numbers from m to n.
let inline (--) (m : int) (n : int) = [m..n]

// using built-in F# range expression.
/// Gives a finite list of nums between the given bounds.
/// 
/// The call m--n returns the list of consecutive numbers from m to n.
let inline (---) (m : num) (n : num) = [m..n]

// NOTE: map2 has been replaced with the equivalent built-in F# function List.map2.

// NOTE: rev has been replaced with the equivalent built-in F# function List.rev.

// NOTE: hd has been replaced with the equivalent built-in F# function List.head.
        
// NOTE: tl has been replaced with the equivalent built-in F# function List.tail.

// NOTE: itlist has been replaced with the equivalent built-in F# function List.foldBack.
        
// NOTE: end_itlist has been replaced with the equivalent built-in F# function List.reduceBack.
        
// NOTE: itlist2 has been replaced with the equivalent built-in F# function List.foldBack2.
        
// NOTE: zip has been replaced with the equivalent built-in F# function List.zip.

// NOTE: forall has been replaced with the equivalent built-in F# function List.forall.
        
// NOTE: exists has been replaced with the equivalent built-in F# function List.exists.
        
// NOTE: partition has been replaced with the equivalent built-in F# function List.partition.

// NOTE: filter has been replaced with the equivalent built-in F# function List.filter.

// NOTE: length has been replaced with the equivalent built-in F# function List.length.
        
/// Computes the last element of a list.
/// 
/// last [x1;...;xn] returns xn.
/// 
/// Fails with last if the list is empty.
let rec last l =
    match l with
    | [] ->
        failwith "last"
    | [x] -> x
    | _ :: tl ->
        last tl

/// Computes the sub-list of a list consisting of all but the last element.
/// 
/// butlast [x1;...;xn] returns [x1;...;x(n-1)].
/// 
/// Fails if the list is empty.
let rec butlast l =
    match l with
    | [_]    -> []
    | (h::t) -> h::(butlast t)
    | []     -> failwith "butlast"
        
// NOTE: find has been replaced with the equivalent built-in F# function List.find.

// In comparing exceptions in terms of computing time, 
// Ocaml's exceptions are inexpensive in comparison with F# exceptions.
// To avoid exceptions from F# List.find, use F# List.tryFind which
// does not return an exception if an item is not found.

// NOTE: el has been replaced with the equivalent built-in F# function List.nth.

// NOTE: map has been replaced with the equivalent built-in F# function List.map.
        
/// Compute list of all results from applying function to pairs from two lists.
/// 
/// The call allpairs f [x1;...;xm] [y1;...;yn] returns the list of results 
/// [f x1 y1; f x1 y2; ... ; f x1 yn; fx2 y1 ; f x2 y2 ...; f xm y1; 
/// f xm y2 ...; f xm yn ]
/// 
/// Never fails.
let rec allpairs f l1 l2 =
    match l1 with
    | [] -> []
    | h1 :: t1 ->
        List.foldBack (fun x a -> f h1 x :: a) l2 (allpairs f t1 l2)

/// produces all pairs of distinct elements from a single list
/// 
/// distinctpairs [1;2;3;4] returns [(1, 2); (1, 3); (1, 4); (2, 3); (2, 4); (3, 4)]
let rec distinctpairs l =
    match l with
    | [] -> []
    | x :: t ->
        List.foldBack (fun y a -> (x, y) :: a) t (distinctpairs t)
        
/// Chops a list into two parts at a specified point.
/// 
/// chop_list i [x1;...;xn] returns ([x0;...;xi-1],[xi;...;xn]).
/// 
/// Fails with chop_list if n is negative or greater than the length of the list.
let rec chop_list n l =
    if n = 0 then [], l
    else
        try
            let m, l' = chop_list (n - 1) (List.tail l) 
            (List.head l) :: m, l'
        with _ ->
            failwith "chop_list"
        
// NOTE: replicate is not used in code.
    
/// Adds an element in a list at the specified index
/// 
/// insertat 2 999 [0;1;2;3] returns [0; 1; 999; 2; 3]
/// 
/// Fails if index is negative or exceeds the list length-1
let rec insertat i x l =
    if i = 0 then x :: l
    else
        match l with
        | [] -> failwith "insertat: list too short for position to exist"
        | h :: t ->
            h :: (insertat (i - 1) x t)
        
// NOTE: forall2 has been replaced with the equivalent built-in F# function List.forall2.
        
/// Returns position of given element in list.
/// 
/// The call index x l where l is a list returns the position number of the 
/// first instance of x in the list, failing if there is none. The indices 
/// start at zero, corresponding to el.
let inline index x xs = List.findIndex ((=) x) xs
        
// NOTE: upzip has been replaced with the equivalent built-in F# function List.unzip.

// ------------------------------------------------------------------------- //
// Whether the first of two items comes earlier in the list.                 //
// ------------------------------------------------------------------------- //

/// Checks if x comes earlier than y in list l
/// 
/// earlier [0;1;2;3] 2 3 return true, earlier [0;1;2;3] 3 2 false.
/// earlier returns false also if x or y is not in the list.
let rec earlier l x y =
    match l with
    | [] -> false
    | h :: t ->
        (compare h y <> 0) && (compare h x = 0 || earlier t x y)
        

// ------------------------------------------------------------------------- //
// Application of (presumably imperative) function over a list.              //
// ------------------------------------------------------------------------- //

// NOTE: do_list has been replaced with the built-in F# function List.iter.