#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Fol
open FolAutomReas.Lib
open FolAutomReas.Unif

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

istriv undefined "x" (Var "x")
// val it: bool = true

istriv undefined "x" (Var "y")
// val it: bool = false

istriv (("y" |-> (Var "x"))undefined) "x" (Fn("f",[Var "y"]))
// System.Exception: cyclic

unify undefined [Var "x", Fn("0",[])]
// x |-> 0 (success with no previous assignment)

// unify success
unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("0",[])]
// x |-> y; y |-> 0 (success with augmented assignment)

unify undefined [Var "y", Fn("f",[Var "y"])]
// System.Exception: cyclic (failure with direct cycle)

unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("f",[Var "y"])]
// System.Exception: cyclic: x |-> y; y |-> f(y); failure 
// (failure with derived cycle)

unify undefined [Fn ("0",[]), Fn("1",[])]
// System.Exception: impossible unification

solve (("x" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
// x |-> 0

solve (("y" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
// input unchanged: x |-> y; y |-> 0

// // StackOverFlow crash caused by cyclic mappings
// solve (("y" |-> Fn("f",[Var "y"]))undefined)

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

fullunify [Var "x", Fn("0",[])]
// x |-> 0

fullunify [Fn ("f",[Var "x"; Fn("g",[Var "y"])]), Fn ("f",[Var "y"; Var "x"])]
// System.Exception: cyclic

fullunify [Fn ("0",[]), Fn("1",[])]
// System.Exception: impossible unification

unify_and_apply [Var "x", Fn("0",[])]
// [(Fn ("0", []), Fn ("0", []))]

unify_and_apply [Var "x", Var "y"; Var "x", Fn("0",[])]
// [(Fn ("0", []), Fn ("0", [])); (Fn ("0", []), Fn ("0", []))]

// handbook example 1
unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"]