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

[<Fact>]
let ``tryapplyd should return the value if the fpf is defined for the argument.``() = 
    tryapplyd (("y" |-> 2)(("x" |-> 1)undefined)) "x" 9
    |> should equal 1

[<Fact>]
let ``tryapplyd should return the default value if the fpf is not defined for the argument.``() = 
    tryapplyd (("y" |-> 2)(("x" |-> 1)undefined)) "z" 9
    |> should equal 9

[<Fact>]
let ``tryapplyl should return the value if the fpf is defined for the argument.``() = 
    tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "x"
    |> should equal [1;2;3]

[<Fact>]
let ``tryapplyl should return [] if the fpf is not defined for the argument.``() = 
    tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "z"
    |> should equal ([]:int list)

[<Fact>]
let ``defined should return true if the fpf is defined for the argument.``() = 
    defined (("y" |-> 2)(("x" |-> 1)undefined)) "x"
    |> should equal true

[<Fact>]
let ``defined should return false if the fpf is not defined for the argument.``() = 
    defined (("y" |-> 2)(("x" |-> 1)undefined)) "z"
    |> should equal false

[<Fact>]
let ``undefine should remove the argument from the ones for which the fpf is defined.``() = 
    ("y" |-> 2)(("x" |-> 1)undefined)
    |> undefine "x"
    |> should equal (("y" |-> 2)undefined)

[<Fact>]
let ``undefine should return the input unchanged if the argument is not one for which the fpf is defined.``() = 
    let input = ("y" |-> 2)(("x" |-> 1)undefined)
    
    input
    |> undefine "z"
    |> should equal input

[<Fact>]
let ``|-> should return the fpf defined for the new argument.``() = 
    ("x" |-> 1)undefined
    |> graph
    |> should equal ["x",1]

[<Fact>]
let ``|=> should return the point function with the defined mapping.``() = 
    "x" |=> 1 |> graph
    |> should equal ["x",1]

[<Fact>]
let ``fpf should return the point function with the defined mapping.``() = 
    fpf [1;2;3] [1;4;9] |> graph
    |> should equal [(1, 1); (2, 4); (3, 9)]

[<Fact>]
let ``valmod should return the new value if applied to the updated argument.``() = 
    valmod 1 100 id 1
    |> should equal 100

[<Fact>]
let ``valmod should return the original value if applied to an argument other of the one updated.``() = 
    valmod 1 100 id 2
    |> should equal 2

[<Fact>]
let ``undef should with 'undefined function' if applied to any argument.``() = 
    (fun () -> 
        ((undef 1):int)
        |> ignore
    )
    |> should (throwWithMessage "undefined function") typeof<System.Exception>

[<Fact>]
let ``undef can be used as an undefined function updatable with 'valmod'.``() = 
    valmod 1 100 (undef) 1
    |> should equal 100