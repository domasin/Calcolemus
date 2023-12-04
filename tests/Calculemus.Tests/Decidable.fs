// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Decidable

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Decidable

[<Fact>]
let ``aedecide should return true on a valid AE formula.``() = 
    !! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
    (forall u v w x y z.
    P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) <=> P(u,z,v)))
    ==> forall a b c. P(a,b,c) ==> P(b,a,c)"
    |> aedecide
    |> shouldEqual true

[<Fact>]
let ``aedecide should fail with 'Not decidable' if the input isn't an AE formula.``() = 
    (fun () -> 
        !! @"forall x. f(x) = 0"
        |> aedecide
        |> ignore
    )
    |> should (throwWithMessage "Not decidable") typeof<System.Exception>