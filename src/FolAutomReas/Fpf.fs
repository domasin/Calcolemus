// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (modified for ease of documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

open FolAutomReas.Lib.Set

[<AutoOpen>]
module Fpf = 

    type func<'a,'b> =
        | Empty
        | Leaf of int * ('a * 'b) list
        | Branch of int * int * func<'a,'b> * func<'a,'b>
    
    let undefined = Empty
    
    let is_undefined = function
        | Empty -> true
        | _     -> false
    
    let mapf mapping fpf =
        let rec map_list mapping list =
            match list with
            | [] -> []
            | (arg, value) :: t ->
                (arg, mapping value) :: (map_list mapping t)
        let rec mapf mapping fpf =
            match fpf with
            | Empty -> Empty
            | Leaf (hash, list) ->
                Leaf (hash, map_list mapping list)
            | Branch (p, b, left, right) ->
                Branch (p, b, mapf mapping left, mapf mapping right)
        mapf mapping fpf
    
    let foldl folder state fpf =
        let rec foldl_list folder state list =
            match list with
            | [] -> state
            | (arg, value) :: t ->
                foldl_list folder (folder state arg value) t
        let rec foldl folder state fpf =
            match fpf with
            | Empty -> state
            | Leaf (hash, list) ->
                foldl_list folder state list
            | Branch (p, b, left, right) ->
                foldl folder (foldl folder state left) right
        foldl folder state fpf 
    
    let graph fpf =
        foldl (fun acc arg value -> (arg, value) :: acc) [] fpf
        |> setify
        
    let dom fpf =
        foldl (fun acc arg _ -> arg :: acc) [] fpf
        |> setify
        
    let ran fpf =
        foldl (fun acc _ value -> value :: acc) [] fpf
        |> setify

    let applyd fpf d a =
        // search x as a fst elem of pairs in list l.
        // If it is there, returns the snd elem of the same pair;
        // otherwise, returns function d applied to d.
        let rec apply_listd l d x =
            match l with
            // if list is empty, returns d applied to x
            | [] -> d x
            | (a, b) :: tl ->
                let c = compare x a
                // otherwise, if x equals the fst elem of pair, 
                // returns the snd elem
                if c = 0 then b
                // otherwise, if x follows (in the default order) the fs 
                // elem of pair, search for a matching in the following pairs
                elif c > 0 then apply_listd tl d x
                // otherwise, returns d applied to x
                else d x
                
        let k = hash a
        // search x in fpf
        let rec look fpf =
            match fpf with
            // if fpf is a leaf and x's hash equals leaf's hash,
            // search x in leaf's list, with apply_listd
            | Leaf (h, l) when h = k ->
                apply_listd l d a
            | Branch (p, b, l, r) when (k ^^^ p) &&& (b - 1) = 0 ->
                if k &&& b = 0 then l else r
                |> look
            | _ -> d a
        look fpf
    
    let apply fpf a =
        applyd fpf (fun _ -> failwith "apply") a
    
    let tryapplyd fpf a d =
        applyd fpf (fun _ -> d) a
    
    let tryapplyl fpf a =
        tryapplyd fpf a []
        
    let defined fpf a =
        try
            apply fpf a |> ignore
            true
        with
        | Failure _ -> false
    
    let undefine a fpf =
        let rec undefine_list x l =
            match l with
            | [] -> []
            | (a, b as ab) :: t ->
                    let c = compare x a
                    if c = 0 then t
                    elif c < 0 then l
                    else
                        let t' = undefine_list x t
                        if t' = t then l
                        else ab :: t'
                                  
        let undefineAux x fpf = 
            let k = hash x
            let rec und t =
                match t with
                | Leaf (h, l) when h = k ->
                    let l' = undefine_list x l
                    if l' = l then t
                    elif l' = [] then Empty
                    else Leaf (h, l')
    
                | Branch (p, b, l, r) when k &&& (b - 1) = p ->
                    if k &&& b = 0 then
                        let l' = und l
                        if l' = l then t
                        else
                            match l' with
                            | Empty -> r
                            | _ -> Branch (p, b, l', r)
                    else
                        let r' = und r
                        if r' = r then t
                        else
                            match r' with
                            | Empty -> l
                            | _ -> Branch (p, b, l, r')
                | _ -> t
            und fpf

        undefineAux a fpf

    let newbranch p1 t1 p2 t2 =
        let zp = p1 ^^^ p2
        let b = zp &&& -zp
        let p = p1 &&& (b - 1)
        if p1 &&& b = 0 then Branch (p, b, t1, t2)
        else Branch (p, b, t2, t1)
    
    let rec define_list (x, y as xy) l =
        match l with
        | [] -> [xy]
        | (a, b as ab) :: t ->
            let c = compare x a
            if c = 0 then xy :: t
            elif c < 0 then xy :: l
            else ab :: (define_list xy t)
    
    // it seems unused
    // and combine_list op z l1 l2 =
    //     match l1, l2 with
    //     | [], x
    //     | x, [] -> x
    //     | ((x1, y1 as xy1) :: t1, (x2, y2 as xy2) :: t2) ->
    //         let c = compare x1 x2
    //         if c < 0 then xy1 :: (combine_list op z t1 l2)
    //         elif c > 0 then xy2 :: (combine_list op z l1 t2)
    //         else
    //             let y = op y1 y2
    //             let l = combine_list op z t1 t2
    //             if z y then l
    //             else (x1, y) :: l
    
    let (|->) argument value fpf =
        let k = hash argument
        let rec upd t =
            match t with
            // If fpf is undefined,
            | Empty -> 
                // add a leaf with the input mapping.
                Leaf (k, [argument, value])
            // If fpf has just one mapping,
            | Leaf (h, l) ->
                // if its argument equals the input one,
                if h = k then 
                    // update the old vale with the new one;
                    Leaf (h, define_list (argument, value) l)
                else 
                    // otherwise, create a new branch.
                    newbranch h t k (Leaf (k, [argument, value]))
            // If fpf has more the one mapping,
            | Branch (p, b, l, r) ->
                // if fpf is not defined for the input argument,
                if k &&& (b - 1) <> p then 
                    // create a new branch;
                    newbranch p t k (Leaf (k, [argument, value]))
                // otherwise, if the input argument is in the left branch,
                elif k &&& b = 0 then 
                    // update the left branch;
                    Branch (p, b, upd l, r)
                // otherwise, update the right branch.
                else 
                    Branch (p, b, l, upd r)
        upd fpf
    
    // it seems unused
    // let rec combine op z t1 t2 =
    //     match t1, t2 with
    //     | Empty, x
    //     | x, Empty -> x
    //     | Leaf (h1, l1), Leaf (h2, l2) ->
    //         if h1 = h2 then
    //             let l = combine_list op z l1 l2
    //             if l = [] then Empty
    //             else Leaf (h1, l)
    //         else newbranch h1 t1 h2 t2
    
    //     | (Leaf (k, lis) as lf), (Branch (p, b, l, r) as br) ->
    //         if k &&& (b - 1) = p then
    //             if k &&& b = 0 then
    //                 match combine op z lf l with
    //                 | Empty -> r
    //                 | l' -> Branch (p, b, l', r)
    //             else
    //                 match combine op z lf r with
    //                 | Empty -> l
    //                 | r' -> Branch (p, b, l, r')
    //         else
    //             newbranch k lf p br
    
    //     | (Branch (p, b, l, r) as br), (Leaf (k, lis) as lf) ->
    //         if k &&& (b - 1) = p then
    //             if k &&& b = 0 then
    //                 match combine op z l lf with
    //                 | Empty -> r
    //                 | l' -> Branch (p, b, l', r)
    //             else
    //                 match combine op z r lf with
    //                 | Empty -> l
    //                 | r' -> Branch (p, b, l, r')
    //         else
    //             newbranch p br k lf
    
    //     | Branch (p1, b1, l1, r1), Branch (p2, b2, l2, r2) ->
    //         if b1 < b2 then
    //             if p2 &&& (b1 - 1) <> p1 then
    //                 newbranch p1 t1 p2 t2
    //             elif p2 &&& b1 = 0 then
    //                 match combine op z l1 t2 with
    //                 | Empty -> r1
    //                 | l -> Branch (p1, b1, l, r1)
    //             else
    //                 match combine op z r1 t2 with
    //                 | Empty -> l1
    //                 | r -> Branch (p1, b1, l1, r)
    
    //         elif b2 < b1 then
    //             if p1 &&& (b2 - 1) <> p2 then
    //                 newbranch p1 t1 p2 t2
    //             elif p1 &&& b2 = 0 then
    //                 match combine op z t1 l2 with
    //                 | Empty -> r2
    //                 | l -> Branch (p2, b2, l, r2)
    //             else
    //                 match combine op z t1 r2 with
    //                 | Empty -> l2
    //                 | r -> Branch (p2, b2, l2, r)
    
    //         elif p1 = p2 then
    //             match combine op z l1 l2, combine op z r1 r2 with
    //             | Empty, x
    //             | x, Empty -> x
    //             | l, r ->
    //                 Branch (p1, b1, l, r)
    //         else
    //             newbranch p1 t1 p2 t2
    
    let (|=>) a b = 
        (a |-> b) undefined
 
    let fpf xs ys =
        List.foldBack2 (|->) xs ys undefined
    
    // it seems unused.
    // // Grab an arbitrary element
    // let rec choose t =
    //     match t with
    //     | Empty ->
    //         failwith "choose: completely undefined function"
    //     | Leaf (_, l) ->
    //         // NOTE : This will fail (crash) when 'l' is an empty list!
    //         List.head l
    //     | Branch (b, p, t1, t2) ->
    //         choose t1
    
    let valmod a b f x =
        if x = a then b
        else f x
        
    let undef x =
        failwith "undefined function"