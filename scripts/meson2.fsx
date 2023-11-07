#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Meson
open Formulas
open Fol
open Unif
open Calculemus.Prop
open Calculemus.Prolog
open Calculemus.Skolem

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let rec tryfind2 f l =
    match l with
    | [] -> Error "tryfind2"
    | h :: t ->
        match f h with
        | Error _ -> tryfind2 f t
        | Ok r -> Ok r

[1;2;3] 
 |> tryfind2 (fun n -> if n % 2 = 0 then Ok n else Error "f")

[1;2;3] 
|> tryfind2 (fun n -> if n > 3 then Ok n else Error "f")

let rec istriv2 env x t =
    match t with
    | Var y -> 
        Ok (y = x
            || defined env y
            && istriv2 env x (apply env y)
        )
    | Fn(f,args) ->
        match args |> List.exists (fun a -> 
                match istriv2 env x a with
                | Ok true -> true
                | _-> false
        ) with
        | false -> Ok false
        | _ -> Error "cyclic"

istriv2 undefined "x" !!!"x"
istriv2 undefined "x" !!!"y"
istriv2 undefined "y" !!!"f(y)"
istriv2 (("y" |-> !!!"x")undefined) "x" !!!"f(y)"

let rec unify2 env eqs =
    match eqs with
    | [] -> Ok env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify2 env (List.zip fargs gargs @ oth)
        else
            Error "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify2 env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match istriv2 env x t with
            | Error err -> Error err
            // If there is a benign cycle in env, env is unchanged; 
            // while if there is a malicious one, the unification 
            // will fail.
            | Ok true -> unify2 env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | Ok false -> unify2 ((x |-> t) env) oth

// unify2 undefined [!!!"x", !!!"0"]
// |> Result.toList
// |> List.map graph

// unify2 (("x" |-> !!!"y")undefined) [!!!"x", !!!"0"]
// |> Result.toList
// |> List.map graph

// unify2 undefined [!!!"y", !!!"f(y)"]

// unify2 (("x" |-> !!!"y")undefined) [!!!"y", !!!"f(x)"]

// unify2 undefined [!!!"0", !!!"1"]

let rec unify_literals2 env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify2 env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals2 env (p, q)
    | False, False -> Ok env
    | _ -> Error "Can't unify literals"

// unify_literals2 undefined (!!"P(x)",!!"P(f(y))")
// |> Result.toList
// |> List.map graph

// unify_literals2 undefined (!!"false",!!"false")
// |> Result.toList
// |> List.map graph

// unify_literals2 undefined (!!"P(y)",!!"P(f(y))")

// unify_literals2 undefined (!!"P(g(x))",!!"P(f(y))")

List.foldBack (fun v acc -> acc + v * v) [1..5] 0

let rec backchain2 rules n k env goals =
    match goals with
    | [] -> env
    | g :: gs ->
        if n = 0 then 
            Error "Too deep" 
        else
            rules
            |> tryfind2 (fun rule ->
                let (a, c), k' = renamerule k rule

                match env with
                | Ok env' -> 
                    backchain2 rules (n - 1) k' 
                        (unify_literals2 env' (c, g)) 
                        (a@gs)
                | Error err -> Error err
            )

!!>["S(x) <= S(S(x))"] 
|> backchain2
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 (Ok undefined)
|> Result.toList
|> List.map graph

backchain2
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ] 2 0 (Ok undefined) [False]


let sprint_fol_formula_list clauses = 
    sprintf "%A" (clauses |> List.map sprint_fol_formula)

let rulesToString rules = 
    rules
    |> List.map (fun (asm,c) -> 
        let asmStr = asm |> sprint_fol_formula_list
        let cStr = c |> sprint_fol_formula
        sprintf "(%A,%s)" asmStr cStr
    )

let rec mexpand_basic2 rules ancestors g cont (env, n, k) =
    printfn "mexpand_basic2 %A %A %A %i"
        (rules |> rulesToString) 
        (ancestors |> sprint_fol_formula_list) 
        (g |> sprint_fol_formula)
        n
    if n < 0 then 
        Error "Too deep"
    else
        match
            // ancestor unification
            ancestors
            |> tryfind2 (fun a -> 
                match env with
                | Error err -> 
                    printfn "ancestors: %A" err
                    Error err
                | Ok env' -> 
                    cont (unify_literals2 env' (g, negate a), n, k)
            ) with
        | Ok env' -> Ok env'
        | _ ->
            // Prolog-style extension
            match 
                rules
                |> tryfind2 (fun rule ->
                    let (asm, c) ,k' = renamerule k rule

                    let expansions = 
                        asm
                        |> List.foldBack (fun subgoal -> 
                            mexpand_basic2 rules (g :: ancestors) subgoal
                    )  

                    match env with
                    | Error err -> Error err
                    | Ok env' -> 
                        match unify_literals2 env' (g, c) with 
                        | Ok env'' -> 
                            expansions cont (Ok env'', n - List.length asm, k')
                        | Error err -> 
                            printfn "prologExtension 2: %A" err
                            Error err
                )
            with
            | Error err -> Error err
            | Ok x -> Ok x

mexpand_basic2
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ]
   [] False (fun x -> Ok x) (Ok undefined,1,0)

let rec deepen2 f n =
    printf "Searching with depth limit "
    printfn "%d" n

    match f n with
    | Ok r -> r
    | Error err -> 
        deepen2 f (n + 1)

let puremeson_basic2 fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen2 (fun n ->
        mexpand_basic2 rules [] False (fun x -> Ok x) ((Ok undefined), n, 0)
        |> ignore
        Ok n) 0
        
!!"P(x) /\ ~P(x)"
 |> puremeson_basic2

let meson_basic2 fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic2 << list_conj) (simpdnf fm1)

!! @"exists x. exists y. forall z.
     (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
     ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"
|> meson_basic2


// !! @"((forall x. P1(x) ==> P0(x)) /\ (exists x. P1(x))) /\
//      ((forall x. P2(x) ==> P0(x)) /\ (exists x. P2(x))) /\
//      ((forall x. P3(x) ==> P0(x)) /\ (exists x. P3(x))) /\
//      ((forall x. P4(x) ==> P0(x)) /\ (exists x. P4(x))) /\
//      ((forall x. P5(x) ==> P0(x)) /\ (exists x. P5(x))) /\
//      ((exists x. Q1(x)) /\ (forall x. Q1(x) ==> Q0(x))) /\
//      (forall x. P0(x)
//                 ==> (forall y. Q0(y) ==> R(x,y)) \/
//                 ((forall y. P0(y) /\ S0(y,x) /\
//                     (exists z. Q0(z) /\ R(y,z))
//                     ==> R(x,y)))) /\
//      (forall x y. P3(y) /\ (P5(x) \/ P4(x)) ==> S0(x,y)) /\
//      (forall x y. P3(x) /\ P2(y) ==> S0(x,y)) /\
//      (forall x y. P2(x) /\ P1(y) ==> S0(x,y)) /\
//      (forall x y. P1(x) /\ (P2(y) \/ Q1(y)) ==> ~(R(x,y))) /\
//      (forall x y. P3(x) /\ P4(y) ==> R(x,y)) /\
//      (forall x y. P3(x) /\ P5(y) ==> ~(R(x,y))) /\
//      (forall x. (P4(x) \/ P5(x)) ==> exists y. Q0(y) /\ R(x,y))
//      ==> exists x y. P0(x) /\ P0(y) /\
//      exists z. Q1(z) /\ R(y,z) /\ R(x,y)"
// |> meson_basic