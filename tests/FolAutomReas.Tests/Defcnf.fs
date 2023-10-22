module FolAutomReas.Tests.Defcnf

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open FolAutomReas.Lib.Fpf

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Defcnf

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