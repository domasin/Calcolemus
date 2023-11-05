// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus.Lib.Function
open Calculemus.Lib.List
open Calculemus.Lib.Set
open Calculemus.Lib.Fpf
open Calculemus.Lib.String
open Calculemus.Lib.Parser

open Formulas

module Prop = 

    type prop = P of string

    let inline pname (P s as p) = s

    // ---------------------------------------------------------------------- //
    // Parsing of propositional formulas.                                     //
    // ---------------------------------------------------------------------- //

    let parse_propvar vs inp =
        match inp with
        | p :: oinp when p <> "(" ->
            Atom (P p), oinp
        | _ ->
            failwith "parse_propvar"

    let parse_prop_formula =
        parse_formula ((fun _ _ -> failwith ""), parse_propvar) []
        |> make_parser

    let (!>) s = parse_prop_formula s

    // ---------------------------------------------------------------------- //
    // Printing of propositional formulas.                                    //
    // ---------------------------------------------------------------------- //

    let fprint_propvar sw prec p =
        fprintf sw "%O" (pname p)

    let inline print_propvar prec p = fprint_propvar stdout prec p

    let inline sprint_propvar prec p = writeToString (fun sw -> fprint_propvar sw   prec p)

    let fprint_prop_formula sw = 
        fprint_qformula sw (fprint_propvar sw)

    let inline print_prop_formula f = fprint_prop_formula stdout f

    let inline sprint_prop_formula f = writeToString (fun sw -> fprint_prop_formula     sw f)

    // ---------------------------------------------------------------------- //
    // Interpretation of formulas.                                            //
    // ---------------------------------------------------------------------- //

    let rec eval fm v =
        match fm with
        | False -> false
        | True -> true
        | Atom x -> v x
        | Not p ->
            not <| eval p v
        | And (p, q) ->
            (eval p v) && (eval q v)
        | Or (p, q) ->
            (eval p v) || (eval q v)
        | Imp (p, q) ->
            not(eval p v) || (eval q v)
        | Iff (p, q) ->
            (eval p v) = (eval q v)
        | Exists _
        | Forall _ ->
            failwith "Not part of propositional logic."

    let atoms fm = 
        atom_union (fun a -> [a]) fm

    // ---------------------------------------------------------------------- //
    // Truth tables.                                                          //
    // ---------------------------------------------------------------------- //

    let rec onallvaluations subfn v ats =
        match ats with
        | [] -> subfn v
        | p :: ps ->
            let v' t q =
                if q = p then t
                else v q
            onallvaluations subfn (v' false) ps
            && onallvaluations subfn (v' true) ps

    let allvaluations fm = 
        let rec allvaluationsAux v pvs =
            match pvs with
            | [] -> [v]
            | p :: ps -> 
                let v' t q =
                    if q = p then t
                    else v q
                allvaluationsAux (v' false) ps @
                allvaluationsAux (v' true) ps
        allvaluationsAux undef (atoms fm)

    let fprint_truthtable sw fm =
        // [P "p"; P "q"; P "r"]
        let ats = atoms fm
        // 5 + 1 = length of false + length of space
        let width = List.foldBack (max << String.length << pname) ats 5 + 1
        let fixw s = s + String.replicate (width - String.length s) " "
        let truthstring p = fixw (if p then "true" else "false")
        let mk_row v =
            let lis = List.map (fun x -> truthstring (v x)) ats
            let ans = truthstring (eval fm v)
            fprintf sw "%s" (List.foldBack (+) lis ("| " + ans))
            fprintfn sw ""
            true
        let separator = String.replicate (width * (List.length ats) + 9) "-"
        fprintfn sw "%s" (List.foldBack (fun s t -> fixw(pname s) + t) ats "|   formula")
        fprintfn sw "%s" separator
        let _ = onallvaluations mk_row (fun x -> false) ats
        fprintfn sw "%s" separator
        fprintfn sw ""

    let inline print_truthtable fm = 
        fprint_truthtable stdout fm

    let inline sprint_truthtable fm = 
        writeToString (fun sw -> fprint_truthtable sw  fm)

    // ---------------------------------------------------------------------- //
    // Tautology, Unsatisfiability, Satisfiability                            //
    // ---------------------------------------------------------------------- //

    let tautology fm =
        onallvaluations (eval fm) (fun s -> false) (atoms fm)

    let unsatisfiable fm = 
        tautology (Not fm)

    let satisfiable fm = 
        not (unsatisfiable fm)

    // ---------------------------------------------------------------------- //
    // Substitution operation.                                                //
    // ---------------------------------------------------------------------- //

    let psubst subfn fm =
        onatoms (fun p -> tryapplyd subfn p (Atom p)) fm

    // ---------------------------------------------------------------------- //
    // Dualization.                                                           //
    // ---------------------------------------------------------------------- //

    let rec dual fm =
        match fm with
        | False -> True
        | True -> False
        | Atom p -> fm
        | Not p ->
            Not (dual p)
        | And (p, q) ->
            Or (dual p, dual q)
        | Or (p, q) ->
            And (dual p, dual q)
        | _ ->
            failwith "Formula involves connectives ==> or <=>"

    // ---------------------------------------------------------------------- //
    // Simplification.                                                        //
    // ---------------------------------------------------------------------- //

    let psimplify1 fm =
        match fm with
        | Not True ->
            False
        | And (p, False)
        | And (False, p) ->
            False

        | Not False
        | Iff (False, False) -> // From Errata
            True
        | Or (p, True)
        | Or (True, p)
        | Imp (False, p)
        | Imp (p, True) ->
            True

        | And (p, True)
        | Not (Not p)
        | And (True, p)
        | Or (p, False)
        | Or (False, p)
        | Imp (True, p)
        | Iff (p, True)
        | Iff (True, p) -> p

        | Imp (p, False)
        | Iff (p, False)
        | Iff (False, p) ->
            Not p

        | fm -> fm

    let rec psimplify fm =
        match fm with
        | Not p ->
            psimplify1 (Not (psimplify p))
        | And (p, q) ->
            psimplify1 (And (psimplify p, psimplify q))
        | Or (p, q) ->
            psimplify1 (Or (psimplify p, psimplify q))
        | Imp (p, q) ->
            psimplify1 (Imp (psimplify p, psimplify q))
        | Iff (p, q) ->
            psimplify1 (Iff (psimplify p, psimplify q))
        | fm -> fm

    // ---------------------------------------------------------------------- //
    // Literals.                                                              //
    // ---------------------------------------------------------------------- //

    let negative lit = 
        match lit with
        | Not p -> true
        | _ -> false

    let positive lit = 
        not (negative lit)

    let negate lit = 
        match lit with
        | Not p -> p
        | p -> Not p

    // ---------------------------------------------------------------------- //
    // Negation normal form.                                                  //
    // ---------------------------------------------------------------------- //

    let rec nnf_naive fm =
        match fm with
        | And (p, q) ->
            And (nnf_naive p, nnf_naive q)
        | Or (p, q) ->
            Or (nnf_naive p, nnf_naive q)
        | Imp (p, q) ->
            Or (nnf_naive (Not p), nnf_naive q)
        | Iff (p, q) ->
            Or (And (nnf_naive p, nnf_naive q),
                And (nnf_naive (Not p), nnf_naive (Not q)))
        | Not (Not p) ->
            nnf_naive p
        | Not (And (p, q)) ->
            Or (nnf_naive (Not p), nnf_naive (Not q))
        | Not (Or (p, q)) ->
            And (nnf_naive (Not p), nnf_naive (Not q))
        | Not (Imp (p, q)) ->
            And (nnf_naive p, nnf_naive (Not q))
        | Not (Iff (p, q)) ->
            Or (And (nnf_naive p, nnf_naive (Not q)),
                And (nnf_naive (Not p), nnf_naive q))
        | fm -> fm

    let nnf fm =
        fm
        |> psimplify
        |> nnf_naive

    // ---------------------------------------------------------------------- //
    // Simple negation-pushing when we don't care to distinguish occurrences. //
    // ---------------------------------------------------------------------- //

    let rec nenf_naive fm =
        match fm with
        | Not (Not p) ->
            nenf_naive p
        | Not (And (p, q)) ->
            Or (nenf_naive (Not p), nenf_naive (Not q))
        | Not (Or (p, q)) ->
            And (nenf_naive (Not p), nenf_naive (Not q))
        | Not (Imp (p, q)) ->
            And (nenf_naive p, nenf_naive (Not q))
        | Not (Iff (p, q)) ->
            Iff (nenf_naive p, nenf_naive (Not q))
        | And (p, q) ->
            And (nenf_naive p, nenf_naive q)
        | Or (p, q) ->
            Or (nenf_naive p, nenf_naive q)
        | Imp (p, q) ->
            Or (nenf_naive (Not p), nenf_naive q)
        | Iff (p, q) ->
            Iff (nenf_naive p, nenf_naive q)
        | fm -> fm

    let nenf fm =
        fm
        |> psimplify
        |> nenf_naive

    // ---------------------------------------------------------------------- //
    // Disjunctive normal form (DNF) via truth tables.                        //
    // ---------------------------------------------------------------------- //

    let list_conj l =
        if l = [] then True
        else List.reduceBack mk_and l

    let list_disj l = 
        if l = [] then False 
        else List.reduceBack mk_or l
    
    let mk_lits pvs v =
        list_conj (List.map (fun p -> if eval p v then p else Not p) pvs)

    let rec allsatvaluations subfn v pvs =
        match pvs with
        | [] ->
            if subfn v then [v] else []
        | p :: ps -> 
            let v' t q =
                if q = p then t
                else v q
            allsatvaluations subfn (v' false) ps @
            allsatvaluations subfn (v' true) ps

    let dnf_by_truth_tables fm =
        let pvs = atoms fm
        let satvals = allsatvaluations (eval fm) (fun s -> false) pvs
        list_disj (List.map (mk_lits (List.map (fun p -> Atom p) pvs)) satvals)

    // ---------------------------------------------------------------------- //
    // DNF via distribution.                                                  //
    // ---------------------------------------------------------------------- //

    let rec distrib_naive fm =
        match fm with
        | And (p, Or (q, r)) ->
            Or (distrib_naive (And (p, q)), distrib_naive (And (p, r)))
        | And (Or (p, q), r) ->
            Or (distrib_naive (And (p, r)), distrib_naive (And (q, r)))
        | _ -> fm

    let rec rawdnf fm =
        match fm with
        | And (p, q) ->
            distrib_naive <| And (rawdnf p, rawdnf q)
        | Or (p, q) ->
            Or (rawdnf p, rawdnf q)
        | _ -> fm

    // ---------------------------------------------------------------------- //
    // A DNF version using a list representation.                             //
    // ---------------------------------------------------------------------- //

    let distrib s1 s2 =
        setify <| allpairs union s1 s2

    let rec purednf fm =
        match fm with
        | And (p, q) ->
            distrib (purednf p) (purednf q)
        | Or (p, q) ->
            union (purednf p) (purednf q)
        | _ -> [[fm]]

    // ---------------------------------------------------------------------- //
    // Filtering out trivial disjuncts (in this guise, contradictory).        //
    // ---------------------------------------------------------------------- //

    let trivial lits =
        let pos, neg = List.partition positive lits
        intersect pos (image negate neg) <> []

    let simpdnf fm =
        if fm = False then [] 
        elif fm = True then [[]] 
        else
            let djs = List.filter (non trivial) (purednf (nnf fm))
            List.filter (fun d -> not (List.exists (fun d' -> psubset d' d) djs))   djs

    /// Transforms the input formula `fm` in disjunctive normal form.
    let dnf fm =
        List.map list_conj (simpdnf fm)
        |> list_disj

    // ---------------------------------------------------------------------- //
    // Conjunctive normal form (CNF) by essentially the same code.            //
    // ---------------------------------------------------------------------- //

    let purecnf fm = image (image negate) (purednf (nnf (Not fm)))

    let simpcnf fm =
        if fm = False then [[]]
        elif fm = True then []
        else
            let cjs = List.filter (non trivial) (purecnf fm)
            List.filter (fun c -> not (List.exists (fun c' -> psubset c' c) cjs))   cjs

    let cnf fm =
        List.map list_disj (simpcnf fm)
        |> list_conj

