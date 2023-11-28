#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Partition
open Fol
open Completion
open Equal
open Order

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

