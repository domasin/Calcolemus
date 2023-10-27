// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// <summary>
/// Prenex and Skolem normal forms.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Calcolemus.Skolem

open Calcolemus.Lib.Set
open Calcolemus.Lib.Fpf

open Formulas
open Prop
open Fol

// ------------------------------------------------------------------------- //
// Routine simplification.                                                   //
// ------------------------------------------------------------------------- //

/// Performs a simplification routine but just at the first level of the input 
/// formula `fm`. It eliminates the basic propositional constants `False` and 
/// `True` and also the vacuous universal and existential quantiﬁers (those 
/// applied to variables that does not occur free in the body).
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// If x not in FV(p) then forall x. p and exists x. p are logically 
/// equivalent to p.
/// 
/// `simplify1 (parse @"exists x. P(y)")` returns `<<P(y)>>`
/// 
/// `simplify1 (parse @"true ==> exists x. P(x)")` returns `<<exists x. P(x)>>`
let simplify1 fm =
    match fm with
    | Forall (x, p) ->
        if mem x (fv p) then fm else p
    | Exists (x, p) ->
        if mem x (fv p) then fm else p
    | _ ->
        psimplify1 fm

/// Performs a simplification routine on the input formula 
/// `fm` eliminating the basic propositional constants `False` and `True`
/// and also the vacuous universal and existential quantiﬁers (those 
/// applied to variables that does not occur free in the body).
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// If x not in FV(p) then forall x. p and exists x. p are logically 
/// equivalent to p.
/// 
/// While `simplify1` performs the transformation just at the first level, 
/// `simplify` performs it at every levels in a recursive bottom-up sweep.
/// 
/// `simplify (parse @"true ==> (p <=> (p <=> false))")` returns 
/// `<<p <=> ~p>>`
/// 
/// `simplify (parse @"exists x y z. P(x) ==> Q(z) ==> false")` 
/// returns `<<exists x z. P(x) ==> ~Q(z)>>`
let rec simplify fm =
    match fm with
    | Not p ->
        simplify1 (Not (simplify p))
    | And (p, q) ->
        simplify1 (And (simplify p, simplify q))
    | Or (p, q) ->
        simplify1 (Or (simplify p, simplify q))
    | Imp (p, q) ->
        simplify1 (Imp (simplify p, simplify q))
    | Iff (p, q) ->
        simplify1 (Iff (simplify p, simplify q))
    | Forall (x, p) ->
        simplify1 (Forall (x, simplify p))
    | Exists (x, p) ->
        simplify1 (Exists (x, simplify p))
    | _ -> fm

// ------------------------------------------------------------------------- //
// Negation normal form.                                                     //
// ------------------------------------------------------------------------- //

/// Transforms the input formula `fm` in negation normal form.
/// 
/// It eliminates implication and equivalence, and pushes down negations 
/// through quantiﬁers.
/// 
/// `nnf (parse @"~ exists x. P(x) <=> Q(x)")` returns 
/// `<<forall x. P(x) /\ ~Q(x) \/ ~P(x) /\ Q(x)>>`
let rec nnf fm =
    match fm with
    | And (p, q) ->
        And (nnf p, nnf q)
    | Or (p, q) ->
        Or (nnf p, nnf q)
    | Imp (p, q) ->
        Or (nnf (Not p), nnf q)
    | Iff (p, q) ->
        Or (And (nnf p, nnf q), And (nnf (Not p), nnf (Not q)))
    | Not (Not p) ->
        nnf p
    | Not (And (p, q)) ->
        Or (nnf (Not p), nnf (Not q))
    | Not (Or (p, q)) ->
        And (nnf (Not p), nnf (Not q))
    | Not (Imp (p, q)) ->
        And (nnf p, nnf (Not q))
    | Not (Iff (p, q)) ->
        Or (And (nnf p, nnf (Not q)), And (nnf (Not p), nnf q))
    | Forall (x, p) ->
        Forall (x, nnf p)
    | Exists (x, p) ->
        Exists (x, nnf p)
    | Not (Forall (x, p)) ->
        Exists (x, nnf (Not p))
    | Not (Exists (x, p)) ->
        Forall (x, nnf (Not p))
    | _ -> fm

// ------------------------------------------------------------------------- //
// Prenex normal form.                                                       //
// ------------------------------------------------------------------------- //

/// It pulls out quantifiers.
let rec pullquants fm =
    match fm with
    | And (Forall (x, p), Forall (y, q)) ->
        pullq (true, true) fm mk_forall mk_and x y p q
    | Or (Exists(x, p), Exists(y, q)) ->
        pullq (true, true) fm mk_exists mk_or x y p q
    | And (Forall (x, p), q) ->
        pullq (true, false) fm mk_forall mk_and x x p q
    | And (p, Forall (y, q)) ->
        pullq (false, true) fm mk_forall mk_and y y p q
    | Or (Forall (x, p), q) ->
        pullq (true, false) fm mk_forall mk_or x x p q
    | Or (p, Forall (y, q)) ->
        pullq (false, true) fm mk_forall mk_or y y p q
    | And (Exists (x, p), q) ->
        pullq (true, false) fm mk_exists mk_and x x p q
    | And (p, Exists (y, q)) ->
        pullq (false, true) fm mk_exists mk_and y y p q
    | Or (Exists (x, p), q) ->
        pullq (true, false) fm mk_exists mk_or x x p q
    | Or (p, Exists (y, q)) ->
        pullq (false, true) fm mk_exists mk_or y y p q
    | _ -> fm
/// calls the main `pullquants` functions again on the body to pull up 
/// further quantiﬁers
and pullq(l,r) fm quant op x y p q =
    let z = variant x (fv fm)
    let p' = if l then subst (x |=> Var z) p else p
    let q' = if r then subst (y |=> Var z) q else q
    quant z (pullquants(op p' q'))

/// leaves quantiﬁed formulas alone, and for conjunctions and disjunctions 
/// recursively prenexes the immediate subformulas and then uses pullquants
let rec prenex fm =
    match fm with
    | Forall (x, p) ->
        Forall (x, prenex p)
    | Exists (x, p) ->
        Exists (x, prenex p)
    | And (p, q) ->
        pullquants (And (prenex p, prenex q))
    | Or (p, q) ->
        pullquants (Or (prenex p, prenex q))
    | _ -> fm

/// Transforms the input formula `fm` in prenex normal form and simplifies it.
/// 
/// * simplifies away False, True, vacuous quantiﬁcation, etc.;
/// * eliminates implication and equivalence, push down negations;
/// * pulls out quantiﬁers.
/// 
/// `pnf (parse @"(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P
/// (z) /\ Q(z))")` 
/// returns `<<exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)>>`
let pnf fm =
    prenex (nnf (simplify fm))

// ------------------------------------------------------------------------- //
// Get the functions in a term and formula.                                  //
// ------------------------------------------------------------------------- //

/// Returns the functions present in the input term `tm`
/// 
/// `funcs (parset @"x + 1")` returns `[("+", 2); ("1", 0)]`
let rec funcs tm =
    match tm with
    | Var x -> []
    | Fn (f, args) ->
        List.foldBack (union << funcs) args [f,List.length args]

/// Returns the functions present in the input formula `fm`
/// 
/// `functions (parse @"x + 1 > 0 /\ f(z) > g(z,i)")`
/// returns `[("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]`
/// `
let functions fm =
    atom_union (fun (R (p, a)) -> List.foldBack (union << funcs) a []) fm

// ------------------------------------------------------------------------- //
// Core Skolemization function.                                              //
// ------------------------------------------------------------------------- //

/// Core Skolemization function specifically intended to be used on NNF 
/// formulas. 
/// 
/// It simply recursively descends the formula, Skolemizing any existential 
/// formulas and then proceeding to subformulas using `skolem2` for binary 
/// connectives.
let rec skolem fm fns =
    match fm with
    | Exists (y, p) ->
        let xs = fv fm
        let f = variant (if xs = [] then "c_" + y else "f_" + y) fns
        let fx = Fn (f,List.map (fun x -> Var x) xs)
        skolem (subst (y |=> fx) p) (f :: fns)
    | Forall (x, p) -> 
        let p', fns' = skolem p fns 
        Forall (x, p'), fns'
    | And (p, q) -> skolem2 (fun (p, q) -> And (p, q)) (p, q) fns
    | Or (p, q) -> skolem2 (fun (p, q) -> Or (p, q)) (p, q) fns
    | _ -> fm, fns
/// Auxiliary to `skolem` when dealing with binary connectives. 
/// It updates the set of functions to avoid with new Skolem functions 
/// introduced into one formula before tackling the other.
and skolem2 cons (p, q) fns =
    let p', fns' = skolem p fns
    let q', fns'' = skolem q fns'
    cons (p', q'), fns''

// ------------------------------------------------------------------------- //
// Overall Skolemization function.                                           //
// ------------------------------------------------------------------------- //

/// Overall Skolemization function, intended to be used with any type of 
/// initial fol formula.
let askolemize fm =
    fst (skolem (nnf (simplify fm)) (List.map fst (functions fm)))

/// Removes all universale quantifiers from the input formula `p`.
/// 
/// `specialize <<forall x y. P(x) /\ P(y)>>` returns `<<P(x) /\ P(y)>>`
let rec specialize fm =
    match fm with
    | Forall (x, p) ->
        specialize p
    | _ -> fm

/// Puts the input formula `fm` into skolem normal form 
/// while also removing all universal quantifiers.
/// 
/// It puts the formula in prenex normal form, substitutes existential 
/// quantifiers with skolem functions and also removes all universal 
/// quantifiers.
/// 
/// `skolemize (parse @"forall x. exists y. R(x,y)")`
/// returns `<<R(x,f_y(x))>>`
let skolemize fm =
    specialize (pnf (askolemize fm))