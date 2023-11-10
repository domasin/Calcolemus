#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Calculemus.Lib.Search
open Calculemus.Tableaux
open Calculemus.Prop
open Calculemus.Lib.Fpf
open Calculemus.Formulas
open Calculemus.Fol
open Calculemus.Prolog
open Calculemus.Skolem
open Calculemus.Meson

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

let rec unify_sf success failure
    (env : func<string, term>) eqs =
    match eqs with
    | [] -> success env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify_sf success failure env (List.zip fargs gargs @ oth)
        else
            failure "impossible unification"
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify_sf success failure env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match check env x t with
            // if there is a malicious cycle
            | Cyclic -> failure "Cyclic"
            // If there is a benign cycle in env, env is unchanged
            | Trivial -> unify_sf success failure env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | _ -> unify_sf success failure ((x |-> t) env) oth

let rec unify_literals_sf success failure env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify_sf success failure env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals_sf success failure  env (p, q)
    | False, False -> success env
    | _ -> failure "Impossible unification"

let sprint_clause cls = 
    cls 
    |> List.map (fun fm -> 
        sprintf "%s" (sprint_fol_formula fm)
    )
    |> String.concat ", "
    |> fun s -> sprintf "[%s]" s

let sprint_ancestors ancs = 
    match ancs with
    | [] -> sprintf "[]"
    | _ -> 
        ancs 
        |> List.map (fun fm -> 
            sprintf "   %s" (sprint_fol_formula fm)
        )
        |> String.concat ",\n"
        |> fun s -> sprintf "[\n%s\n]" s

let sprint_rule (asm,c) = 
    asm 
    |> sprint_clause
    |> fun asm -> 
        sprintf "(%s, %s)" asm (sprint_fol_formula c)

let sprint_rules xs = 
        xs 
        |> List.map (fun fm -> 
            sprintf "   %s" (sprint_rule fm)
        )
        |> String.concat ";\n"
        |> fun s -> sprintf "[\n%s\n]" s

let sprint_inst env = 
    env 
    |> graph
    |> List.map (fun (x,t) -> 
        sprintf "%s |-> %s" x (t |> sprint_term)
    )
    |> String.concat "; "
    |> fun s -> sprintf "[%s]" s

let rec mexpand_basic_n rules ancestors g cont res =
//     printfn "mexpand_basic_n"
//     printfn "%s\n%s %s (%s, %i, %i)" (rules |> sprint_rules) 
//         (ancestors |> sprint_ancestors) (g |> sprint_fol_formula) (env |> sprint_inst) n k
    match res with
    | None -> None
    | Some (env, n, k) -> 
        if n < 0 then 
            None
        else
            match
                // ancestor unification
                ancestors 
                |> List.tryPick (fun a -> 
                    unify_literals_sf Some (fun _ -> None) env (g, negate a)
                )
            with
                | Some env -> cont (Some (env, n, k))
                | None -> 
                    // Prolog-style extension
                    rules
                    |> List.tryPick (fun rule ->
                        let (asm, c) ,k' = renamerule k rule

                        // on found check also the subgoals
                        let cont = 
                            (asm, cont) 
                            ||> List.foldBack (fun subgoal cont -> 
                                mexpand_basic_n rules (g::ancestors) subgoal cont
                            ) 

                        let success env = 
                            // printfn "cont"
                            cont (Some (env, n - List.length asm, k'))
                            // |> Some // pick
                        
                        let failure env = 
                            None // pick
                            // failwith "cont failure" // tryfind
                        
                        unify_literals_sf success failure env (g, c)
                    )

let rec deepen_opt f n =
    printf "Searching with depth limit "
    printfn "%d" n
    match f n with
    | Some x -> x
    | None -> deepen_opt f (n + 1)
    
let puremeson_basic_n fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen_opt (fun n ->
        match mexpand_basic_n rules [] False id (Some (undefined, n, 0)) with
        | Some _ -> Some n
        | None -> None
    ) 0

let meson_basic_n fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic_n << list_conj) (simpdnf fm1)

// mexpand_basic_n 
//     [
//         ([!!"A"], !!"B")
//         ([!!"C"], !!"B")
//         ([], !!"C")
//     ] [] !!"B" id (Some (undefined, 1, 0))

let rec myTest rules g cont xs = 
    rules
    |> List.tryPick (fun (asm,g') -> 
        if g' = g then 
            if asm = [] then
                Some g
            else
                xs
                |> (asm 
                    |> List.foldBack (fun a -> 
                        myTest rules a cont
                    )
                )
        else
            None
    )
    
let rules = [(["A"],"B");(["C"],"B");([],"D");([],"C");(["D"],"B")] 

myTest rules "B" id None

let davis_putnam_example = 
    !! @"exists x. exists y. forall z.
    (F(x,y) ==> (F(y,z) /\ F(z,z))) /\
    ((F(x,y) /\ G(x,y)) ==> (G(x,z) /\ G(z,z)))"

// meson_basic_o davis_putnam_example
// meson_basic_n davis_putnam_example

// meson_basic_o Pelletier.p32
// meson_basic_n Pelletier.p32

// [1;2;3] |> List.pick (fun n -> if n % 2 = 0 then Some (string n) else None)
// [1;2;3] |> tryfind (fun n -> if n % 2 = 0 then string n else invalidOp "")

let steamroller = 
    !! @"((forall x. P1(x) ==> P0(x)) /\ (exists x. P1(x))) /\
        ((forall x. P2(x) ==> P0(x)) /\ (exists x. P2(x))) /\
        ((forall x. P3(x) ==> P0(x)) /\ (exists x. P3(x))) /\
        ((forall x. P4(x) ==> P0(x)) /\ (exists x. P4(x))) /\
        ((forall x. P5(x) ==> P0(x)) /\ (exists x. P5(x))) /\
        ((exists x. Q1(x)) /\ (forall x. Q1(x) ==> Q0(x))) /\
        (forall x. P0(x)
                    ==> (forall y. Q0(y) ==> R(x,y)) \/
                    ((forall y. P0(y) /\ S0(y,x) /\
                        (exists z. Q0(z) /\ R(y,z))
                        ==> R(x,y)))) /\
        (forall x y. P3(y) /\ (P5(x) \/ P4(x)) ==> S0(x,y)) /\
        (forall x y. P3(x) /\ P2(y) ==> S0(x,y)) /\
        (forall x y. P2(x) /\ P1(y) ==> S0(x,y)) /\
        (forall x y. P1(x) /\ (P2(y) \/ Q1(y)) ==> ~(R(x,y))) /\
        (forall x y. P3(x) /\ P4(y) ==> R(x,y)) /\
        (forall x y. P3(x) /\ P5(y) ==> ~(R(x,y))) /\
        (forall x. (P4(x) \/ P5(x)) ==> exists y. Q0(y) /\ R(x,y))
        ==> exists x y. P0(x) /\ P0(y) /\
        exists z. Q1(z) /\ R(y,z) /\ R(x,y)"

// meson_basic_n steamroller

let rec equal env fm1 fm2 =
    unify_literals_sf (fun x -> x = env) (fun _ -> false) env (fm1, fm2)

let expand2 expfn goals1 n1 goals2 n2 n3 cont env k =
    (env, n1, k)
    |> expfn goals1 (fun ((e1:func<string,term>), r1, (k1:int)) ->
        (e1, n2 + r1, k1)
        |> expfn goals2 (fun (e2, r2, k2) ->
            if n2 + r1 <= n3 + r2 then 
                failwith "pair"
            else 
                cont(e2, r2, k2)
        )
    )

let rec mexpand rules ancestors g cont (env, n, k) =
    if n < 0 then
        failwith "Too deep"
    elif List.exists (equal env g) ancestors then
        failwith "repetition"
    else
        try 
            ancestors
            |> tryfind (fun a -> 
                cont (unify_literals env (g, negate a), n, k)
            )     
        with Failure _ ->
            rules
            |> tryfind (fun r ->
                let (asm, c), k' = renamerule k r
                
                (unify_literals env (g, c), n - List.length asm, k')
                |> mexpands rules (g :: ancestors) asm cont 
            )
and mexpands rules ancestors gs cont (env, n, k) =
    if n < 0 then 
        failwith "Too deep" 
    else
        let m = List.length gs
        if m <= 1 then 
            List.foldBack (mexpand rules ancestors) gs cont (env, n, k) 
        else
            let n1 = n / 2
            let n2 = n - n1
            let goals1,goals2 = Lib.List.chop_list (m / 2) gs
            let expfn = expand2 (mexpands rules ancestors)
            try expfn goals1 n1 goals2 n2 -1 cont env k
            with _ -> expfn goals2 n1 goals1 n2 n1 cont env k