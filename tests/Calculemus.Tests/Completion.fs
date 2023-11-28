// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Completion

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Equal
open Order
open Completion

[<Fact>]
let ``renamepair should return the pair of the given formulas with the variables renamed by schematic variables xn.``() = 
    (!!"i(x) * y = z", !!"z * t = y")
    |> renamepair
    |> fun (x, y) -> (sprint_fol_formula x, sprint_fol_formula y)
    |> shouldEqual ("`i(x0) * x1 = x2`", "`x5 * x3 = x4`")

[<Fact>]
let ``overlaps should return all possible overlaps of an equation with a term.``() = 
    let eq = !!"f(f(x)) = g(x)" 

    overlaps 
        (!!!"f(f(x))",!!!"g(x)") !!!"f(f(y))" 
        (fun i t -> subst i (mk_eq t !!!"g(y)"))
    |> List.map sprint_fol_formula
    |> shouldEqual 
        ["`f(g(x)) = g(f(x))`"; "`g(y) = g(y)`"]

[<Fact>]
let ``critical_pairs should return all critical pairs between two equations.``() = 
    let eq = !!"f(f(x)) = g(x)" 

    critical_pairs eq eq
    |> List.map sprint_fol_formula
    |> shouldEqual 
        ["`f(g(x0)) = g(f(x0))`"; "`g(x1) = g(x1)`"]

[<Fact>]
let ``normalize_and_orient should return the pair of LHS and RHS of the normalized and oriented equation w.r.t. the input set fo equations and the given ordering.``() = 
    let eqs = !!>[
            "1 * x = x"; 
            "i(x) * x = 1"; 
            "(x * y) * z = x * y * z"
    ]

    let ord = lpo_ge (weight ["1"; "*"; "i"])

    !!"i(y) * i(x) = i(x * (1 * y))"
    |> normalize_and_orient ord eqs 
    |> fun (x, y) -> (sprint_term x, sprint_term y)
    |> shouldEqual 
        ("``i(x * y)``", "``i(y) * i(x)``")