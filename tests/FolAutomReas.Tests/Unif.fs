module Tests.Unif

open Xunit
open FsUnit.Xunit

open FolAutomReas.Fol
open FolAutomReas.Unif
open FolAutomReas.Lib

[<Fact>]
let ``unify undefined [!!!"f(x,y)",!!!"f(y,x)"] should return (("x" |-> Var "y")undefined).``() = 
    unify undefined [!!!"f(x,y)",!!!"f(y,x)"]
    |> should equal (("x" |-> Var "y")undefined)

[<Fact>]
let ``unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"] should return [(!!!"f(f(z),g(y))", !!!"f(f(z),g(y))")].``() = 
    unify_and_apply [!!!"f(x,g(y))",!!!"f(f(z),w)"]
    |> should equal [(!!!"f(f(z),g(y))", !!!"f(f(z),g(y))")]

[<Fact>]
let ``unify_and_apply [!!!"f(x,y)",!!!"f(y,x)"] should return [(!!!"f(y,y)", !!!"f(y,y)")].``() = 
    unify_and_apply [!!!"f(x,y)",!!!"f(y,x)"]
    |> should equal [(!!!"f(y,y)", !!!"f(y,y)")]

[<Fact>]
let ``unify_and_apply [!!!"f(x,g(y))",!!!"f(y,x)"] should return [(!!!"f(y,y)", !!!"f(y,y)")].``() = 
    (fun () -> unify_and_apply [!!!"f(x,g(y))",!!!"f(y,x)"] |> ignore) 
    |> should (throwWithMessage "cyclic") typeof<System.Exception>

[<Fact>]
let ``unify_and_apply [!!!"x_0",!!!"f(x_1,x_1)";!!!"x_1",!!!"f(x_2,x_2)";!!!"x_2",!!!"f(x_3,x_3)"] should return [(!!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))",!!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))");(!!!"f(f(x_3,x_3),f(x_3,x_3))", !!!"f(f(x_3,x_3),f(x_3,x_3))");(!!!"f(x_3,x_3)", !!!"f(x_3,x_3)")].``() = 
    unify_and_apply [!!!"x_0",!!!"f(x_1,x_1)";!!!"x_1",!!!"f(x_2,x_2)";!!!"x_2",!!!"f(x_3,x_3)"]
    |> should equal [(!!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))",!!!"f(f(f(x_3,x_3),f(x_3,x_3)),f(f(x_3,x_3),f(x_3,x_3)))");(!!!"f(f(x_3,x_3),f(x_3,x_3))", !!!"f(f(x_3,x_3),f(x_3,x_3))");(!!!"f(x_3,x_3)", !!!"f(x_3,x_3)")]