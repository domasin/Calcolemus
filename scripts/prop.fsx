#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Fpf

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


let file = System.IO.File.CreateText("out.txt")
fprint_truthtable file (!>"p ==> q")
file.Close()

print_truthtable !>"p /\ q ==> q /\ r"
sprint_truthtable !>"p ==> q"


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

// fsi.AddPrinter sprint_prop_formula

!> "p /\ q /\ p /\ q"
|> psubst (P"p" |=> !>"p /\ q")

!> @"p \/ ~p"
|> dual

!> "false /\ p"
|> psimplify1

!> "false /\ p"
|> psimplify