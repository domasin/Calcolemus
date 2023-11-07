#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Meson
open Formulas
open Fol
open Skolem
open Prop
open Clause

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!! @"exists x. exists y. forall z.
        (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
        ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
|> meson

!! @"((forall x. P1(x) ==> P0(x)) /\ (exists x. P1(x))) /\
     ((forall x. P2(x) ==> P0(x)) /\ (exists x. P2(x))) /\
     ((forall x. P3(x) ==> P0(x)) /\ (exists x. P3(x))) /\
     ((forall x. P4(x) ==> P0(x)) /\ (exists x. P4(x))) /\
     ((forall x. P5(x) ==> P0(x)) /\ (exists x. P5(x))) /\
     ((exists x. Q1(x)) /\ (forall x. Q1(x) ==> Q0(x))) /\
     (forall x. P0(x)
                ==> (forall y. Q0(y) ==> R(x,y)) \/
                ((forall y. P0(y) /\ S0(y,x) /\
                    (exists z. Q0(z) /\ R(y,z))
                    ==> R(x,y)))) /\
     (forall x y. P3(y) /\ (P5(x) \/ P4(x)) ==> S0(x,y)) /\
     (forall x y. P3(x) /\ P2(y) ==> S0(x,y)) /\
     (forall x y. P2(x) /\ P1(y) ==> S0(x,y)) /\
     (forall x y. P1(x) /\ (P2(y) \/ Q1(y)) ==> ~(R(x,y))) /\
     (forall x y. P3(x) /\ P4(y) ==> R(x,y)) /\
     (forall x y. P3(x) /\ P5(y) ==> ~(R(x,y))) /\
     (forall x. (P4(x) \/ P5(x)) ==> exists y. Q0(y) /\ R(x,y))
     ==> exists x y. P0(x) /\ P0(y) /\
     exists z. Q1(z) /\ R(y,z) /\ R(x,y)"
|> meson_basic


contrapositives !!>["P";"Q";"~R"]
contrapositives !!>["~P";"~Q";"~R"]

!! @"P(x) \/ ~P(x)"
|> meson

// meson
Pelletier.p18
|> generalize
|> Not
|> askolemize
|> simpdnf
|> List.map list_conj
|> List.map puremeson

// pure meson
!!"forall y. P(y) /\ ~P(f_x(y))"
|> pnf
|> specialize
|> simpcnf

// in CNF
let cls = !!>>[["P(y)"]; ["~P(f_x(y))"]]
let rules = List.foldBack ((@) << contrapositives) cls []

mexpand_basic
    [
        ([], !!"P(x)"); 
        ([!!"P(x)"], False);
    ]
    [] False id (undefined,1,0)
|> fun (env,n,k) -> (graph env,n,k)

mexpand_basic
    [
        ([], !!"P(x)"); 
        ([!!"P(x)"], False);
    ]
    [] False id (undefined,0,0)
// System.Exception: tryfind

mexpand_basic
    [
        ([], !!"P(x)"); 
        ([!!"P(x)"], False);
    ]
    [] False id (undefined,-1,0)
// System.Exception: Too deep

!!"P(x) /\ ~P(x)"
|> puremeson_basic

!! @"P(x)"
|> puremeson_basic

!! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
|> meson_basic

!! @"P /\ ~P"
|> meson_basic
// crashes

equal 
    (("x" |-> !!!"f(y)")undefined) 
    !!"P(x)" !!"P(f(y))"

equal 
    (("x" |-> !!!"f(z)")undefined) 
    !!"P(x)" !!"P(f(y))"

equal 
    (("x" |-> !!!"f(y)")undefined) 
    !!"P(x) /\ P(x)" !!"P(f(y)) /\ P(f(y))"

