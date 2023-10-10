#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Set
open FolAutomReas.Lib.Fpf

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Skolem
open FolAutomReas.Herbrand

open FolAutomReas.Prop
open FolAutomReas.Pelletier
open FolAutomReas.DP

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let fm = !!"exists x. forall y. P(x) ==> P(y)" //p42
davisputnam fm

let sfm = skolemize (Not (generalize fm))
let fvs = fv sfm
let consts, funcs = herbfuns sfm
fvs,consts, funcs
let cnf = simpcnf sfm
let cntms = image (fun (c, _) -> Fn (c, [])) consts

// 0 ground instances tried; 0 items in list.
// 0 ground instances tried; 0 items in list.
// 1 ground instances tried; 5 items in list.
// 1 ground instances tried; 5 items in list.
// 2 ground instances tried; 8 items in list.

let newtups0 = groundtuples cntms funcs 0 (List.length fvs)

dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf fvs [!!!"c"])) []

dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf fvs [!!!"f_y(c)"])) [[!!"P(c)"]; [!!"~P(f_y(c))"]]

let fl0 = union (image (image (subst (fpf fvs (newtups0 |> List.head)))) cnf) []
not (dpll fl0), 0

let newtups1 = groundtuples cntms funcs 1 (List.length fvs)
let fl1 = union (image (image (subst (fpf fvs (newtups1 |> List.head)))) cnf) fl0
not (dpll fl1), fl0.Length

let newtups2 = groundtuples cntms funcs 2 (List.length fvs)
let fl2_1 = union (image (image (subst (fpf fvs (newtups2 |> List.item 0)))) cnf) fl1
not (dpll fl2_1), fl1.Length

let fl2_2 = union (image (image (subst (fpf fvs (newtups2 |> List.item 1)))) cnf) fl1
not (dpll fl2_2), fl1.Length

let fl2_3 = union (image (image (subst (fpf fvs (newtups2 |> List.item 2)))) cnf) fl1
not (dpll fl2_3), fl1.Length

let fl2_4 = union (image (image (subst (fpf fvs (newtups2 |> List.item 3)))) cnf) fl1
not (dpll fl2_4), fl1.Length

// let newtups3 = groundtuples cntms funcs 3 (List.length fvs)
// let fl3 = union (image (image (subst (fpf fvs (newtups3 |> List.head)))) cnf) fl2
// not (dpll fl3)

// let newtups4 = groundtuples cntms funcs 4 (List.length fvs)
// let fl4 = union (image (image (subst (fpf fvs (newtups4 |> List.head)))) cnf) fl3
// not (dpll fl4)

// let newtups5 = groundtuples cntms funcs 5 (List.length fvs)
// let fl5 = union (image (image (subst (fpf fvs (newtups5 |> List.head)))) cnf) fl4
// not (dpll fl5)

// let newtups6 = groundtuples cntms funcs 6 (List.length fvs)
// let fl6 = union (image (image (subst (fpf fvs (newtups6 |> List.head)))) cnf) fl5
// not (dpll fl6)

// let newtups7 = groundtuples cntms funcs 7 (List.length fvs)
// let fl7 = union (image (image (subst (fpf fvs (newtups7 |> List.head)))) cnf) fl6
// not (dpll fl7)
// let newtups8 = groundtuples cntms funcs 8 (List.length fvs)
// let fl8 = union (image (image (subst (fpf fvs (newtups8 |> List.head)))) cnf) fl7
// not (dpll fl8)
// let newtups9 = groundtuples cntms funcs 9 (List.length fvs)
// let fl9 = union (image (image (subst (fpf fvs (newtups9 |> List.head)))) cnf) fl8
// not (dpll fl9)
// let newtups10 = groundtuples cntms funcs 10 (List.length fvs)
// let fl10 = union (image (image (subst (fpf fvs (newtups10 |> List.head)))) cnf) fl9
// not (dpll fl10)
// let newtups11 = groundtuples cntms funcs 11 (List.length fvs)
// let fl11 = union (image (image (subst (fpf fvs (newtups11 |> List.head)))) cnf) fl10
// not (dpll fl11)
// let newtups12 = groundtuples cntms funcs 12 (List.length fvs)
// let fl12 = union (image (image (subst (fpf fvs (newtups12 |> List.head)))) cnf) fl11
// not (dpll fl12)
// let newtups13 = groundtuples cntms funcs 13 (List.length fvs)
// let fl13 = union (image (image (subst (fpf fvs (newtups13 |> List.head)))) cnf) fl12
// not (dpll fl13)
// let newtups14 = groundtuples cntms funcs 14 (List.length fvs)
// let fl14 = union (image (image (subst (fpf fvs (newtups14 |> List.head)))) cnf) fl13
// not (dpll fl14)
// let newtups15 = groundtuples cntms funcs 15 (List.length fvs)
// let fl15 = union (image (image (subst (fpf fvs (newtups15 |> List.head)))) cnf) fl14
// not (dpll fl15)
// let newtups16 = groundtuples cntms funcs 16 (List.length fvs)
// let fl16 = union (image (image (subst (fpf fvs (newtups16 |> List.head)))) cnf) fl15
// not (dpll fl16)
// let newtups17 = groundtuples cntms funcs 17 (List.length fvs)
// let fl17 = union (image (image (subst (fpf fvs (newtups17 |> List.head)))) cnf) fl16
// not (dpll fl17)
// let newtups18 = groundtuples cntms funcs 18 (List.length fvs)
// let fl18 = union (image (image (subst (fpf fvs (newtups18 |> List.head)))) cnf) fl17
// not (dpll fl18)
// let newtups19 = groundtuples cntms funcs 19 (List.length fvs)
// let fl19 = union (image (image (subst (fpf fvs (newtups19 |> List.head)))) cnf) fl18
// not (dpll fl19)
// let newtups20 = groundtuples cntms funcs 20 (List.length fvs)
// let fl20 = union (image (image (subst (fpf fvs (newtups20 |> List.head)))) cnf) fl19
// not (dpll fl20)
