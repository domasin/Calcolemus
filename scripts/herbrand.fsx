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

let fm = 
    !!"exists x. forall y. P(x) ==> P(y)"
    |> Not
    |> skolemize

let fm' = 
    !!"~ (exists x. forall y. P(x) ==> P(y))"
    |> skolemize

fm = fm'

!!"exists x. forall y. P(x) ==> P(y)"
|> gilmore

let sfm = skolemize(Not !!"exists x. forall y. P(x) ==> P(y)")
let fvs = fv sfm
let consts, funcs = herbfuns sfm
let cntms = image (fun (c, _) -> Fn (c, [])) consts

// Gilmore loop
// List.length (gilmore_loop (simpdnf sfm) cntms funcs fvs 0 [[]] [] [])

let mfn djs0 ifn djs =
        List.filter (non trivial) (distrib (image (image ifn) djs0) djs)

// let rec herbloop mfn tfn fl0 cntms funcs fvs n fl tried tuples =
// herbloop mfn (fun djs -> djs <> []) (simpdnf sfm) cntms funcs fvs 0 [[]] [] []

let newtups = groundtuples cntms funcs 0 (List.length fvs)
// [[<<|c|>>]]

// herbloop mfn tfn fl0 cntms funcs fvs (n + 1) fl tried newtups

let fl' = mfn (simpdnf sfm) (subst (fpf fvs (newtups |> List.head))) [[]]
not ((fun djs -> djs <> []) fl') 

let newtups' = groundtuples cntms funcs 1 (List.length fvs)

mfn (simpdnf sfm) (subst (fpf fvs [!!!"f_y(c)"; ])) [[]]
// [[<<P(f_y(c))>>; <<~P(f_y(f_y(c)))>>]]

let fl'' = mfn (simpdnf sfm) (subst (fpf fvs [!!!"f_y(c)"; ])) [[!!"P(c)"; !!"~P(f_y(c))"]]
not ((fun djs -> djs <> []) fl'') 

gilmore p24

gilmore p45

//fails with out of memory
// gilmore p20

#time
davisputnam p20
#time
// Real: 00:00:00.012, CPU: 00:00:00.020, GC gen0: 1, gen1: 0, gen2: 0

#time
davisputnam002 p20
// there is something unclear
#time
// Real: 00:00:00.013, CPU: 00:00:00.010, GC gen0: 0, gen1: 0, gen2: 0

#time
davisputnam p36
#time
// Real: 00:00:00.422, CPU: 00:00:00.430, GC gen0: 5, gen1: 1, gen2: 0

#time
davisputnam002 p36
// there is something unclear
#time
// Real: 00:00:00.790, CPU: 00:00:00.750, GC gen0: 11, gen1: 1, gen2: 0