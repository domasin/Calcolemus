// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

// ========================================================================= //
// LCF-style basis for Tarski-style Hilbert system of first order logic.     //
// ========================================================================= //

/// <summary>
/// LCF-style system for first order logic.
/// </summary>
/// 
/// <remarks>
/// Basic first order deductive system.
/// <p></p>
/// This is based on Tarski's trick for avoiding use of a substitution  
/// primitive. It seems about the simplest possible system we could use.
/// </remarks>
/// 
/// <category index="7">Interactive theorem proving</category>
module Calculemus.Lcf

//  if |- p ==> q and |- p then |- q                                   
//  if |- p then |- forall x. p                                        
//                                                                     
//  |- p ==> (q ==> p)                                                 
//  |- (p ==> q ==> r) ==> (p ==> q) ==> (p ==> r)                     
//  |- ((p ==> false) ==> false) ==> p                                 
//  |- (forall x. p ==> q) ==> (forall x. p) ==> (forall x. q)         
//  |- p ==> forall x. p                            [x not free in p]  
//  |- exists x. x = t                              [x not free in t]  
//  |- t = t                                                           
//  |- s1 = t1 ==> ... ==> sn = tn ==> f(s1,..,sn) = f(t1,..,tn)       
//  |- s1 = t1 ==> ... ==> sn = tn ==> P(s1,..,sn) ==> P(t1,..,tn)     
//  |- (p <=> q) ==> p ==> q                                           
//  |- (p <=> q) ==> q ==> p                                           
//  |- (p ==> q) ==> (q ==> p) ==> (p <=> q)                           
//  |- true <=> (false ==> false)                                      
//  |- -p <=> (p ==> false)                                            
//  |- p /\ q <=> (p ==> q ==> false) ==> false                        
//  |- p \/ q <=> -(-p /\ -q)                                          
//  |- (exists x. p) <=> -(forall x. -p)                               

open Calculemus.Lib.String

open Formulas
open Fol
open Equal
    
// ------------------------------------------------------------------------- //
// Auxiliary functions.                                                      //
// ------------------------------------------------------------------------- //

/// checks whether a term s occurs as a sub-term of another term t
let rec occurs_in s t =
    s = t ||
    match t with
    | Var y -> false
    | Fn (f, args) ->
        List.exists (occurs_in s) args

/// checks whether a term t occurs free in a formula fm
let rec free_in t fm =
    match fm with
    | False | True ->               false
    | Not p ->                      free_in t p
    | And (p, q) | Or (p, q) 
    | Imp (p, q) | Iff (p, q) ->    free_in t p || free_in t q
    | Forall (y, p)
    | Exists (y, p) ->              not (occurs_in (Var y) t) && free_in t p
    | Atom (R (p, args)) ->         List.exists (occurs_in t) args

/// The Core LCF proof system
/// 
/// The core proof system is the minimum set of inference rules and/or axioms 
/// sound and complete with respect to the defined semantics.
[<AutoOpen>]
module ProofSystem =

    (*************************************************************)
    (*                Core LCF proof system                      *)
    (*************************************************************)

    type thm = private Theorem of formula<fol>

    /// modusponens (proper inference rule)
    /// 
    /// |- p -> q |- p ==> |- q
    let modusponens (pq : thm) (Theorem p : thm) : thm =
        match pq with
        | Theorem (Imp (p', q)) when p = p' -> Theorem q
        | _ -> failwith "modusponens"

    /// generalization (proper inference rule)
    /// 
    /// |- p ==> !x. p
    let gen x (Theorem p : thm) : thm =
        Theorem (Forall (x, p))

    /// |- p -> (q -> p)
    let axiom_addimp p q : thm =
        Theorem (Imp (p,Imp (q, p)))

    /// |- (p -> q -> r) -> (p -> q) -> (p -> r)
    let axiom_distribimp p q r : thm =
        Theorem (Imp (Imp (p, Imp (q, r)), Imp (Imp (p, q), Imp (p, r))))

    /// |- ((p -> ⊥) -> ⊥) -> p
    let axiom_doubleneg p : thm =
        Theorem (Imp (Imp (Imp (p, False), False), p))

    /// |- (!x. p -> q) -> (!x. p) -> (!x. q)
    let axiom_allimp x p q : thm =
        Theorem (Imp (Forall (x, Imp (p, q)), Imp (Forall (x, p), Forall (x, q))))

    /// |- p -> !x. p [provided x not in FV(p)]
    let axiom_impall x p : thm =
        if free_in (Var x) p then
            failwith "axiom_impall: variable free in formula"
        else
            Theorem (Imp (p, Forall (x, p)))

    /// |- (?x. x = t) [provided x not in FVT(t)]
    let axiom_existseq x t : thm =
        if occurs_in (Var x) t then
            failwith "axiom_existseq: variable free in term"
        else
            Theorem (Exists (x, mk_eq (Var x) t))

    /// |- t = t
    let axiom_eqrefl t : thm =
        Theorem (mk_eq t t)

    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    let axiom_funcong f lefts rights : thm =
        List.foldBack2 (fun s t (Theorem p) -> Theorem (Imp (mk_eq s t, p))) lefts rights
            (Theorem (mk_eq (Fn (f, lefts)) (Fn (f, rights))))

    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    let axiom_predcong p lefts rights : thm =
        List.foldBack2 (fun s t (Theorem p)  -> Theorem (Imp (mk_eq s t, p))) lefts rights
            (Theorem (Imp (Atom (R (p, lefts)), Atom (R (p, rights)))))

    (*************************************************************)
    (*             Definitions of other connectives              *)
    (*************************************************************)

    /// |- (p <-> q) -> p -> q
    let axiom_iffimp1 p q : thm =
        Theorem (Imp (Iff (p, q), Imp (p, q)))

    /// |- (p <-> q) -> q -> p
    let axiom_iffimp2 p q : thm =
        Theorem (Imp (Iff (p, q), Imp (q, p)))

    /// |- (p -> q) -> (q -> p) -> (p <-> q)
    let axiom_impiff p q : thm =
        Theorem (Imp (Imp (p, q), Imp (Imp (q, p), Iff (p, q))))

    /// |- ⊤ <-> (⊥ -> ⊥)
    let axiom_true : thm =
        Theorem (Iff (True, Imp (False, False)))

    /// |- ~p <-> (p -> ⊥)
    let axiom_not p : thm =
        Theorem (Iff (Not p, Imp (p, False)))

    /// |- p /\ q <-> (p -> q -> ⊥) -> ⊥
    let axiom_and p q : thm =
        Theorem (Iff (And (p, q), Imp (Imp (p, Imp (q, False)), False)))

    /// |- p \/ q <-> ~(~p /\ ~q)
    let axiom_or p q : thm =
        Theorem (Iff (Or (p, q), Not (And (Not p, Not q))))

    /// (?x. p) <-> ~(!x. ~p)
    let axiom_exists x p : thm =
        Theorem (Iff (Exists (x, p), Not (Forall (x, Not p))))

    /// maps a theorem back to the formula that it proves
    let concl (Theorem c : thm) : formula<fol> = c

// ------------------------------------------------------------------------- //
// A printer for theorems.                                                   //
// ------------------------------------------------------------------------- //

/// Prints a theorem using a TextWriter.
let fprint_thm sw th =
    fprintf sw "|- " // write on the same line
    fprint_formula sw (fprint_atom sw) (concl th)

/// A printer for theorems
let inline print_thm th = fprint_thm stdout th

/// Theorem to string
let inline sprint_thm th = writeToString (fun sw -> fprint_thm sw th)
