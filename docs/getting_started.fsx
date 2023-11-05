(**
---
title: Getting started
category: Documentation
categoryindex: 1
index: 1
---

# Getting started

## Introduction

Calculemus contains functions to perform formal deductive inference in 
propositional or first order logic.

This document demonstrates how to use this library.

First, we reference and open Calculemus

    #r "nuget: Calculemus, 1.0.5"
    open Calculemus
*)

(*** hide ***)
#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"
open Calculemus

(** 
## Custom printers

Then, it is necessary to load the module for the type of logical objects we are going to work with
*)

open Fol

(**
and also convenient to setup the corresponding custom printers

    fsi.AddPrinter sprint_fol_formula
*)

(** 

## Entering formulas

Now, to create a formula, let's say first order, it is necessary to apply the parser to the desired string expression.
*)

let fm = !!"exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)"

(** 
## Automated proving

The library contains various procedures to prove propositional tautologies or first order valid formulas. Note that, depending on the complexity of the formulas involved, these procedures may or may not be able to complete the task.

To try an automatic proof, it is enough to load the module containing it and apply the function to the desired formula.

### Propositional logic

The functions for tautology checking of propositional formulas return `true` or `false`, depending if the formula is a tautology or not:
*)

open Prop
open DP

dplbtaut !>"(p ==> q) <=> (~q ==> ~p)"
(*** include-fsi-merged-output ***)

(**
### First order logic

Unlike propositional logic, for first order logic automated validity checking is only semidecidable. There are methods that, given a valid formula, can generate a sequence of inferences such that after a finite (but not definable in advance) number of steps prove the validity of the formula. However, if the formula is not valid, the same procedures will continue ad infinitum without returning any result. This is not simply a question of how well designed such procedures are but the best that can be hoped also from a theoretical point of view.

Thus, the automated validity checking functions for first order logic, in general, don't return a simple `true` or `false` but rather the information that the procedure ended successfully or, in some cases, that it was interrupted and therefore did not produce any significant results.

For example, `cref:M:Calculemus.Herbrand.davisputnam`
*)

open Herbrand

let test = davisputnam fm

(*** include-fsi-merged-output ***)

(** 
as the api reference reports, returns 

> The number of ground tuples generated and prints to the `stdout` how many ground instances were tried, if the procedure terminates (thus indicating that the formula is valid); otherwise, loops indefinitely till the memory is full. 

*)

(**

## Interactive theorem proving

The library contains also various interactive theorem proving features in the LCF styles, both with simple forward rules (starting from axioms and deriving new theorems by applying inference rules); as with goal oriented proofs based on tactics.

*)

open Lcf
open Lcfprop
open Folderived
open Tactics

(**

Like before is convenient to install the custom printers:

    fsi.AddPrinter sprint_thm
    fsi.AddPrinter sprint_goal

*)

(**
### Forward rules
*)

axiom_addimp !!"P(x)" !!"Q(x)"  // |- P(x) ==> Q(x) ==> P(x)
|> add_assum !!"R(x,y)"         // |- R(x,y) ==> P(x) ==> Q(x) ==> P(x)

(**
### Goals proving
*)

let g0 = 
    !! @"
        (forall x. x <= x) /\
        (forall x y z. x <= y /\ y <= z ==> x <= z) /\
        (forall x y. f(x) <= y <=> x <= g(y))
        ==> (forall x y. x <= y ==> f(x) <= f(y)) /\
            (forall x y. x <= y ==> g(x) <= g(y))"
    |> set_goal

g0 |> print_goal
(*** include-output ***)

let g1 = imp_intro_tac "ant" g0

g1 |> print_goal
(*** include-output ***)

let g2 = conj_intro_tac g1

g2 |> print_goal
(*** include-output ***)

let g3 = Lib.Function.funpow 2 (auto_tac by ["ant"]) g2

g3 |> print_goal
(*** include-output ***)

extract_thm g3
|> print_thm
(*** include-output ***)

(**
### Declarative proof
*)
    
[note("eq_sym",(!!"forall x y. x = y ==> y = x"))
    using [eq_sym (!!!"x") (!!!"y")];
note("eq_trans",(!!"forall x y z. x = y /\ y = z ==> x = z"))
    using [eq_trans (!!!"x") (!!!"y") (!!!"z")];
note("eq_cong",(!!"forall x y. x = y ==> f(x) = f(y)"))
    using [axiom_funcong "f" [(!!!"x")] [(!!!"y")]];
assume ["le",(!!"forall x y. x <= y <=> x * y = x");
        "hom",(!!"forall x y. f(x * y) = f(x) * f(y)")];
fix "x"; fix "y";
assume ["xy",(!!"x <= y")];
so have (!!"x * y = x") by ["le"];
so have (!!"f(x * y) = f(x)") by ["eq_cong"];
so have (!!"f(x) = f(x * y)") by ["eq_sym"];
so have (!!"f(x) = f(x) * f(y)") by ["eq_trans"; "hom"];
so have (!!"f(x) * f(y) = f(x)") by ["eq_sym"];
so conclude (!!"f(x) <= f(y)") by ["le"];
qed]
|> prove !! @"
        (forall x y. x <= y <=> x * y = x) /\
        (forall x y. f(x * y) = f(x) * f(y))
        ==> forall x y. x <= y ==> f(x) <= f(y)"
|> print_thm
(*** include-output ***)