// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Intro

open Xunit
open FsUnit.Xunit

open Calcolemus.Intro

[<Fact>]
let ``simplify1 should return a simplified expression if simplifiable.``() = 
    Add(Const 0, Const 1) |> simplify1
    |> should equal (Const 1)

[<Fact>]
let ``simplify1 should return the input expression itself if rules are not applicable at the first level``() = 
    Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 
    |> should equal (Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)))

[<Fact>]
let ``simplify should apply the simplification rules at every level.``() = 
    Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify 
    |> should equal (Const 0)

[<Fact>]
let ``simplify-2.``() = 
    Add (Mul (Add (Mul (Const 0, Var "x"), Const 1), Const 3), Const 12) 
    |> simplify 
    |> should equal (Const 15)

[<Fact>]
let ``parse_atom should parse variables.``() = 
    parse_atom ["x"; "+"; "3"]
    |> should equal (Var "x", ["+"; "3"])

[<Fact>]
let ``parse_atom should parse constants.``() = 
    parse_atom ["12"; "+"; "3"]
    |> should equal (Const 12, ["+"; "3"])

[<Fact>]
let ``parse_atom should parse expressions enclosed in brackets.``() = 
    parse_atom ["(";"12"; "+"; "3";")"]
    |> should equal (Add (Const 12, Const 3), ([]:string list))

[<Fact>]
let ``parse_atom should fail if applied to an empty list.``() = 
    (fun () -> 
        parse_atom []
        |> ignore
    )
    |> should (throwWithMessage "Expected an expression at end of input") typeof<System.Exception>

[<Fact>]
let ``parse_atom should fail if applied to an expression with an opening but no closing bracket.``() = 
    (fun () -> 
        parse_atom ["(";"12"; "+"; "3"]
        |> ignore
    )
    |> should (throwWithMessage "Expected closing bracket") typeof<System.Exception>

[<Fact>]
let ``parse_product should parse atoms.``() = 
    parse_product ["x"; "+"; "3"]
    |> should equal (Var "x", ["+"; "3"])

[<Fact>]
let ``parse_product should parse products.``() = 
    parse_product ["x"; "*"; "3"]
    |> should equal (Mul (Var "x", Const 3), ([]:string list))

[<Fact>]
let ``parse_product should parse expressions enclosed in brackets.``() = 
    parse_product ["(";"12"; "+"; "3";")"]
    |> should equal (Add (Const 12, Const 3), ([]:string list))

[<Fact>]
let ``parse_product should fail if applied to an empty list.``() = 
    (fun () -> 
        parse_product []
        |> ignore
    )
    |> should (throwWithMessage "Expected an expression at end of input") typeof<System.Exception>

[<Fact>]
let ``parse_product should fail if applied to an expression with an opening but no closing bracket.``() = 
    (fun () -> 
        parse_product ["(";"12"; "+"; "3"]
        |> ignore
    )
    |> should (throwWithMessage "Expected closing bracket") typeof<System.Exception>

[<Fact>]
let ``parse_expression should parse additions.``() = 
    parse_expression ["x"; "+"; "3"]
    |> should equal (Add (Var "x", Const 3), ([]:string list))

[<Fact>]
let ``parse_expression should parse products.``() = 
    parse_expression ["x"; "*"; "3"]
    |> should equal (Mul (Var "x", Const 3), ([]:string list))

[<Fact>]
let ``parse_expression should parse atoms.``() = 
    parse_expression ["x"; ]
    |> should equal (Var "x", ([]:string list))

[<Fact>]
let ``parse_expression should fail on uncompleted expressions.``() = 
    (fun () -> 
        parse_expression ["x"; "+"]
        |> ignore
    )
    |> should (throwWithMessage "Expected an expression at end of input") typeof<System.Exception>

[<Fact>]
let ``parse_expression should fail if applied to an expression with an opening but no closing bracket.``() = 
    (fun () -> 
        parse_expression ["(";"12"; "+"; "3"]
        |> ignore
    )
    |> should (throwWithMessage "Expected closing bracket") typeof<System.Exception>

[<Fact>]
let ``parse_exp should parse a correct string to the corresponding expression.``() = 
    parse_exp "(x1 + x2 + x3) * (1 + 2 + 3 * x + y)"
    |> should equal 
        (
            Mul
                (Add (Var "x1", Add (Var "x2", Var "x3")),
                Add (Const 1, Add (Const 2, Add (Mul (Const 3, Var "x"), Var "y"))))
        )

[<Fact>]
let ``parse_exp should fail on uncompleted strings.``() = 
    (fun () -> 
        parse_exp "x +"
        |> ignore
    )
    |> should (throwWithMessage "Expected an expression at end of input") typeof<System.Exception>

[<Fact>]
let ``parse_exp should fail if applied to a string with an opening but no closing bracket.``() = 
    (fun () -> 
        parse_exp "(12 + 3"
        |> ignore
    )
    |> should (throwWithMessage "Expected closing bracket") typeof<System.Exception>

[<Fact>]
let ``string_of_exp_naive should return the concrete syntax string representation of the expression with possibly redundant brackets.``() = 
    Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
    |> string_of_exp_naive
    |> should equal "((0 + 1) * (0 + 0))"

[<Fact>]
let ``sprint_exp should return the concrete syntax string representation of the expression without redundant brackets.``() = 
    Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
    |> sprint_exp
    |> should equal "<<(0 + 1) * (0 + 0)>>"

[<Fact>]
let ``string_of_exp should not add brackets to an addition if the precedence level of the operator of which the addition is a sub-expression is less then 3.``() = 
    "x + 1"
    |> parse_exp
    |> string_of_exp 2
    |> should equal "x + 1"

[<Fact>]
let ``string_of_exp should add brackets to an addition if the precedence level of the operator of which the addition is a sub-expression is >= 3.``() = 
    "x + 1"
    |> parse_exp
    |> string_of_exp 3
    |> should equal "(x + 1)"