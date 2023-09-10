// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Basic stuff for propositional logic: datatype, parsing and printing. 
module FolAutomReas.Prop

open FolAutomReas.Lib

open Intro
open Formulas

/// Type of primitive propositions indexed by names.
type prop = P of string

/// Returns constant or variable name of a propositional formula.
let inline pname (P s) = s

// ------------------------------------------------------------------------- //
// Parsing of propositional formulas.                                        //
// ------------------------------------------------------------------------- //

/// Parses atomic propositions.
let parse_propvar vs inp =
    match inp with
    | p :: oinp when p <> "(" ->
        Atom (P p), oinp
    | _ ->
        failwith "parse_propvar"
        
/// Parses a string in a propositional formula.
let parse_prop_formula =
    parse_formula ((fun _ _ -> failwith ""), parse_propvar) []
    |> make_parser
                
// ------------------------------------------------------------------------- //
// Printing of propositional formulas.                                       //
// ------------------------------------------------------------------------- //

/// Prints a prop variable using a TextWriter.
let fprint_propvar sw prec p =
    fprintf sw "%O" (pname p)

/// Prints a prop variable
let inline print_propvar prec p = fprint_propvar stdout prec p

/// Returns a string representation of a prop variable.
let inline sprint_propvar prec p = writeToString (fun sw -> fprint_propvar sw prec p)
        
/// Prints a prop formula using a TextWriter.
let fprint_prop_formula sw = 
    fprint_qformula sw (fprint_propvar sw)

/// Prints a prop formula
let inline print_prop_formula f = fprint_prop_formula stdout f

/// Returns a string representation of a propositional formula instead of 
/// its abstract syntax tree..
let inline sprint_prop_formula f = writeToString (fun sw -> fprint_prop_formula sw f)

// ------------------------------------------------------------------------- //
// Interpretation of formulas.                                               //
// ------------------------------------------------------------------------- //

/// Interpretation of formulas. 
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

/// Return the set of propositional variables in a formula.
let atoms fm = 
    atom_union (fun a -> [a]) fm

// ------------------------------------------------------------------------- //
// Truth tables.                                                             //
// ------------------------------------------------------------------------- //

/// Tests whether a function `subfn` returns `true` on all possible valuations 
/// of the atoms `ats`, using an existing valuation `v` for all other atoms.
let rec onallvaluations subfn v ats =
    match ats with
    | [] -> subfn v
    | p :: ps ->
        let v' t q =
            if q = p then t
            else v q
        onallvaluations subfn (v' false) ps
        && onallvaluations subfn (v' true) ps

/// Prints the truth table of a formula `fm` using a TextWriter.
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
    let seperator = String.replicate (width * (List.length ats) + 9) "-"
    fprintfn sw "%s" (List.foldBack (fun s t -> fixw(pname s) + t) ats "| formula")
    fprintfn sw "%s" seperator
    let _ = onallvaluations mk_row (fun x -> false) ats
    fprintfn sw "%s" seperator
    fprintfn sw ""

/// Prints the truth table of the propositional formula `f`.
let inline print_truthtable f = fprint_truthtable stdout f

/// Returns a string representation of the truth table of the propositional 
/// formula `f`.
let inline sprint_truthtable f = writeToString (fun sw -> fprint_truthtable sw f)

// ------------------------------------------------------------------------- //
// Recognizing tautologies.                                                  //
// ------------------------------------------------------------------------- //

/// Checks if a propositional formula is a tautology.
let tautology fm =
    onallvaluations (eval fm) (fun s -> false) (atoms fm)

// ------------------------------------------------------------------------- //
// Related concepts.                                                         //
// ------------------------------------------------------------------------- //

/// Checks if a propositional formula is unsatisfiable.
let unsatisfiable fm = 
    tautology <| Not fm
        
/// Checks if a propositional formula is satisfiable.
let satisfiable fm = 
    not <| unsatisfiable fm

// ------------------------------------------------------------------------- //
// Substitution operation.                                                   //
// ------------------------------------------------------------------------- //

/// Returns the formula resulting from applying the substitution `sbfn` 
/// to the input formula.
let psubst subfn =
    onatoms <| fun p ->
        tryapplyd subfn p (Atom p)

// ------------------------------------------------------------------------- //
// Dualization.                                                              //
// ------------------------------------------------------------------------- //

/// Returns the dual of the input formula `fm`: i.e. the result of 
/// systematically exchanging `/\`s with `\/`s and also `True` with 
/// `False`.
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

// ------------------------------------------------------------------------- //
// Routine simplification.                                                   //
// ------------------------------------------------------------------------- //

/// Performs a simplification routine but just at the first level of the input 
/// formula `fm`. It eliminates the basic propositional constants `False` and 
/// `True`. 
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
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
        
/// Performs a simplification routine on the input formula 
/// `fm` eliminating the basic propositional constants `False` and `True`. 
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// While `psimplify1` performs the transformation just at the first level, 
/// `psimplify` performs it at every levels in a recursive bottom-up sweep.

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

// ------------------------------------------------------------------------- //
// Some operations on literals.                                              //
// ------------------------------------------------------------------------- //

/// Checks if a literal is negative.
let negative = function
    | Not p -> true
    | _ -> false
    
/// Checks if a literal is positive.
let positive lit = not <| negative lit

/// Changes a literal into its contrary.
let negate = function
    | Not p -> p
    | p -> Not p

// ------------------------------------------------------------------------- //
// Negation normal form.                                                     //
// ------------------------------------------------------------------------- //

/// Changes a formula into its negation normal form without simplifying it.
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

// ------------------------------------------------------------------------- //
// Roll in simplification.                                                   //
// ------------------------------------------------------------------------- //

/// Changes a formula into its negation normal and applies it the routine 
/// simplification `psimplify`.
let nnf fm =
    nnf_naive <| psimplify fm

// ------------------------------------------------------------------------- //
// Simple negation-pushing when we don't care to distinguish occurrences.    //
// ------------------------------------------------------------------------- //

/// Simply pushes negations in the input formula `fm` down to the level of atoms without simplifying it.
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
        
/// Simply pushes negations in the input formula `fm` down to the level of 
/// atoms and applies it the routine simplification `psimplify`.
let nenf fm =
    nenf_naive <| psimplify fm

// ------------------------------------------------------------------------- //
// Disjunctive normal form (DNF) via truth tables.                           //
// ------------------------------------------------------------------------- //

/// Creates a conjunction of all the formulas in the input list `l`.
let list_conj l =
    if l = [] then True
    else List.reduceBack mk_and l

/// Creates a disjunction of all the formulas in the input list `l`.
let list_disj l = 
    if l = [] then False 
    else List.reduceBack mk_or l
   
/// Given a list of formulas `pvs`, makes a conjunction of these formulas and 
/// their negations according to whether each is satisﬁed by the valuation `v`.
let mk_lits pvs v =
    list_conj (List.map (fun p -> if eval p v then p else Not p) pvs)
        
/// A close analogue of `onallvaluations` that collects the valuations for 
/// which `subfn` holds into a list.
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
            
/// Transforms a formula `fm` in disjunctive normal form using truth tables.
let dnf_by_truth_tables fm =
    let pvs = atoms fm
    let satvals = allsatvaluations (eval fm) (fun s -> false) pvs
    list_disj (List.map (mk_lits (List.map (fun p -> Atom p) pvs)) satvals)

// ------------------------------------------------------------------------- //
// DNF via distribution.                                                     //
// ------------------------------------------------------------------------- //

/// Applies the distributive laws to the input formula `fm`.
let rec distrib_naive fm =
    match fm with
    | And (p, Or (q, r)) ->
        Or (distrib_naive (And (p, q)), distrib_naive (And (p, r)))
    | And (Or (p, q), r) ->
        Or (distrib_naive (And (p, r)), distrib_naive (And (q, r)))
    | _ -> fm

/// Transforms the input formula `fm` in disjunctive normal form.
let rec rawdnf fm =
    match fm with
    | And (p, q) ->
        distrib_naive <| And (rawdnf p, rawdnf q)
    | Or (p, q) ->
        Or (rawdnf p, rawdnf q)
    | _ -> fm

// ------------------------------------------------------------------------- //
// A DNF version using a list representation.                                //
// ------------------------------------------------------------------------- //

/// Applies the distributive laws of propositional connectives `/\` and `\/` 
/// using a list representation of the formulas `s1` and `s2` on which 
/// to operate.
let distrib s1 s2 =
    setify <| allpairs union s1 s2
    
/// Transforms the input formula `fm` in disjunctive normal form using 
/// (internally) a list representation of the formula as a set of sets. 
/// `p /\ q \/ ~ p /\ r` as `[[p; q]; [~ p; r]]`
let rec purednf fm =
    match fm with
    | And (p, q) ->
        distrib (purednf p) (purednf q)
    | Or (p, q) ->
        union (purednf p) (purednf q)
    | _ -> [[fm]]

// ------------------------------------------------------------------------- //
// Filtering out trivial disjuncts (in this guise, contradictory).           //
// ------------------------------------------------------------------------- //

/// Check if there are complementary literals of the form p and ~ p 
/// in the same list.
let trivial lits =
    let pos, neg = List.partition positive lits
    intersect pos (image negate neg) <> []

// ------------------------------------------------------------------------- //
// With subsumption checking, done very naively (quadratic).                 //
// ------------------------------------------------------------------------- //

/// Transforms the input formula `fm` in a list of list representation of  
/// disjunctive normal form. It exploits the list of list representation 
/// filtering out trivial complementary literals and subsumed ones.
let simpdnf fm =
    if fm = False then [] 
    elif fm = True then [[]] 
    else
        let djs = List.filter (non trivial) (purednf (nnf fm))
        List.filter (fun d -> not (List.exists (fun d' -> psubset d' d) djs)) djs

// ------------------------------------------------------------------------- //
// Mapping back to a formula.                                                //
// ------------------------------------------------------------------------- //

/// Transforms the input formula `fm` in disjunctive normal form.
let dnf fm =
    List.map list_conj (simpdnf fm)
    |> list_disj

// ------------------------------------------------------------------------- //
// Conjunctive normal form (CNF) by essentially the same code.               //
// ------------------------------------------------------------------------- //

/// Transforms the input formula `fm` in conjunctive normal form 
/// by using `purednf`.
let purecnf fm = image (image negate) (purednf (nnf (Not fm)))
    
/// Transforms the input formula `fm` in a list of list representation of  
/// conjunctive normal form. It exploits the list of list representation 
/// filtering out trivial complementary literals and subsumed ones.
let simpcnf fm =
    if fm = False then [[]]
    elif fm = True then []
    else
        let cjs = List.filter (non trivial) (purecnf fm)
        List.filter (fun c -> not (List.exists (fun c' -> psubset c' c) cjs)) cjs

/// Transforms the input formula `fm` in conjunctive normal form.
let cnf fm =
    List.map list_disj (simpcnf fm)
    |> list_conj

