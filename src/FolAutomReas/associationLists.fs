// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Association Lists
[<AutoOpen>]
module FolAutomReas.Lib.AssociationLists

// ------------------------------------------------------------------------- //
// Association lists.                                                        //
// ------------------------------------------------------------------------- //

/// Searches a list of pairs for a pair whose first component equals a 
/// specified value. 
/// 
/// assoc x [(x1,y1);...;(xn,yn)] returns the first yi in the list such that xi
/// equals x.
/// 
/// Fails if no matching pair is found. This will always be the case if the list is empty.
let rec assoc a l =
    match l with
    | [] -> failwith "find"
    | (x, y) :: t ->
        if compare x a = 0 then y
        else assoc a t

/// Searches a list of pairs for a pair whose second component equals a 
/// specified value.
/// 
/// rev_assoc y [(x1,y1);...;(xn,yn)] returns the first xi in the list such 
/// that yi equals y.
/// 
/// Fails if no matching pair is found. This will always be the case if the 
/// list is empty.
let rec rev_assoc a l =
    match l with
    | [] -> failwith "find"
    | (x, y) :: t ->
        if compare y a = 0 then x
        else rev_assoc a t