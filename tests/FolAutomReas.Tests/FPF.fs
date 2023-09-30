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