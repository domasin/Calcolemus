// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib.fs)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Functions to handle repetitions
[<AutoOpen>]
module FolAutomReas.Lib.Repetitions

// ------------------------------------------------------------------------- //
// Eliminate repetitions of adjacent elements, with and without counting.    //
// ------------------------------------------------------------------------- //

/// removes adjacent duplicated elements from a list
///
/// uniq [1;1;3;2;2] returns [1;3;2]
let rec uniq l =
    match l with
    | x :: (y :: _ as t) -> 
        let t' = uniq t
        if compare x y = 0 then t' 
        else
            if t' = t then l 
            else x :: t'
    | _ -> l

/// returns the number of repetitions in a list
/// 
/// repetitions [1;1;3;2;2] returns [(1,2);(3,1);(2,2)].
let repetitions =
    let rec repcount n l =
        match l with
        | [] -> failwith "repcount"
        | [x] -> [x,n]
        |  x :: (y :: _ as ys) -> 
            if compare y x = 0 then
                repcount (n + 1) ys
            else (x, n) :: (repcount 1 ys)
            
    fun l -> 
        if l = [] then [] 
        else repcount 1 l

/// Returns the result of the first successful application of a function `f` 
/// to the elements of a list `l`.
/// 
/// `tryfind f [x1;...;xn]` returns `f xi` for the first `xi` in the list for 
/// which application of `f` succeeds.
let rec tryfind f l =
    match l with
    | [] -> failwith "tryfind"
    | (h::t) -> try f h with _ -> tryfind f t