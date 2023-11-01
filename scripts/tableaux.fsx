#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus
open Lib.Fpf
open Fol
open Clause
open Tableaux
open Unif

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

unify_literals undefined (!!"P(x)",!!"P(f(y))")
|> graph
// x |-> f(y)

unify_literals (("x" |-> Var "z")undefined) (!!"P(y)",!!"P(y)")
|> graph
// (("x" |-> Var "z")undefined)

unify_literals undefined (!!"False",!!"False")
// Empty

unify_literals undefined (!!"P(y)",!!"P(f(y))")
// System.Exception: cyclic

unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
// System.Exception: impossible unification

unify_literals undefined (!!"P(x) /\ P(x)",!!"P(f(y)) /\ P(f(y))")
// System.Exception: Can't unify literals

unify_complements undefined (!!"P(x)",!!"~P(f(y))")
|> graph

unify_literals undefined (!!"P(x)",!!"~P(f(y))")
|> graph

undefined
|> unify_refute !!>>[
        ["P(x)";"~P(f(y))";"R(x,y)"];
        ["Q(x)";"~Q(x)"]
]
|> graph

unify_refute !!>>[["P(c)"];["Q(c)"]] undefined

prawitz_loop !!>>[
        ["P(x)";"~P(f(y))";"R(x,y)"];
        ["Q(x)";"~Q(x)"]
] ["x";"y"] [[]] 0
|> fun (env,nr) -> env |> graph, nr

prawitz_loop !!>>[
        ["P(0)";"~P(f(y))";"R(x,y)"];
        ["Q(x)";"~Q(x)"]
] ["x";"y"] [[]] 0
|> fun (env,nr) -> env |> graph, nr

prawitz Pelletier.p20

open Lib.Function
open Lcf
open Folderived
open Tactics


// fsi.AddPrinter sprint_goal
// fsi.AddPrinter sprint_thm


let g0 = 
    set_goal (parse @"
        (forall x. x <= x) /\
        (forall x y z. x <= y /\ y <= z ==> x <= z) /\
        (forall x y. f(x) <= y <=> x <= g(y))
        ==> (forall x y. x <= y ==> f(x) <= f(y)) /\
            (forall x y. x <= y ==> g(x) <= g(y))")

// tactics.p002
let g1 = imp_intro_tac "ant" g0;;

// tactics.p003
let g2 = conj_intro_tac g1;;

// tactics.p004
let g3 = funpow 2 (auto_tac by ["ant"]) g2;;

// tactics.p005
extract_thm g3;;

let ewd954 = 
    prove (parse @"
        (forall x y. x <= y <=> x * y = x) /\
        (forall x y. f(x * y) = f(x) * f(y))
        ==> forall x y. x <= y ==> f(x) <= f(y)")
        [note("eq_sym",(parse @"forall x y. x = y ==> y = x"))
            using [eq_sym (parset @"x") (parset @"y")];
        note("eq_trans",(parse @"forall x y z. x = y /\ y = z ==> x = z"))
            using [eq_trans (parset @"x") (parset @"y") (parset @"z")];
        note("eq_cong",(parse @"forall x y. x = y ==> f(x) = f(y)"))
            using [axiom_funcong "f" [(parset @"x")] [(parset @"y")]];
        assume ["le",(parse @"forall x y. x <= y <=> x * y = x");
                "hom",(parse @"forall x y. f(x * y) = f(x) * f(y)")];
        fix "x"; fix "y";
        assume ["xy",(parse @"x <= y")];
        so have (parse @"x * y = x") by ["le"];
        so have (parse @"f(x * y) = f(x)") by ["eq_cong"];
        so have (parse @"f(x) = f(x * y)") by ["eq_sym"];
        so have (parse @"f(x) = f(x) * f(y)") by ["eq_trans"; "hom"];
        so have (parse @"f(x) * f(y) = f(x)") by ["eq_sym"];
        so conclude (parse @"f(x) <= f(y)") by ["le"];
        qed]