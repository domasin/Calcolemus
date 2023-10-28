// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.String

open Xunit
open FsUnit.Xunit

open Calcolemus.Lib.String

[<Fact>]
let ``explode should explode into a list of single-character strings.``() = 
    explode "The Quick fox."
    |> should equal 
        ["T"; "h"; "e"; " "; "Q"; "u"; "i"; "c"; "k"; " "; "f"; "o"; "x"; "."]

[<Fact>]
let ``explode should return an empty list for an empty string.``() = 
    explode ""
    |> should equal ([]:string list)

[<Fact>]
let ``implode should concatenate input strings.``() = 
    implode ["e";"x";"a";"m";"p";"l";"e"]
    |> should equal "example"

[<Fact>]
let ``implode should accept multi-character component strings.``() = 
    implode ["ex";"a";"mpl";"";"e"]
    |> should equal "example"

[<Fact>]
let ``implode should return an empty string for an empty list.``() = 
    implode ["ex";"a";"mpl";"";"e"]
    |> should equal "example"