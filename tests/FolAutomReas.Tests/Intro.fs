module FolAutomReas.Tests.Intro

open Xunit
open FsUnit.Xunit

open FolAutomReas.Intro

[<Fact>]
let ``simplify1 should return a simplified expression if simplifiable.``() = 
    Add(Const 0, Const 1) |> simplify1
    |> should equal (Const 1)

[<Fact>]
let ``simplify1 should return the input expression itself if not simplifiable.``() = 
    Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 
    |> should equal (Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)))