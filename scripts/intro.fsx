#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Intro
open FolAutomReas.Lib.Lexer
open FolAutomReas.Lib.String

Add(Const 0, Const 1) |> simplify1

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify

Add (Mul (Add (Mul (Const 0, Var "x"), Const 1), Const 3), Const 12)
|> simplify

parse_atom ["x"; "+"; "3"]

parse_atom ["12"; "+"; "3"]

parse_atom []

parse_atom ["(";"12"; "+"; "3";")"]

parse_atom ["(";"12"; "+"; "3"]

parse_product ["x"; "+"; "3"]
parse_product ["x"; "*"; "3"]
parse_product ["(";"12"; "+"; "3";")"]

parse_product []