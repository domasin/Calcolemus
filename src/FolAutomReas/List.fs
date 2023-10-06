// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (adapted from lib for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

open FolAutomReas.Lib.Num

module List = 

    let rec last l =
        match l with
        | [] ->
            failwith "last"
        | [x] -> x
        | _ :: tl ->
            last tl

    let rec butlast l =
        match l with
        | [_]    -> []
        | (h::t) -> h::(butlast t)
        | []     -> failwith "butlast"

    let rec allpairs f l1 l2 =
        match l1 with
        | [] -> []
        | h1 :: t1 ->
            List.foldBack (fun x a -> f h1 x :: a) l2 (allpairs f t1 l2)

    let rec distinctpairs l =
        match l with
        | [] -> []
        | x :: t ->
            List.foldBack (fun y a -> (x, y) :: a) t (distinctpairs t)

    let rec chop_list n l =
        if n = 0 then 
            [], l
        else
            try
                let m, l' = chop_list (n - 1) (List.tail l) 
                (List.head l) :: m, l'
            with _ ->
                failwith "chop_list"

    let rec insertat i x l =
        if i = 0 then 
            x :: l
        else
            match l with
            | [] -> failwith "insertat: list too short for position to exist"
            | h :: t ->
                h :: (insertat (i - 1) x t)

    let inline index x xs = List.findIndex ((=) x) xs

    let rec earlier l x y =
        match l with
        | [] -> false
        | h :: t ->
            (compare h y <> 0) && (compare h x = 0 || earlier t x y)
