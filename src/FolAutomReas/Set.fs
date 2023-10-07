// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib.fs)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

module Set = 

    // ---------------------------------------------------------------------- //
    // Set operations on ordered lists                                        //
    // ---------------------------------------------------------------------- //

    let setify l =
        let rec canonical lis =
            match lis with
            | x :: (y :: _ as rest) ->
                compare x y < 0
                && canonical rest
            | _ -> true

        if canonical l then 
            l
        else
            List.distinct (List.sort l)

    let union l1 l2 =
        Set.union (l1 |> Set.ofList) (l2 |> Set.ofList)
        |> Set.toList
        // let rec union l1 l2 =
        //     match l1, l2 with
        //     | [], l2 -> l2
        //     | l1, [] -> l1
        //     | (h1 :: t1 as l1), (h2 :: t2 as l2) ->
        //         if h1 = h2 then
        //             h1 :: (union t1 t2)
        //         elif h1 < h2 then
        //             h1 :: (union t1 l2)
        //         else
        //             h2 :: (union l1 t2)

        // union (setify l1) (setify l2)

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

    let rec set_eq s1 s2 =
        setify s1 = setify s2

    let insert x s =
        union [x] s

    let image f s =
        setify (List.map f s)

    // ---------------------------------------------------------------------- //
    // Union of a family of sets.                                             //
    // ---------------------------------------------------------------------- //

    let unions s =
        List.foldBack (@) s []
        |> setify

    // ---------------------------------------------------------------------- //
    // List membership. This does *not* assume the list is a set.             //
    // ---------------------------------------------------------------------- //

    let rec mem x lis =
        match lis with
        | [] -> false
        | hd :: tl ->
            hd = x
            || mem x tl

    // ---------------------------------------------------------------------- //
    // Finding all subsets or all subsets of a given size.                    //
    // ---------------------------------------------------------------------- //

    let rec allsets m l =
        if m = 0 then [[]]
        else
            match l with
            | [] -> []
            | h :: t ->
                union (image (fun g -> h :: g) (allsets (m - 1) t)) (allsets m t)

    let rec allsubsets s =
        match s with
        | [] -> [[]]
        | a :: t ->
            let res = allsubsets t
            union (image (fun b -> a :: b) res) res

    let allnonemptysubsets s =
        subtract (allsubsets s) [[]]