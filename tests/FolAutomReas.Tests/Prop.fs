module FolAutomReas.Tests.Prop

open Xunit
open FsUnit.Xunit

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