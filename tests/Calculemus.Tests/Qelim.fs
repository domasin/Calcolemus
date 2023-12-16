// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Qelim

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Qelim

[<Fact>]
let ``cnnf should return a formula in disjunctive normal form equivalent to the input formula and with literals modified based on the literal modification function.``() = 
    !!"~(s = t) /\ ~(s < t)"
    |> cnnf lfn_dlo
    |> sprint_fol_formula
    |> shouldEqual @"`(s < t \/ t < s) /\ (s = t \/ t < s)`"

[<Fact>]
let ``lfn_dlo-1.``() = 
    lfn_dlo !!"~s < t"
    |> sprint_fol_formula
    |> shouldEqual @"`s = t \/ t < s`"

[<Fact>]
let ``lfn_dlo-2.``() = 
    lfn_dlo !!"~s = t"
    |> sprint_fol_formula
    |> shouldEqual @"`s < t \/ t < s`"

[<Fact>]
let ``lfn_dlo-3.``() = 
    lfn_dlo !!"~s = t /\ ~s < t"
    |> sprint_fol_formula
    |> shouldEqual @"`~s = t /\ ~s < t`"

[<Fact>]
let ``afn_dlo should return a formula equivalent to the input literal with inequality relations not included in the language converted into admitted ones.``() = 
    afn_dlo [] !!"s <= t"
    |> sprint_fol_formula
    |> shouldEqual @"`~t < s`"

[<Fact>]
let ``quelim_dlo-1.``() = 
    quelim_dlo !!"forall x y. exists z. z < x /\ z < y"
    |> sprint_fol_formula
    |> shouldEqual @"`true`"

[<Fact>]
let ``quelim_dlo-2.``() = 
    quelim_dlo !!"exists z. z < x /\ z < y"
    |> sprint_fol_formula
    |> shouldEqual @"`true`"

[<Fact>]
let ``quelim_dlo should return a quantifier free formula equivalent to input one (provided this is a dlo formula).``() = 
    quelim_dlo !!"exists z. x < z /\ z < y"
    |> sprint_fol_formula
    |> shouldEqual @"`x < y`"

[<Fact>]
let ``quelim_dlo-4.``() = 
    quelim_dlo !!"(forall x. x < a ==> x < b)"
    |> sprint_fol_formula
    |> shouldEqual @"`~(b < a \/ b < a)`"