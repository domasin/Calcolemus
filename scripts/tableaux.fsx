#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Lib.Fpf

open Calcolemus.Fol
open Calcolemus.Tableaux

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

unify_literals undefined (!!"P(x)",!!"P(f(y))")
// x |-> f(y)

unify_literals (("x" |-> Var "z")undefined) (!!"P(y)",!!"P(y)")
// (("x" |-> Var "z")undefined)

unify_literals undefined (!!"False",!!"False")
// Empty

unify_literals undefined (!!"P(y)",!!"P(f(y))")
// System.Exception: cyclic

unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
// System.Exception: impossible unification

unify_literals undefined (!!"P(x) /\ P(x)",!!"P(f(y)) /\ P(f(y))")
// System.Exception: Can't unify literals

