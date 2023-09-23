module Tests.Tableaux

open Xunit
open FsUnit.Xunit

open FolAutomReas.Tableaux
open FolAutomReas.Pelletier

[<Fact>]
let ``prawitz should succeed on p20 after trying 2 ground instance.``() = 
    prawitz p20
    |> should equal 2

[<Fact>]
let ``tab should succeed on p38 after trying 4 ground instance.``() = 
    tab p38
    |> should equal 4
