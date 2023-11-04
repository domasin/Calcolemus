#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus
open Lib.Fpf
open Formulas
open Fol
open Clause
open Resolution

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

presolve_clauses 
   !!>["P(x)";"Q(x)";"P(0)";]
   !!>["~P(f(y))";"~P(z)";"~Q(z)"]

presolve_clauses 
   !!>["P(x)";"Q(x)";"P(0)";"~A"]
   !!>["~P(f(y))";"~P(z)";"~Q(z)"]

// inserted since neither tautological nor subsumed
!!>>[["P(x)"];["Q(y)"]]
|> incorporate [!!"R(0)"] [!!"R(f(z))"]
|> List.map (List.map sprint_fol_formula)
// [[`P(x)`]; [`Q(y)`]; [`R(f(z))`]] 

// not inserted since subsumed by gcl
!!>>[["P(x)"];["Q(y)"]]
|> incorporate [!!"R(w)"] [!!"R(f(z))"]
|> List.map (List.map sprint_fol_formula)
// [[`P(x)`]; [`Q(y)`]] 

// not inserted since subsumed by another clause in the list
!!>>[["P(x)"];["Q(y)"]]
|> incorporate [!!"R(0)"] [!!"P(f(z))"]
// [[`Q(z)`]; [`P(y)`]] 

// not inserted since tautological
!!>>[["P(x)"];["Q(y)"]]
|> incorporate [!!"R(0)"] !!>["R(f(z))";"~R(f(z))"]
// [[`P(x)`]; [`Q(y)`]]

!!>>[["Q(0)";"P(f(y))"];["P(x)";"~P(x)"]]
|> replace !!>["P(x)"]

subsumes_clause !!>["P(x)"] !!>["Q(0)";"P(f(y))"]

subsumes_clause !!>["Q(0)";"P(f(y))"] !!>["P(x)"]

(!!"P(x)",!!"P(f(y))")
|> match_literals undefined
|> graph
// [("x", ``f(y)``)]

(!!"P(f(y))",!!"P(x)")
|> match_literals undefined
// System.Exception: term_match

(!!"P(x) /\ Q(x)",!!"P(f(y)) /\ Q(f(y))")
|> match_literals undefined
|> graph
// System.Exception: match_literals

[!!!"x",!!!"f(y)"]
|> term_match undefined
|> graph

[!!!"f(y)",!!!"x"]
|> term_match undefined

mgu !!>["P(x)";"P(f(y))"] undefined
|> graph
// [("x", ``f(y)``)]

mgu !!>["false";"false"] undefined
|> graph
// []

mgu !!>["P(x)";"P(f(x))"] undefined
|> graph
// System.Exception: cyclic

mgu !!>["P(0)";"P(f(y))"] undefined
|> graph
// System.Exception: impossible unification

mgu !!>["P(x) /\ Q(x)";"P(f(y)) /\ Q(f(y))"] undefined
|> graph
// System.Exception: Can't unify literals

unifiable !!"P(x) /\ Q(x)" !!"P(f(y)) /\ Q(f(y))"

unifiable !!"P(x)" !!"P(f(x))"

rename "old_" !!>["P(x)";"Q(y)"]

resolvents 
    !!>["P(x)";"~R(x,y)";"Q(x)";"P(0)"]
    !!>["~P(f(y))";"T(x,y,z)";"~P(z)"]
    !!"P(x)"
    []
|> List.map (List.map sprint_fol_formula)

// [[`Q(f(y))`; `T(f(y),y,z)`; `~R(f(y),y)`]]

resolve_clauses 
    !!>["P(x)";"Q(x)";"P(0)"]
    !!>["~P(f(y))";"~P(z)";"~Q(z)"]
|> List.map (List.map sprint_fol_formula)

basic_resloop ([],!!>>[["P(x)"];["~P(x)"]])

basic_resloop ([],!!>>[["P(x)"]])

!!"P(x) /\ ~P(x)"
|> pure_resolution_wsubs

!!"P(x)"
|> pure_resolution_wsubs

!!"""P(x) \/ ~P(x)"""
|> pure_resolution_wsubs

!! @"P(x)"
|> basic_resolution

!! @"P(x)"
|> pure_basic_resolution

!!"P(x) /\ ~P(x)"
|> basic_resolution

!! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
|> basic_resolution