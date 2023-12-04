#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Fol
open Decidable

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
(forall u v w x y z.
P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) <=> P(u,z,v)))
==> forall a b c. P(a,b,c) ==> P(b,a,c)"
|> aedecide

!! @"forall x. f(x) = 0"
|> aedecide