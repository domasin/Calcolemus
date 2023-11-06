// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Meson

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Lib.Fpf

open Formulas
open Fol
open Meson


[<Fact>]
let ``contrapositives should return the contrapositives of the input clause.``() = 
    contrapositives !!>["P";"Q";"~R"]
    |> shouldEqual 
        [
            (!!>["~Q"; "R"], !!"P"); 
            (!!>["~P"; "R"], !!"Q"); 
            (!!>["~P"; "~Q"], !!"~R")
        ]

[<Fact>]
let ``contrapositives should return also the rule with false as conclusion if the input clause is all negative.``() = 
    contrapositives !!>["~P";"~Q";"~R"]
    |> shouldEqual 
        [
            (!!>["P"; "Q"; "R"], !!"false"); 
            (!!>["Q"; "R"], !!"~P"); 
            (!!>["P"; "R"], !!"~Q");
            (!!>["P"; "Q"], !!"~R")
        ]

[<Fact>]
let ``mexpand_basic should return the successful instantiation if it exists together with the depth reached and the number of variables renamed.``() = 
    mexpand_basic
        [
            ([], !!"P(x)"); 
            ([!!"P(x)"], False);
        ]
        [] False id (undefined,1,0)
    |> fun (env,n,k) -> (graph env,n,k)
    |> shouldEqual 
        ([("_0", !!!"_1")], 0, 2)

[<Fact>]
let ``mexpand_basic should fail with 'tryfind' if the goal cann't be solved according to the rules with the given depth limit.``() = 
    (fun () -> 
        mexpand_basic
            [
                ([], !!"P(x)"); 
                ([!!"P(x)"], False);
            ]
            [] False id (undefined,0,0)
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>

[<Fact>]
let ``mexpand_basic should fail with 'Too deep' if the goal depth is less then 0.``() = 
    (fun () -> 
        mexpand_basic
            [
                ([], !!"P(x)"); 
                ([!!"P(x)"], False);
            ]
            [] False id (undefined,-1,0)
        |> ignore
    )
    |> should (throwWithMessage "Too deep") typeof<System.Exception>

[<Fact>]
let ``puremeson_basic should return the depth limit reached if the formula is unsatisfiable.``() = 
    !!"P(x) /\ ~P(x)"
    |> puremeson_basic
    |> shouldEqual 1

[<Fact>]
let ``meson_basic should return the list of depth limits reached if the formula is valid.``() = 
    !! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
    |> meson_basic
    |> shouldEqual [8]

[<Fact>]
let ``equal should return true if the literals are identical under the given instantiation.``() = 
    equal 
        (("x" |-> !!!"f(y)")undefined) 
        !!"P(x)" !!"P(f(y))"
    |> shouldEqual true

[<Fact>]
let ``equal should return false if the literals are not identical under the given instantiation.``() = 
    equal 
        (("x" |-> !!!"f(z)")undefined) 
        !!"P(x)" !!"P(f(y))"
    |> shouldEqual false

[<Fact>]
let ``equal should return false if the input formulas are identical but not literals.``() = 
    equal 
        (("x" |-> !!!"f(y)")undefined) 
        !!"P(x) /\ P(x)" !!"P(f(y)) /\ P(f(y))"
    |> shouldEqual false

[<Fact>]
let ``mexpand should return the successful instantiation if it exists together with the depth reached and the number of variables renamed.``() = 
    mexpand
        [
            ([], !!"P(x)"); 
            ([!!"P(x)"], False);
        ]
        [] False id (undefined,1,0)
    |> fun (env,n,k) -> (graph env,n,k)
    |> shouldEqual 
        ([("_0", !!!"_1")], 0, 2)

[<Fact>]
let ``mexpand should fail with 'tryfind' if the goal cann't be solved according to the rules with the given depth limit.``() = 
    (fun () -> 
        mexpand
            [
                ([], !!"P(x)"); 
                ([!!"P(x)"], False);
            ]
            [] False id (undefined,0,0)
        |> ignore
    )
    |> should (throwWithMessage "tryfind") typeof<System.Exception>

[<Fact>]
let ``mexpand should fail with 'Too deep' if the goal depth is less then 0.``() = 
    (fun () -> 
        mexpand
            [
                ([], !!"P(x)"); 
                ([!!"P(x)"], False);
            ]
            [] False id (undefined,-1,0)
        |> ignore
    )
    |> should (throwWithMessage "Too deep") typeof<System.Exception>

[<Fact>]
let ``puremeson should return the depth limit reached if the formula is unsatisfiable.``() = 
    !!"P(x) /\ ~P(x)"
    |> puremeson
    |> shouldEqual 1