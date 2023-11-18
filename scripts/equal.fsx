#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Fol
open Equal

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

lhs !!"P(x) <=> Q(y)"

function_congruence ("f",2)

equalitize !!"P(x) <=> Q(y)"


!! @"(forall x. f(x) ==> g(x)) /\
     (exists x. f(x)) /\
     (forall x y. g(x) /\ g(y) ==> x = y)
     ==> forall y. g(y) ==> f(y)"
|> equalitize
|> Meson.meson