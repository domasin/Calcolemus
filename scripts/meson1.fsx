#load "meson2.fsx"

open Meson2

open Calculemus
open Lib.Fpf
open Meson
open Formulas
open Fol
open Unif
open Calculemus.Prop
open Calculemus.Prolog
open Calculemus.Skolem

let rec mexpand_basic rules ancestors g cont (env, n, k) 
    : func<string,term> * int * int =
    printfn "mexpand_basic2 %A %A %A %i" 
        (rules |> rulesToString) 
        (ancestors |> sprint_fol_formula_list) 
        (g |> sprint_fol_formula)
        n
    if n < 0 then failwith "Too deep"
    else
        try 
            // ancestor unification
            ancestors
            |> Lib.Search.tryfind (fun a -> 
                cont (Tableaux.unify_literals env (g, negate a), n, k)
            )    
        with _ ->
            // Prolog-style extension
            rules
            |> Lib.Search.tryfind (fun rule ->
                let (asm, c) ,k' = renamerule k rule
                (Tableaux.unify_literals env (g, c), n - List.length asm, k')
                |> List.foldBack 
                    (mexpand_basic rules (g :: ancestors)) asm cont
            )

backchain
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ] 1 0 (undefined) [False]

mexpand_basic
   [
       ([], !!"P(x)"); 
       ([!!"P(x)"], False);
   ]
   [] False (fun x -> x) (undefined,1,0)

let puremeson_basic fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    Tableaux.deepen (fun n ->
        mexpand_basic rules [] False id (undefined, n, 0)
        |> ignore
        n) 0

!!"P(x) /\ ~P(x)"
 |> puremeson_basic