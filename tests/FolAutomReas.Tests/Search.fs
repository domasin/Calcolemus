module FolAutomReas.Tests.Search

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Search
open FolAutomReas.Lib.List

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
let ``maximize (( * ) -1) [-1;2;3] returns -1``() = 
    maximize (( * ) -1) [-1;2;3] 
    |> should equal -1

[<Fact>]
let ``minimize (( * ) -1) [-1;2;3] returns 3``() = 
    minimize (( * ) -1) [-1;2;3] 
    |> should equal 3