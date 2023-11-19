// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Cong

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Partition

open Fol
open Cong

[<Fact>]
let ``subterms should return the list of subterms of the input term.``() = 
    subterms !!!"f(g(x),y)"
    |> List.map sprint_term
    |> shouldEqual ["``x``"; "``y``"; "``f(g(x),y)``"; "``g(x)``"]

[<Fact>]
let ``congruent should return true if all the immediate subterms are already equivalent.``() = 
    congruent 
        (equate (!!!"2",!!!"4")unequal) 
        (!!!"f(4)", !!!"f(2)")
    |> shouldEqual true

[<Fact>]
let ``congruent should return false if not all the immediate subterms are already equivalent.``() = 
    congruent 
        (equate (!!!"2",!!!"4")unequal) 
        (!!!"f(4)", !!!"f(3)")
    |> shouldEqual false