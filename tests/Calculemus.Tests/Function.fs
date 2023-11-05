// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Functions

open Xunit
open FsUnit.Xunit

open Calculemus.Lib.Function

[<Fact>]
let ``non p x should return true if x doesn't satisfy the predicate.``() = 
    4
    |> non ((=) 2)
    |> should equal true

[<Fact>]
let ``non p x should return false if x satisfies the predicate.``() = 
    2
    |> non ((=) 2)
    |> should equal false

[<Fact>]
let ``check p x should return x if it satisfies p.``() = 
    2 
    |> check ((=) 2)
    |> should equal 2

[<Fact>]
let ``check p x should fail if x doesn't satisfy p.``() = 
    (fun () -> 
        4
        |> check ((=) 2)
        |> ignore
    ) 
    |> should (throwWithMessage "check") typeof<System.Exception>

[<Fact>]
let ``funpow iterates a function a fixed number of times.``() = 
    2.
    |> funpow 2 (fun x -> x ** 2.)
    |> should equal 16.

[<Fact>]
let ``funpow n f x should return x unchanged if n = 0.``() = 
    2.
    |> funpow 0 (fun x -> x ** 2.)
    |> should equal 2.

[<Fact>]
let ``funpow n f x should return x unchanged if n < 0.``() = 
    2.
    |> funpow -1 (fun x -> x ** 2.)
    |> should equal 2.

[<Fact>]
let ``funpow n f x should fail if some applications of f to x fail.``() = 
    (fun () -> 
        2
        |> funpow 2 ((/) 0)
        |> ignore
    ) 
    |> should (throwWithMessage "Attempted to divide by zero.") typeof<System.DivideByZeroException>

[<Fact>]
let ``can f x evaluates to true if the application of f to x succeeds.``() = 
    can List.head [1;2]
    |> should equal true

[<Fact>]
let ``can f x evaluates to false if the application of f to x fails.``() = 
    can List.head []
    |> should equal false

[<Fact>]
let ``repeat f x evaluates successively applies f over and over until it fails``() = 
    repeat (List.removeAt 0) [1;2;3;4;5]
    |> should equal ([]:int list)