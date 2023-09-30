module FolAutomReas.Tests.Tableaux

open Xunit
open FsUnit.Xunit

open FolAutomReas.Tableaux
open FolAutomReas.Pelletier
open FolAutomReas.Lib
open FolAutomReas.Fol

[<Fact>]
let ``unify_literals should return an augmented unification assignment on a unifiable pair not already unified.``() = 
    unify_literals undefined (!!"P(x)",!!"P(f(y))")
    |> should equal (("x" |-> Fn("f",[Var "y"]))undefined)

[<Fact>]
let ``unify_literals should return the input mappings unchanged on a pair already unified.``() = 
    unify_literals (("x" |-> Var "z")undefined) (!!"P(x)",!!"P(x)")
    |> should equal (("x" |-> Var "z")undefined)

[<Fact>]
let ``unify_literals should handle also the pari False,False.``() = 
    unify_literals (("x" |-> Var "z")undefined) (!!"False",!!"False")
    |> should equal (("x" |-> Var "z")undefined)

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
let ``prawitz should succeed on p20 after trying 2 ground instance.``() = 
    prawitz p20
    |> should equal 2

[<Fact>]
let ``tab should succeed on p38 after trying 4 ground instance.``() = 
    tab p38
    |> should equal 4
