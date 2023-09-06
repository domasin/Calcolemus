module Tests.Sorting

open Xunit
open FsUnit.Xunit

open FolAutomReas.lib.sorting

[<Fact>]
let ``merge (<) [1;3;7] [2;4;5;6] returns [1;2;3;4;5;6;7].``() = 
    merge (<) [1;3;7] [2;4;5;6]
    |> should equal [1;2;3;4;5;6;7]

[<Fact>]
let ``sort (<) [3;1;4;1;5;9;2;6;5;3;5] returns [1;1;2;3;3;4;5;5;5;6;9].``() = 
    sort (<) [3;1;4;1;5;9;2;6;5;3;5] 
    |> should equal [1;1;2;3;3;4;5;5;5;6;9]

[<Fact>]
let ``increasing List.length [1] [1;2] returns true.``() = 
    increasing List.length [1] [1;2]
    |> should equal true

[<Fact>]
let ``decreasing List.length [1;2] [1] returns true.``() = 
    decreasing List.length [1;2] [1]
    |> should equal true
