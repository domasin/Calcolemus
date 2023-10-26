// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

// ========================================================================= //
// Goals, LCF-like tactics and Mizar-like proofs.                            //
// ========================================================================= //

/// <summary>
/// Tactics and Mizar-style proofs.
/// </summary>
/// 
/// <category index="7">Interactive theorem proving</category>
module Calcolemus.Tactics

open Calcolemus.Lib.Function
open Calcolemus.Lib.List
open Calcolemus.Lib.Set
open Calcolemus.Lib.Fpf
open Calcolemus.Lib.Time
open Calcolemus.Lib.String

open Formulas
open Prop
open Fol
open Lcf
open Lcfprop
open Folderived
open Lcffol
    
// pg. 507
type goals = Goals of ((string * formula<fol>) list * formula<fol>)list * (thm list -> thm)
    
// pg. 507
// ------------------------------------------------------------------------- //
// Printer for goals (just shows first goal plus total number).              //
// ------------------------------------------------------------------------- //

let fprint_goal sw =
    let fprint_hyp (l, fm) =
        fprintf sw "%s: " l
        fprint_formula sw (fprint_atom sw) fm
        fprintfn sw ""

    fun (Goals (gls, jfn)) ->
        match gls with
        | [] ->
            fprintf sw "No subgoals"
        | (asl, w) :: ogls ->
            fprintfn sw ""

            if ogls = [] then
                fprintf sw "1 subgoal:"
            else
                fprintf sw "%i subgoals starting with" (List.length gls)

            fprintfn sw ""
            List.iter fprint_hyp (List.rev asl)
            fprintf sw "---> "
            fprint_formula sw (fprint_atom sw) w
            fprintfn sw ""

let inline print_goal g = fprint_goal stdout g
let inline sprint_goal g = writeToString (fun sw -> fprint_goal sw g)
    
// pg. 508
// ------------------------------------------------------------------------- //
// Setting up goals and terminating them in a theorem.                       //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let set_goal p =
    let chk th =
        if concl th = p then th
        else failwith "wrong theorem"
    Goals (
        [[], p], 
        fun thms -> 
            match thms with 
            | [th] -> 
                chk (modusponens th truth)
            | _ -> failwith "chooselang: incomplete pattern matching"
    )

let extract_thm gls : thm =
    match gls with
    | Goals ([], jfn) ->
        jfn []
    | _ ->
        failwith "extract_thm: unsolved goals"

let tac_proof g prf : thm =
    extract_thm (List.foldBack id (List.rev prf) g)

let prove p prf : thm =
    tac_proof (set_goal p) prf
        
// pg. 508
// ------------------------------------------------------------------------- //
// Conjunction introduction tactic.                                          //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let conj_intro_tac gls =
    match gls with
    | Goals ((asl, And (p, q)) :: gls, jfn) -> 
        let jfn' thms =
            match thms with
            | (thp :: thq :: ths) -> 
                jfn(imp_trans_chain [thp; thq] (and_pair p q) :: ths)
            | _ -> failwith "conj_intro_tac: incomplete pattern matching"
        Goals ((asl, p) :: (asl, q) :: gls, jfn')
    | _ -> failwith "conj_intro_tac: incomplete pattern matching"
        
// pg. 509
// ------------------------------------------------------------------------- //
// Handy idiom for tactic that does not split subgoals.                      //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let jmodify jfn tfn xs =
    match xs with
    | (th :: oths) -> 
        jfn (tfn th :: oths)
    | _ -> failwith "jmodify: incomplete pattern matching"
        
// pg. 509
// ------------------------------------------------------------------------- //
// Version of gen_right with a bound variable change.                        //
// ------------------------------------------------------------------------- //

let gen_right_alpha y x (th : thm) : thm =
    let th1 = gen_right y th
    imp_trans th1 (alpha x (consequent (concl th1)))
        
// pg. 509
// ------------------------------------------------------------------------- //
// Universal introduction.                                                   //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let forall_intro_tac y gls =
    match gls with
    | Goals ((asl, (Forall (x, p) as fm)) :: gls, jfn) -> 
        if mem y (fv fm) || List.exists (mem y << fv << snd) asl then
            failwith "fix: variable already free in goal"
        else
            Goals ((asl,subst(x |=> Var y) p) :: gls,
                    jmodify jfn (gen_right_alpha y x))
    | _ -> failwith "forall_intro_tac: incomplete pattern matching"
                    
// pg. 510
// ------------------------------------------------------------------------- //
// Another inference rule: |- P[t] ==> exists x. P[x]                        //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let right_exists x t p : thm =
    let th = contrapos (ispec t (Forall (x, Not p)))
    let p' = 
        match antecedent (concl th) with 
        | Not (Not p') -> p'
        | _ -> failwith "right_exists: incomplete pattern matching"
    List.reduceBack imp_trans [
        imp_contr p' False;
        imp_add_concl False (iff_imp1 (axiom_not p'));
        iff_imp2 (axiom_not (Not p'));
        th;
        iff_imp2 (axiom_exists x p); ]
            
// pg. 510
// ------------------------------------------------------------------------- //
// Existential introduction.                                                 //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let exists_intro_tac t gls =
    match gls with
    | Goals ((asl, Exists (x, p)) :: gls, jfn) -> 
        Goals ((asl, subst(x |=> t) p) :: gls,
            jmodify jfn (fun th -> imp_trans th (right_exists x t p)))
    | _ -> failwith "exists_intro_tac: incomplete pattern matching"
            
// pg. 510
// ------------------------------------------------------------------------- //
// Implication introduction tactic.                                          //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let imp_intro_tac s gls =
    match gls with 
    | Goals ((asl, Imp (p, q)) :: gls, jfn) -> 
        let jmod = if asl = [] then add_assum True else imp_swap << shunt
        Goals (((s, p) :: asl, q) :: gls, jmodify jfn jmod)
    | _ -> failwith "imp_intro_tac: incomplete pattern matching"
        
// pg. 510
// ------------------------------------------------------------------------- //
// Append contextual hypothesis to unconditional theorem.                    //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let assumptate gls (th : thm) : thm =
    match gls with 
    | Goals ((asl, w) :: gls, jfn) -> 
        add_assum (list_conj (List.map snd asl)) th
    | _ -> failwith "assumptate: incomplete pattern matching"
        
// pg. 511
// ------------------------------------------------------------------------- //
// Append contextual hypothesis to unconditional theorem.                    //
// ------------------------------------------------------------------------- //

let firstassum asl : thm =
    let rec p = snd (List.head asl)
    and q = list_conj (List.map snd (List.tail asl))
    if List.tail asl = [] then imp_refl p else and_left p q
        
// pg. 511
// ------------------------------------------------------------------------- //
// Import "external" theorem.                                                //
// ------------------------------------------------------------------------- //

let using ths p g =
    let ths' = List.map (fun th -> List.foldBack gen (fv (concl th)) th) ths
    List.map (assumptate g) ths'
        
// pg. 511
// ------------------------------------------------------------------------- //
// Turn assumptions p1,...,pn into theorems |- p1 /\ ... /\ pn ==> pi        //
// ------------------------------------------------------------------------- //

let rec assumps asl =
    match asl with
    | [] -> []
    | [l, p] ->
        [l, imp_refl p]
    | (l, p) :: lps ->
        let ths = assumps lps
        let q = antecedent (concl (snd (List.head ths)))
        let rth = and_right p q
        (l, and_left p q) :: List.map (fun (l, th) -> l, imp_trans rth th) ths
            
// pg. 511
// ------------------------------------------------------------------------- //
// Produce canonical theorem from list of theorems or assumption labels.     //
// ------------------------------------------------------------------------- //

let by hyps p gls =
    match gls with 
    | Goals ((asl, w) :: gls, jfn) -> 
        let ths = assumps asl
        List.map (fun s -> assoc s ths) hyps
    | _ -> failwith "by: incomplete pattern matching"
        
// pg. 512
// ------------------------------------------------------------------------- //
// Main automatic justification step.                                        //
// ------------------------------------------------------------------------- //

let justify byfn hyps p g =
    match byfn hyps p g with
    | [th] when consequent (concl th) = p -> th
    | ths ->
        let th = lcffol (List.foldBack (mk_imp << consequent << concl) ths p)
        if ths = [] then assumptate g th else imp_trans_chain ths th
            
// pg. 512
// ------------------------------------------------------------------------- //
// Nested subproof.                                                          //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let proof tacs p gls =
    match gls with 
    | Goals ((asl, w) :: gls, jfn) -> 
        [tac_proof (Goals(
            [asl, p], 
            fun thms -> 
            match thms with 
            | [th] -> th
            | _ -> failwith "proof: incomplete pattern matching"
            )) tacs]
    | _ -> failwith "proof: incomplete pattern matching"
        
// pg. 512
// ------------------------------------------------------------------------- //
// Nested subproof.                                                          //
// ------------------------------------------------------------------------- //

let rec at once p gl = []
and once = []
    
// pg. 512
// ------------------------------------------------------------------------- //
// Nested subproof.                                                          //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let auto_tac byfn hyps g =
    match g with 
    | Goals ((asl, w) :: gls, jfn) -> 
        let th = justify byfn hyps w g
        Goals (gls, fun ths -> jfn (th :: ths))
    | _ -> failwith "auto_tac: incomplete pattern matching"
        
// pg. 512
// ------------------------------------------------------------------------- //
// A "lemma" tactic.                                                         //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let lemma_tac s p byfn hyps g =
    match g with 
    | Goals ((asl, w) :: gls, jfn) -> 
        let tr = imp_trans (justify byfn hyps p g)
        let mfn = if asl = [] then tr else imp_unduplicate << tr << shunt
        Goals (((s, p) :: asl, w) :: gls, jmodify jfn mfn)
    | _ -> failwith "lemma_tac: incomplete pattern matching"
        
// pg. 513
// ------------------------------------------------------------------------- //
// Elimination tactic for existential quantification.                        //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let exists_elim_tac l fm byfn hyps g =
    match fm,g with 
    | Exists (x, p), Goals ((asl, w) :: gls, jfn) -> 
        if List.exists (mem x << fv) (w :: List.map snd asl) then
            failwith "exists_elim_tac: variable free in assumptions"
        else
            let th = justify byfn hyps (Exists (x, p)) g
            let jfn' pth =
                imp_unduplicate(imp_trans th (exists_left x (shunt pth)))
            Goals (((l, p) :: asl, w) :: gls, jmodify jfn jfn')
    | _ -> failwith "exists_elim_tac: incomplete pattern matching"
            
// pg. 513
// ------------------------------------------------------------------------- //
// Elimination tactic for existential quantification.                        //
// ------------------------------------------------------------------------- //

let ante_disj th1 th2 =
    let p, r = dest_imp (concl th1)
    let q, s = dest_imp (concl th2)
    let ths = List.map contrapos [th1; th2]
    let th3 = imp_trans_chain ths (and_pair (Not p) (Not q))
    let th4 = contrapos (imp_trans (iff_imp2 (axiom_not r)) th3)
    let th5 = imp_trans (iff_imp1 (axiom_or p q)) th4
    right_doubleneg (imp_trans th5 (iff_imp1 (axiom_not (Imp (r, False)))))
        
// pg. 513
// ------------------------------------------------------------------------- //
// Elimination tactic for existential quantification.                        //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let disj_elim_tac l fm byfn hyps g =
    match g with 
    | Goals ((asl, w) :: gls, jfn) -> 
        let th = justify byfn hyps fm g
        let p, q = 
            match fm with 
            | Or (p, q) -> p, q
            | _ -> failwith "disj_elim_tac: incomplete pattern matching"
        let jfn' xs = 
            match xs with 
            | (pth :: qth :: ths) -> 
                let th1 = imp_trans th (ante_disj (shunt pth) (shunt qth))
                jfn (imp_unduplicate th1 :: ths)
            | _ -> failwith "disj_elim_tac: incomplete pattern matching"
        Goals (((l, p) :: asl, w) :: ((l, q) :: asl, w) :: gls, jfn')
    | _ -> failwith "disj_elim_tac: incomplete pattern matching"
    
// pg. 515
// ------------------------------------------------------------------------- //
// Declarative proof.                                                        //
// ------------------------------------------------------------------------- //

let multishunt i th =
    let th1 = imp_swap (funpow i (imp_swap << shunt) th)
    imp_swap (funpow (i - 1) (unshunt << imp_front 2) th1)

// dom modified to remove warning
let assume lps (gls) =
    match gls with 
    | Goals((asl, Imp (p, q)) :: gls, jfn) -> 
        if List.reduceBack mk_and (List.map snd lps) <> p then
            failwith "assume"
        else
            let jfn' th =
                if asl = [] then add_assum True th
                else multishunt (List.length lps) th
            Goals ((lps @ asl, q) :: gls, jmodify jfn jfn')
    | _ -> failwith "assume: incomplete pattern matching"

let note (l, p) = lemma_tac l p

let have p = note ("", p)

// dom modified to remove warning
let so tac arg byfn =
    tac arg (fun hyps p gl ->
        match gl with 
        | Goals ((asl, w) :: _, _) ->
            firstassum asl :: byfn hyps p gl
        | _ -> failwith "so: incomplete pattern matching"
    )

let fix = forall_intro_tac

let consider (x, p) =
    exists_elim_tac "" (Exists (x, p))

let take = exists_intro_tac

let cases fm byfn hyps g =
    disj_elim_tac "" fm byfn hyps g
        
// pg. 517
// ------------------------------------------------------------------------- //
// Thesis modification.                                                      //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let conclude p byfn hyps gl =
    match gl with 
    | Goals ((asl, w) :: gls, jfn) -> 
        let th = justify byfn hyps p gl
        if p = w then
            Goals ((asl, True) :: gls, jmodify jfn (fun _ -> th))
        else
            let p', q = dest_and w
            if p' <> p then
                failwith "conclude: bad conclusion"
            else
                let mfn th' = imp_trans_chain [th; th'] (and_pair p q)
                Goals ((asl, q) :: gls, jmodify jfn mfn)
    | _ -> failwith "conclude: incomplete pattern matching"
                
// pg. 517
// ------------------------------------------------------------------------- //
// A useful shorthand for solving the whole goal.                            //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let rec our thesis byfn hyps gl =
    match gl with 
    | Goals ((asl, w) :: gls, jfn) -> 
        conclude w byfn hyps gl
    | _ -> failwith "our: incomplete pattern matching"
and thesis = ""
    
// pg. 518
// ------------------------------------------------------------------------- //
// Termination.                                                              //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let qed gl =
    match gl with 
    | Goals ((asl, w) :: gls, jfn) -> 
        if w = True then
            Goals (gls, fun ths -> jfn (assumptate gl truth :: ths))
        else
            failwith "qed: non-trivial goal"
    | _ -> failwith "qed: incomplete pattern matching"

// Not in book
// ------------------------------------------------------------------------- //
// Some amusing efficiency tests versus a "direct" spec.                     //
// ------------------------------------------------------------------------- //

let test001 n = gen "x"

let double_th th =
    let tm = concl th in modusponens (modusponens (and_pair tm tm) th) th

let testcase n = gen "x" (funpow n double_th (lcftaut (parse "p(x) ==> q(1) \/ p(x)")))

let test002 n = 
    time (spec (parset "2"))  (testcase n),
    time (subst ("x" |=> (parset "2"))) (concl(testcase n))
