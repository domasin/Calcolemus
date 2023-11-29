// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Lib.Function
open Lib.List
open Lib.Set
open Lib.Fpf

open Formulas
open Fol
open Unif
open Equal
open Rewrite
open Order

module Completion = 

    // ====================================================================== //
    // Knuth-Bendix completion.                                               //
    // ====================================================================== //

    let renamepair (fm1,fm2) =
        let fvs1 = fv fm1
        let fvs2 = fv fm2
        let nms1, nms2 = 
            chop_list (List.length fvs1)
                (List.map (fun n -> Var ("x" + string n))
                    [0..(List.length fvs1 + List.length fvs2 - 1)])
        subst (fpf fvs1 nms1) fm1, subst (fpf fvs2 nms2) fm2
    
    // ---------------------------------------------------------------------- //
    // Rewrite (using unification) with l = r inside tm to give a critical    //
    // pair.                                                                  //
    // ---------------------------------------------------------------------- //

    let rec listcases fn rfn lis acc =
        match lis with
        | [] -> acc
        | h :: t ->
            fn h (fun i h' ->
                rfn i (h' :: t))
            @ listcases fn (fun i t' ->
                rfn i (h :: t'))
                t acc

    let traceOverlaps l r f args tm = 
        printfn 
            """listcases (overlaps ("%s", "%s")) (fun i a -> rfn i (Fn ("%s",a))) %A
                    (try [rfn (fullunify ["%s", ">%s"]) "%s"] with Failure _ -> [])""" 
            (sprint_term l) (sprint_term r) f (args |> List.map sprint_term) (sprint_term l) (sprint_term tm) (sprint_term r)
    let rec overlaps (l, r) tm rfn =
        match tm with
        | Var x -> []
        | Fn (f, args) ->
            // traceOverlaps l r f args tm
            listcases (overlaps (l, r)) (fun i a -> rfn i (Fn (f, a))) args
                (try [rfn (fullunify [l, tm]) r] with Failure _ -> [])
    
    // ---------------------------------------------------------------------- //
    // Generate all critical pairs between two equations.                     //
    // ---------------------------------------------------------------------- //

    let traceCrit1 l1 r1 l2 r2 = 
        printfn 
            """overlaps ("%s","%s") "%s" (fun i t -> subst i (mk_eq t "%s"))""" 
            (l1 |> sprint_term) (r1 |> sprint_term) (l2 |> sprint_term) (r2 |> sprint_term)

    let crit1 eq1 eq2 =
        match eq1, eq2 with
        | (Atom (R ("=", [l1;r1]))), (Atom (R ("=", [l2;r2]))) -> 
            // traceCrit1 l1 r1 l2 r2
            overlaps (l1,r1) l2 (fun i t -> subst i (mk_eq t r2))
        | _ -> failwith "crit1: incomplete pattern matching"  

    let critical_pairs eq1 eq2 =
        let fm1, fm2 = renamepair (eq1, eq2)
        if eq1 = eq2 then crit1 fm1 fm2
        else union (crit1 fm1 fm2) (crit1 fm2 fm1)

    // ---------------------------------------------------------------------- //
    // Orienting an equation.                                                 //
    // ---------------------------------------------------------------------- //

    let normalize_and_orient ord eqs atm =
        match atm with
        | (Atom (R ("=", [s;t])))  -> 
            let s' = rewrite eqs s
            let t' = rewrite eqs t
            if ord s' t' then s', t'
            elif ord t' s' then t', s'
            else failwith "Can't orient equation"
        | _ -> failwith "normalize_and_orient: incomplete pattern matching" 
    
    // ---------------------------------------------------------------------- //
    // Status report so the user doesn't get too bored.                       //
    // ---------------------------------------------------------------------- //

    let status (eqs, def, crs) eqs0 =
        if not (eqs = eqs0 && (List.length crs) % 1000 <> 0) then
            printfn "%i equations and %i pending critical pairs + %i deferred"
                (List.length eqs) (List.length crs) (List.length def)
    
    // ---------------------------------------------------------------------- //
    // Completion main loop (deferring non-orientable equations).             //
    // ---------------------------------------------------------------------- //

    let rec complete ord (eqs,def,crits) =
        match crits with
        | eq :: ocrits ->
            let trip =
                try
                    let s', t' = normalize_and_orient ord eqs eq
                    if s' = t' then
                        eqs, def, ocrits
                    else
                        let eq' = Atom (R ("=", [s'; t']))
                        let eqs' = eq' :: eqs
                        eqs', def, ocrits @ List.foldBack ((@) << critical_pairs    eq') eqs' []
                with Failure _ ->
                    eqs, eq :: def, ocrits
            status trip eqs
            complete ord trip
        | _ -> 
            if def = [] then eqs
            else
                let e = List.find (can (normalize_and_orient ord eqs)) def
                complete ord (eqs, subtract def [e], [e])

    // ---------------------------------------------------------------------- //
    // Inter-reduction.                                                       //
    // ---------------------------------------------------------------------- //

    let rec interreduce dun eqs =
        match eqs with
        | [] -> List.rev dun
        | (Atom (R ("=", [l; r]))) :: oeqs ->
            let dun' =
                if rewrite (dun @ oeqs) l <> l then dun
                else mk_eq l (rewrite (dun @ eqs) r) :: dun
            interreduce dun' oeqs
        | _ -> failwith "interreduce: incomplete pattern matching"

    // ---------------------------------------------------------------------- //
    // Overall function with post-simplification (but not dynamically).       //
    // ---------------------------------------------------------------------- //

    let complete_and_simplify wts eqs =
        let ord = lpo_ge (weight wts)
        let eqs' = 
            List.map (fun e -> 
                let l, r = normalize_and_orient ord [] e
                mk_eq l r) eqs
        (interreduce [] << complete ord) (eqs', [], unions (allpairs critical_pairs     eqs' eqs'))

    // --------------------------------------------------------------------- //
    // Step-by-step; note that we *do* deduce commutativity, deferred of     //
    // course.                                                               //
    // ---------------------------------------------------------------------- //

    let eqs = [
        parse "(x * y) * z = x * (y * z)";
        parse "1 * x = x";
        parse "x * 1 = x";
        parse "x * x = 1"; ]
    let wts = ["1"; "*"; "i"]

    let ord = lpo_ge (weight wts)

    let def = []

    let crits = unions (allpairs critical_pairs eqs eqs)
    
    let complete1 ord (eqs, def, crits) =
        match crits with
        | eq :: ocrits ->
            let trip =
                try
                    let s', t' = normalize_and_orient ord eqs eq
                    if s' = t' then
                        eqs, def, ocrits
                    else
                        let eq' = Atom (R ("=", [s';t']))
                        let eqs' = eq' :: eqs
                        eqs', def,
                            ocrits @ List.foldBack ((@) << critical_pairs eq') eqs'     []
                with Failure _ ->
                    eqs, eq :: def, ocrits

            status trip eqs
            trip
        | _ ->
            if def = [] then
                eqs, def, crits
            else
                let e = List.find (can (normalize_and_orient ord eqs)) def
                eqs, subtract def [e], [e]
