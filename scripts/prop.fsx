#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Fpf

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Fol

// fsi.AddPrinter sprint_prop_formula

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



!> "p /\ q /\ p /\ q"
|> psubst (P"p" |=> !>"p /\ q")

!> @"p \/ ~p"
|> dual

!> "false /\ p"
|> psimplify1

!> "false /\ p"
|> psimplify

!> "~ (p ==> false)"
|> nnf_naive

!> "~ (p ==> false)"
|> psimplify
|> nnf_naive

!> "~ (p ==> ~ false)"
|> nnf_naive

!> "~ (p ==> ~ false)"
|> nnf_naive

!> "~ ~ (p ==> ~ ~ false)"
|> nnf_naive

!> @"a /\ (b \/ ~c)"
|> nnf_naive

!> @"(a /\ b) \/ (a /\ ~c)"
|> nnf_naive

!> @"~(a \/ ~c)"
|> nnf_naive

!> @"a /\ (b \/ ~c)"
|> nnf

!> @"(a /\ b) \/ (a /\ ~c)"
|> nnf


!> "~ (p ==> q)"
|> nenf_naive

!> "~ (p <=> q)"
|> nenf_naive

!> "~ (p <=> q)"
|> nenf

!> "~ (false <=> p)"
|> nenf_naive

!> "~ (false <=> p)"
|> nenf

list_conj [!>"p";!>"q";!>"r"]
list_disj [!>"p";!>"q";!>"r"]

mk_lits [!>"p";!>"q"] 
    (function P"p" -> true | P"q" -> false | _ -> failwith "")

let fm = !> "p /\ q"
let atms = atoms fm
let satvals = allsatvaluations (eval fm) (fun _ -> false) atms

satvals[0] (P"p") // true
satvals[0] (P"q") // true
satvals[0] (P"a") // false

allsatvaluations (eval fm) (fun _ -> false) atms
|> List.map (mk_lits (List.map Atom atms))

allsatvaluations (eval (And (Atom 1, Atom 2))) (fun s -> false) [1; 2]

!> @"(p \/ q /\ r) /\ (~p \/ ~r)"
|> dnf_by_truth_tables