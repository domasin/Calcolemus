module Tests.Search

open Xunit
open FsUnit.Xunit

open FolAutomReas.lib.search
open FolAutomReas.lib.listFunctions

[<Fact>]
let ``tryfind butlast [[];[1;2;3];[4;3;5]] returns [1;2].``() = 
    tryfind butlast [[];[1;2;3];[4;3;5]] 
    |> should equal [1;2]

[<Fact>]
let ``mapfilter last [[1;2;3];[4;5];[];[6;7;8];[]] returns [3;5;8].``() = 
    mapfilter last [[1;2;3];[4;5];[];[6;7;8];[]]  
    |> should equal [3;5;8]

[<Fact>]
let ``maximize ((*) -1) [-1;2;3] returns -1``() = 
    maximize ((*) -1) [-1;2;3] 
    |> should equal -1

[<Fact>]
let ``minimize ((*) -1) [-1;2;3] returns 3``() = 
    minimize ((*) -1) [-1;2;3] 
    |> should equal 3