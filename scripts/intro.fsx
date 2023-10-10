#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Intro

Add(Const 0, Const 1) |> simplify1

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 

Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify