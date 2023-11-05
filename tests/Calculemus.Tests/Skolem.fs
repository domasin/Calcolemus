// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Skolem

open Xunit
open FsUnit.Xunit

open Calculemus

open Fol
open Skolem
open Formulas
open FsUnitTyped

[<Fact>]
let ``simplify1 `exists x. P(y)` returns `P(y).``() = 
    simplify1 (parse "exists x. P(y)")
    |> sprint_fol_formula
    |> should equal "`P(y)`"

[<Fact>]
let ``simplify1 `true ==> exists x. P(x)` returns `exists x. P(x).``() = 
    simplify1 (parse "true ==> exists x. P(x)")
    |> sprint_fol_formula
    |> should equal "`exists x. P(x)`"

[<Fact>]
let ``simplify `true ==> (p <=> (p <=> false))` returns `p <=> ~p.``() = 
    simplify (parse "true ==> (p <=> (p <=> false))")
    |> sprint_fol_formula
    |> should equal "`p <=> ~p`"

[<Fact>]
let ``simplify `exists x y z. P(x) ==> Q(z) ==> false` returns `exists x z. P(x) ==> ~Q(z).``() = 
    simplify (parse "exists x y z. P(x) ==> Q(z) ==> false")
    |> sprint_fol_formula
    |> should equal "`exists x z. P(x) ==> ~Q(z)`"

[<Fact>]
let ``simplify `(forall x y. P(x) \/ (P(y) /\ false)) ==> exists z. Q` returns `exists x z. P(x) ==> ~Q(z).``() = 
    simplify (parse @"(forall x y. P(x) \/ (P(y) /\ false)) ==> exists z. Q")
    |> sprint_fol_formula
    |> should equal "`(forall x. P(x)) ==> Q`"

[<Fact>]
let ``nnf `(forall x. P(x)) ==> ((exists y. Q(y)) <=> exists z. P(z) /\ Q(z))` returns `(exists x. ~P(x)) \/ (exists y. Q(y)) /\ (exists z. P(z) /\ Q(z)) \/ (forall y. ~Q(y)) /\ (forall z. ~P(z) \/ ~Q(z)).``() = 
    nnf (parse @"(forall x. P(x)) ==> ((exists y. Q(y)) <=> exists z. P(z) /\ Q(z))")
    |> sprint_fol_formula
    |> should equal @"`(exists x. ~P(x)) \/ (exists y. Q(y)) /\ (exists z. P(z) /\ Q(z)) \/ (forall y. ~Q(y)) /\ (forall z. ~P(z) \/ ~Q(z))`"

[<Fact>]
let ``pullquants should return the conjunctions and disjunctions with quantifiers pulled out.``() = 
    !!"(forall x. P(x)) /\ (exists y. P(y))"
    |> pullquants
    |> sprint_fol_formula
    |> should equal "`forall x. exists y. P(x) /\ P(y)`"

[<Fact>]
let ``pullq should return the formula with quantifiers pulled out for various similar transformation steps of conjunctions and disjunctions.``() = 
    let fm = !!"P(x) /\ exists y. Q(y)"
    pullq (false, true) fm mk_exists mk_and "y" "y" !!"P(x)" !!"Q(y)"
    |> sprint_fol_formula
    |> should equal "`exists y. P(x) /\ Q(y)`"

[<Fact>]
let ``prenex should return the formula (already simplified and in nnf) with quantifiers pulled out.``() = 
    !!"forall x. P(X) /\ forall y. Q(x,y)"
    |> prenex
    |> sprint_fol_formula
    |> should equal "`forall x y. P(X) /\ Q(x,y)`"

[<Fact>]
let ``pnf `(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P(z) /\ Q(z))` returns `exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z).``() = 
    pnf (parse @"(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P(z) /\ Q(z))")
    |> sprint_fol_formula
    |> should equal @"`exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)`"

[<Fact>]
let ``funcs should return the functions of the term.``() = 
    funcs !!!"x + 1"
    |> should equal [("+", 2); ("1", 0)]

[<Fact>]
let ``functions should return the functions of the formula.``() = 
    functions !!"x + 1 > 0 /\ f(z) > g(z,i)"
    |> should equal [("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]

[<Fact>]
let ``skolem should return the pair of skolemized function and names to avoid.``() = 
    skolem !!"forall x. exists y. P(x,y)" []
    |> should equal (!!"forall x. P(x,f_y(x))", ["f_y"])

[<Fact>]
let ``skolem2 should return the pair of skolemized function and names to avoid reconstructing the binary formula.``() = 

    let p,q = !!"forall x. exists y. P(x,y)", !!"forall x. exists y. Q(x,y)"

    skolem2 And (p,q) []
    |> should equal (!!"(forall x. P(x,f_y(x))) /\ (forall x. Q(x,f_y'(x)))", ["f_y'"; "f_y"])

[<Fact>]
let ``askolemize should return the input with all existential quantifiers replaced by Skolem functions.``() = 

    askolemize !!"forall x. exists y. R(x,y)"
    |> sprint_fol_formula
    |> should equal "`forall x. R(x,f_y(x))`"

[<Fact>]
let ``specialize should return the input with all universal quantifiers removed.``() = 

    specialize !!"forall x y. P(x) /\ P(y)"
    |> sprint_fol_formula
    |> should equal "`P(x) /\ P(y)`"

[<Fact>]
let ``skolemize should return an equisatisfiable Skolem normal form of the input with also the universal quantifiers removed.``() = 
    skolemize !!"exists y. x < y ==> forall u. exists v. x * u < y * v"
    |> sprint_fol_formula
    |> should equal @"`~x < f_y(x) \/ x * u < f_y(x) * f_v(u,x)`"