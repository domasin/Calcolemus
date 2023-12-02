// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Eqelim

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Fol
open Clause
open Eqelim

[<Fact>]
let ``modify_S should return the list of all the clauses that arise from all the possible combinations of forward and backward positive equations in the original clause.``() = 
    !!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
    |> modify_S
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`s1 = t1`"; "`s2 = t2`"; "`s3 = t3`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s1 = t1`"; "`s2 = t2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s1 = t1`"; "`s3 = t3`"; "`t2 = s2`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s1 = t1`"; "`t2 = s2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s2 = t2`"; "`s3 = t3`"; "`t1 = s1`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s2 = t2`"; "`t1 = s1`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`s3 = t3`"; "`t1 = s1`"; "`t2 = s2`"; "`~s1 = t1`"; "`~s2 = t2`"];
         ["`t1 = s1`"; "`t2 = s2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"]]

[<Fact>]
let ``modify_T should return a new clause with all the negative equation untouched and each positive equation s = t replaced by ~t = w1 \/ s = w1 creating fresh new variables wi for each of them.``() = 
    !!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
    |> modify_T
    |> List.map sprint_fol_formula
    |> shouldEqual 
        [
            "`~t1 = w''`"; "`s1 = w''`"; 
            "`~t2 = w'`"; "`s2 = w'`"; 
            "`~t3 = w`"; "`s3 = w`"; 
            "`~s1 = t1`"; "`~s2 = t2`"
        ]

[<Fact>]
let ``is_nonvar should return true if the input is a non-variable term.``() = 
    !!!"f(x)"
    |> is_nonvar
    |> shouldEqual true

[<Fact>]
let ``is_nonvar should return false if the input is a variable.``() = 
    !!!"x"
    |> is_nonvar
    |> shouldEqual false

[<Fact>]
let ``find_nestnonvar should return first non-variable subterm if it exists.``() = 
    !!!"f(0,1)"
    |> find_nestnonvar
    |> sprint_term
    |> shouldEqual "``0``"

[<Fact>]
let ``find_nestnonvar should fail with 'findnvsubt' if the input is just a variable.``() = 
    (fun () -> 
        !!!"x"
        |> find_nestnonvar
        |> ignore
    )
    |> should (throwWithMessage "findnvsubt") typeof<System.Exception>

[<Fact>]
let ``find_nestnonvar should fail with KeyNotFoundException if the input is not just a variable but there isn't a non-variable subterm.``() = 
    (fun () -> 
        !!!"f(x,y)"
        |> find_nestnonvar
        |> ignore
    )
    |> should (throwWithMessage "An index satisfying the predicate was not found in the collection.") typeof<System.Collections.Generic.KeyNotFoundException>

[<Fact>]
let ``find_nvsubterm should return first (not necessarily nested) non-variable subterm if it exists and the input literal is not an equation.``() = 
    !!"R(0,1)"
    |> find_nvsubterm
    |> sprint_term
    |> shouldEqual "``0``"

[<Fact>]
let ``find_nvsubterm should return first nested non-variable subterm if it exists and the formula is an equation.``() = 
    !!"~x = f(0)"
    |> find_nvsubterm
    |> sprint_term
    |> shouldEqual "``0``"

[<Fact>]
let ``find_nvsubterm should fail with System.ArgumentException if the input is not a literal.``() = 
    (fun () -> 
        !!"~x = f(0) /\ P(x)"
        |> find_nvsubterm
        |> ignore
    )
    |> should (throwWithMessage "find_nvsubterm: not a literal (Parameter 'fm')") typeof<System.ArgumentException>

[<Fact>]
let ``find_nvsubterm should fail with `tryfind' here isn't any non-variable subterm in the formula.``() = 
    (fun () -> 
        !!"~x = f(y)"
        |> find_nvsubterm
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>

[<Fact>]
let ``replacet should return the input term with subterms replaced based on the given substitution function.``() = 
    !!!"f(0,1)"
    |> replacet ((!!!"0" |-> !!!"1")undefined)
    |> sprint_term
    |> shouldEqual "``f(1,1)``"

[<Fact>]
let ``replace should return the input formula with terms replaced based on the given substitution function.``() = 
    !!"f(0,1) = 0"
    |> replace ((!!!"0" |-> !!!"1")undefined)
    |> sprint_fol_formula
    |> shouldEqual "`f(1,1) = 1`"

[<Fact>]
let ``emodify should return the list of clauses that represents the e-modification of the input clause.``() = 
    !!>["(x * y) * z = x * (y * z)"]
    |> emodify ["x";"y";"z"]
    |> List.map sprint_fol_formula
    |> shouldEqual ["`~y * z = w'`"; "`~x * y = w`"; "`w * z = x * w'`"]

[<Fact>]
let ``modify_E should return the list of clauses that represents the e-modification of the input clause.``() = 
    !!>["(x * y) * z = x * (y * z)"]
    |> modify_E
    |> List.map sprint_fol_formula
    |> shouldEqual ["`~y * z = w'`"; "`~x * y = w`"; "`w * z = x * w'`"]

[<Fact>]
let ``brand should return the input clauses E- S- and T-modified plus the reflexive clause x = x.``() = 
    !!>>[["x = f(0)"]]
    |> brand
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [
            ["`x = x`"]; 
            ["`~f(w) = w'`"; "`x = w'`"; "`~0 = w`"];
            ["`~x = w'`"; "`f(w) = w'`"; "`~0 = w`"]
        ]

[<Fact>]
let ``bpuremeson should return the number of depth limit reached trying refuting the formula if it is unsatisfiable after applying brands transformations and a refutation could be found.``() = 
    !!"~ x = x"
    |> bpuremeson
    |> shouldEqual 1