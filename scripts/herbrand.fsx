#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Skolem
open FolAutomReas.Herbrand
open FolAutomReas.Lib
open FolAutomReas.Prop

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

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

let fl' = mfn (simpdnf sfm) (subst (fpf fvs [!|"c"])) [[]]
not ((fun djs -> djs <> []) fl') 

let newtups' = groundtuples cntms funcs 1 (List.length fvs)

mfn (simpdnf sfm) (subst (fpf fvs [!|"f_y(c)"; ])) [[]]
// [[<<P(f_y(c))>>; <<~P(f_y(f_y(c)))>>]]

let fl'' = mfn (simpdnf sfm) (subst (fpf fvs [!|"f_y(c)"; ])) [[!!"P(c)"; !!"~P(f_y(c))"]]
not ((fun djs -> djs <> []) fl'') 

let p24 = 
    !! @"~(exists x. U(x) /\ Q(x)) /\
    (forall x. P(x) ==> Q(x) \/ R(x)) /\
    ~(exists x. P(x) ==> (exists x. Q(x))) /\
    (forall x. Q(x) /\ R(x) ==> U(x))
    ==> (exists x. P(x) /\ R(x))"

gilmore p24

let p45 = 
    !! @"(forall x. P(x) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y))
    ==> (forall y. G(y) /\ H(x,y) ==> R(y))) /\
    ~(exists y. L(y) /\ R(y)) /\
    (exists x. P(x) /\ (forall y. H(x,y) ==> L(y)) /\
    (forall y. G(y) /\ H(x,y) ==> J(x,y)))
    ==> (exists x. P(x) /\ ~(exists y. G(y) /\ H(x,y)))"

gilmore p45

let p20 = 
    !!"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w))
    ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))"

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

let p36 = 
    !! @"(forall x. exists y. J(x,y)) /\
        (forall x. exists y. G(x,y)) /\
        (forall x y. J(x,y) \/ G(x,y) ==> (forall z. J(y,z) \/ G(y,z) ==> H(x, z)))
    ==> (forall x. exists y. H(x,y))"

#time
davisputnam p36
#time
// Real: 00:00:00.422, CPU: 00:00:00.430, GC gen0: 5, gen1: 1, gen2: 0

#time
davisputnam002 p36
// there is something unclear
#time
// Real: 00:00:00.790, CPU: 00:00:00.750, GC gen0: 11, gen1: 1, gen2: 0