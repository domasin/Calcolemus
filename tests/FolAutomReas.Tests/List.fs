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
let ``index fails if no element in the list equals x.``() = 
    (fun () -> 
        index 5 [0;1;3;3;2;3]
        |> ignore
    ) 
    |> should (throwWithMessage "An index satisfying the predicate was not found in the collection.") typeof<System.Collections.Generic.KeyNotFoundException>

[<Fact>]
let ``earlier should return true if x comes earlier than y in list.``() = 
    earlier [0;1;2;3] 2 3
    |> should equal true

[<Fact>]
let ``earlier should return true if x is in list but not y.``() = 
    earlier [0;1;2;3] 3 4
    |> should equal true

[<Fact>]
let ``earlier should return false if x doesn't come earlier than y in list.``() = 
    earlier [0;1;2;3] 3 2
    |> should equal false

[<Fact>]
let ``earlier should return false if y is in list but not x.``() = 
    earlier [0;1;3] 2 3
    |> should equal false

[<Fact>]
let ``earlier should return false if both x and y are not in list.``() = 
    earlier [0;1;2;3] 4 5
    |> should equal false

[<Fact>]
let ``assoc should return the second component of the pair if a matching for the first is found.``() = 
    assoc 2 [(1,2);(2,3)]
    |> should equal 3

[<Fact>]
let ``assoc should return just the first occurrence if a matching is found.``() = 
    assoc 2 [(1,2);(2,3);(2,4)]
    |> should equal 3

[<Fact>]
let ``assoc should fail with message 'find' if no matching is found.``() = 
    (fun () -> 
        assoc 3 [(1,2);(2,3)]
        |> ignore
    ) 
    |> should (throwWithMessage "find") typeof<System.Exception>

[<Fact>]
let ``rev_assoc should return the first component of the list if a matching for the second is found.``() = 
    rev_assoc 2 [(1,2);(2,3)]
    |> should equal 1

[<Fact>]
let ``rev_assoc should return just the first occurrence if a matching is found.``() = 
    rev_assoc 2 [(1,2);(2,2);(2,3)]
    |> should equal 1

[<Fact>]
let ``rev_assoc should fail with message 'find' if no matching is found.``() = 
    (fun () -> 
        rev_assoc 1 [(1,2);(2,3)]
        |> ignore
    ) 
    |> should (throwWithMessage "find") typeof<System.Exception>


