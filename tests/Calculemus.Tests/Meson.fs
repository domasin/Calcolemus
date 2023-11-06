// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Meson

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Lib.Fpf

open Fol
open Meson


[<Fact>]
let ``contrapositives should return the contrapositives of the input clause.``() = 
    contrapositives !!>["P";"Q";"~R"]
    |> shouldEqual 
        [
            (!!>["~Q"; "R"], !!"P"); 
            (!!>["~P"; "R"], !!"Q"); 
            (!!>["~P"; "~Q"], !!"~R")
        ]

[<Fact>]
let ``contrapositives should return also the rule with false as conclusion if the input clause is all negative.``() = 
    contrapositives !!>["~P";"~Q";"~R"]
    |> shouldEqual 
        [
            (!!>["P"; "Q"; "R"], !!"false"); 
            (!!>["Q"; "R"], !!"~P"); 
            (!!>["P"; "R"], !!"~Q");
            (!!>["P"; "Q"], !!"~R")
        ]