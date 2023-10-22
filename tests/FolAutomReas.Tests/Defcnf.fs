module FolAutomReas.Tests.Defcnf

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Defcnf

[<Fact>]
let ``mkprop should return the index variable ``() = 
    mkprop 3I
    |> should equal (Atom (P "p_3"), 4I)