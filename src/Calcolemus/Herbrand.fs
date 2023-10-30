// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Calcolemus.Lib.Function
open Calcolemus.Lib.List
open Calcolemus.Lib.Set
open Calcolemus.Lib.Fpf

open Formulas
open Prop
open DP
open Fol
open Skolem

module Herbrand = 

    // ---------------------------------------------------------------------- //
    // Propositional valuation.                                               //
    // ---------------------------------------------------------------------- //

    let pholds d fm = eval fm (fun p -> d (Atom p))

    // ---------------------------------------------------------------------- //
    // Herbrand models.                                                       //
    // ---------------------------------------------------------------------- //

    let herbfuns fm =
        let cns, fns = List.partition (fun (_, ar) -> ar = 0) (functions fm)
        if cns = [] then ["c", 0], fns else cns, fns

    // ---------------------------------------------------------------------- //
    // Enumeration of ground terms and m-tuples, ordered by total fns.        //
    // ---------------------------------------------------------------------- //

    let rec groundterms cntms funcs n =
        if n = 0 then cntms else
        List.foldBack (fun (f, m) l -> 
            List.map (fun args -> 
                Fn (f, args))
                (groundtuples cntms funcs (n - 1) m) @ l)
            funcs []

    and groundtuples cntms funcs n m =
        if m = 0 then 
            if n = 0 then [[]] 
            else [] 
        else
            List.foldBack (fun k l -> 
                allpairs (fun h t -> h :: t)
                    (groundterms cntms funcs k)
                    (groundtuples cntms funcs (n - k) (m - 1)) @ l)
                    [0..n] []

    let clausesToString = List.map (List.map sprint_fol_formula)

    let termListListToString = List.map (List.map sprint_term)

    let rec herbloop mfn tfn fl0 cntms funcs fvs n fl tried tuples =
        printfn "%i ground instances tried; %i items in list."
            (List.length tried) (List.length fl) 
        
        let flStr = clausesToString fl
        let triedStr = termListListToString tried
        let tuplesStr = termListListToString tuples
        // printfn "herbloop %i %A %A %A" n flStr triedStr tuplesStr

        match tuples with
        // if tuples is empty, generate the next level 
        // and step n up to n + 1
        | [] ->
            let newtups = groundtuples cntms funcs n (List.length fvs)
            herbloop mfn tfn fl0 cntms funcs fvs (n + 1) fl tried newtups
        // otherwise,
        | tup :: tups ->
            // use the modification function to update fl with another instance
            // printfn "instance: %A %A" fvs (List.map sprint_term tup)
            let fl' = mfn fl0 (subst (fpf fvs tup)) fl
            // If this is unsatisfiable
            if not (tfn fl') then 
                // return the successful set of instances tried
                tup :: tried
            // otherwise 
            else 
                // continue
                herbloop mfn tfn fl0 cntms funcs fvs n fl' (tup :: tried) tups

    // ---------------------------------------------------------------------- //
    // A gilmore-like procedure                                               //
    // ---------------------------------------------------------------------- //

    let gilmore_mfn djs0 ifn djs =

        let djs0Str = clausesToString djs0
        let djsStr = clausesToString djs
        // printfn "gilmore_mfn %A ifn %A" djs0Str djsStr

        let updatedDjs = (distrib (image (image ifn) djs0) djs)
        let contradictions = 
            updatedDjs 
            |> List.filter trivial
            |> clausesToString

        // printfn "ground instance: %A" 
        //     (updatedDjs |> clausesToString)

        // if contradictions |> List.length > 0 then
        //     printfn "contradictions: %A" contradictions

        updatedDjs
        |> List.filter (non trivial)

    let gilmore_tfn djs =
        djs |> List.length > 0

    let gilmore_loop fl0 cntms funcs fvs n fl tried tuples =
        herbloop gilmore_mfn gilmore_tfn fl0 cntms funcs fvs n fl tried tuples 

    let gilmore fm =
        let sfm = skolemize (Not (generalize fm))
        let fvs = fv sfm
        let consts, funcs = herbfuns sfm
        let cntms = image (fun (c, _) -> Fn (c, [])) consts
        List.length (gilmore_loop (simpdnf sfm) cntms funcs fvs 0 [[]] [] [])

    // ---------------------------------------------------------------------- //
    // The Davis-Putnam procedure for first order logic.                      //
    // ---------------------------------------------------------------------- //

    let dp_mfn cjs0 ifn cjs = 
        union (image (image ifn) cjs0) cjs

    let dp_loop fl0 cntms funcs fvs n fl tried tuples = 
        herbloop dp_mfn dpll fl0 cntms funcs fvs n fl tried tuples

    let davisputnam fm =
        let sfm = skolemize (Not (generalize fm))
        let fvs = fv sfm 
        let consts, funcs = herbfuns sfm
        let cntms = image (fun (c, _) -> Fn (c, [])) consts
        List.length (dp_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])

    // ---------------------------------------------------------------------- //
    // Try to cut out useless instantiations in final result.                 //
    // ---------------------------------------------------------------------- //

    let rec dp_refine cjs0 fvs dunno need =
        // let cjs0Str = clausesToString cjs0
        // let dunnoStr = termListListToString dunno
        // let needStr = termListListToString need
        // printfn "dp_refine %A %A %A %A" cjs0Str fvs dunnoStr needStr

        match dunno with
        | [] -> need
        | cl :: dknow ->
            let mfn = dp_mfn cjs0 << subst << fpf fvs
            let need' =
                if dpll (List.foldBack mfn (need @ dknow) []) then 
                    cl :: need
                else 
                    need
            dp_refine cjs0 fvs dknow need'

    let dp_refine_loop cjs0 cntms funcs fvs n cjs tried tuples =
        let tups = dp_loop cjs0 cntms funcs fvs n cjs tried tuples
        dp_refine cjs0 fvs tups []

    // ---------------------------------------------------------------------- //
    // Show how few of the instances we really need. Hence unification!       //
    // ---------------------------------------------------------------------- //

    let davisputnam002 fm =
        let sfm = skolemize (Not (generalize fm))
        let fvs = fv sfm 
        let consts,funcs = herbfuns sfm
        let cntms = image (fun (c, _) -> Fn (c, [])) consts
        List.length (dp_refine_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])
