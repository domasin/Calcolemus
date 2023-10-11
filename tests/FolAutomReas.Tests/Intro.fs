module FolAutomReas.Tests.Intro

open Xunit
open FsUnit.Xunit

open FolAutomReas.Intro

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

//

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