module FolAutomReas.Tests.Fpf

open Xunit
open FsUnit.Xunit
open FolAutomReas.Lib

[<Fact>]
let ``is_undefined should return true on undefined function``() = 
    is_undefined undefined
    |> should equal true

[<Fact>]
let ``is_undefined should return false on undefined function``() = 
    is_undefined (("x" |-> 1)undefined)
    |> should equal false

[<Fact>]
let ``mapf should return a new fpf with values transformed according to the given normal f# function.``() = 
    ("x" |-> 1)undefined
    |> mapf (fun x -> x * 10) 
    |> should equal (("x" |-> 10)undefined)

[<Fact>]
let ``foldl should return sum of values given appropriate inputs.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined) 
    |> foldl (fun acc i j -> acc + j) 0
    |> should equal 3

[<Fact>]
let ``graph should return the graph of the input fpf.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined) 
    |> graph 
    |> should equal [("x", 1); ("y", 2)]

[<Fact>]
let ``dom should return the domain of the input fpf.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined) 
    |> dom 
    |> should equal ["x"; "y"]

[<Fact>]
let ``ran should return the range of the input fpf.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined) 
    |> ran 
    |> should equal [1; 2]

[<Fact>]
let ``apply should return fpf if applied to an fpf defined argument.``() = 
    apply (("y" |-> 2)(("x" |-> 1)undefined)) "y"
    |> should equal 2

[<Fact>]
let ``apply should fail fpf if applied to an fpf not defined argument.``() = 
    (fun () -> 
        apply (("y" |-> 2)(("x" |-> 1)undefined)) "z"
        |> ignore
    )
    |> should (throwWithMessage "apply") typeof<System.Exception>