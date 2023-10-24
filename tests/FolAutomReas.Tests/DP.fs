module FolAutomReas.Tests.DP

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.DP

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
let ``resolve_on resolves clauses on p``() = 
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
let ``resolve_on returns the input unchanged if the rule is not applicable.``() = 
    !>> [["a"];["b"]]
    |> resolve_on !>"p"
    |> should equal 
        !>> [["a"];["b"]]