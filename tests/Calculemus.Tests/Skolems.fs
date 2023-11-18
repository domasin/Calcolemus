// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Skolems

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Fol
open Skolems

[<Fact>]
let ``rename_term should return the term with an 'old_' prefix added to each function symbols.``() = 
    !!!"f(g(x),z)"
    |> rename_term
    |> sprint_term
    |> shouldEqual "``old_f(old_g(x),z)``"