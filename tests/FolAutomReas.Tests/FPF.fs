module FolAutomReas.Tests.FPF

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
let ``foldl should return sum of values given appropriate inputs.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined) 
    |> foldl (fun acc i j -> acc + j) 0
    |> should equal 3