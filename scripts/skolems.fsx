#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Fol
open Skolems

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

skolems !!>["exists x. P(f(g(x),z))"; "forall x. exists y. P(x,y)"] []
