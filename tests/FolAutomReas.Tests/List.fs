module FolAutomReas.Tests.List

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.List

[<Fact>]
let ``last gives the last element for a non empty list.``() = 
    last [1;2;3;4;5]
    |> should equal 5

[<Fact>]
let ``last fails for empty list.``() = 
    (fun () -> ([]:int list) |> last |> ignore) 
    |> should (throwWithMessage "last") typeof<System.Exception>

[<Fact>]
let ``butlast should return of the elements of the input list but the last one.``() = 
    butlast [1;2;3;4;5]
    |> should equal [1;2;3;4]

[<Fact>]
let ``butlast fails for empty list.``() = 
    (fun () -> ([]:int list) |> butlast |> ignore) 
    |> should (throwWithMessage "butlast") typeof<System.Exception>

[<Fact>]
let ``all pairs apply f to all pairs of elements of two lists.``() = 
    allpairs (+) [1;2;3] [1;2]
    // [1+1; 1+2; 2+1; 2+2; 3+1; 3+2]
    |> should equal [2; 3; 3; 4; 4; 5]

[<Fact>]
let ``distinctpairs produces all pairs of distinct elements from a single list.``() = 
    distinctpairs [1;2;3;4]
    |> should equal [(1, 2); (1, 3); (1, 4); (2, 3); (2, 4); (3, 4)]

[<Fact>]
let ``chop_list chops a list at the specified index.``() = 
    chop_list 3 [1;2;3;4;5;6]
    |> should equal ([1;2;3], [4;5;6])

[<Fact>]
let ``chop_list fails if n is negative.``() = 
    (fun () -> ['a';'b';'c';'d'] |> chop_list -1 |> ignore) 
    |> should (throwWithMessage "chop_list") typeof<System.Exception>

[<Fact>]
let ``chop_list fails if n is greater than the length of the list.``() = 
    (fun () -> ['a';'b';'c';'d'] |> chop_list 5 |> ignore) 
    |> should (throwWithMessage "chop_list") typeof<System.Exception>

[<Fact>]
let ``insertat adds an element in a list at the specified index.``() = 
    [ 0; 1; 2 ] |> insertat 1 9
    |> should equal [0; 9; 1; 2]

[<Fact>]
let ``insertat fails if n is negative.``() = 
    (fun () -> 
        [ 0; 1; 2 ] |> insertat -1 9
        |> ignore
    ) 
    |> should (throwWithMessage "insertat: list too short for position to exist") typeof<System.Exception>

[<Fact>]
let ``insertat fails if n is greater than the length of the list.``() = 
    (fun () -> 
        [ 0; 1; 2 ] |> insertat 4 9
        |> ignore
    ) 
    |> should (throwWithMessage "insertat: list too short for position to exist") typeof<System.Exception>

[<Fact>]
let ``index returns position of given element in list.``() = 
    index 2 [0;1;3;3;2;3]
    |> should equal 4

[<Fact>]
let ``earlier [0;1;2;3] 2 3 return true.``() = 
    earlier [0;1;2;3] 2 3
    |> should equal true

[<Fact>]
let ``earlier [0;1;2;3] 3 2 returns false.``() = 
    earlier [0;1;2;3] 3 2
    |> should equal false