#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Fpf

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Fol
open FolAutomReas.Lib.Set
open FolAutomReas.Lib.List

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

let fm = !> @"(p /\ q) \/ (s /\ t)"
let atms = atoms fm

// graphs of all valuations satisfying fm
allsatvaluations (eval fm) (fun _ -> false) atms
|> List.map (fun v -> 
    atms
    |> List.map (fun a -> (a, v a))
)

!> @"p ==> q"
|> dnf_by_truth_tables 
// Evaluates to `~p /\ ~q \/ ~p /\ q \/ p /\ q`

// Note the symmetry between the conjunctions 
// and the true-rows of the truth table.
!> @"p ==> q"
|> print_truthtable

// p     q     |   formula
// ---------------------
// false false | true  
// false true  | true  
// true  false | false 
// true  true  | true  
// ---------------------

!> @"(p \/ q /\ r) /\ (~p \/ ~r)"
|> dnf_by_truth_tables

!> @"(p <=> q) <=> ~(r ==> s)"
|> dnf_by_truth_tables

!> @"p /\ (q \/ r)" |> distrib_naive

!> @"p ==> q" |> distrib_naive

!> @"(p ==> q) /\ q" 
|> rawdnf

!> @"p ==> q" 
|> rawdnf


!> @"p /\ (q \/ r) \/ ~s" 
|> rawdnf

!> @"p /\ (q \/ r) \/ s"
|> rawdnf

!> @"p /\ (q \/ r) \/ s"
|> dnf

!> @"p /\ (q \/ r) \/ s"
|> dnf_by_truth_tables

!> @"p ==> q"
|> dnf

!> @"p ==> q"
|> rawdnf

!> @"(p \/ q /\ r) /\ (~p \/ ~r)" 
|> dnf


!> @"p /\ ~ (q \/ r) \/ s"
|> rawdnf

!> @"a ==> b ==> c"
|> dnf_by_truth_tables

!> @"a ==> b ==> c"
|> dnf

!> @"p /\ (q \/ r)"
|> dnf_by_truth_tables

!> @"p /\ (q \/ r)"
|> dnf

let fm1 = 
    !> @"p /\ ~q /\ r \/ p /\ q /\ ~r \/ p /\ q /\ r"

let fm2 = 
    !> @"p /\ q \/ p /\ r"

tautology(mk_iff fm1 fm2)




!> @"p /\ (q \/ r)" |> distrib_naive
// `p /\ q \/ p /\ r`
distrib [["p"]] [["q"];["r"]]
// [["p"; "q"]; ["p"; "r"]]

!> @"(p \/ q) /\ r" |> distrib_naive
// `p /\ r \/ q /\ r`
distrib [["p"];["q"]] [["r"]]
// [["p"; "r"]; ["q"; "r"]]



[!>"p"; !>"q"; !>"x"; !>"~y"]
|> List.fold (fun acc c -> And (acc,c)) True


allpairs union [["p"]] [["q"];["r"]]

allpairs union [["p"]] [["q"]]


distrib [[1;2];[2]] [[3];[4]]

!> @"p /\ q \/ ~ p /\ r"
|> purednf

!> @"(p \/ q /\ r) /\ (~p \/ ~r)"
|> purednf

!> @"p ==> q"
|> nnf
|> purednf

trivial [!>"p";!>"~p"]

!> @"p ==> q"
|> simpdnf
