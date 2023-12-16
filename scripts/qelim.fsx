#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Formulas
open Fol
open Qelim
open Prop
open Skolem
open Decidable

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

lfn_dlo !!"~(s < t)"

lfn_dlo !!"~(s = t)"

lfn_dlo !!"~(s = t) /\ ~(s < t)"

!!"~(s = t) /\ ~(s < t)"
|> cnnf lfn_dlo

afn_dlo [] !!"s <= t"

quelim_dlo !!"exists z. x < z /\ z < y"

let afn = afn_dlo
let nfn = (dnf << cnnf lfn_dlo)
let qfn = (fun v -> dlobasic)

let rec qelift vars fm =
    match fm with
    | Atom (R (_,_)) ->
        afn vars fm
    | Not p ->
        Not (qelift vars p)
    | And (p, q) ->
        And (qelift vars p, qelift vars q)
    | Or (p, q) ->
        Or (qelift vars p, qelift vars q)
    | Imp (p, q) ->
        Imp (qelift vars p, qelift vars q)
    | Iff (p, q) ->
        Iff (qelift vars p, qelift vars q)
    | Forall (x, p) ->
        Not (qelift vars (Exists (x, Not p)))
    | Exists (x, p) ->
            let djs = disjuncts (nfn (qelift (x :: vars) p))
            list_disj (List.map (qelim (qfn vars) x) djs)
    | _ -> fm

// simplify (qelift (fv fm) (miniscope fm))

let fm = !!"exists z. x < z /\ z < y"

let vars = ["x"; "y"]

fm
|> miniscope
|> fun fm' -> 
    match fm' with
    | Exists (x, p) -> 
        p
        |> qelift (x :: (fv fm))
        |> nfn
        |> disjuncts
        |> List.map (qelim (qfn vars) x)
    | _ -> failwith "non importa"
    
!!"x < z /\ z < y"
|> dlobasic

dlobasic !!"exists z. x < z /\ z < y"


!!"exists z. x < z /\ z < y"
|> lift_qelim afn_dlo (dnf << cnnf lfn_dlo) (fun v -> dlobasic)