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

type unification = 
    | Trivial
    | NonTrivial
    | Cyclic
    | Impossible

let rec istriv3 env x t =
    match t with
    | Var y when y = x -> Trivial
    | Var y when not (defined env y) -> NonTrivial
    | Var y -> istriv3 env x (apply env y) 
    | Fn(f,args) -> 
        match 
            args
            |> List.exists (fun a -> 
                istriv3 env x a = Trivial
            )
        with 
        | true -> Cyclic
        | false -> NonTrivial

// istriv3 undefined "x" !!!"x"
// istriv3 (("x" |-> !!!"0")undefined) "x" !!!"y"

// istriv3 undefined "x" !!!"y"
// istriv3 undefined "y" !!!"f(y)"
// istriv3 (("y" |-> !!!"x")undefined) "x" !!!"f(y)"

let rec unify3 success failure
    (env : func<string, term>) eqs =
    match eqs with
    | [] -> success env
    | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
        if f = g && List.length fargs = List.length gargs then
            unify3 success failure env (List.zip fargs gargs @ oth)
        else
            failure Impossible
    | (Var x, t) :: oth
    | (t, Var x) :: oth ->
        // If there is already a definition (say x |-> s) in env, then 
        // the pair is expanded into (s, t) and the recursion proceeds.
        if defined env x then
            unify3 success failure env ((apply env x,t) :: oth)
        // Otherwise we know that condition x |-> s is not in env,
        // so x |-> t is a candidate for incorporation into env.
        else
            match istriv3 env x t with
            // if there is a malicious cycle
            | Cyclic -> failure Cyclic
            // If there is a benign cycle in env, env is unchanged
            | Trivial -> unify3 success failure env oth
            // Otherwise, x |-> t is incorporated into env for the 
            // next recursive call.
            | _ -> unify3 success failure ((x |-> t) env) oth

let unifiableAfter = unify3 (fun _ -> true) (fun _ -> false) 

unifiableAfter undefined [!!!"x", !!!"0"]
unifiableAfter (("x" |-> !!!"1")undefined) [!!!"x", !!!"0"]

unify3 id (fun _ -> failwith "pippo") undefined [!!!"y", !!!"f(y)"]



let rec unify_literals3 success failure env (p, q) =
    match (p, q) with
    // To unify atomic formulas, predicates are treated 
    // as if they were functions.
    | Atom (R (p1, a1)), Atom (R (p2, a2)) ->
        unify3 success failure env [Fn (p1, a1), Fn (p2, a2)]
    | Not p, Not q ->
        unify_literals3 success failure  env (p, q)
    | False, False -> success env
    | _ -> failure Impossible

unify_literals3 id (fun x -> failwith "impossible") undefined (!!"P(g(x))",!!"P(f(y))")

// [
//     (!!!"y", !!!"f(y)")
//     (!!!"x", !!!"0")
//     (!!!"x", !!!"1")
// ]
// |> List.fold (fun acc (p,q) -> 
//     unify_literals3 (fun env -> acc@env) 
// ) []

type successFailure<'a,'b,'c,'d> = ('a -> 'b) -> ('c -> 'b) -> 'd -> 'b

let isEven success failure n = 
    if (n % 2 = 0)
    then n |> success
    else n |> failure

let rec tryfindIsEven success failure l =
    match l with
    | [] -> failure "tryfind"
    | h :: t -> 
        isEven id (fun x -> tryfindIsEven success failure t) h

let rec tryUnifyLit success failure env l =
    match l with
    | [] -> failure "tryfind"
    | h :: t -> 
        unify_literals3 success (fun x -> tryUnifyLit success failure env t) env h

[
    (!!"P(y)", !!"P(f(y))")
    (!!"P(x)", !!"P(0)")
    (!!"P(x)", !!"P(1)")
]
|> tryUnifyLit id (fun x -> failwith "pippo") undefined

[1;2;3] 
|> tryfindIsEven id (fun x -> failwith "pippo")

[1;3] 
|> tryfindIsEven id (fun x -> failwith "pippo")

let rec backchain2 success failure rules n k env goals =
    match goals with
    | [] -> env
    | g :: gs ->
        if n = 0 then 
            failure "Too deep" 
        else
            let rec tryUnifyLit success failure env l =
                match l with
                | [] -> failure "tryfind"
                | rule :: t -> 
                    let (a, c), k' = renamerule k rule
                    backchain2 success failure rules (n - 1) k' 
                        (unify_literals3 success (fun x -> tryUnifyLit success failure env t) env (c, g)) 
                        (a@gs)
            rules
            |> tryUnifyLit success failure env

!!>["S(x) <= S(S(x))"] 
|> backchain2 id (fun x -> failwith "pippo")
    [
        ([], !!"0 <= x"); 
        ([!!"x <= y"], !!"S(x) <= S(y)")
    ] 2 0 undefined
|> graph

let rec mexpand_basic3 success failure rules ancestors g 
    cont
    (env, n, k) =
    
    if n < 0 then 
        failure "Too deep"
    else
        // ancestor unification
        let rec unifyAnc success failure env l =
            match l with
            | [] -> failure "unifyAnc"
            | a :: t -> 
                cont (unify_literals3 
                    success
                    (fun x -> unifyAnc success failure env t) env (g, negate a), n, k)
        try
            ancestors
            |> unifyAnc success (fun err -> failwith err) env  
        with _ -> 
            // Prolog-style extension
            let rec tryRules success failure env l =
                match l with
                | [] -> failure "tryAncestors"
                | rule :: t -> 
                    let (asm, c) ,k' = renamerule k rule

                    (unify_literals3 
                        success
                        (fun x -> tryRules success failure env t)  
                        env (g, c), n - List.length asm, k')
                    |> List.foldBack (fun subGoal -> 
                        
                        mexpand_basic3 
                            success 
                            failure rules (g :: ancestors)
                            subGoal
                            ) asm cont

            rules
            |> tryRules success failure env  

mexpand_basic3 id (fun x -> failwith "pippo")
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ]
   [] False (fun (x,_,_) -> x) (undefined,1,0)