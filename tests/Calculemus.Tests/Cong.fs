// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Cong

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Lib.Partition

open Fol
open Cong

[<Fact>]
let ``subterms should return the list of subterms of the input term.``() = 
    subterms !!!"f(g(x),y)"
    |> List.map sprint_term
    |> shouldEqual ["``x``"; "``y``"; "``f(g(x),y)``"; "``g(x)``"]

[<Fact>]
let ``congruent should return true if all the immediate subterms are already equivalent.``() = 
    congruent 
        (equate (!!!"2",!!!"4")unequal) 
        (!!!"f(4)", !!!"f(2)")
    |> shouldEqual true

[<Fact>]
let ``emerge should return the extended congruence relation together with the updated predecessor function.``() = 
    (unequal,undefined)
    |> emerge (!!!"0",!!!"1") 
    |> fun (Partition f,pfn) -> graph f, graph pfn
    |> shouldEqual 
        ([(!!!"0", Nonterminal !!!"1"); (!!!"1", Terminal (!!!"1", 2))], [(!!!"1", [])])

[<Fact>]
let ``predecessors should return the input pfn updated with a mapping for each immediate subterms of the input term.``() = 
    predecessors !!!"f(0,g(1,0))" undefined
    |> graph
    |> shouldEqual 
        [(!!!"0", [!!!"f(0,g(1,0))"]); (!!!"g(1,0)", [!!!"f(0,g(1,0))"])]

[<Fact>]
let ``ccsatisfiable should return true if the input set of equations is satisfiable.``() = 
    !!>["m(0,1)=1";"~(m(0,1)=0)"]
    |> ccsatisfiable
    |> shouldEqual true

[<Fact>]
let ``ccsatisfiable should return false if the input set of equations is satisfiable.``() = 
    !!>["m(0,1)=1";"~(m(0,1)=1)"]
    |> ccsatisfiable
    |> shouldEqual false

[<Fact>]
let ``ccsatisfiable should fail with 'dest_eq: not an equation' if the input set of equations is not satisfiable.``() = 
    (fun () -> 
        !!>["P(0)"]
        |> ccsatisfiable
        |> ignore
    )
    |> should (throwWithMessage "dest_eq: not an equation") typeof<System.Exception>

[<Fact>]
let ``ccvalid should return true if the input equation is valid.``() = 
    !! @"f(f(f(f(f(c))))) = c /\ f(f(f(c))) = c
    ==> f(c) = c \/ f(g(c)) = g(f(c))"
    |> ccvalid
    |> shouldEqual true

[<Fact>]
let ``ccvalid should return false if the input equation is not valid.``() = 
    !! @"f(f(f(f(c)))) = c /\ f(f(c)) = c ==> f(c) = c"
    |> ccvalid
    |> shouldEqual false

[<Fact>]
let ``ccvalid should fail with 'dest_eq: not an equation' if the input is not an equation/inequation``() = 
    (fun () -> 
        !!"P(0)"
        |> ccvalid
        |> ignore
    )
    |> should (throwWithMessage "dest_eq: not an equation") typeof<System.Exception>