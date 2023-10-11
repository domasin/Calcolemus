#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Intro

Add(Const 0, Const 1) |> simplify1

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify

Add (Mul (Add (Mul (Const 0, Var "x"), Const 1), Const 3), Const 12)
|> simplify

parse_atom ["x"; "+"; "3"]
parse_atom ["x"; "+"]

parse_atom ["12"; "+"; "3"]

parse_atom []

parse_atom ["(";"12"; "+"; "3";")"]

parse_atom ["(";"12"; "+"; "3"]

parse_product ["x"; "+"; "3"]
parse_product ["x"; "*"; "3"]
parse_product ["(";"12"; "+"; "3";")"]

parse_product []

parse_expression ["x"; "+"; "3"]

parse_expression ["x"; "+";]
parse_expression ["(";"x"; "+"; "3"]

parse_expression ["(";"x"; "+";")"]

parse_expression []

parse_exp "x +"
parse_exp "(x + 3"

parse_exp "(x1 + x2 + x3) * (1 + 2 + 3 * x + y)"

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
|> string_of_exp_naive


Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
|> sprint_exp

"x + 1"
|> parse_exp
|> string_of_exp 2

"x + 1"
|> parse_exp
|> string_of_exp 3

"x * 1"
|> parse_exp
|> string_of_exp 5