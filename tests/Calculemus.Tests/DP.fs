// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.DP

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open Calculemus.Formulas
open Calculemus.Prop
open Calculemus.DP
open Calculemus.Propexamples
open Calculemus.Lib.Fpf

[<Fact>]
let ``!>> should return a list of clauses.``() = 
    !>> [["p"];["p";"~q"]]
    |> should equal [[Atom (P "p")]; [Atom (P "p"); Not (Atom (P "q"))]]

[<Fact>]
let ``hasUnitClause should return true if clauses contain unit clauses.``() = 
    !>> [["p"];["p";"~q"]] 
    |> hasUnitClause
    |> should equal true

[<Fact>]
let ``hasUnitClause should return false if clauses don't contain unit clauses.``() = 
    !>> [["p";"~q"]] 
    |> hasUnitClause
    |> should equal false

[<Fact>]
let ``one_literal_rule should remove the first unit clause if present.``() = 
    !>> [["p"];["s";"t"];["q"]]
    |> one_literal_rule
    |> should equal [[Atom (P "q")]; [Atom (P "s"); Atom (P "t")]]

[<Fact>]
let ``one_literal_rule should remove complements of the literal from other clauses.``() = 
    !>> [["p"];["s";"~p"];["~p";"t"]]
    |> one_literal_rule
    |> should equal [[Atom (P "s")]; [Atom (P "t")]]

[<Fact>]
let ``one_literal_rule should remove all clauses containing the literal.``() = 
    !>> [["p"];["s";"~p"];["~p";"t"]]
    |> one_literal_rule
    |> should equal [[Atom (P "s")]; [Atom (P "t")]]

[<Fact>]
let ``one_literal_rule should fail if there aren't unit clauses.``() = 
    (fun () -> 
        !>> [["s";"p"];["q";"t"]] 
        |> one_literal_rule
        |> ignore
    )
    |> should (throwWithMessage "An index satisfying the predicate was not found in the collection.") typeof<System.Collections.Generic.KeyNotFoundException>

[<Fact>]
let ``hasPureLiteral should return false if clauses don't contain pure literals.``() = 
    !>> [["p";"q"];["~p";"~q"]]
    |> hasUnitClause
    |> should equal false

[<Fact>]
let ``hasPureLiteral should return true if clauses contain pure literals.``() = 
    !>> [["p"];["p";"~q"]] 
    |> hasPureLiteral
    |> should equal true

[<Fact>]
let ``pureLiterals should return the pure literals in clauses.``() = 
    !>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
    |> pureLiterals
    |> should equal [Atom (P "q")]

[<Fact>]
let ``pureLiterals should return an empty list if there aren't any pure literal.``() = 
    !>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]]
    |> pureLiterals
    |> shouldEqual []

[<Fact>]
let ``affirmative_negative_rule should remove all clauses containing pure literals if there are.``() = 
    !>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
    |> affirmative_negative_rule
    |> should equal [[Atom (P "p"); Atom (P "t")]]

[<Fact>]
let ``affirmative_negative_rule should fail if there aren't pure literals.``() = 
    (fun () -> 
        !>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]] 
        |> affirmative_negative_rule
        |> ignore
    )
    |> should (throwWithMessage "affirmative_negative_rule") typeof<System.Exception>

[<Fact>]
let ``resolve_on resolves clauses on p.``() = 
    !>> [["p";"c1";"c2"];
        ["~p";"d1";"d2";"d3";"d4"];
        ["q";"t"];
        ["p";"e1";"e2"]]
    |> resolve_on !>"p"
    |> should equal 
        !>> [["c1"; "c2"; "d1"; "d2"; "d3"; "d4"]; 
             ["d1"; "d2"; "d3"; "d4"; "e1"; "e2"];
             ["q"; "t"]]

[<Fact>]
let ``resolve_on should return the input unchanged if the rule is not applicable.``() = 
    !>> [["a"];["b"]]
    |> resolve_on !>"p"
    |> should equal 
        !>> [["a"];["b"]]

[<Fact>]
let ``resolution_blowup should return a number that drives the choice of the literal on which to resolve.``() = 
    let cls = !>> [
        ["p";"c"];["~p";"d"]
        ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    ]

    Assert.Equal(-1,resolution_blowup cls !>"c")
    Assert.Equal(-1,resolution_blowup cls !>"d")
    Assert.Equal(-1,resolution_blowup cls !>"e")
    Assert.Equal(-1,resolution_blowup cls !>"p")
    Assert.Equal(1,resolution_blowup cls !>"q")

[<Fact>]
let ``resolution_rule resolves clauses on the literal which minimizes resolution_blowup.``() = 
    !>> [
        ["p";"c"];["~p";"d"]
        ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    ]
    |> resolution_rule
    |> should equal !>> [
            ["c"; "d"]; ["q"; "~c"]; ["q"; "~d"]; 
            ["q"; "~e"]; ["~q"; "e"];["~q"; "~d"]
        ]

[<Fact>]
let ``dp should return true if the input is satisfiable.``() = 
    dp !>> [["p"]]
    |> should equal true

[<Fact>]
let ``dp should return false if the input is satisfiable.``() = 
    dp !>> [["p"];["~p"]]
    |> should equal false

[<Fact>]
let ``dpsat should return true if the input is satisfiable.``() = 
    dpsat !> "p"
    |> should equal true

[<Fact>]
let ``dpsat should return false if the input is satisfiable.``() = 
    dpsat !> "p /\ ~p"
    |> should equal false

[<Fact>]
let ``dptaut should return false if the input is not a tautology.``() = 
    dptaut !> "p"
    |> should equal false

[<Fact>]
let ``dptaut should return true if the input is satisfiable.``() = 
    dptaut (prime 11)
    |> should equal true

[<Fact>]
let ``posneg_count should return the number of l's occurrences in cls.``() = 
    posneg_count !>> [
        ["p";"c"];["~p";"d"]
        ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    ] !>"q"
    |> should equal 5

[<Fact>]
let ``dpll should return true if the input is satisfiable.``() = 
    dpll !>> [["p"]]
    |> should equal true

[<Fact>]
let ``dpll should return false if the input is satisfiable.``() = 
    dpll !>> [["p"];["~p"]]
    |> should equal false

[<Fact>]
let ``dpllsat should return true if the input is satisfiable.``() = 
    dpllsat !> "p"
    |> should equal true

[<Fact>]
let ``dpllsat should return false if the input is satisfiable.``() = 
    dpllsat !> "p /\ ~p"
    |> should equal false

[<Fact>]
let ``dplltaut should return false if the input is not a tautology.``() = 
    dplltaut !> "p"
    |> should equal false

[<Fact>]
let ``dplltaut should return true if the input is satisfiable.``() = 
    dplltaut (prime 11)
    |> should equal true

[<Fact>]
let ``unassigned should return the list of literals in clauses that are not in trail.``() = 
    let trail = [!>"p", Deduced;!>"q", Guessed]

    unassigned !>> [
        ["p";"c"];["~p";"d"]
        ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
    ] trail
    |> List.map sprint_prop_formula
    |> should equal ["`c`"; "`d`"; "`e`"]

[<Fact>]
let ``unit_subpropagate should return the fpf and trail updated with the result of unit propagation.``() = 

    ((!>> [["p"];["p";"q"]]), undefined,[])
    |> unit_subpropagate 
    |> fun (cls,fpf,trail) -> (cls,fpf |> graph,trail)
    |> should equal 
        (!>> [["p"]; ["p"; "q"]], [(!>"p", ())], [(!>"p", Deduced)])

[<Fact>]
let ``unit_subpropagate should return the fpf and trail updated with the result of unit propagation, updating clauses if there are complementary literals.``() = 

    ((!>> [["p"];["~p";"q"]]), undefined,[])
    |> unit_subpropagate 
    |> fun (cls,fpf,trail) -> (cls,fpf |> graph,trail)
    |> should equal 
        (!>> [["p"]; ["q"]], [(!>"p", ()); (!>"q", ())], [(!>"q", Deduced);(!>"p", Deduced)])

[<Fact>]
let ``unit_propagate should return the trail updated with flags on literals on which apply unit propagation.``() = 

    ((!>> [["p"];["p";"q"]]), [])
    |> unit_propagate 
    |> should equal 
        (!>> [["p"]; ["p"; "q"]], [(!>"p", Deduced)])

[<Fact>]
let ``unit_propagate should return the trail updated with the result of unit propagation, updating clauses if there are complementary literals.``() = 

    ((!>> [["p"];["~p";"q"]]), [])
    |> unit_propagate 
    |> should equal 
        (!>> [["p"]; ["q"]], [(!>"q", Deduced);(!>"p", Deduced)])

[<Fact>]
let ``backtrack should return trail from the first guessed literal.``() = 
    [
        !>"c", Deduced; 
        !>"b", Deduced; 
        !>"a", Guessed

        !>"e", Deduced; 
        !>"d", Guessed
    ]
    |> backtrack
    |> should equal 
        [(Atom (P "a"), Guessed); 
         (Atom (P "e"), Deduced); 
         (Atom (P "d"), Guessed)]

[<Fact>]
let ``backtrack should return the empty list if there are no more guessed literals``() =
    [
        !>"c", Deduced; 
        !>"b", Deduced; 
        !>"e", Deduced; 
    ]
    |> backtrack
    |> shouldEqual []

[<Fact>]
let ``dpli should return true if the input is satisfiable.``() = 
    dpli !>> [["p"]] []
    |> should equal true

[<Fact>]
let ``dpli should return false if the input is unsatisfiable.``() = 
    dpli !>> [["p"];["~p"]] []
    |> should equal false

[<Fact>]
let ``dplisat should return true if the input is satisfiable.``() = 
    dplisat !> "p"
    |> should equal true

[<Fact>]
let ``dplisat should return false if the input is satisfiable.``() = 
    dplisat !> "p /\ ~p"
    |> should equal false

[<Fact>]
let ``dplitaut should return false if the input is not a tautology.``() = 
    dplitaut !> "p"
    |> should equal false

[<Fact>]
let ``dplitaut should return true if the input is satisfiable.``() = 
    dplitaut (prime 11)
    |> should equal true

[<Fact>]
let ``backjump Goes back through the trail as far as possible while literal still leads to a conflict.``() =
    backjump !>>[["~p";"q"];["~q"]] !>"a"
        [
            !>"c", Deduced; 
            !>"b", Deduced; 
            !>"~a", Deduced
            !>"e", Guessed; 
            !>"p", Deduced; 
            !>"d", Guessed
        ]
    |> List.map (fun (fm,tm) -> sprint_prop_formula fm, tm)
    |> shouldEqual [("`p`", Deduced); ("`d`", Guessed)]

[<Fact>]
let ``dplb should return true if the input is satisfiable based on trail.``() = 
    dplb !>>[["~p";"q"];["~q"]] []
    |> should equal true

[<Fact>]
let ``dplb should return false if the input is unsatisfiable based on trail.``() = 
    dplb !>>[["~p";"q"];["~q"]] [!>"p", Deduced; !>"~q", Deduced]
    |> should equal false

[<Fact>]
let ``dplbsat should return true if the input is satisfiable.``() = 
    dplbsat !> "p"
    |> should equal true

[<Fact>]
let ``dplbsat should return false if the input is satisfiable.``() = 
    dplbsat !> "p /\ ~p"
    |> should equal false

[<Fact>]
let ``dplbtaut should return false if the input is not a tautology.``() = 
    dplbtaut !> "p"
    |> should equal false

[<Fact>]
let ``dplbtaut should return true if the input is satisfiable.``() = 
    dplbtaut (prime 11)
    |> should equal true

// [<Fact>] // slow test
let ``dplbtaut-3.``() = 
    dplbtaut (prime 101)
    |> should equal true
