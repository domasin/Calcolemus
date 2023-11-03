#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus
open Lib.Fpf
open Formulas
open Fol
open Clause
open Tableaux
open Unif
open Skolem
open Lib.Search

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!! @"(forall x.
        P(a) /\ (P(x) ==> (exists y. P(y) /\ R(x,y))) ==>
        (exists z w. P(z) /\ R(x,w) /\ R(w,z))) <=>
        (forall x.
        (~P(a) \/ P(x) \/ (exists z w. P(z) /\ R(x,w) /\ R(w,z))) /\
        (~P(a) \/ ~(exists y. P(y) /\ R(x,y)) \/
        (exists z w. P(z) /\ R(x,w) /\ R(w,z))))"
|> generalize
|> Not
|> askolemize
|> Prop.simpdnf

!!"((exists x. forall y. P(x) <=> P(y)) <=>
    ((exists x. Q(x)) <=> (forall y. Q(y)))) <=>
   ((exists x. forall y. Q(x) <=> Q(y)) <=>
    ((exists x. P(x)) <=> (forall y. P(y))))"
|> splittab


[!! "forall x y. P(x) /\ ~P(f(y))"; 
 !! "R(x,y) /\ ~R(x,y)"]
|> tabrefute

!!"(P(x) ==> q) <=> (~q ==> ~p)"
|> generalize
|> Not
|> askolemize

deepen id 1

tableau ([!!"P(x)"], [!!"~P(f(y))"], 0) id (undefined, 0)
|> fun (inst,nrInst) -> inst |> graph,nrInst
// ([("x", ``f(y)``)], 0)

tableau ([!!"P(x)"], [!!"~P(f(y))"], -1) id (undefined, 0)
|> fun (inst,nrInst) -> inst |> graph,nrInst
// System.Exception: no proof at this level

tableau ([], [!!"~P(f(x))"], 0) id (undefined, 0)
|> fun (inst,nrInst) -> inst |> graph,nrInst
// System.Exception: tableau: no proof


// tableau ([!!"P(x) /\ ~P(f(y))"],[],0) id (undefined,0)
// |> fun (inst,nrInst) -> inst |> graph,nrInst

// tableau ([!!"forall x y. P(x) /\ ~P(f(y))"],[],2) id (undefined,0)
// |> fun (inst,nrInst) -> inst |> graph,nrInst

// !!"exists y. forall x. P(y) ==> P(x)"
// |> tab
// // |> generalize
// // |> Not        // `~(forall t u. R(t,u) ==> (exists v. R(t,v) /\ R(u,v)))`
// // |> askolemize // `R(c_t,c_u) /\ (forall v. ~R(c_t,v) \/ ~R(c_u,v))`
// // |> fun fm -> tabrefute [fm]

// !! @"R(x,y) /\ (forall z. ~R(x,z) \/ ~R(y,z))"
// |> fun fm ->
//     tableau ([fm], [], 1) id (undefined, 0)
// |> fun (inst,nrInst) -> graph inst, nrInst

// unify_literals undefined (!!"P(x)",!!"P(f(y))")
// |> graph
// // x |-> f(y)

// unify_literals (("x" |-> Var "z")undefined) (!!"P(y)",!!"P(y)")
// |> graph
// // (("x" |-> Var "z")undefined)

// unify_literals undefined (!!"False",!!"False")
// // Empty

// unify_literals undefined (!!"P(y)",!!"P(f(y))")
// // System.Exception: cyclic

// unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
// // System.Exception: impossible unification

// unify_literals undefined (!!"P(x) /\ P(x)",!!"P(f(y)) /\ P(f(y))")
// // System.Exception: Can't unify literals

// unify_complements undefined (!!"P(x)",!!"~P(f(y))")
// |> graph

// unify_literals undefined (!!"P(x)",!!"~P(f(y))")
// |> graph

// undefined
// |> unify_refute !!>>[
//         ["P(x)";"~P(f(y))";"R(x,y)"];
//         ["Q(x)";"~Q(x)"]
// ]
// |> graph

// unify_refute !!>>[["P(c)"];["Q(c)"]] undefined

// prawitz_loop !!>>[
//         ["P(x)";"~P(f(y))";"R(x,y)"];
//         ["Q(x)";"~Q(x)"]
// ] ["x";"y"] [[]] 0
// |> fun (env,nr) -> env |> graph, nr

// prawitz_loop !!>>[
//         ["P(0)";"~P(f(y))";"R(x,y)"];
//         ["Q(x)";"~Q(x)"]
// ] ["x";"y"] [[]] 0
// |> fun (env,nr) -> env |> graph, nr
