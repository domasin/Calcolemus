#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Fol
open Unif

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

istriv undefined "x" (Var "x")
// val it: bool = true

istriv undefined "x" (Var "y")
// val it: bool = false

istriv (("y" |-> !!!"x")undefined) "x" !!!"f(y)"
// System.Exception: cyclic

unify undefined [!!!"x", !!!"0"]
|> graph
// x |-> 0 (success with no previous assignment)

// unify success
unify (("x" |-> !!!"y")undefined) [!!!"x", !!!"0"]
|> graph
// x |-> y; y |-> 0 (success with augmented assignment)

unify undefined [!!!"y", !!!"f(y)"]
// System.Exception: cyclic (failure with direct cycle)

unify (("x" |-> !!!"y")undefined) [!!!"x", !!!"f(y)"]
// System.Exception: cyclic: x |-> y; y |-> f(y); failure 
// (failure with derived cycle)

unify undefined [!!!"0", !!!"1"]
// System.Exception: impossible unification

solve (("x" |-> !!!"0")(("x" |-> !!!"y")undefined))
|> graph
// x |-> 0

solve (("y" |-> !!!"0")(("x" |-> !!!"1")undefined))
|> graph
// input unchanged: x |-> 1; y |-> 0

// // StackOverFlow crash caused by cyclic mappings
// solve (("y" |-> !!!"f(y)")undefined)

unify undefined [Var "x1", Var "x1";Var "x2", Var "x2"]
// Empty

unify undefined [
    Var "x1", tsubst (("x1" |-> Var "x1")undefined) (Var "x1");
    Var "x2", tsubst (("x2" |-> Var "x2")undefined) (Var "x2")
]
// Empty

unify undefined [
    Var "x1", tsubst (("x1" |-> Var "x1")undefined) (Var "x1");
    Var "x2", tsubst (("x1" |-> Var "x1")undefined) (Var "x2")
]
// Empty

fullunify [(!!!"x", !!!"0"); (!!!"x", !!!"y")]
|> graph
// x |-> 0

fullunify [!!!"f(x,g(y))", !!!"f(y,x)"]
// System.Exception: cyclic

fullunify [!!!"0",!!!"1"]
// System.Exception: impossible unification

unify_and_apply [(!!!"x", !!!"0"); (!!!"x", !!!"y")]
// [(Fn ("0", []), Fn ("0", []))]

// handbook example 1
unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"]

unify_and_apply [!!!"f(x,g(y))", !!!"f(y,x)"]