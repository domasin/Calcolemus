module Calcolemus.Tests.Skolem

open Xunit
open FsUnit.Xunit

open Calcolemus.Fol
open Calcolemus.Skolem

[<Fact>]
let ``simplify1 `exists x. P(y)` returns `P(y)`.``() = 
    simplify1 (parse "exists x. P(y)")
    |> sprint_fol_formula
    |> should equal "`P(y)`"

[<Fact>]
let ``simplify1 `true ==> exists x. P(x)` returns `exists x. P(x)`.``() = 
    simplify1 (parse "true ==> exists x. P(x)")
    |> sprint_fol_formula
    |> should equal "`exists x. P(x)`"

[<Fact>]
let ``simplify `true ==> (p <=> (p <=> false))` returns `p <=> ~p`.``() = 
    simplify (parse "true ==> (p <=> (p <=> false))")
    |> sprint_fol_formula
    |> should equal "`p <=> ~p`"

[<Fact>]
let ``simplify `exists x y z. P(x) ==> Q(z) ==> false` returns `exists x z. P(x) ==> ~Q(z)`.``() = 
    simplify (parse "exists x y z. P(x) ==> Q(z) ==> false")
    |> sprint_fol_formula
    |> should equal "`exists x z. P(x) ==> ~Q(z)`"

[<Fact>]
let ``simplify `(forall x y. P(x) \/ (P(y) /\ false)) ==> exists z. Q` returns `exists x z. P(x) ==> ~Q(z)`.``() = 
    simplify (parse @"(forall x y. P(x) \/ (P(y) /\ false)) ==> exists z. Q")
    |> sprint_fol_formula
    |> should equal "`(forall x. P(x)) ==> Q`"

[<Fact>]
let ``nnf `(forall x. P(x)) ==> ((exists y. Q(y)) <=> exists z. P(z) /\ Q(z))` returns `(exists x. ~P(x)) \/ (exists y. Q(y)) /\ (exists z. P(z) /\ Q(z)) \/ (forall y. ~Q(y)) /\ (forall z. ~P(z) \/ ~Q(z))`.``() = 
    nnf (parse @"(forall x. P(x)) ==> ((exists y. Q(y)) <=> exists z. P(z) /\ Q(z))")
    |> sprint_fol_formula
    |> should equal @"`(exists x. ~P(x)) \/ (exists y. Q(y)) /\ (exists z. P(z) /\ Q(z)) \/ (forall y. ~Q(y)) /\ (forall z. ~P(z) \/ ~Q(z))`"

[<Fact>]
let ``pnf `(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P(z) /\ Q(z))` returns `exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)`.``() = 
    pnf (parse @"(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P(z) /\ Q(z))")
    |> sprint_fol_formula
    |> should equal @"`exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)`"

[<Fact>]
let ``funcs (parset "x + 1") returns [("+", 2); ("1", 0)].``() = 
    funcs (parset "x + 1") 
    |> should equal [("+", 2); ("1", 0)]

[<Fact>]
let ``functions `x + 1 > 0 /\ f(z) > g(z,i)` returns [("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)].``() = 
    functions (parse @"x + 1 > 0 /\ f(z) > g(z,i)") 
    |> should equal [("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]

[<Fact>]
let ``skolemize `exists y. x < y ==> forall u. exists v. x * u < y * v` returns `~x < f_y(x) \/ x * u < f_y(x) * f_v(u,x)`.``() = 
    skolemize (parse @"exists y. x < y ==> forall u. exists v. x * u < y * v") 
    |> sprint_fol_formula
    |> should equal @"`~x < f_y(x) \/ x * u < f_y(x) * f_v(u,x)`"