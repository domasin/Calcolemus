// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Relation between FOL and propositional logic; Herbrand theorem.
module FolAutomReas.Herbrand

open FolAutomReas.Lib

open Formulas
open Prop
open Dp
open Fol
open Skolem

// ------------------------------------------------------------------------- //
// Propositional valuation.                                                  //
// ------------------------------------------------------------------------- //

/// A variant of the notion of propositional evaluation `eval` where the 
/// input propositional valuation `d` maps atomic formulas themselves to 
/// truth values.
/// 
/// It determines if the input formula `fm` holds in the sense of propositional 
/// logic for this notion of valuation.
/// 
/// `pholds (function Atom (R ("P", [Var "x"])) -> true) (parse "P(x)")`
/// returns `true`
let pholds d fm = eval fm (fun p -> d (Atom p))

// ------------------------------------------------------------------------- //
// Herbrand models.                                                          //
// ------------------------------------------------------------------------- //

/// Gets the constants for Herbrand base, adding nullary one if necessary. 
let herbfuns fm =
    let cns, fns = List.partition (fun (_, ar) -> ar = 0) (functions fm)
    if cns = [] then ["c", 0], fns else cns, fns

// ------------------------------------------------------------------------- //
// Enumeration of ground terms and m-tuples, ordered by total fns.           //
// ------------------------------------------------------------------------- //

/// Enumerates all ground terms involving `n` functions.
/// 
/// If `n` = 0, it returns the constant terms, otherwise tries all possible 
/// functions.
/// 
/// `groundterms [!|"0";!|"1"] ["f",1;"g",1] 0`
/// returns `[<<|0|>>; <<|1|>>]`.
/// 
/// `groundterms [!|"0";!|"1"] ["f",1;"g",1] 1`
/// returns `[<<|f(0)|>>; <<|f(1)|>>; <<|g(0)|>>; <<|g(1)|>>]`
let rec groundterms cntms funcs n =
    if n = 0 then cntms else
    List.foldBack (fun (f, m) l -> 
        List.map (fun args -> 
            Fn (f, args))
            (groundtuples cntms funcs (n - 1) m) @ l)
        funcs []
/// generates all `m`-tuples of ground terms involving (in total) `n` functions.
/// 
/// `groundtuples [!|"0";] ["f",1] 1 1` returns `[[<<|f(0)|>>]]`
/// 
/// `groundtuples [!|"0";] ["f",1] 1 2` returns 
/// `[[<<|0|>>; <<|f(0)|>>]; [<<|f(0)|>>; <<|0|>>]]`
and groundtuples cntms funcs n m =
    if m = 0 then 
        if n = 0 then [[]] 
        else [] 
    else
        List.foldBack (fun k l -> 
            allpairs (fun h t -> h :: t)
                (groundterms cntms funcs k)
                (groundtuples cntms funcs (n - k) (m - 1)) @ l)
                (0 -- n) []

/// A generic function to be used with different sat procedures.
/// 
/// It iterates modifier "mfn" over ground terms till "tfn" fails. 
let rec herbloop mfn tfn fl0 cntms funcs fvs n fl tried tuples =
    printfn "%i ground instances tried; %i items in list."
        (List.length tried) (List.length fl)

    match tuples with
    | [] ->
        let newtups = groundtuples cntms funcs n (List.length fvs)
        herbloop mfn tfn fl0 cntms funcs fvs (n + 1) fl tried newtups
    | tup :: tups ->
        let fl' = mfn fl0 (subst (fpf fvs tup)) fl
        if not (tfn fl') then tup :: tried
        else herbloop mfn tfn fl0 cntms funcs fvs n fl' (tup :: tried) tups

// pg. 160
// ------------------------------------------------------------------------- //
// Hence a simple Gilmore-type procedure.                                    //
// ------------------------------------------------------------------------- //

let gilmore_loop =
    let mfn djs0 ifn djs =
        List.filter (non trivial) (distrib (image (image ifn) djs0) djs)
    herbloop mfn (fun djs -> djs <> [])

let gilmore fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm
    let consts, funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (gilmore_loop (simpdnf sfm) cntms funcs fvs 0 [[]] [] [])

// pg. 163
// ------------------------------------------------------------------------- //
// The Davis-Putnam procedure for first order logic.                         //
// ------------------------------------------------------------------------- //

let dp_mfn cjs0 ifn cjs = union (image (image ifn) cjs0) cjs

let dp_loop = herbloop dp_mfn dpll

let davisputnam fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm 
    let consts, funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (dp_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])

// pg. 163
// ------------------------------------------------------------------------- //
// Try to cut out useless instantiations in final result.                    //
// ------------------------------------------------------------------------- //

let rec dp_refine cjs0 fvs dunno need =
    match dunno with
    | [] -> need
    | cl :: dknow ->
        let mfn = dp_mfn cjs0 << subst << fpf fvs
        let need' =
            if dpll (List.foldBack mfn (need @ dknow) []) then cl :: need
            else need
        dp_refine cjs0 fvs dknow need'

let dp_refine_loop cjs0 cntms funcs fvs n cjs tried tuples =
    let tups = dp_loop cjs0 cntms funcs fvs n cjs tried tuples
    dp_refine cjs0 fvs tups []

// pg. 163
// ------------------------------------------------------------------------- //
// Show how few of the instances we really need. Hence unification!          //
// ------------------------------------------------------------------------- //

let davisputnam002 fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm 
    let consts,funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (dp_refine_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])
