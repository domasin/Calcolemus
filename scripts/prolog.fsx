#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus
open Lib.Fpf
open Formulas
open Prop
open Fol
open Clause
open Skolem
open Prolog
open Calcolemus.Unif

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"]

simpleprolog lerules "S(S(0)) <= S(S(S(0)))"
|> graph

simpleprolog lerules "S(0) <= 0"
|> graph

simpleprolog lerules "S(S(x)) <= S(S(S(0)))"
|> solve
|> graph

prolog lerules "S(S(x)) <= S(S(S(0)))"

prolog lerules "S(0) <= 0"

"S(X) <= S(Y) :- X <= Y"
|> parserule

"S(X) >"
|> parserule

Pelletier.p32
|> hornprove
|> fun (inst,level) -> 
    graph inst, level

!!"P(x) /\ ~P(x)"
|> hornprove

!! @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) 
        ==> ~(~q \/ ~q)"
|> hornprove

Fn ("c_x",[])
Var ("c_x")

!!!"c_x()"

!!>["~P(x)";"Q(y)";"~T(x)"]
|> hornify

// System.Exception: non-Horn clause

renamerule 0 (!!>["P(x)";"Q(y)"],!!"P(f(x))")

!!"(0 <= x /\ (x <= y ==> S(x) <= S(y)))"
|> generalize
|> skolemize
|> simpcnf
|> List.map hornify

!!>["S(x) <= S(S(x))"] 
|> backchain 
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 undefined
|> graph

!!>["S(x) <= S(S(y))"] 
|> backchain 
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 0 0 undefined
|> graph

!!>[
    "S(S(0)) <= 0";
] 
|> backchain 
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 undefined
// |> solve
|> graph

!!>["0 <= 1";"0 <= 1"] 
|> backchain 
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 undefined
// |> solve
|> graph