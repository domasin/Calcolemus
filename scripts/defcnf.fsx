#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Lib.Fpf
open FolAutomReas.Defcnf

// fsi.AddPrinter sprint_prop_formula

mk_imp !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mk_iff !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mkprop (1 |> bigint)
