// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Unif

open Xunit
open FsUnit.Xunit

open Calculemus.Lib.Fpf

open Calculemus.Fol
open Calculemus.Unif

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
        unify (("x" |-> (Var "y"))undefined) [Var "y", Fn("f",[Var "x"])]
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

let ``fullunify should fail, with 'cyclic', if the unification is cyclic.``() = 
    (fun () -> 
        fullunify 
            [Fn ("f",[Var "x"; Fn("g",[Var "y"])]), Fn ("f",[Var "y"; Var "x"])]
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

let ``fullunify should fail, with 'impossible unification', if the input is not unifiable.``() = 
    (fun () -> 
        fullunify [Fn ("0",[]), Fn("1",[])]
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

let ``unify_and_apply should return the unified input, if it is unifiable.``() = 
    unify_and_apply [Var "x", Fn("0",[])]
    |> should equal [(Fn ("0", []), Fn ("0", []))]

let ``unify_and_apply fail, with 'cyclic', if the unification is cyclic (handbook example 3).``() = 
    (fun () -> 
        unify_and_apply 
            [Fn ("f",[Var "x"; Fn("g",[Var "y"])]), Fn ("f",[Var "y"; Var "x"])]
        |> ignore
    )
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

let ``unify_and_apply should fail, with 'impossible unification', if the input is not unifiable.``() = 
    (fun () -> 
        unify_and_apply [Fn ("0",[]), Fn("1",[])]
        |> ignore
    )
    |> should (throwWithMessage "impossible unification") typeof<System.Exception>

let ``unify_and_apply should succeed on handbook example 1.``() = 
    unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"]
    |> should equal [(!!!"f(f(z),g(y))", !!!"f(f(z),g(y))")]

let ``unify_and_apply should succeed on handbook example 2.``() = 
    unify_and_apply [!!!"f(x,y)",!!!"f(y,x)"]
    |> should equal [(!!!"f(y,y)", !!!"f(y,y)")]

let ``unify_and_apply should succeed on handbook example 4.``() = 
    unify_and_apply [
        !!!"x_0",!!!"f(x_1,x_1)";
        !!!"x_1",!!!"f(x_2,x_2)";
        !!!"x_2",!!!"f(x_3,x_3)"
    ]
    |> should equal [
        (!!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))",
            !!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))");
        (!!!"f(f(x_3,x_3),f(x_3,x_3))", !!!"f(f(x_3,x_3),f(x_3,x_3))");
        (!!!"f(x_3,x_3)", !!!"f(x_3,x_3)")
    ]