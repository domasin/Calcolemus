// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Decidable

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Skolem
open Decidable

[<Fact>]
let ``aedecide should return true on a valid AE formula.``() = 
    !! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
    (forall u v w x y z.
    P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) <=> P(u,z,v)))
    ==> forall a b c. P(a,b,c) ==> P(b,a,c)"
    |> aedecide
    |> shouldEqual true

[<Fact>]
let ``aedecide should fail with 'Not decidable' if the input isn't an AE formula.``() = 
    (fun () -> 
        !! @"forall x. f(x) = 0"
        |> aedecide
        |> ignore
    )
    |> should (throwWithMessage "Not decidable") typeof<System.Exception>

[<Fact>]
let ``separate should return the existential formula of the conjunction of the cjs in which x is free conjuncted with the cjs in which x is not.``() = 
    !!>["P(x)"; "Q(y)"; "T(y) /\ R(x,y)"; "S(z,w) ==> Q(i)"]
    |> separate "x"
    |> sprint_fol_formula
    |> shouldEqual "`(exists x. P(x) /\ T(y) /\ R(x,y)) /\ Q(y) /\ (S(z,w) ==> Q(i))`"

[<Fact>]
let ``pushquant should return the formula <c>exists x. p</c> transformed into an equivalent with the scope of the quantifier reduced.``() = 
    !!"P(x) ==> forall y. Q(y)"
    |> pushquant "x"
    |> sprint_fol_formula
    |> shouldEqual @"`(exists x. ~P(x)) \/ (forall y. Q(y))`"

[<Fact>]
let ``miniscope should return a formula equivalent to the input with the scope of quantifiers minimized.``() = 
    miniscope(nnf !!"exists y. forall x. P(y) ==> P(x)")
    |> sprint_fol_formula
    |> shouldEqual @"`(exists y. ~P(y)) \/ (forall x. P(x))`"

[<Fact>]
let ``wang should return true on a valid formula that after miniscoping is in AE.``() = 
    wang Pelletier.p20
    |> shouldEqual true

[<Fact>]
let ``wang should fail with 'Not decidable' if the input even after applying miniscoping is not in AE.``() = 
    (fun () -> 
        !! @"forall x. f(x) = 0"
        |> wang
        |> ignore
    )
    |> should (throwWithMessage "Not decidable") typeof<System.Exception>