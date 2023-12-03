// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Paramodulation

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Fol
open Clause
open Paramodulation

[<Fact>]
let ``Paramodulation should return the paramodulation of the first clause to the second.``() = 
    paramodulate !!>["C";"S(0) = 1"] !!>["P(S(x))";"D"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`C`"; "`D`"; "`P(1)`"]]