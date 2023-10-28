// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

module Search = 

    let rec tryfind f l =
        match l with
        | [] ->
            failwith "tryfind"
        | h :: t ->
            try f h
            with _ ->
                tryfind f t
    
    let rec mapfilter f l =
        match l with
        | [] -> []
        | h :: t ->
            let rest = mapfilter f t
            try (f h) :: rest
            with _ -> rest

    // ---------------------------------------------------------------------- //
    // Find list member that maximizes or minimizes a function.               //
    // ---------------------------------------------------------------------- //

    let optimize ord f l =
        l
        |> List.map (fun x -> x, f x)
        |> List.reduceBack (fun (_, y1 as p1) (_, y2 as p2) ->
            if ord y1 y2 then p1 else p2)
        |> fst

    let maximize f l =
        optimize (>) f l

    let minimize f l =
        optimize (<) f l