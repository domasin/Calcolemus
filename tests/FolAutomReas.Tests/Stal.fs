module FolAutomReas.Tests.Stal

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FolAutomReas.Stal
open FolAutomReas.Propexamples

[<Fact>]
let ``stalmarck (prime 11) should return true.``() = 
    stalmarck (prime 11)
    |> should equal true

// [<Fact>] // slow test
let ``stalmarck (mk_adder_test 6 3) should return true.``() = 
    stalmarck (mk_adder_test 6 3)
    |> should equal true