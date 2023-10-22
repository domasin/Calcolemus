#load "init.fsx"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Lib.Fpf
open FolAutomReas.Defcnf

mk_imp !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mk_iff !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mkprop 3I

let fm = !> @"(p \/ (q /\ ~r)) /\ s" |> nenf
// `~p \/ q`
maincnf (!> @"p \/ (p \/ q)", undefined, 0I) 
// (`p_1`,
//    `p \/ p_0` |-> (`p_1`, `p_1 <=> p \/ p_0`)
//    `p \/ q`   |-> (`p_0`, `p_0 <=> p \/ q`),
//    2I)

