module Calcolemus.Tests.Fol

open Xunit
open FsUnit.Xunit

open Calcolemus.Fol
open Calcolemus.Formulas
open Calcolemus.Lib.Fpf

[<Fact>]
let ``onformula applies a function on all top terms of fol formula.``() = 
    !! "P(x,f(z)) ==> Q(x)"
    |> onformula (function 
        | Var x -> Var (x + "_1") 
        | tm -> tm
    )
    |> sprint_fol_formula
    |> should equal "`P(x_1,f(z)) ==> Q(x_1)`"

[<Fact>]
let ``is_const_name returns if string is a constant name.``() = 
    Assert.Equal(true, is_const_name "nil")
    Assert.Equal(true, is_const_name "123")
    Assert.Equal(false, is_const_name "x")

[<Fact>]
let ``parset parses a string to a term.``() = 
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
let ``!!! parses a string to a term.``() = 
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
let ``parse parses a string to a fol formula.``() = 
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
let ``!! parses a string to a fol formula.``() = 
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

[<Fact>]
let ``sprint_term should return the concrete syntax representation of a term.``() = 
    (Fn("sqrt",[Fn("-",[Fn("1",[]);
                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
                                            Fn("2",[])])])])]))
    |> sprint_term
    |> should equal "``sqrt(1 - power(cos(x + y,2)))``"

[<Fact>]
let ``sprint_fol_formula should return the concrete syntax representation of a fol formula.``() = 
    Forall
      ("x",
       Forall
         ("y",
          Exists
            ("z",
             And
               (Atom (R ("<", [Var "x"; Var "z"])),
                Atom (R ("<", [Var "y"; Var "z"]))))))
    |> sprint_fol_formula
    |> should equal "`forall x y. exists z. x < z /\ y < z`"

[<Fact>]
let ``termval should return the element of the domain that corresponds to the term value in the given interpretation and valuation.``() = 
    Assert.Equal(false, !!! "0" |> termval bool_interp undefined)
    Assert.Equal(0, !!! "0" |> termval (mod_interp 3) undefined)

[<Fact>]
let ``holds should return the truth-value of the formula in the given interpretation and valuation.``() = 

    let fm = !! @"forall x. (x = 0) \/ (x = 1)"

    Assert.Equal(true, holds bool_interp undefined fm)
    Assert.Equal(true, holds (mod_interp 2) undefined fm)
    Assert.Equal(false, holds (mod_interp 3) undefined fm)