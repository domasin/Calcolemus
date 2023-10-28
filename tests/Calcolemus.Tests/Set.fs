// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Set

open Xunit
open FsUnit.Xunit

open Calcolemus.Lib.Set

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
let ``subset should return true if first list is subset of the second [1;2;4;1].``() = 
    subset [1;2;3] [1;4;3;2] 
    |> should equal true

[<Fact>]
let ``subset should return true also if first list elements are the same of the second.``() = 
    subset [1;2;3] [2;3;1] 
    |> should equal true

[<Fact>]
let ``subset should return false if first list has elements that are not in the second.``() = 
    subset [1;2;3;4] [2;3;1] 
    |> should equal false

[<Fact>]
let ``psubset should return true if first list is a proper subset of the second [1;2;4;1].``() = 
    psubset [1;2;3] [1;4;3;2] 
    |> should equal true

[<Fact>]
let ``psubset should return false if first list elements are the same of the second.``() = 
    psubset [1;2;3] [2;3;1] 
    |> should equal false

[<Fact>]
let ``psubset should return false if first list has elements that are not in the second.``() = 
    psubset [1;2;3;4] [2;3;1] 
    |> should equal false

[<Fact>]
let ``subset should return false if first lista ha elements that are not in the second.``() = 
    subset [1;2;3;4] [2;3;1] 
    |> should equal false

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

[<Fact>]
let ``image should return the image of s under the function f.``() = 
    [1;2;3] |> image (fun x -> x * 2) 
    |> should equal [2; 4; 6]

[<Fact>]
let ``unions should compute the union of the input sets.``() = 
    unions [[1;2;3]; [4;3;2;6;2]; [5;3;1;3]]
    |> should equal [1; 2; 3; 4; 5; 6]

[<Fact>]
let ``mem should return false if the source does not contain the value.``() = 
    [1..9] |> mem 0
    |> should equal false

[<Fact>]
let ``mem should return true if the source contains the value.``() = 
    [1..9] |> mem 3
    |> should equal true

[<Fact>]
let ``mem-3.``() = 
    [1, "SpongeBob"; 2, "Patrick"; 3, "Squidward"; 4, "Mr. Krabs"] 
    |> mem (2, "Patrick")
    |> should equal true

[<Fact>]
let ``mem-4.``() = 
    [1, "SpongeBob"; 2, "Patrick"; 3, "Squidward"; 4, "Mr. Krabs"] 
    |> mem (22, "Patrick")
    |> should equal false

[<Fact>]
let ``allsets should return all subsets of the given size.``() = 
    allsets 2 [1;2;3]
    |> should equal [[1; 2]; [1; 3]; [2; 3]]

[<Fact>]
let ``allsubsets should return all subsets of the input set.``() = 
    allsubsets [1;2;3]
    |> should equal [[]; [1]; [1; 2]; [1; 2; 3]; [1; 3]; [2]; [2; 3]; [3]]

[<Fact>]
let ``allnonemptysubsets should return all nonempty subsets of the input set.``() = 
    allnonemptysubsets [1;2;3]
    |> should equal [[1]; [1; 2]; [1; 2; 3]; [1; 3]; [2]; [2; 3]; [3]]

