// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Lexer

open Xunit
open FsUnit.Xunit

open Calcolemus.Lib.Lexer
open Calcolemus.Lib.String

[<Fact>]
let ``matches should return a function that returns true if single character string matches the given pattern.``() = 
    matches "abc" "a"
    |> should equal true

[<Fact>]
let ``matches should return a function that returns false if single character string doesn't match the given pattern.``() = 
    matches "abc" "d"
    |> should equal false

[<Fact>]
let ``space should return true if single character string is considered a space.``() = 
    space " "
    |> should equal true

[<Fact>]
let ``space should return false if single character string is not considered a space.``() = 
    space "."
    |> should equal false

[<Fact>]
let ``punctuation should return true if single character string is considered a punctuation symbol.``() = 
    punctuation ","
    |> should equal true

[<Fact>]
let ``punctuation should return false if single character string is not considered a punctuation symbol.``() = 
    punctuation "."
    |> should equal false

[<Fact>]
let ``symbolic should return true if single character string is considered symbolic.``() = 
    symbolic "."
    |> should equal true

[<Fact>]
let ``symbolic should return false if single character string is not considered symbolic.``() = 
    symbolic "1"
    |> should equal false

[<Fact>]
let ``numeric should return true if single character string is considered numeric.``() = 
    numeric "1"
    |> should equal true

[<Fact>]
let ``numeric should return false if single character string is not considered numeric.``() = 
    numeric "z"
    |> should equal false

[<Fact>]
let ``number are alphanumeric.``() = 
    alphanumeric "1"
    |> should equal true

[<Fact>]
let ``letters are alphanumeric.``() = 
    alphanumeric "z"
    |> should equal true

[<Fact>]
let ``symbolic are not alphanumeric.``() = 
    alphanumeric "."
    |> should equal false

[<Fact>]
let ``lexwhile should locate the longest initial sequence satisfying prop.``() = 
    "((1 + 2) * x_1)"
    |> explode
    |> lexwhile punctuation 
    |> should equal ("((", ["1"; " "; "+"; " "; "2"; ")"; " "; "*"; " "; "x"; "_"; "1"; ")"])

[<Fact>]
let ``lex should return the input string list tokenized.``() = 
    "((11 + 2) * x_1)"
    |> explode
    |> lex
    |> should equal ["("; "("; "11"; "+"; "2"; ")"; "*"; "x_1"; ")"]