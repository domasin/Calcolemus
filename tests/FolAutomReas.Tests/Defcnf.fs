module FolAutomReas.Tests.Defcnf

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

[<Fact>]
let ``1=1``() = 
    1=1
    |> should equal true