#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Formulas
open Fol
open Skolem
open Decidable
open Lib.Fpf

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
|> decide_fmp

!! @"(forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)) ==> forall x. R(x,x)"
|> decide_fmp

!! @"~((forall x. ~R(x,x)) /\
       (forall x. exists z. R(x,z)) /\
       (forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)))"
|> decide_fmp

!! @"((exists x. forall y. P(x) <=> P(y)) <=>
      ((exists x. Q(x)) <=> (forall y. Q(y)))) <=>
     ((exists x. forall y. Q(x) <=> Q(y)) <=>
      ((exists x. P(x)) <=> (forall y. P(y))))"
|> decide_monadic

!! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
|> decide_finite 2 



!! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
|> limited_meson 2
|> List.map (fun (inst, n, k) -> (inst |> graph, n, k))

!! @"~R(c_x,c_x) /\ (forall x y z. (~R(x,y) \/ ~R(y,z)) \/ R(x,z))"
|> limmeson 1

!! @"~R(x,x) /\ (forall x y. R(x,y) \/ R(y,x))"
|> limmeson 2
|> fun (inst, n, k) -> (inst |> graph, n, k)

!! @"R(x,x) /\ (forall x y. R(x,y) \/ R(y,x))"
|> limmeson 2

!! @"R(x) ==> (forall x. R(x))"
|> limmeson 2

!! @"~R(x,x) /\ (forall x y. R(x,y) \/ R(y,x))"
|> decide_fmp

!! @"(forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)) ==> forall x. R(x,x)"
|> decide_fmp

[1;2;3;4;5;6;7]
|> alltuples 3

let dom,ran = [1..3],[1..3]

allmappings dom ran
|> List.mapi (fun i f -> i, dom |> List.map f)

allfunctions [1..3] 2
|> List.mapi (fun i f -> 
    i, 
    dom 
    |> alltuples 2
    |> List.map (fun args -> args, f args)
)
|> List.take 3

allpredicates [1..3] 2
|> List.mapi (fun i f -> 
    i, 
    dom 
    |> alltuples 2
    |> List.map (fun args -> args, f args)
)
|> List.take 3

let functionSymbols = [("g",2)]
let functions = allfunctions [1..3]

alldepmappings functionSymbols functions
|> List.mapi (fun i f -> 
    i, 
    dom 
    |> alltuples 2
    |> List.map (fun args -> args,f "g" args))
|> List.take 3

atom "P" "x"

premiss_A ("P", "S")
premiss_E ("P", "S")
premiss_I ("P", "S")
premiss_O ("P", "S")

premiss_A ("P", "S")
|> anglicize_premiss 

!!"P(x)"
|> anglicize_premiss 

premiss_A ("M", "P")
|> fun x -> mk_and x (premiss_A ("S", "M"))
|> fun x -> mk_imp x (premiss_A ("S", "P"))
|> anglicize_syllogism

let all_valid_syllogisms = 
    List.filter aedecide all_possible_syllogisms;;

List.length all_valid_syllogisms;;

List.map anglicize_syllogism all_valid_syllogisms;;

// Darapti

premiss_A ("M", "P")
|> fun x -> mk_and x (premiss_A ("M", "S"))
|> fun x -> mk_imp x (premiss_I ("S", "P"))
|> anglicize_syllogism

!!"P(x)"
|> anglicize_syllogism 

(premiss_A ("M", "P"), premiss_A ("S", "M"))
|> And

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