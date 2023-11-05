// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Prolog

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Lib.Fpf

open Fol
open Clause
open Resolution
open Prolog


[<Fact>]
let ``renamerule should return the input rule with the variables renamed.``() = 
    renamerule 0 (!!>["P(x)";"Q(y)"],!!"P(f(x))")
    |> shouldEqual ((!!>["P(_0)"; "Q(_1)"], !!"P(f(_0))"), 2)

[<Fact>]
let ``backchain should return the successful instantiation if it exists.``() = 
    !!>["S(x) <= S(S(x))"] 
    |> backchain 
        [
            ([], !!"0 <= x"); 
            ([!!"x <= y"], !!"S(x) <= S(y)")
        ] 2 0 undefined
    |> graph
    |> List.map (fun (x,t) -> x, sprint_term t)
    |> shouldEqual 
        [("_0", "``x``"); ("_1", "``S(x)``"); 
         ("_2", "``_1``"); ("x", "``0``")]

[<Fact>]
let ``hornify should return the rule equivalent of the input if this is an Horn clause.``() = 
    !!>["~P(x)";"Q(y)";"~T(x)"]
    |> hornify
    |> shouldEqual (!!>["P(x)"; "T(x)"], !!"Q(y)")

[<Fact>]
let ``hornify should fail if the input is not an Horn clause.``() = 
    (fun () -> 
        !!>["P(x)";"Q(y)";"~T(x)"]
        |> hornify
        |> ignore
    )
    |> should (throwWithMessage "non-Horn clause") typeof<System.Exception>

[<Fact>]
let ``hornprove should return the successful instantiation ad depth level if the formula is valid and Horn clauses convertible.``() = 
    Pelletier.p32
    |> hornprove
    |> fun (inst,level) -> 
        graph inst, level
    |> shouldEqual 
        ([("_0", !!!"c_x()"); 
          ("_1", !!!"_0"); 
          ("_2", !!!"_0"); 
          ("_3", !!!"_2")], 8)

[<Fact>]
let ``hornprove should fail if the input is not an Horn clause.``() = 
    (fun () -> 
        !! @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) 
                ==> ~(~q \/ ~q)"
        |> hornprove
        |> ignore
    )
    |> should (throwWithMessage "non-Horn clause") typeof<System.Exception>

[<Fact>]
let ``parserule should return the rule corresponding to the input string, if this is syntactically valid.``() = 
    "S(X) <= S(Y) :- X <= Y"
    |> parserule
    |> shouldEqual ([!!"X <= Y"], !!"S(X) <= S(Y)")

[<Fact>]
let ``parserule should fail if the input is not syntactically valid.``() = 
    (fun () -> 
        "S(X) >"
        |> parserule
        |> ignore
    )
    |> should (throwWithMessage "Extra material after rule") typeof<System.Exception>

[<Fact>]
let ``simpleprolog should return the successful instantiation resulting from backchaining, if the goal is solvable from the rules.``() = 
    let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"]

    simpleprolog lerules "S(S(0)) <= S(S(S(0)))"
    |> graph
    |> List.map (fun (x,t) -> x, t |> sprint_term)
    |> shouldEqual 
        [("_0", "``S(0)``"); ("_1", "``S(S(0))``"); ("_2", "``0``"); 
         ("_3", "``S(0)``");("_4", "``_3``")]

[<Fact>]
let ``simpleprolog should fail if the goal is not solvable from the rules.``() = 
    let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"]

    (fun () -> 
        simpleprolog lerules "S(0) <= 0"
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>

[<Fact>]
let ``prolog should return a successful goal's variables instantiation, if the goal is solvable from the rules.``() = 
    let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"]

    prolog lerules "S(S(x)) <= S(S(S(0)))"
    |> List.map sprint_fol_formula
    |> shouldEqual ["`x = 0`"]

[<Fact>]
let ``prolog should fail if the goal is not solvable from the rules.``() = 
    let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"]

    (fun () -> 
        prolog lerules "S(0) <= 0"
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>