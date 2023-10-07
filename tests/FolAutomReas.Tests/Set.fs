module FolAutomReas.Tests.Set

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Set

[<Fact>]
let ``setify [1;2;3;1;4;3] returns [1;2;3;4].``() = 
    setify [1;2;3;1;4;3] 
    |> should equal [1;2;3;4]

[<Fact>]
let ``union [1;2;3] [1;5;4;3] returns [1;2;3;4;5].``() = 
    union [1;2;3] [1;5;4;3]
    |> should equal [1;2;3;4;5]

[<Fact>]
let ``union [1;1;1] [1;2;3;2] returns [1;2;3].``() = 
    union [1;1;1] [1;2;3;2]
    |> should equal [1;2;3]

[<Fact>]
let ``intersect [1;2;3] [3;5;4;1] returns [1;3].``() = 
    intersect [1;2;3] [3;5;4;1]
    |> should equal [1;3]

[<Fact>]
let ``intersect [1;2;4;1] [1;2;3;2] returns [1;2].``() = 
    intersect [1;2;4;1] [1;2;3;2]
    |> should equal [1;2]

[<Fact>]
let ``subtract [1;2;3] [3;5;4;1] returns [2].``() = 
    subtract [1;2;3] [3;5;4;1]
    |> should equal [2]

[<Fact>]
let ``subtract [1;2;4;1] [4;5] returns [1;2].``() = 
    subtract [1;2;4;1] [4;5]
    |> should equal [1;2]

[<Fact>]
let ``set_eq [1;2] [2;1;2] returns true.``() = 
    set_eq [1;2] [2;1;2]
    |> should equal true

[<Fact>]
let ``set_eq [1;2] [1;3] returns false.``() = 
    set_eq [1;2] [1;3]
    |> should equal false

[<Fact>]
let ``insert 4 [2;3;3;5] returns [2;3;4;5].``() = 
    insert 4 [2;3;3;5] 
    |> should equal [2;3;4;5]

