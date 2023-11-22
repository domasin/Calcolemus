#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Search
open Lib.Time
open Lib.List
open Formulas
open Prop
open Fol
open Skolem
open Prolog
open Meson
open Tableaux
open System.IO

let rec mexpand file rules ancestors g cont (env, n, k) 
    : func<string,term> * int * int =
    fprintf file "%s" (g |> sprint_fol_formula)

    let rec mexpands rules ancestors gs cont (env, n, k) =
        if n < 0 then failwith "Too deep" 
        else
            let m = List.length gs
            if m <= 1 then List.foldBack (mexpand file rules ancestors) gs cont   (env, n, k) 
            else
                let n1 = n / 2
                let n2 = n - n1
                let goals1,goals2 = chop_list (m / 2) gs
                let expfn = expand2 (mexpands rules ancestors)
                try expfn goals1 n1 goals2 n2 -1 cont env k
                with _ -> expfn goals2 n1 goals1 n2 n1 cont env k

    if n < 0 then
        failwith "Too deep"
    elif List.exists (equal env g) ancestors then
        failwith "repetition"
    else
        try 
            ancestors
            |> tryfind (fun a -> 
                let af = unify_literals env (g, negate a)
                fprintf file "ancestorFound: %s\n" (a |> sprint_fol_formula)
                cont (af, n, k)
            )
        with Failure _ ->
            rules
            |> tryfind (fun r ->
                let (asm, c), k' = renamerule k r
                
                (unify_literals env (g, c), n - List.length asm, k')
                |> fun env -> 
                    let (assumptions,concl) = r
                    fprintf file " -> (%A,%s)\n" 
                        (assumptions |> List.map sprint_fol_formula)
                        (concl |> sprint_fol_formula)
                    env
                |> mexpands rules (g :: ancestors) asm cont 
            )

let streamroller = parse "((forall x. P1(x) ==> P0(x)) /\\ (exists x. P1(x))) /\\ ((forall x. P2(x) ==> P0(x)) /\\ (exists x. P2(x))) /\\ ((forall x. P3(x) ==> P0(x)) /\\ (exists x. P3(x))) /\\ ((forall x. P4(x) ==> P0(x)) /\\ (exists x. P4(x))) /\\ ((forall x. P5(x) ==> P0(x)) /\\ (exists x. P5(x))) /\\ ((exists x. Q1(x)) /\\ (forall x. Q1(x) ==> Q0(x))) /\\ (forall x. P0(x) ==> (forall y. Q0(y) ==> R(x,y)) \\/ ((forall y. P0(y) /\\ S0(y,x) /\\ (exists z. Q0(z) /\\ R(y,z)) ==> R(x,y)))) /\\ (forall x y. P3(y) /\\ (P5(x) \\/ P4(x)) ==> S0(x,y)) /\\ (forall x y. P3(x) /\\ P2(y) ==> S0(x,y)) /\\ (forall x y. P2(x) /\\ P1(y) ==> S0(x,y)) /\\ (forall x y. P1(x) /\\ (P2(y) \\/ Q1(y)) ==> ~(R(x,y))) /\\ (forall x y. P3(x) /\\ P4(y) ==> R(x,y)) /\\ (forall x y. P3(x) /\\ P5(y) ==> ~(R(x,y))) /\\ (forall x. (P4(x) \\/ P5(x)) ==> exists y. Q0(y) /\\ R(x,y)) ==> exists x y. P0(x) /\\ P0(y) /\\ exists z. Q1(z) /\\ R(y,z) /\\ R(x,y)";;

let davis_putnam_example = 
    !! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"

let los = 
 !! @"(forall x y z. P(x,y) ==> P(y,z) ==> P(x,z)) /\
   (forall x y z. Q(x,y) ==> Q(y,z) ==> Q(x,z)) /\
   (forall x y. Q(x,y) ==> Q(y,x)) /\
   (forall x y. P(x,y) \/ Q(x,y))
   ==> (forall x y. P(x,y)) \/ (forall x y. Q(x,y))";;

let file = File.CreateText("./compare/los_we.txt")

#time
// streamroller
// davis_putnam_example
// Pelletier.p32
los
|> generalize
|> Not
|> askolemize
|> simpdnf
|> List.head 
|> list_conj
|> pnf
|> specialize
|> simpcnf
|> fun cls -> List.foldBack ((@) << contrapositives) cls []
|> fun rules -> 
    mexpand file rules [] (!!"false") id (undefined, 9, 0)
|> fun (env,n,k) -> 
    env |> graph |> List.map (fun (s,t) -> (s, t |> sprint_term), n, k)
#time

file.Close();;