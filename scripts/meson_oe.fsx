#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Time
open Lib.List
open Formulas
open Prop
open Fol
open Skolem
open Prolog
open Meson

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

type transformation = 
    | Trivial
    | NonTrivial
    | Cyclic

let rec check env x t =
    match t with
    | Var y when y = x -> Trivial
    | Var y when not (defined env y) -> NonTrivial
    | Var y -> check env x (apply env y) 
    | Fn(f,args) -> 
        match 
            args
            |> List.exists (fun a -> 
                check env x a = Trivial
            )
        with 
        | true -> Cyclic
        | false -> NonTrivial

let rec unify_oe
    (env : func<string, term>) eqs =
    match eqs with
    | [] -> Ok env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify_oe env (List.zip fargs gargs @ oth)
        else
            Error "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify_oe env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match check env x t with
            // if there is a malicious cycle
            | Cyclic -> Error "Cyclic"
            // If there is a benign cycle in env, env is unchanged
            | Trivial -> unify_oe env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | _ -> unify_oe ((x |-> t) env) oth

let rec unify_literals_oe env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify_oe env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals_oe env (p, q)
    | False, False -> Ok env
    | _ -> Error "Impossible unification"

let rec tryfind_oe f l =
    match l with
    | [] ->
        Error "tryfind"
    | h :: t ->
        match f h with
        | Ok res -> Ok res
        | Error _ -> tryfind_oe f t

let rec mexpand_basic_oe rules ancestors g cont (env, n, k) =
    // : Result<func<string,term> * int * int, string> =
    if n < 0 then Error "Too deep"
    else
        match 
            // ancestor unification
            ancestors
            |> tryfind_oe (fun a -> 
                unify_literals_oe env (g, negate a)
            )    
        with
        | Ok env ->  cont (env, n, k)
        | _ ->
            // Prolog-style extension
            rules
            |> tryfind_oe (fun rule ->
                let (asm, c) ,k' = renamerule k rule

                let cont = 
                    (asm, cont) 
                    ||> List.foldBack (fun subgoal cont -> 
                        mexpand_basic_oe rules (g::ancestors) subgoal cont
                    ) 

                match unify_literals_oe env (g, c) with
                | Ok env -> cont (env,n - List.length asm, k')
                | Error err -> Error err
            )

// mexpand_basic_oe
//    [
//        ([], !!"P(x)"); 
//        ([!!"P(x)"], False);
//    ]
//    [] False (fun x -> Ok x) (undefined,1,0)

// [
//     ([], !!"P(x)"); 
//     ([!!"P(x)"], False);
// ]
// |> tryfind_oe (fun rule ->
//     let (asm, c) ,k' = renamerule 0 rule
//     unify_literals_oe undefined (False, c)
// )

// unify_literals_oe undefined (False, False)

let rec deepen_oe f n =
    printf "Searching with depth limit "
    printfn "%d" n
    match f n with
    | Ok x -> x
    | Error _ -> deepen_oe f (n + 1)

[
    ([!!"A"],!!"B");
    ([!!"C"],!!"B");
    ([],!!"D");
    ([!!"D"],!!"P(0)")] 
|> fun rules -> 
    deepen_oe (fun n -> 
        mexpand_basic_oe rules [] (!!"P(x)") (fun x -> Ok x) (undefined, n, 0)
    ) 0
    

let puremeson_basic_oe fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen_oe (fun n ->
        match 
            mexpand_basic_oe rules [] False (fun x -> Ok x) (undefined, n, 0) 
        with
        | Ok _ -> Ok n
        | Error err -> Error err
    ) 0
        // |> ignore
        // Ok n) 0

let meson_basic_oe fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic_oe << list_conj) (simpdnf fm1)

let davis_putnam_example = 
    !! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"

davis_putnam_example
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
    deepen_oe (fun n -> 
        mexpand_basic_oe rules [] (!!"false") (fun x -> Ok x) (undefined, n, 0)
    ) 0

// time meson_basic_oe davis_putnam_example // CPU time (user): 0.008062
// time meson_basic davis_putnam_example // CPU time (user): 0.021944
// time meson_basic_oe Pelletier.p32 // CPU time (user): 0.001617
// time meson_basic Pelletier.p32 // CPU time (user): 0.022436

let streamroller = parse "((forall x. P1(x) ==> P0(x)) /\\ (exists x. P1(x))) /\\ ((forall x. P2(x) ==> P0(x)) /\\ (exists x. P2(x))) /\\ ((forall x. P3(x) ==> P0(x)) /\\ (exists x. P3(x))) /\\ ((forall x. P4(x) ==> P0(x)) /\\ (exists x. P4(x))) /\\ ((forall x. P5(x) ==> P0(x)) /\\ (exists x. P5(x))) /\\ ((exists x. Q1(x)) /\\ (forall x. Q1(x) ==> Q0(x))) /\\ (forall x. P0(x) ==> (forall y. Q0(y) ==> R(x,y)) \\/ ((forall y. P0(y) /\\ S0(y,x) /\\ (exists z. Q0(z) /\\ R(y,z)) ==> R(x,y)))) /\\ (forall x y. P3(y) /\\ (P5(x) \\/ P4(x)) ==> S0(x,y)) /\\ (forall x y. P3(x) /\\ P2(y) ==> S0(x,y)) /\\ (forall x y. P2(x) /\\ P1(y) ==> S0(x,y)) /\\ (forall x y. P1(x) /\\ (P2(y) \\/ Q1(y)) ==> ~(R(x,y))) /\\ (forall x y. P3(x) /\\ P4(y) ==> R(x,y)) /\\ (forall x y. P3(x) /\\ P5(y) ==> ~(R(x,y))) /\\ (forall x. (P4(x) \\/ P5(x)) ==> exists y. Q0(y) /\\ R(x,y)) ==> exists x y. P0(x) /\\ P0(y) /\\ exists z. Q1(z) /\\ R(y,z) /\\ R(x,y)";;

// meson_basic_oe streamroller

let rec equal_oe env fm1 fm2 =
    match unify_literals_oe env (fm1, fm2) with
    | Ok env' -> env = env'
    | _ -> false

let expand2_oe expfn (goals1:formula<fol> list) n1 goals2 n2 n3 cont env k =
        (env, n1, k)
        |> expfn goals1 (fun (e1, r1, (k1:int)) ->
            (e1, n2 + r1, k1)
            |> expfn goals2 (fun (e2, r2, k2) ->
                if n2 + r1 <= n3 + r2 then 
                    Error "pair"
                else 
                    cont(e2, r2, k2)
            )
        )

let rec mexpand_oe rules ancestors g cont (env, n, k) =

    // let rec mexpands rules ancestors gs cont (env, n, k) =
    //     if n < 0 then 
    //         Error "Too deep" 
    //     else
    //         let m = List.length gs
    //         if m <= 1 then 
    //             List.foldBack (mexpand_oe rules ancestors) gs cont (env, n, k) 
    //         else
    //             let n1 = n / 2
    //             let n2 = n - n1
    //             let goals1,goals2 = chop_list (m / 2) gs
    //             let expfn = expand2_oe (mexpands rules ancestors)
    //             match expfn goals1 n1 goals2 n2 -1 cont env k with
    //             | Ok x -> Ok x
    //             | Error _ -> expfn goals2 n1 goals1 n2 n1 cont env k

    if n < 0 then
        Error "Too deep"
    elif List.exists (equal env g) ancestors then
        Error "repetition"
    else
        match 
            ancestors
            |> tryfind_oe (fun a -> 
                unify_literals_oe env (g, negate a)
            )     
        with 
        | Ok env -> cont (env, n, k)
        | _ -> 
            rules
            |> tryfind_oe (fun r ->
                let (asm, c), k' = renamerule k r

                match unify_literals_oe env (g, c) with
                | Ok env -> 
                    mexpands rules (g :: ancestors) asm cont 
                        (env,n - List.length asm, k')
                | Error err -> Error err
            )
and mexpands rules ancestors gs cont (env,n,k) =
  if n < 0 then Error "Too deep" else
  let m = List.length gs in
  if m <= 1 then List.foldBack (mexpand_oe rules ancestors) gs cont (env,n,k) else
  let n1 = n / 2 in
  let n2 = n - n1 in
  let goals1,goals2 = chop_list (m / 2) gs in
  let expfn = expand2_oe (mexpands rules ancestors) in
  match expfn goals1 n1 goals2 n2 (-1) cont env k with
  | Error _ -> expfn goals2 n1 goals1 n2 n1 cont env k
  | result ->  result

let puremeson_oe fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen_oe (fun n ->
        match 
            mexpand_oe rules [] False (fun x -> Ok x) (undefined, n, 0) 
        with
        | Ok _ -> Ok n
        | Error err -> Error err
    ) 0

let meson_oe fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_oe << list_conj) (simpdnf fm1)

// time meson_oe davis_putnam_example // CPU time (user): 0.005575
// time meson_oe Pelletier.p32 // CPU time (user): 0.004691
// meson_oe streamroller