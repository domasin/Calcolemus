// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (adapted from lib for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

module Sort = 

    // ---------------------------------------------------------------------- //
    // Merging of sorted lists (maintaining repetitions).                     //
    // ---------------------------------------------------------------------- //
    
    let rec merge comparer l1 l2 =
        match l1, l2 with
        | [], x
        | x, [] -> x
        | h1 :: t1, h2 :: t2 ->
            if comparer h1 h2 then
                h1 :: (merge comparer t1 l2)
            else
                h2 :: (merge comparer l1 t2)
    
    // ---------------------------------------------------------------------- //
    // Bottom-up mergesort.                                                   //
    // ---------------------------------------------------------------------- //
    
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
    
    
    // ---------------------------------------------------------------------- //
    // Common measure predicates to use with "sort".                          //
    // ---------------------------------------------------------------------- //
    
    let increasing f x y =
        compare (f x) (f y) < 0
        
    let decreasing f x y =
        compare (f x) (f y) > 0