#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus

open Lib.Function
open Lib.Set
open Lib.Fpf

open Formulas
open Fol
open Skolem
open Herbrand
open Prop
open Pelletier
open Clause

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

gilmore p19

gilmore !!"true"

let consts, funcs = herbfuns !! @"(~P(f_y(x)) \/ Q(f_z(x))) /\ P(x) /\ ~Q(x)"

let cls = !!>>[["P(x)"; "Q(f_z(x))"; "~Q(x)"]; ["P(x)"; "~P(f_y(x))"; "~Q(x)"]]

gilmore_loop cls !!!>["c"] [("f_z",1);("f_y",1)] ["x"] 1
    !!>>[
        ["P(c)"; "P(f_y(c))"; "Q(f_z(c))"; 
        "Q(f_z(f_y(c)))"; "~Q(c)";"~Q(f_y(c))"];
        ["P(c)"; "P(f_y(c))"; "Q(f_z(c))"; 
        "~P(f_y(f_y(c)))"; "~Q(c)";"~Q(f_y(c))"]
    ]
    [[!!!"f_y(c)"]; [!!!"c"]] [[!!!"f_z(c)"]]

gilmore_loop !!>>[["Q(f_z(x))"; "~Q(y)"]] !!!>["c"] [("f_z",1);("f_y",1)] ["x";"y"] 0 [[]] [] [] 

!!>>[["P(f(c))"]; ["P(f(c))"; "~P(c)"]; ["~P(c)"; "~P(f(c))"]]
|> clausesToDnf


gilmore_loop !!>>[["P(x)"]; ["~P(x)"]] !!!>["c"] [] ["x"] 0 [] [] []

gilmore_loop !!>>[["P(f(x))"]; ["~P(y)"]] !!!>["0";"1"] [("f",1)] ["x";"y"] 1 [] [] []

!! "~(P(x) /\ ~P(x))"
|> dnf


let sfm = skolemize(Not !!"exists x. forall y. P(x) ==> P(y)")
let fvs = fv sfm
let consts, funcs = herbfuns sfm
let cntms = image (fun (c, _) -> Fn (c, [])) consts

// Gilmore loop
// List.length (gilmore_loop (simpdnf sfm) cntms funcs fvs 0 [[]] [] [])

let mfn djs0 ifn djs =
    (distrib (image (image ifn) djs0) djs)
    |> List.filter (non trivial) 

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