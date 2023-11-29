#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Set
open Lib.List
open Fol
open Completion
open Equal
open Order
open Rewrite
open Meson

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let eq = !!"f(f(x)) = g(x)" 
critical_pairs eq eq

critical_pairs eq eq

overlaps 
    (!!!"f(f(x))",!!!"g(x)") !!!"f(f(y))" 
    (fun i t -> subst i (mk_eq t !!!"g(y)"))

(!!"i(x) * y = z", !!"z * t = y")
|> renamepair

overlaps 
    (!!!"·(·(x,y),z)",!!!"·(x,·(y,z))") 
    !!!"·(1,x)" 
    (fun i t -> subst i (mk_eq t !!!"x"))

overlaps
    (!!!"p(p(x,y),z)",!!!"p(x,p(y,z))") !!!"p(1,x)" 
    (fun i t -> subst i (mk_eq t !!!"x"))

critical_pairs !!"p(p(x,y),z) = p(x,p(y,z))" !!"p(1,x) = x"

overlaps (!!!"p(p(x0,x1),x2)",!!!"p(x0,p(x1,x2))") !!!"p(1,x3)" 
    (fun i t -> subst i (mk_eq t !!!"x3"))

overlaps (!!!"p(1,x3)",!!!"x3") !!!"p(p(x0,x1),x2)" (fun i t -> subst i (mk_eq t !!!"p(x0,p(x1,x2))"))

// 1·x = x, (x·y)·z 
// x·(y·z)

let eqs = !!>[
        "1 * x = x"; 
        "i(x) * x = 1"; 
        "(x * y) * z = x * y * z"
]

let ord = lpo_ge (weight ["1"; "*"; "i"])

!!"i(y) * i(x) = i(x * (1 * y))"
|> normalize_and_orient ord eqs 

let eqs' = 
    (eqs,[],unions(allpairs critical_pairs eqs eqs))
    |> complete ord

rewrite eqs' !!!"i(x * i(x)) * (i(i((y * z) * u) * y) * i(u))"

!!>[
    "i(x4 * x5) = i(x5) * i(x4)"; "x1 * i(x5 * x1) = i(x5)";
    "i(x4) * x1 * i(x3 * x1) = i(x4) * i(x3)";
    "x1 * i(i(x4) * i(x3) * x1) = x3 * x4";
    "i(x3 * x5) * x0 = i(x5) * i(x3) * x0";
    "i(x4 * x5 * x6 * x3) * x0 = i(x3) * i(x4 * x5 * x6) * x0";
    "i(x0 * i(x1)) = x1 * i(x0)"; "i(i(x2 * x1) * x2) = x1";
    "i(i(x4) * x2) * x0 = i(x2) * x4 * x0"; "x1 * i(x2 * x1) * x2 = 1";
    "x1 * i(i(x4 * x5) * x1) * x3 = x4 * x5 * x3";
    "i(x3 * i(x1 * x2)) = x1 * x2 * i(x3)";
    "i(i(x3 * i(x1 * x2)) * i(x5 * x6)) * x1 * x2 * x0 = x5 * x6 * x3 * x0";
    "x1 * x2 * i(x1 * x2) = 1"; "x2 * x3 * i(x2 * x3) * x1 = x1";
    "i(x3 * x4) * x3 * x1 = i(x4) * x1";
    "i(x1 * x3 * x4) * x1 * x3 * x4 * x0 = x0";
    "i(x1 * i(x3)) * x1 * x4 = x3 * x4";
    "i(i(x5 * x2) * x5) * x0 = x2 * x0";
    "i(x4 * i(x1 * x2)) * x4 * x0 = x1 * x2 * x0"; "i(i(x1)) = x1";
    "i(1) = 1"; "x0 * i(x0) = 1"; "x0 * i(x0) * x3 = x3";
    "i(x2 * x3) * x2 * x3 * x1 = x1"; "x1 * 1 = x1"; "i(1) * x1 = x1";
    "i(i(x0)) * x1 = x0 * x1"; "i(x1) * x1 * x2 = x2"; "1 * x = x";
    "i(x) * x = 1"; "(x * y) * z = x * y * z"
]
|> interreduce []
|> List.map sprint_fol_formula

[!!"i(a) * (a * b) = b"]
|> complete_and_simplify ["1"; "*"; "i"]

!! @"(forall x y z. x * y = x * z ==> y = z) <=>
    (forall x z. exists w. forall y. z = x * y ==> w = y)"
|> (meson << equalitize)
    
!! @"(forall x y z. x * y = x * z ==> y = z) <=>
     (forall x z. exists w. forall y. z = x * y ==> w = y)"
|> equalitize
|> meson

!! @"forall x z. exists w. forall y. z = x * y ==> w = y"
|> Skolem.askolemize