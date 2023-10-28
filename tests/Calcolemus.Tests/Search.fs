// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Search

open Xunit
open FsUnit.Xunit

open Calcolemus.Lib.Search
open Calcolemus.Lib.List

[<Fact>]
let ``tryfind should return first successful application.``() = 
    [1;2;3] 
    |> tryfind (fun n -> if n % 2 = 0 then string n else failwith "f")
    |> should equal "2"

[<Fact>]
let ``tryfind should fail with 'tryfind' if all applications fail.``() = 
    (fun () -> 
        [1;2;3] 
        |> tryfind (fun n -> if n > 3 then string n else failwith "f")
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>

[<Fact>]
let ``mapfilter should return the list of successful results.``() = 
    [1;2;3;4] 
    |> mapfilter (fun n -> if n % 2 = 0 then string n else failwith "f") 
    |> should equal ["2"; "4"]

[<Fact>]
let ``mapfilter should return the empty list if no application succeeds.``() = 
    [1;2;3] 
    |> mapfilter (fun n -> if n > 3 then string n else failwith "f")
    |> should equal ([]:string list)

[<Fact>]
let ``optimize should return the element in the list that maximizes the function if the ord is increasing.``() = 
    optimize (>) (( * ) -1) [-1;2;3]
    |> should equal -1

[<Fact>]
let ``optimize should return the element in the list that minimizes the function if the ord is decreasing.``() = 
    optimize (<) (( * ) -1) [-1;2;3]
    |> should equal 3

[<Fact>]
let ``maximize should return the element in the list that maximizes the function.``() = 
    maximize (( * ) -1) [-1;2;3] 
    |> should equal -1

[<Fact>]
let ``minimize should return the element in the list that minimizes the function``() = 
    minimize (( * ) -1) [-1;2;3] 
    |> should equal 3