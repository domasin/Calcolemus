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

[<Fact>]
let ``rename_form should return the formula with an 'old_' prefix added to each function symbols.``() = 
    !!"P(f(g(x),z))"
    |> rename_form
    |> sprint_fol_formula
    |> shouldEqual "`P(old_f(old_g(x),z))`"

[<Fact>]
let ``skolems should return the pair of the skolemized functions set and the names to avoid.``() = 
    skolems !!>["exists x. P(f(g(x),z))"; "forall x. exists y. P(x,y)"] []
    |> fun (x,y) -> x |> List.map sprint_fol_formula, y
    |> should equal 
        (["`P(old_f(old_g(f_x(z)),z))`"; "`forall x. P(x,f_y(x))`"], ["f_y"; "f_x"])

[<Fact>]
let ``skolemizes should return the pair of the skolemized functions set.``() = 
    skolemizes !!>["exists x. P(f(g(x),z))"; "forall x. exists y. P(x,y)"]
    |> List.map sprint_fol_formula
    |> should equal 
        ["`P(old_f(old_g(f_x(z)),z))`"; "`forall x. P(x,f_y(x))`"]