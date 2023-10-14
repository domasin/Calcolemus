module FolAutomReas.Tests.Prop

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Fpf

open FolAutomReas.Prop
open FolAutomReas.Formulas

[<Fact>]
let ``pname should return the name of the proposition.``() = 
    P "x" |> pname
    |> should equal "x"

[<Fact>]
let ``!> should return the parsed prop formula.``() = 
    !> "p /\ q ==> q /\ r"
    |> should equal (Imp (And (Atom (P "p"), Atom (P "q")), And (Atom (P "q"), Atom (P "r"))))

[<Fact>]
let ``eval should return the truth-value of the formula in the given valuation.``() = 
    eval (!>"p /\ q ==> q /\ r") 
        (function P"p" -> true | P"q" -> false | P"r" -> true | _ -> failwith "undefined")
    |> should equal true

[<Fact>]
let ``atoms should return the atoms of the formula.``() = 
    !>"p /\ q ==> q /\ r" 
    |> atoms
    |> should equal [P "p"; P "q"; P "r"]

[<Fact>]
let ``onallvaluations should return true if subfn returns true for each atoms on all valuations.``() = 
    onallvaluations (eval True) (fun _ -> false) []
    |> should equal true

[<Fact>]
let ``sprint_truthtable should return a string representation of the truth table of the input formula.``() = 
    sprint_truthtable !>"p ==> q"
    |> should equal 
        "p     q     |   formula
---------------------
false false | true  
false true  | true  
true  false | false 
true  true  | true  
---------------------

"

[<Fact>]
let ``tautology-1 should return false.``() = 
    !> @"p \/ ~p"
    |> tautology
    |> should equal true

[<Fact>]
let ``tautology-2 should return false.``() = 
    !> @"p \/ q ==> p"
    |> tautology
    |> should equal false

[<Fact>]
let ``tautology-3 should return false.``() = 
    !> @"p \/ q ==> q \/ (p <=> q)"
    |> tautology
    |> should equal false

[<Fact>]
let ``tautology-4 should return true.``() = 
    !> @"(p \/ q) /\ ~(p /\ q) ==> (~p <=> q)"
    |> tautology
    |> should equal true

[<Fact>]
let ``unsatisfiable should return true if the formula is unsatisfiable.``() = 
    !> "p /\ ~p"
    |> unsatisfiable
    |> should equal true

[<Fact>]
let ``unsatisfiable should return false if the formula is satisfiable.``() = 
    !> "p"
    |> unsatisfiable
    |> should equal false

[<Fact>]
let ``satisfiable should return false if the formula is unsatisfiable.``() = 
    !> "p /\ ~p"
    |> unsatisfiable
    |> should equal true

[<Fact>]
let ``satisfiable should return true if the formula is satisfiable.``() = 
    !> "p"
    |> unsatisfiable
    |> should equal false

[<Fact>]
let ``psubst should replace atoms with formulas based on fpf mapping.``() = 
    !> "p /\ q /\ p /\ q"
    |> psubst (P"p" |=> !>"p /\ q")
    |> sprint_prop_formula
    |> should equal "`(p /\ q) /\ q /\ (p /\ q) /\ q`"

[<Fact>]
let ``dual should return the dual of the input formula.``() = 
    !> @"p \/ ~p"
    |> dual
    |> sprint_prop_formula
    |> should equal "`p /\ ~p`"