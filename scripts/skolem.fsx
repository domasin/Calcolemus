#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus

open Fol
open Skolem
open Formulas

// fsi.AddPrinter sprint_fol_formula

!!"(forall x. P(x)) /\ (exists x. P(x))"
|> pullquants

let fm = !!"P(x) /\ exists y. Q(y)"
pullq (false, true) fm mk_exists mk_and "y" "y" !!"P(x)" !!"Q(y)"

!!"forall x. P(X) /\ forall y. Q(x,y)"
|> pullquants

!!"forall x. P(X) /\ forall y. Q(x,y)"
|> prenex

skolem !!"forall y. Q(f(y)) ==> exists x. P(g(x))" ["f";"g"]

skolem !!"exists x. P(f(x)) /\ exists y. Q(y)" []

skolem !!"forall x. exists y. P(x,y)" []      // evaluates to 
skolem !!"forall x. exists y. P(x,y)" ["f_y"] 

skolem2 (fun (p, q) -> And (p, q)) (!!"forall x. exists y. P(x,y)", !!"Q(x)") []

let p,q = !!"forall x. exists y. P(x,y)", !!"forall x. exists y. Q(x,y)"
skolem2 And (p,q) []