// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Completion

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Set
open Lib.List
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

[<Fact>]
let ``complete should return the completed set of equations that defines a confluent term rewriting system, if the procedure has success.``() = 
    let eqs = !!>[
            "1 * x = x"; 
            "i(x) * x = 1"; 
            "(x * y) * z = x * y * z"
    ]

    let ord = lpo_ge (weight ["1"; "*"; "i"])

    (eqs,[],unions(allpairs critical_pairs eqs eqs))
    |> complete ord
    |> List.map sprint_fol_formula
    |> shouldEqual
        ["`i(x4 * x5) = i(x5) * i(x4)`"; "`x1 * i(x5 * x1) = i(x5)`";
   "`i(x4) * x1 * i(x3 * x1) = i(x4) * i(x3)`";
   "`x1 * i(i(x4) * i(x3) * x1) = x3 * x4`";
   "`i(x3 * x5) * x0 = i(x5) * i(x3) * x0`";
   "`i(x4 * x5 * x6 * x3) * x0 = i(x3) * i(x4 * x5 * x6) * x0`";
   "`i(x0 * i(x1)) = x1 * i(x0)`"; "`i(i(x2 * x1) * x2) = x1`";
   "`i(i(x4) * x2) * x0 = i(x2) * x4 * x0`"; "`x1 * i(x2 * x1) * x2 = 1`";
   "`x1 * i(i(x4 * x5) * x1) * x3 = x4 * x5 * x3`";
   "`i(x3 * i(x1 * x2)) = x1 * x2 * i(x3)`";
   "`i(i(x3 * i(x1 * x2)) * i(x5 * x6)) * x1 * x2 * x0 = x5 * x6 * x3 * x0`";
   "`x1 * x2 * i(x1 * x2) = 1`"; "`x2 * x3 * i(x2 * x3) * x1 = x1`";
   "`i(x3 * x4) * x3 * x1 = i(x4) * x1`";
   "`i(x1 * x3 * x4) * x1 * x3 * x4 * x0 = x0`";
   "`i(x1 * i(x3)) * x1 * x4 = x3 * x4`";
   "`i(i(x5 * x2) * x5) * x0 = x2 * x0`";
   "`i(x4 * i(x1 * x2)) * x4 * x0 = x1 * x2 * x0`"; "`i(i(x1)) = x1`";
   "`i(1) = 1`"; "`x0 * i(x0) = 1`"; "`x0 * i(x0) * x3 = x3`";
   "`i(x2 * x3) * x2 * x3 * x1 = x1`"; "`x1 * 1 = x1`"; "`i(1) * x1 = x1`";
   "`i(i(x0)) * x1 = x0 * x1`"; "`i(x1) * x1 * x2 = x2`"; "`1 * x = x`";
   "`i(x) * x = 1`"; "`(x * y) * z = x * y * z`"]

[<Fact>]
let ``interreduce should return the set of equations interreduced.``() = 
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
    |> shouldEqual 
        ["`i(x4 * x5) = i(x5) * i(x4)`"; "`i(i(x1)) = x1`"; "`i(1) = 1`";
   "`x0 * i(x0) = 1`"; "`x0 * i(x0) * x3 = x3`"; "`x1 * 1 = x1`";
   "`i(x1) * x1 * x2 = x2`"; "`1 * x = x`"; "`i(x) * x = 1`";
   "`(x * y) * z = x * y * z`"]

[<Fact>]
let ``complete_and_simplify should return the completed and interreduced set of equations, if the procedure has success.``() = 
    [!!"i(a) * (a * b) = b"]
    |> complete_and_simplify ["1"; "*"; "i"]
    |> List.map sprint_fol_formula
    |> shouldEqual 
        [
            "`x0 * i(x0) * x3 = x3`"; 
            "`i(i(x0)) * x1 = x0 * x1`"; 
            "`i(a) * a * b = b`"
        ]