#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus
open Lib.Fpf
open Formulas
open Fol
open Clause
open Resolution

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term


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
|> pure_basic_resolution

!!"P(x)"
|> pure_basic_resolution

!! @"P(x) \/ ~P(x)"
|> basic_resolution

!! @"P(x) /\ ~P(x)"
|> basic_resolution