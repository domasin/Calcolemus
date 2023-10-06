// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Relation between FOL and propositional logic; Herbrand theorem.
module FolAutomReas.Herbrand

open FolAutomReas.Lib
open Functions

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
/// `groundterms [0;1] [(f,1);(g,2)] 0` returns `[0,1]`.
/// 
/// `groundterms [0;1] [(f,1);(g,2)] 1` returns `[f(0);f(1);g(0,0);g(0,1);g...]`
/// 
/// `groundterms [0;1] [(f,1);(g,1)] 2` returns `[f(f(0));...;f(g(0,0));...]`
let rec groundterms cntms funcs n =
    if n = 0 then cntms else
    List.foldBack (fun (f, m) l -> 
        List.map (fun args -> 
            Fn (f, args))
            (groundtuples cntms funcs (n - 1) m) @ l)
        funcs []

/// generates all `m`-tuples of ground terms involving (in total) `n` functions.
/// 
/// `groundtuples [0] [(f,1)] 1 1` returns `[[f(0)]]`
/// 
/// `groundtuples [0] [(f,1)] 1 2` returns `[[0;f(0)]; [f(0);0]]`
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

/// <summary>
/// A generic function to be used with different 'herbrand procedures'.
/// 
/// It tests larger and larger conjunctions of ground instances for 
/// unsatisfiability, iterating modifier `mfn` over ground terms 
/// till `tfn` fails. 
/// </summary>
/// <param name="mfn">The modification function that augments the ground 
/// instances with a new instance.</param>
/// <param name="tfn">The satisfiability test to be done.</param>
/// <param name="fl0">The initial formula in some transformed list 
/// representation.</param>
/// <param name="cntms">The constant terms of the formula.</param>
/// <param name="funcs">The functions (name, arity) of the formula.</param>
/// <param name="fvs">The free variables of the formula.</param>
/// <param name="n">The next level of the enumeration to generate.</param>
/// <param name="fl">The set of ground instances so far.</param>
/// <param name="tried">The instances tried.</param>
/// <param name="tuples">The remaining ground instances in the current level.
/// </param>
let rec herbloop mfn tfn fl0 cntms funcs fvs n fl tried tuples =
    printfn "%i ground instances tried; %i items in list."
        (List.length tried) (List.length fl) 
        // (fl |> List.map (fun xs -> xs |> List.map sprint_fol_formula)) 
        // de-comment to add log of fl list and add %A to printfn
    match tuples with
    // when tuples is empty, we simply generate the next level 
    // and step n up to n + 1
    | [] ->
        let newtups = groundtuples cntms funcs n (List.length fvs)
        herbloop mfn tfn fl0 cntms funcs fvs (n + 1) fl tried newtups
    | tup :: tups ->
        // we use the modification function to update fl with another instance
        let fl' = mfn fl0 (subst (fpf fvs tup)) fl
        // If this is unsatisfiable
        if not (tfn fl') then 
            // return the successful set of instances tried
            tup :: tried
        // otherwise 
        else 
            // continue
            herbloop mfn tfn fl0 cntms funcs fvs n fl' (tup :: tried) tups

// ------------------------------------------------------------------------- //
// A gilmore-like procedure                                                  //
// ------------------------------------------------------------------------- //

/// In the specific case of the gilmore procedure, the generic herbrand loop 
/// `herbloop` is called with the initial formula `fl0` and the ground 
/// instances so far `fl` are maintained in a DNF list representation and the 
/// modification function applies the instantiation to the starting formula 
/// and combines the DNFs by distribution.
let gilmore_loop =
    let mfn djs0 ifn djs =
        List.filter (non trivial) (distrib (image (image ifn) djs0) djs)
    herbloop mfn (fun djs -> djs <> [])

/// Tests an input fol formula `fm` for validity based on a gilmore-like 
/// procedure.
/// 
/// The initial formula is generalized, negated and Skolemized, then the 
/// specific herbrand loop for the gilmore procedure is called to test for 
/// the unsatisfiability of the transformed formula.
/// 
/// If the test terminates, it reports how many ground instances where tried.
let gilmore fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm
    let consts, funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (gilmore_loop (simpdnf sfm) cntms funcs fvs 0 [[]] [] [])

// ------------------------------------------------------------------------- //
// The Davis-Putnam procedure for first order logic.                         //
// ------------------------------------------------------------------------- //

/// <summary>
/// The modification function (specific to the Davis-Putnam procedure), that 
/// augments the ground instances with a new one.
/// </summary>
/// <example>
/// This example shows the first generation of ground instance when the set is 
/// initially empty.
/// <code lang="fsharp">
/// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"c"])) []
/// </code>
/// returns 
/// <code lang="fsharp">
/// [[P(c)]; [~P(f_y(c))]]
/// </code>
/// This example shows the second generation of ground instance when the 
/// nonempty set is augmented.
/// <code lang="fsharp">
/// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"f_y(c)"])) [[!!"P(c)"]; [!!"~P(f_y(c))"]]
/// </code>
/// returns 
/// <code lang="fsharp">
/// [[P(c)]; [P(f_y(c))]; [~P(f_y(c))]; [~P(f_y(f_y(c)))]]
/// </code>
/// </example>
/// <param name="cjs0">The initial formula in a list of list representation of conjunctive normal.</param>
/// <param name="ifn">The instantiation to be applied to the formula to generate ground instances.</param>
/// <param name="cjs">The set of ground instances so far.</param>
/// <returns>
/// The set of ground instances incremented.
/// </returns>
let dp_mfn cjs0 ifn cjs = union (image (image ifn) cjs0) cjs

/// In the specific case of the davis-putnam procedure, the generic 
/// herbrand loop `herbloop` is called with the initial formula `fl0` 
/// and the ground instances so far `fl` are maintained in a CNF list 
/// representation and each time we incorporate a new instance, we check for 
/// unsatisfiability using `dpll`.
let dp_loop = herbloop dp_mfn dpll

/// Tests an input fol formula `fm` for validity based on the Davis-Putnam 
/// procedure.
/// 
/// The initial formula is generalized, negated and Skolemized, then the 
/// specific herbrand loop for the davis-putnam procedure is called to test for 
/// the unsatisfiability of the transformed formula.
/// 
/// If the test terminates, it reports how many ground instances where tried.
let davisputnam fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm 
    let consts, funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (dp_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])

// ------------------------------------------------------------------------- //
// Try to cut out useless instantiations in final result.                    //
// ------------------------------------------------------------------------- //

/// Auxiliary function to redefine the Davis-Putnam procedure to run through 
/// the list of possibly-needed instances `dunno`, putting them onto the list 
/// of needed ones `need` only if the other instances are satisfiable.
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

// ------------------------------------------------------------------------- //
// Show how few of the instances we really need. Hence unification!          //
// ------------------------------------------------------------------------- //

/// Tests an input fol formula `fm` for validity based on the Davis-Putnam 
/// procedure redefined to run through the list of possibly-needed 
/// instances, putting them onto the list of needed ones only if 
/// the other instances are satisfiable.
let davisputnam002 fm =
    let sfm = skolemize (Not (generalize fm))
    let fvs = fv sfm 
    let consts,funcs = herbfuns sfm
    let cntms = image (fun (c, _) -> Fn (c, [])) consts
    List.length (dp_refine_loop (simpcnf sfm) cntms funcs fvs 0 [] [] [])
