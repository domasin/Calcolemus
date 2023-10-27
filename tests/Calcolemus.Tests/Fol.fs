module Calcolemus.Tests.Fol

open Xunit
open FsUnit.Xunit

open Calcolemus.Fol
open Calcolemus.Formulas

[<Fact>]
let ``onformula applies a function on all top terms of fol formula`.``() = 
    !! "P(x,f(z)) ==> Q(x)"
    |> onformula (function 
        | Var x -> Var (x + "_1") 
        | tm -> tm
    )
    |> sprint_fol_formula
    |> should equal "`P(x_1,f(z)) ==> Q(x_1)`"

[<Fact>]
let ``is_const_name returns if string is a constant name`.``() = 
    Assert.Equal(true, is_const_name "nil")
    Assert.Equal(true, is_const_name "123")
    Assert.Equal(false, is_const_name "x")

[<Fact>]
let ``parset parses a string to a term`.``() = 
    parset "sqrt(1 - power(cos(x + y,2)))"
    |> should equal 
        (Fn("sqrt",[Fn("-",[Fn("1",[]);
                        Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
                                             Fn("2",[])])])])]))

[<Fact>]
let ``parset should fail if the input is not a valid term.``() = 
    (fun () -> 
        parset "sqrt(1 - power(cos(x + y,2"
        |> ignore
    )
    |> should (throwWithMessage "Closing bracket expected") typeof<System.Exception>

[<Fact>]
let ``!!! parses a string to a term`.``() = 
    !!! "sqrt(1 - power(cos(x + y,2)))"
    |> should equal 
        (Fn("sqrt",[Fn("-",[Fn("1",[]);
                        Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
                                             Fn("2",[])])])])]))

[<Fact>]
let ``!!! should fail if the input is not a valid term.``() = 
    (fun () -> 
        !!! "sqrt(1 - power(cos(x + y,2"
        |> ignore
    )
    |> should (throwWithMessage "Closing bracket expected") typeof<System.Exception>

[<Fact>]
let ``parse parses a string to a fol formula`.``() = 
    parse "x + y < z"
    |> should equal 
        (Atom (R ("<", [Fn ("+", [Var "x"; Var "y"]); Var "z"])))

[<Fact>]
let ``parse should fail if the input is not a syntactically valid fol formula.``() = 
    (fun () -> 
        parse "x + y"
        |> ignore
    )
    |> should (throwWithMessage "Unparsed input: 2 tokens remaining in buffer.") typeof<System.Exception>

[<Fact>]
let ``!! parses a string to a fol formula`.``() = 
    !! "x + y < z"
    |> should equal 
        (Atom (R ("<", [Fn ("+", [Var "x"; Var "y"]); Var "z"])))

[<Fact>]
let ``!! should fail if the input is not a syntactically valid fol formula.``() = 
    (fun () -> 
        !! "x + y"
        |> ignore
    )
    |> should (throwWithMessage "Unparsed input: 2 tokens remaining in buffer.") typeof<System.Exception>