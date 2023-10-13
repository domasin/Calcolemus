#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Fol

P "x" |> pname

!> "p /\ q ==> q /\ r"
!> @"p \/ q ==> q /\ r"

eval (!>"p /\ q ==> q /\ r") 
    (function P"p" -> true | P"q" -> false | P"r" -> true | _ -> failwith "undefined")


!>"p /\ q ==> q /\ r" |> atoms

And (Atom 1, Atom 2) |> atoms

And (Atom (R ("Q",[Var "x"])), Atom (R ("S",[Var "y"])))
|> atoms


onallvaluations (eval True) (fun _ -> false) []

let sbfn (v:'a -> bool) = true

onallvaluations (function _ -> true) (fun _ -> false) []

onallvaluations (function _ -> false) (fun _ -> true) []


Atom (P "x")


 
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

