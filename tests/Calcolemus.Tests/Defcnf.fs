// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Defcnf

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open Calcolemus.Lib.Fpf

open Calcolemus.Formulas
open Calcolemus.Prop
open Calcolemus.Defcnf

[<Fact>]
let ``mkprop should return the index variable.``() = 
    mkprop 3I
    |> should equal (Atom (P "p_3"), 4I)

[<Fact>]
let ``maincnf-1.``() = 
    maincnf (!> @"p \/ (p \/ q)", undefined, 0I)
    |> shouldEqual 
        (!>"p_1",
        undefined
        |> (!> @"p \/ q" |-> (!>"p_0", !> @"p_0 <=> p \/ q"))
        |> (!> @"p \/ p_0" |-> (!>"p_1", !> @"p_1 <=> p \/ p_0")),
        2I)

[<Fact>]
let ``max_varindex should return the greater between the variable index and n.``() = 
    Assert.Equal(1I, max_varindex "p_" "p_0" 1I)
    Assert.Equal(2I, max_varindex "p_" "p_2" 2I)
    Assert.Equal(1I, max_varindex "p_" "x_2" 1I)

[<Fact>]
let ``mk_defcnf should return the result of a specific CNF procedure in a set-of-sets representation.``() = 
    !>"p ==> q"
    |> mk_defcnf maincnf
    |> should equal
        [[Atom (P "p"); Atom (P "p_1")]; 
         [Atom (P "p_1")];
         [Atom (P "p_1"); Not (Atom (P "q"))];
         [Atom (P "q"); Not (Atom (P "p")); Not (Atom (P "p_1"))]]

[<Fact>]
let ``defcnf01 returns an equisatisfiable CNF of the input formula.``() = 
    !>"p ==> q"
    |> defcnf01
    |> sprint_prop_formula
    |> should equal @"`(p \/ p_1) /\ p_1 /\ (p_1 \/ ~q) /\ (q \/ ~p \/ ~p_1)`"

[<Fact>]
let ``defcnfs should return the result of a specific CNF procedure in a set-of-sets representation.``() = 
    !> @"(p \/ (q /\ ~r)) /\ s"
    |> defcnfs
    |> List.map (fun xs -> 
        xs |> List.map (fun fm -> fm |> sprint_prop_formula)
    )
    |> shouldEqual
        [["`p`"; "`p_1`"]; ["`p_1`"; "`r`"; "`~q`"]; ["`q`"; "`~p_1`"]; ["`s`"]; ["`~p_1`"; "`~r`"]]

[<Fact>]
let ``defcnf returns an equisatisfiable CNF of the input formula.``() = 
    !> @"(p \/ (q /\ ~r)) /\ s"
    |> defcnf
    |> sprint_prop_formula
    |> should equal @"`(p \/ p_1) /\ (p_1 \/ r \/ ~q) /\ (q \/ ~p_1) /\ s /\ (~p_1 \/ ~r)`"

[<Fact>]
let ``defcnf3 returns an equisatisfiable CNF of the input formula that is in 3-CNF.``() = 
    !> @"(a \/ b \/ c \/ d) /\ s"
    |> defcnf3
    |> sprint_prop_formula
    |> should equal @"`(a \/ p_2 \/ ~p_3) /\ (b \/ p_1 \/ ~p_2) /\ (c \/ d \/ ~p_1) /\ (p_1 \/ ~c) /\ (p_1 \/ ~d) /\ (p_2 \/ ~b) /\ (p_2 \/ ~p_1) /\ p_3 /\ (p_3 \/ ~a) /\ (p_3 \/ ~p_2) /\ s`"