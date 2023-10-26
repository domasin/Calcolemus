#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Lib.Function
open Calcolemus.Lib.Set
open Calcolemus.Lib.Fpf

open Calcolemus.Formulas
open Calcolemus.Fol
open Calcolemus.Skolem
open Calcolemus.Herbrand

open Calcolemus.Prop
open Calcolemus.Pelletier

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let fm = !!"exists x. forall y. P(x) ==> P(y)"
let sfm = skolemize (Not (generalize fm))
// <<P(x) /\ ~P(f_y(x))>>

let fvs = fv sfm
let consts, funcs = herbfuns sfm
fvs,consts, funcs
// (["x"], [("c", 0)], [("f_y", 1)])

let dnf = simpdnf sfm
// [[<<P(x)>>; <<~P(f_y(x))>>]]

// 0 generation
let cntms = image (fun (c, _) -> Fn (c, [])) consts
let newtups = groundtuples cntms funcs 0 (List.length fvs)
let fl' = (distrib (image (image (subst (fpf fvs (newtups |> List.head)))) dnf) [[]])
// [[<<P(c)>>; <<~P(f_y(c))>>]]

fl'
|> List.filter (non trivial)
|> (<>) []
|> not
// false

let newtups' = groundtuples cntms funcs 1 (List.length fvs)
let fl'' = (distrib (image (image (subst (fpf fvs (newtups' |> List.head)))) dnf) fl')
fl'' 
// [[<<P(c)>>; <<P(f_y(c))>>; <<~P(f_y(c))>>; <<~P(f_y(f_y(c)))>>]]

fl''
|> List.filter (non trivial)
|> (<>) []
|> not
// true

groundtuples [!!!"0"] [("f",1)] 2 1

groundterms [!!!"0";!!!"1"] [("f",1);("g",2)] 2