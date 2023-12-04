// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Paramodulation

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Paramodulation

[<Fact>]
let ``paramodulate should return the paramodulation of the first clause to the second.``() = 
    paramodulate !!>["C";"S(0) = 1"] !!>["P(S(x))";"D"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`C`"; "`D`"; "`P(1)`"]]

[<Fact>]
let ``paramodulation-1.``() = 
    !! @"(forall x. f(f(x)) = f(x)) /\ (forall x. exists y. f(y) = x)
        ==> forall x. f(x) = x"
    |> paramodulation
    |> shouldEqual [true]