module FolAutomReas.Tests.Parser

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Lexer
open FolAutomReas.Lib.Parser

[<Fact>]
let ``make_parser should wrap lexer and parser together.``() = 
    let rec parseIntList i = 
        match parseInt i with
        | e1, "," :: i1 ->
            let e2, i2 = parseIntList i1
            e1@e2, i2
        | x -> x
    and parseInt i = 
        match i with
        | [] -> failwith "eof"
        | tok :: i1 -> [int tok], i1

    make_parser parseIntList "11,12"
    |> should equal [11;12]