// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Tableaux

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Fol
open Tableaux

[<Fact>]
let ``unify_literals should return an augmented unification assignment on a unifiable pair not already unified.``() = 
    unify_literals undefined (!!"P(x)",!!"P(f(y))")
    |> shouldEqual (("x" |-> Fn("f",[Var "y"]))undefined)

[<Fact>]
let ``unify_literals should return the input mappings unchanged on a pair already unified.``() = 
    unify_literals (("x" |-> Var "z")undefined) (!!"P(x)",!!"P(x)")
    |> shouldEqual (("x" |-> Var "z")undefined)

[<Fact>]
let ``unify_literals should handle also the pari False,False.``() = 
    unify_literals (("x" |-> Var "z")undefined) (!!"False",!!"False")
    |> shouldEqual (("x" |-> Var "z")undefined)

[<Fact>]
let ``unify_literals should fail when there is a cycle.``() = 
    (fun () -> 
        unify_literals undefined (!!"P(y)",!!"P(f(y))")
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``unify_literals should fail when the unification is not possible.``() = 
    (fun () -> 
        unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

[<Fact>]
let ``unify_literals should fail when the input are not litterals or the pair False,False.``() = 
    (fun () -> 
        unify_literals undefined (!!"P(x) /\ P(x)",!!"P(f(y)) /\ P(f(y))")
        |> ignore
    )
    |> should (throwWithMessage "Can't unify literals") typeof<System.Exception>

[<Fact>]
let ``unify_complements should return the instantiation that makes two literals become complementary, if such an instantiation exits.``() = 
    unify_complements undefined (!!"P(x)",!!"~P(f(y))")
    |> graph
    |> shouldEqual [("x", !!!"f(y)")]

[<Fact>]
let ``unify_complements should fail with cyclic if trying to unify complementary literals gives rise to a cycle.``() = 
    (fun () -> 
        unify_complements undefined (!!"P(y)",!!"~P(f(y))")
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``prawitz should succeed on p20 after trying 2 ground instance.``() = 
    prawitz Pelletier.p20
    |> shouldEqual 2

[<Fact>]
let ``tableau should return the successful instantiation together with the number of universal variables replaced, if the assumptions are unsatisfiable.``() = 
    tableau ([!!"P(x)"], [!!"~P(f(y))"], 0) id (undefined, 0)
    |> fun (inst,nrInst) -> inst |> graph,nrInst
    |> shouldEqual ([("x", !!!"f(y)")], 0)

[<Fact>]
let ``deepen should return the first successful call of the input function.``() = 
    deepen id 1
    |> shouldEqual 1

[<Fact>]
let ``tabrefute should return the number of universal variables replaced if the procedure succeeds.``() = 
    [!! "forall x y. P(x) /\ ~P(f(y))"; 
     !! "R(x,y) /\ ~R(x,y)"]
    |> tabrefute
    |> shouldEqual 2

[<Fact>]
let ``tab should succeed on p38 after trying 4 ground instance.``() = 
    tab Pelletier.p38
    |> shouldEqual 4

[<Fact>]
let ``splittab should succeed on p34.``() = 
    splittab Pelletier.p34
    |> shouldEqual [5; 4; 5; 3; 3; 3; 2; 4; 6; 2; 3; 3; 4; 3; 3; 3; 3; 2; 2; 3; 6; 3; 2; 4; 3; 3; 3; 3; 3; 4; 4; 4]
