module Tests.ListFunctions

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib

[<Fact>]
let ``last fails for empty list``() = 
    (fun () -> ([]:int list) |> last |> ignore) 
    |> should (throwWithMessage "last") typeof<System.Exception>

[<Fact>]
let ``last gves the last element for a non empty list``() = 
    last [1;2;3;4;5]
    |> should equal 5

[<Fact>]
let ``butlast fails for empty list``() = 
    (fun () -> ([]:int list) |> butlast |> ignore) 
    |> should (throwWithMessage "butlast") typeof<System.Exception>

let private butLastValues : (int list * int list)[] = [|
    [1],[]
    [1; 2],[1]
    [1; 2; 3],[1; 2]
|]
    
[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(2)>]
let ``List butlast`` idx = 
    let (list, result) = butLastValues[idx]
    butlast list
    |> should equal result

[<Fact>]
let ``all pairs apply f to all pairs of elements of two lists``() = 
    allpairs (+) [1;2;3] [1;2]
    |> should equal [2; 3; 3; 4; 4; 5]

    // allpairs (fun x y -> "f",x,y) [1;2;3] [1;2]
    // [("f", 1, 1); ("f", 1, 2); ("f", 2, 1); ("f", 2, 2); ("f", 3, 1);("f", 3, 2)]

[<Fact>]
let ``distinctpairs produces all pairs of distinct elements from a single list``() = 
    distinctpairs [1;2;3;4]
    |> should equal [(1, 2); (1, 3); (1, 4); (2, 3); (2, 4); (3, 4)]

[<Fact>]
let ``chop_list fails if n is negative``() = 
    (fun () -> ['a';'b';'c';'d'] |> chop_list -1 |> ignore) 
    |> should (throwWithMessage "chop_list") typeof<System.Exception>

[<Fact>]
let ``chop_list fails if n is greater than the length of the list``() = 
    (fun () -> ['a';'b';'c';'d'] |> chop_list 5 |> ignore) 
    |> should (throwWithMessage "chop_list") typeof<System.Exception>

[<Fact>]
let ``chop_list chops a list at the specified index``() = 
    chop_list 3 [1;2;3;4;5;6]
    |> should equal ([1;2;3], [4;5;6])

[<Fact>]
let ``insertat adds an element in a list at the specified index``() = 
    insertat 2 999 [0;1;2;3]
    |> should equal [0; 1; 999; 2; 3]

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