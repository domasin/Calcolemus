// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Resolution

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Lib.Fpf

open Fol
open Clause
open Resolution


[<Fact>]
let ``mgu should return an mgu for a set of literals if they are unifiable.``() = 
    mgu !!>["P(x)";"P(f(y))"] undefined
    |> graph
    |> shouldEqual [("x", !!!"f(y)")]

[<Fact>]
let ``mgu should return an empty instantiation if the list contains only false terms.``() = 
    mgu !!>["false";"false"] undefined
    |> graph
    |> shouldEqual []

[<Fact>]
let ``mgu should fail when there is a cycle.``() = 
    (fun () -> 
        mgu !!>["P(x)";"P(f(x))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``mgu should fail when the unification is impossible.``() = 
    (fun () -> 
        mgu !!>["P(0)";"P(f(y))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

[<Fact>]
let ``mgu should fail when the input is not a list of literals.``() = 
    (fun () -> 
        mgu !!>["P(x) /\ Q(x)";"P(f(y)) /\ Q(f(y))"] undefined
        |> ignore
    )
    |> should (throwWithMessage "Can't unify literals") typeof<System.Exception>

[<Fact>]
let ``unifiable should return true if the input are literals and unifiable.``() = 
    unifiable !!"P(x)" !!"P(f(y))"
    |> shouldEqual true

[<Fact>]
let ``unifiable should return false if the input are not literals or they are not unifiable.``() = 
    unifiable !!"P(x)" !!"P(f(x))"
    |> shouldEqual false

[<Fact>]
let ``rename should return the list of formulas with the free variables renamed with the given prefix.``() = 
    rename "old_" !!>["P(x)";"Q(y)"]
    |> List.map sprint_fol_formula
    |> shouldEqual ["`P(old_x)`"; "`Q(old_y)`"]

[<Fact>]
let ``resolvents should return the result of resolving the input clauses on the given literal.``() = 
    resolvents 
        !!>["P(x)";"~R(x,y)";"Q(x)";"P(0)"]
        !!>["~P(f(y))";"T(x,y,z)";"~P(z)"]
        !!"P(x)"
        []
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`P(0)`"; "`Q(z)`"; "`T(z,y,z)`"; "`~P(f(y))`"; "`~R(z,y)`"];
         ["`P(0)`"; "`Q(f(y))`"; "`T(f(y),y,z)`"; "`~P(z)`"; "`~R(f(y),y)`"];
         ["`P(0)`"; "`Q(f(y))`"; "`T(f(y),y,f(y))`"; "`~R(f(y),y)`"];
         ["`Q(0)`"; "`T(0,y,0)`"; "`~P(f(y))`"; "`~R(0,y)`"]]

[<Fact>]
let ``resolve_clauses should return all the resolvents of the input clauses.``() = 
    resolve_clauses 
        !!>["P(x)";"Q(x)";"P(0)"]
        !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`P(0)`"; "`Q(yz)`"; "`~P(f(yy))`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~P(yz)`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~Q(f(yy))`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`P(yz)`"; "`P(0)`"; "`~P(yz)`"; "`~P(f(yy))`"];
         ["`P(xx)`"; "`Q(xx)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"]]

[<Fact>]
let ``basic_resloop should return true if the input set of formulas is unsatisfiable and a proof is found.``() = 
    basic_resloop ([],!!>>[["P(x)"];["~P(x)"]])
    |> shouldEqual true

[<Fact>]
let ``basic_resloop should fail if it can't find a proof.``() = 
    (fun () -> 
        basic_resloop ([],!!>>[["P(x)"]])
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_basic_resolution should return true on an unsatisfiable formula and it can find a refutation.``() = 
    !!"P(x) /\ ~P(x)"
    |> pure_basic_resolution
    |> shouldEqual true

[<Fact>]
let ``pure_basic_resolution should fail on a satisfiable formula.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_basic_resolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_basic_resolution should fail on a valid formula.``() = 
    (fun () -> 
        !!"""P(x) \/ ~P(x)"""
        |> pure_basic_resolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``basic_resolution should prove davis putnam example.``() = 
    !! @"exists x. exists y. forall z.
        (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
        ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    |> basic_resolution
    |> shouldEqual [true]

[<Fact>]
let ``basic_resolution should fail on satisfiable but not valid formulas.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_basic_resolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``basic_resolution should fail on unsatisfiable formulas.``() = 
    (fun () -> 
        !!"P(x) /\ ~P(x)"
        |> basic_resolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``term_match should return the instantiations that makes the first terms to match the seconds.``() = 
    [!!!"x",!!!"f(y)"]
    |> term_match undefined
    |> graph
    |> shouldEqual [("x", !!!"f(y)")]

[<Fact>]
let ``term_match should fail if the first terms don't match the seconds.``() = 
    (fun () -> 
        [!!!"f(y)",!!!"x"]
        |> term_match undefined
        |> ignore
    )
    |> should (throwWithMessage "term_match") typeof<System.Exception>

[<Fact>]
let ``match_literals should return the instantiation that makes the first literal match the second, if there is.``() = 
    (!!"P(x)",!!"P(f(y))")
    |> match_literals undefined
    |> graph
    |> shouldEqual [("x", !!!"f(y)")]

[<Fact>]
let ``match_literals should fail with 'term_match' if the first literal doesn't match the second.``() = 
    (fun () -> 
        (!!"P(f(y))",!!"P(x)")
        |> match_literals undefined
        |> ignore
    )
    |> should (throwWithMessage "term_match") typeof<System.Exception>

[<Fact>]
let ``match_literals should fail with 'match_literals' if the input formulas are not literals.``() = 
    (fun () -> 
        (!!"P(x) /\ Q(x)",!!"P(f(y)) /\ Q(f(y))")
        |> match_literals undefined
        |> ignore
    )
    |> should (throwWithMessage "match_literals") typeof<System.Exception>

[<Fact>]
let ``subsumes_clause should return true, if the first clause subsumes the second.``() = 
    subsumes_clause !!>["P(x)"] !!>["Q(0)";"P(f(y))"]
    |> shouldEqual true

[<Fact>]
let ``subsumes_clause should return false, if the first clause doesn't subsume the second.``() = 
    subsumes_clause !!>["Q(0)";"P(f(y))"] !!>["P(x)"]
    |> shouldEqual false

[<Fact>]
let ``replace should return the input list with all subsumed clauses replaced by the given one.``() = 
    !!>>[["Q(0)";"P(f(y))"];["P(x)";"~P(x)"]]
    |> replace !!>["P(x)"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`P(x)`"]; ["`P(x)`"; "`~P(x)`"]]

[<Fact>]
let ``incorporate should return the input list incremented, if the new clause is neither tautological nor subsumed.``() = 
    !!>>[["P(x)"];["Q(y)"]]
    |> incorporate [!!"R(0)"] [!!"R(f(z))"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`P(x)`"]; ["`Q(y)`"]; ["`R(f(z))`"]]

[<Fact>]
let ``incorporate should return the input list unchanged, if the new clause is subsumed by the given clause.``() = 
    !!>>[["P(x)"];["Q(y)"]]
    |> incorporate [!!"R(w)"] [!!"R(f(z))"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`P(x)`"]; ["`Q(y)`"]]

[<Fact>]
let ``incorporate should return the input list unchanged, if the new clause is subsumed by another clause already in the list.``() = 
    !!>>[["P(x)"];["Q(y)"]]
    |> incorporate [!!"R(0)"] [!!"P(f(z))"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`P(x)`"]; ["`Q(y)`"]]

[<Fact>]
let ``incorporate should return the input list unchanged, if the new clause is tautological.``() = 
    !!>>[["P(x)"];["Q(y)"]]
    |> incorporate [!!"R(0)"] !!>["R(f(z))";"~R(f(z))"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual [["`P(x)`"]; ["`Q(y)`"]]

[<Fact>]
let ``resloop_wsubs should return true if the input set of formulas is unsatisfiable and a proof is found.``() = 
    resloop_wsubs ([],!!>>[["P(x)"];["~P(x)"]])
    |> shouldEqual true

[<Fact>]
let ``resloop_wsubs should fail if it can't find a proof.``() = 
    (fun () -> 
        resloop_wsubs ([],!!>>[["P(x)"]])
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_resolution_wsubs should return true on an unsatisfiable formula and it can find a refutation.``() = 
    !!"P(x) /\ ~P(x)"
    |> pure_resolution_wsubs
    |> shouldEqual true

[<Fact>]
let ``pure_resolution_wsubs should fail on a satisfiable formula.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_resolution_wsubs
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_resolution_wsubs should fail on a valid formula.``() = 
    (fun () -> 
        !!"""P(x) \/ ~P(x)"""
        |> pure_resolution_wsubs
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``resolution_wsubs should prove davis putnam example.``() = 
    !! @"exists x. exists y. forall z.
        (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
        ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    |> resolution_wsubs
    |> shouldEqual [true]

[<Fact>]
let ``resolution_wsubs should fail on satisfiable but not valid formulas.``() = 
    (fun () -> 
        !!"P(x)"
        |> resolution_wsubs
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``resolution_wsubs should fail on unsatisfiable formulas.``() = 
    (fun () -> 
        !!"P(x) /\ ~P(x)"
        |> resolution_wsubs
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``presolve_clauses should return all the resolvents of the input clauses, if at least one of them is completely positive.``() = 
    presolve_clauses 
        !!>["P(x)";"Q(x)";"P(0)"]
        !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual 
        [["`P(0)`"; "`Q(yz)`"; "`~P(f(yy))`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~P(yz)`"; "`~Q(yz)`"];
         ["`P(0)`"; "`Q(f(yy))`"; "`~Q(f(yy))`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`P(yz)`"; "`P(0)`"; "`~P(yz)`"; "`~P(f(yy))`"];
         ["`P(xx)`"; "`Q(xx)`"; "`~P(f(yy))`"; "`~Q(0)`"];
         ["`Q(0)`"; "`~P(f(yy))`"; "`~Q(0)`"]]

[<Fact>]
let ``presolve_clauses should return the empty list if none of the input clauses is completely positive.``() = 
    presolve_clauses 
        !!>["P(x)";"Q(x)";"P(0)";"~A"]
        !!>["~P(f(y))";"~P(z)";"~Q(z)"]
    |> List.map (List.map sprint_fol_formula)
    |> shouldEqual []

[<Fact>]
let ``presloop should return true if the input set of formulas is unsatisfiable and a proof is found.``() = 
    presloop ([],!!>>[["P(x)"];["~P(x)"]])
    |> shouldEqual true

[<Fact>]
let ``presloop should fail if it can't find a proof.``() = 
    (fun () -> 
        presloop ([],!!>>[["P(x)"]])
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_presolution should return true on an unsatisfiable formula and it can find a refutation.``() = 
    !!"P(x) /\ ~P(x)"
    |> pure_presolution
    |> shouldEqual true

[<Fact>]
let ``pure_presolution should fail on a satisfiable formula.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_presolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_presolution should fail on a valid formula.``() = 
    (fun () -> 
        !!"""P(x) \/ ~P(x)"""
        |> pure_presolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``presolution should prove Los example.``() = 
    !! @"(forall x y z. P(x,y) /\ P(y,z) ==> P(x,z)) /\
         (forall x y z. Q(x,y) /\ Q(y,z) ==> Q(x,z)) /\
         (forall x y. Q(x,y) ==> Q(y,x)) /\
         (forall x y. P(x,y) \/ Q(x,y))
         ==> (forall x y. P(x,y)) \/ (forall x y. Q(x,y))"
    |> presolution
    |> shouldEqual [true]

[<Fact>]
let ``presolution should fail on satisfiable but not valid formulas.``() = 
    (fun () -> 
        !!"P(x)"
        |> presolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``presolution should fail on unsatisfiable formulas.``() = 
    (fun () -> 
        !!"P(x) /\ ~P(x)"
        |> presolution
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_resolution_wsos should return true on an unsatisfiable formula and it can find a refutation.``() = 
    !!"P(x) /\ ~P(x)"
    |> pure_resolution_wsos
    |> shouldEqual true

[<Fact>]
let ``pure_resolution_wsos should fail on a satisfiable formula.``() = 
    (fun () -> 
        !!"P(x)"
        |> pure_resolution_wsos
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``pure_resolution_wsos should fail on a valid formula.``() = 
    (fun () -> 
        !!"""P(x) \/ ~P(x)"""
        |> pure_resolution_wsos
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``resolution_wsos should prove Los example.``() = 
    !! @"exists x. exists y. forall z.
        (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
        ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    |> resolution_wsos
    |> shouldEqual [true]

[<Fact>]
let ``resolution_wsos should fail on satisfiable but not valid formulas.``() = 
    (fun () -> 
        !!"P(x)"
        |> resolution_wsos
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>

[<Fact>]
let ``resolution_wsos should fail on unsatisfiable formulas.``() = 
    (fun () -> 
        !!"P(x) /\ ~P(x)"
        |> resolution_wsos
        |> ignore
    )
    |> should (throwWithMessage "No proof found") typeof<System.Exception>