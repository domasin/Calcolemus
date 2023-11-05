// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Clause

open Xunit
open FsUnit.Xunit

open Calculemus


open Fol
open Clause
open FsUnitTyped
open Calculemus.Formulas
open Calculemus.Herbrand

[<Fact>]
let ``literals should return the literals in the formula.``() = 
    !!"P(x) ==> Q(x)"
    |> literals
    |> List.map sprint_fol_formula
    |> shouldEqual ["`P(x)`";"`Q(x)`"]

[<Fact>]
let ``opposite should return the literals that occur both positively and negatively.``() = 
    List.map (!!) ["P(x)";"Q(x)";"~P(x)"]
    |> opposites
    |> List.map sprint_fol_formula
    |> shouldEqual ["`P(x)`"]

[<Fact>]
let ``djsToClauses should return the list of clauses corresponding to the dnf formula.``() = 
    !! @"(Q(x) /\ ~R(x,y) /\ P(f(z)) \/ (~P(x) /\ Q(x)))"
    |> djsToClauses
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`Q(x)`"; "`~R(x,y)`"; "`P(f(z))`"]; ["`~P(x)`"; "`Q(x)`"]]

[<Fact>]
let ``cjsToClauses should return the list of clauses corresponding to the cnf formula.``() = 
    !! @"(Q(x) \/ ~R(x,y) \/ P(f(z))) /\ (~P(x) \/ Q(x))"
    |> cjsToClauses
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`Q(x)`"; "`~R(x,y)`"; "`P(f(z))`"]; ["`~P(x)`"; "`Q(x)`"]]

[<Fact>]
let ``clausesToDnf should return a dnf formula equivalent to the input clauses.``() = 
    !!>>[["Q(x)"; "~R(x,y)"; "P(f(z))"]; ["~P(x)"; "Q(x)"]]
    |> clausesToDnf
    |> sprint_fol_formula
    |> shouldEqual @"`Q(x) /\ ~R(x,y) /\ P(f(z)) \/ ~P(x) /\ Q(x)`"

