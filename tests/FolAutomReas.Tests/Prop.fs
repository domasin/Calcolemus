module FolAutomReas.Tests.Prop

open Xunit
open FsUnit.Xunit

open FolAutomReas.Prop

[<Fact>]
let ``pname should return the name of the proposition.``() = 
    P "x" |> pname
    |> should equal "x"