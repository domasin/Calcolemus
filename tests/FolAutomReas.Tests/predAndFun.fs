module Tests.PredAndFun

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib

[<Fact>]
let ``non revereses a predicate``() = 
    4
    |> non (fun x -> x = 2)
    |> should equal true

[<Fact>]
let ``check p x fails if x doesn't satisfiy p``() = 
    (fun () -> check (fun x -> x = 2) 3 |> ignore) 
    |> should (throwWithMessage "check") typeof<System.Exception>

[<Fact>]
let ``check p x should return x if x satisfies p``() = 
    2 
    |> check (fun x -> x = 2)
    |> should equal 2

[<Fact>]
let ``funpow iterates a function a fixed number of times``() = 
    2.
    |> funpow 2 (fun x -> x ** 2.)
    |> should equal 16.

[<Fact>]
let ``can f x evaluates to true if the application of f to x succeeds``() = 
    can List.head [1;2]
    |> should equal true

[<Fact>]
let ``can f x evaluates to false if the application of f to x fails``() = 
    can List.head []
    |> should equal false

[<Fact>]
let ``repeat f x evaluates successively applies f over and over until it fails``() = 
    repeat (List.removeAt 0) [1;2;3;4;5]
    |> should equal ([]:int list)