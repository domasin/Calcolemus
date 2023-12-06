// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Decidable

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Formulas
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

[<Fact>]
let ``atom should return the atom p(x).``() = 
    atom "P" "x"
    |> sprint_fol_formula
    |> shouldEqual "`P(x)`"

[<Fact>]
let ``premiss_A should return an A premiss.``() = 
    premiss_A ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`forall x. P(x) ==> S(x)`"

[<Fact>]
let ``premiss_E should return an E premiss.``() = 
    premiss_E ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`forall x. P(x) ==> ~S(x)`"

[<Fact>]
let ``premiss_I should return an I premiss.``() = 
    premiss_I ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`exists x. P(x) /\ S(x)`"

[<Fact>]
let ``premiss_O should return an O premiss.``() = 
    premiss_O ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`exists x. P(x) /\ ~S(x)`"

[<Fact>]
let ``anglicize_premiss should return an English reading of the input syllogism premiss.``() = 
    premiss_A ("P", "S")
    |> anglicize_premiss 
    |> shouldEqual "all P are S"

[<Fact>]
let ``anglicize_premiss should fail if applied to a formula that is not a syllogism premiss.``() = 
    (fun () -> 
        !!"P(x)"
        |> anglicize_premiss 
        |> ignore
    )
    |> should (throwWithMessage "anglicize_premiss: not a syllogism premiss (Parameter 'fm')") typeof<System.ArgumentException>

[<Fact>]
let ``anglicize_syllogism should return an English reading of the input syllogism.``() = 
    premiss_A ("M", "P")
    |> fun x -> mk_and x (premiss_A ("S", "M"))
    |> fun x -> mk_imp x (premiss_A ("S", "P"))
    |> anglicize_syllogism
    |> shouldEqual "If all M are P and all S are M, then all S are P"

[<Fact>]
let ``anglicize_syllogism should fail if applied to a formula that is not a syllogism.``() = 
    (fun () -> 
        !!"P(x)"
        |> anglicize_syllogism 
        |> ignore
    )
    |> should (throwWithMessage "anglicize_syllogism: not a syllogism (Parameter 'fm')") typeof<System.ArgumentException>