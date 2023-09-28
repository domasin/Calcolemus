module Tests.Unif

open Xunit
open FsUnit.Xunit

open FolAutomReas.Fol
open FolAutomReas.Unif
open FolAutomReas.Lib

[<Fact>]
let ``istriv should return true on trivial assignment.``() = 
    istriv undefined "x" (Var "x")
    |> should equal true

let ``istriv should return false on acyclic assignment.``() = 
    istriv undefined "x" (Var "y")
    |> should equal false

let ``istriv should fail on cyclic nontrivial assignment.``() = 
    (fun () -> 
        istriv (("y" |-> (Var "x"))undefined) "x" (Fn("f",[Var "y"]))) 
        |> ignore
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``unify should return a unification assignment on success.``() = 
    unify undefined [Var "x", Fn("0",[])]
    |> should equal (("x" |-> Fn("0",[]))undefined)

[<Fact>]
let ``unify should return an augmented unification assignment on success with a nonempty environment.``() = 
    unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("0",[])]
    |> should equal (("x" |-> (Var "y"))(("y" |-> Fn("0",[]))undefined))

[<Fact>]
let ``unify should fail with 'cyclic' with a direct cycle.``() = 
    (fun () -> 
        unify undefined [Var "y", Fn("f",[Var "y"])]
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``unify should fail with 'cyclic' with a derived cycle.``() = 
    (fun () -> 
        unify (("x" |-> (Var "y"))undefined) [Var "x", Fn("f",[Var "y"])]
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``unify should fail with 'impossible unification' if the unification is not possible.``() = 
    (fun () -> 
        unify undefined [Fn ("0",[]), Fn("1",[])]
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

let ``solve should return an MGU of the input env.``() = 
    solve (("x" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
    |> should equal (("x" |-> Fn("0",[]))undefined)

let ``solve should return the input unchanged it is already an MGU.``() = 
    let env = (("y" |-> Fn("0",[]))(("x" |-> Var "y")undefined))
    solve env
    |> should equal env

let ``fullunify should return an MGU for the input, if it is unifiable.``() = 
    fullunify [Var "x", Fn("0",[])]
    |> should equal (("x" |-> Fn("0",[]))undefined)

let ``fullunify should fail, with 'impossible unification', if the input is not unifiable.``() = 
    (fun () -> 
        fullunify [Fn ("0",[]), Fn("1",[])]
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>