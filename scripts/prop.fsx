#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Fol

P "x" |> pname

Atom (P "x")

And (Atom (R ("Q",[Var "x"])), Atom (R ("S",[Var "y"])))
|> atoms
 
Or (Atom (R ("Q",[Var "x"])), Not (Atom (R ("Q",[Var "x"]))))
|> tautology

Or (Atom (R ("Q",[Var "x"])), (Atom (R ("Q",[Var "x"]))))
|> tautology

Or (Atom (R ("Q",[Var "x"])), (Atom (R ("Q",[Var "x"]))))
|> satisfiable

And (Atom (R ("Q",[Var "x"])), Not (Atom (R ("Q",[Var "x"]))))
|> unsatisfiable

Or (Atom (R ("Q",[Var "x"])), (Atom (R ("Q",[Var "x"]))))
|> unsatisfiable

// Or (Atom (R ("Q",[Var "x"])), (Atom (R ("Q",[Var "x"]))))
// |> print_truthtable