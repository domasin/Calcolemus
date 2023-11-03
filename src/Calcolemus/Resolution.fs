// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Calcolemus

open Lib.List
open Lib.Search
open Lib.Set
open Lib.Fpf

open Formulas
open Prop
open Fol
open Skolem
open Herbrand
open Unif
open Tableaux

module Resolution = 

    // ====================================================================== //
    // Resolution.                                                            //
    // ====================================================================== //
    
    // ---------------------------------------------------------------------- //
    // MGU of a set of literals.                                              //
    // ---------------------------------------------------------------------- //
    
    let rec mgu l env =
        match l with
        | a :: b :: rest ->
            mgu (b :: rest) (unify_literals env (a, b))
        | _ -> solve env
    
    let unifiable p q =
        let f = unify_literals undefined
        let x = p, q
        try f x |> ignore
            true
        with _ -> false 
    
    // ---------------------------------------------------------------------- //
    // Rename a clause.                                                       //
    // ---------------------------------------------------------------------- //
    
    let rename pfx cls =
        let fvs = fv (list_disj cls)
        let vvs = List.map (fun s -> Var (pfx + s)) fvs
        List.map (subst (fpf fvs vvs)) cls
    
    // ---------------------------------------------------------------------- //
    // General resolution rule, incorporating factoring                       //
    // as in Robinson's paper.                                                //
    // ---------------------------------------------------------------------- //
    
    let resolvents cl1 cl2 p acc =
        let ps2 = List.filter (unifiable (negate p)) cl2
        if ps2 = [] then acc 
        else
            let ps1 = List.filter (fun q -> q <> p && unifiable p q) cl1
            let pairs = allpairs (fun s1 s2 -> s1, s2)
                                (List.map (fun pl -> p :: pl) (allsubsets ps1))
                                (allnonemptysubsets ps2)
            List.foldBack (fun (s1, s2) sof ->
                    try 
                        image (subst (mgu (s1 @ List.map negate s2) undefined))
                                (union (subtract cl1 s1) (subtract cl2 s2)) :: sof
                    with 
                    | Failure _ -> sof) pairs acc
    
    let resolve_clauses cls1 cls2 =
        let cls1' = rename "x" cls1 
        let cls2' = rename "y" cls2
        List.foldBack (resolvents cls1' cls2') cls1' []
    
    // ---------------------------------------------------------------------- //
    // Basic "Argonne" loop.                                                  //
    // ---------------------------------------------------------------------- //
    
    let rec basic_resloop (used,unused) =
        match unused with
        | [] -> failwith "No proof found"
        | cl :: ros ->
            printfn "%i used; %i unused." (List.length used) (List.length unused)
            let used' = insert cl used
            let news = List.foldBack (@) (mapfilter (resolve_clauses cl) used') []
            if mem [] news then true
            else basic_resloop (used', ros @ news)
    
    let pure_basic_resolution fm =
        basic_resloop ([], simpcnf (specialize (pnf fm)))
    
    let basic_resolution fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (pure_basic_resolution << list_conj) (simpdnf fm1)
    
    // ---------------------------------------------------------------------- //
    // Matching of terms and literals.                                        //
    // ---------------------------------------------------------------------- //
    
    let rec term_match env eqs =
        match eqs with
        | [] -> env
        | (Fn (f, fa), Fn(g, ga)) :: oth
            when f = g
            && List.length fa = List.length ga ->
            term_match env (List.zip fa ga @ oth)
        | (Var x, t) :: oth ->
            if not (defined env x) then
                term_match ((x |-> t) env) oth
            elif apply env x = t then
                term_match env oth
            else
                failwith "term_match"
        | _ ->
            failwith "term_match"
    
    let rec match_literals env tmp =
        match tmp with
        | Atom (R (p, a1)), Atom (R (q, a2))
        | Not (Atom (R (p, a1))), Not (Atom (R (q, a2))) ->
            term_match env [Fn (p, a1), Fn (q, a2)]
        | _ -> failwith "match_literals"
    
    // ---------------------------------------------------------------------- //
    // Test for subsumption                                                   //
    // ---------------------------------------------------------------------- //
    
    let subsumes_clause cls1 cls2 =
        let rec tryfind f l =
            match l with
            | [] -> failwith "tryfind"
            | h :: t ->
                try f h
                with _ -> tryfind f t
    
        let rec subsume env cls =
            match cls with
            | [] -> env
            | l1 :: clt ->
                tryfind (fun l2 -> subsume (match_literals env (l1,l2)) clt) cls2
        try 
            (subsume undefined) cls1 |> ignore
            true 
        with _ -> false
    
    // ---------------------------------------------------------------------- //
    // With deletion of tautologies and bi-subsumption with "unused".         //
    // ---------------------------------------------------------------------- //
    
    let rec replace cl lis =
        match lis with
        | [] -> [cl]
        | c :: cls ->
            if subsumes_clause cl c then
                cl :: cls
            else c :: (replace cl cls)
    
    let incorporate gcl cl unused =
        if trivial cl ||
            List.exists (fun c -> subsumes_clause c cl) (gcl :: unused)
        then unused
        else replace cl unused
    
    let rec resloop_wsubs (used,unused) =
        match unused with
        | [] -> failwith "No proof found"
        | cl :: ros ->
            printfn "%i used; %i unused." (List.length used) (List.length unused)
            let used' = insert cl used
            let news = List.foldBack (@) (mapfilter (resolve_clauses cl) used') []
            if mem [] news then true
            else resloop_wsubs (used', List.foldBack (incorporate cl) news ros)
    
    let pure_resolution_wsubs fm =
        resloop_wsubs ([], simpcnf (specialize (pnf fm)))
    
    let resolution_wsubs fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (pure_resolution_wsubs << list_conj) (simpdnf fm1)
    
    // ---------------------------------------------------------------------- //
    // Positive (P1) resolution.                                              //
    // ---------------------------------------------------------------------- //
    
    let presolve_clauses cls1 cls2 =
        if List.forall positive cls1 || List.forall positive cls2 then
            resolve_clauses cls1 cls2
        else []
    
    let rec presloop (used, unused) =
        match unused with
        | [] -> failwith "No proof found"
        | cl :: ros ->
            printfn "%i used; %i unused." (List.length used) (List.length unused)
            let used' = insert cl used
            let news = List.foldBack (@) (mapfilter (presolve_clauses cl) used') []
            if mem [] news then true 
            else presloop (used', List.foldBack (incorporate cl) news ros)
    
    let pure_presolution fm =
        presloop ([], simpcnf (specialize (pnf fm)))
    
    let presolution fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (pure_presolution << list_conj) (simpdnf fm1)
    
    // ---------------------------------------------------------------------- //
    // Introduce a set-of-support restriction.                                //
    // ---------------------------------------------------------------------- //
    
    let pure_resolution_wsos fm =
        fm
        |> pnf
        |> specialize
        |> simpcnf
        |> List.partition (List.exists positive)
        |> resloop_wsubs
    
    let resolution_wsos fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (pure_resolution_wsos << list_conj) (simpdnf fm1)
