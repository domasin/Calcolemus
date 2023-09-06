module Tests.AssociationLists

open Xunit
open FsUnit.Xunit

open FolAutomReas.lib.associationLists

[<Fact>]
let ``assoc 2 [(1,2);(2,3)] returns 3.``() = 
    assoc 2 [(1,2);(2,3)]
    |> should equal 3

[<Fact>]
let ``rev_assoc 2 [(1,2);(2,3)] returns 1.``() = 
    rev_assoc 2 [(1,2);(2,3)]
    |> should equal 1

