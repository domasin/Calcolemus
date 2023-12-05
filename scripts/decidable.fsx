#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Fol
open Skolem
open Decidable

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

pnf(nnf(miniscope(nnf
!! @"((exists x. forall y. P(x) <=> P(y)) <=>
((exists x. Q(x)) <=> (forall y. Q(y)))) <=>
((exists x. forall y. Q(x) <=> Q(y)) <=>
((exists x. P(x)) <=> (forall y. P(y))))")))

wang Pelletier.p20

wang !!"forall x. f(x) = 0"


miniscope(nnf !!"exists y. forall x. P(y) ==> P(x)")

let fm = miniscope(nnf
!! @"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w))
==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))")

pnf(nnf fm)

let fm' = miniscope(nnf
!! @"exists x. P(x) /\ Q(x)")

pnf(nnf fm')

!!"P(x) ==> forall y. Q(y)"
|> pushquant "x"

!!>["P(x)"; "Q(y)"; "T(y) /\ R(x,y)"; "S(z,w) ==> Q(i)"]
|> separate "x"

!! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
(forall u v w x y z.
P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) <=> P(u,z,v)))
==> forall a b c. P(a,b,c) ==> P(b,a,c)"
|> aedecide

!! @"forall x. f(x) = 0"
|> aedecide