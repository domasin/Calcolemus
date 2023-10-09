module FolAutomReas.Tests.Partition

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Partition
open FolAutomReas.Lib.Fpf

let (Partition f as ptn) = 
    unequal
    |> equate (2,1) 
    |> equate (3,1)
    |> equate (4,1) 
    |> equate (6,5) 
    |> equate (7,5) 

[<Fact>]
let ``equated should return the domain of the partition.``() = 
    equated ptn 
    |> should equal [1; 2; 3; 4; 5; 6; 7]

[<Fact>]
let ``terminus should return the canonical element and the size of the equivalence class of element if found.``() = 
    terminus ptn 3
    |> should equal (1, 4)

[<Fact>]
let ``terminus should fail with 'apply' if element is not found.``() = 
    (fun () -> 
        terminus ptn 8
        |> ignore
    )
    |> should (throwWithMessage "apply") typeof<System.Exception>

[<Fact>]
let ``tryterminus should return the canonical element and the size of the equivalence class of element if found.``() = 
    tryterminus ptn 3
    |> should equal (1, 4)

[<Fact>]
let ``tryterminus should return the input element itself with size one if element is not found.``() = 
    tryterminus ptn 8
    |> should equal (8, 1)

[<Fact>]
let ``canonize should return the canonical representative of the equivalence class of element if found.``() = 
    canonize ptn 3
    |> should equal 1

[<Fact>]
let ``canonize should return the input element itself if element is not found.``() = 
    canonize ptn 8
    |> should equal 8

[<Fact>]
let ``equivalent should return true if elements belong to the same equivalent class in the partition.``() = 
    equivalent ptn 3 2
    |> should equal true

[<Fact>]
let ``equivalent should return false if elements don't belong to the same equivalent class in the partition.``() = 
    equivalent ptn 6 1
    |> should equal false

[<Fact>]
let ``equate should return a new partition with the updated equivalence classes.``() = 
    unequal
    |> equate (2,1)
    |> fun (Partition f) -> graph f
    |> should equal [(1, Terminal (1, 2)); (2, Nonterminal 1)]
