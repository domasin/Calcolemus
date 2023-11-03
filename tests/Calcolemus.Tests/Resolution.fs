// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Resolution

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calcolemus

open Lib.Fpf

open Fol
open Clause
open Resolution


[<Fact>]
let ``mgu should return an mgu for a set of literals if they are unifiable.``() = 
    mgu !!>["P(x)";"P(f(y))"] undefined
    |> graph
    |> shouldEqual [("x", !!!"f(y)")]

[<Fact>]
let ``mgu should return an empty instantiation if the list contains only false terms.``() = 
    mgu !!>["false";"false"] undefined
    |> graph
    |> shouldEqual []

[<Fact>]
let ``mgu should fail when there is a cycle.``() = 
    (fun () -> 
        mgu !!>["P(x)";"P(f(x))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``mgu should fail when the unification is impossible.``() = 
    (fun () -> 
        mgu !!>["P(0)";"P(f(y))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

[<Fact>]
let ``mgu should fail when the input is not a list of literals.``() = 
    (fun () -> 
        mgu !!>["P(x) /\ Q(x)";"P(f(y)) /\ Q(f(y))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "Can't unify literals") typeof<System.Exception>

[<Fact>]
let ``unifiable should return true if the input are literals and unifiable.``() = 
    unifiable !!"P(x)" !!"P(f(y))"
    |> shouldEqual true

[<Fact>]
let ``unifiable should return false if the input are not literals or they are not unifiable.``() = 
    unifiable !!"P(x)" !!"P(f(x))"
    |> shouldEqual false

[<Fact>]
let ``rename should return the list of formulas with the free variables renamed with the given prefix.``() = 
    rename "old_" !!>["P(x)";"Q(y)"]
    |> List.map sprint_fol_formula
    |> shouldEqual ["`P(old_x)`"; "`Q(old_y)`"]

[<Fact>]
let ``resolvents should return the result of resolving the input clauses on the given literal.``() = 
    resolvents 
        !!>["P(x)";"~R(x,y)";"Q(x)";"P(0)"]
        !!>["~P(f(y))";"T(x,y,z)";"~P(z)"]
        !!"P(x)"
        []
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`P(0)`"; "`Q(z)`"; "`T(z,y,z)`"; "`~P(f(y))`"; "`~R(z,y)`"];
         ["`P(0)`"; "`Q(f(y))`"; "`T(f(y),y,z)`"; "`~P(z)`"; "`~R(f(y),y)`"];
         ["`P(0)`"; "`Q(f(y))`"; "`T(f(y),y,f(y))`"; "`~R(f(y),y)`"];
         ["`Q(0)`"; "`T(0,y,0)`"; "`~P(f(y))`"; "`~R(0,y)`"]]

[<Fact>]
let ``resolve_clauses should return all the resolvents of the input clauses.``() = 
    resolve_clauses 
        !!>["P(x)";"Q(x)";"P(0)"]
        !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`P(0)`"; "`Q(yz)`"; "`~P(f(yy))`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~P(yz)`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~Q(f(yy))`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`P(yz)`"; "`P(0)`"; "`~P(yz)`"; "`~P(f(yy))`"];
         ["`P(xx)`"; "`Q(xx)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"]]

[<Fact>]
let ``basic_resloop should return true if the input set of formulas is unsatisfiable and a proof is found.``() = 
    basic_resloop ([],!!>>[["P(x)"];["~P(x)"]])
    |> shouldEqual true

[<Fact>]
let ``basic_resloop should fail if it can't find a proof.``() = 
    (fun () -> 
        basic_resloop ([],!!>>[["P(x)"]])
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_basic_resolution should return true if the input formula is unsatisfiable and a proof is found.``() = 
    !!"P(x) /\ ~P(x)"
    |> pure_basic_resolution
    |> shouldEqual true

[<Fact>]
let ``pure_basic_resolution should fail if it can't find a proof.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_basic_resolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>