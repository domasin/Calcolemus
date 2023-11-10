#load "meson_new.fsx"

open Meson_new

open Calculemus
open Calculemus.Lib.Fpf
open Calculemus.Fol
open Calculemus.Lib.Search
open Calculemus.Tableaux
open Calculemus.Prop
open Calculemus.Prolog
open Calculemus.Skolem
open Calculemus.Meson
open Calculemus.Formulas

let rec mexpand_basic_o rules ancestors g cont (env, n, k) 
    : func<string,term> * int * int =
    // printfn "mexpand_basic_o"
    // printfn "%s %s %s (%s, %i, %i)" (rules |> sprint_rules) 
    //     (ancestors |> sprint_clause) (g |> sprint_fol_formula) (env |> sprint_inst) n k

    if n < 0 then failwith "Too deep"
    else
        try 
            // ancestor unification
            ancestors
            |> tryfind (fun a -> 
                cont (unify_literals env (g, negate a), n, k)
            )    
        with _ ->
            // Prolog-style extension
            rules
            |> tryfind (fun rule ->
                let (asm, c) ,k' = renamerule k rule

                (unify_literals env (g, c), n - List.length asm, k')
                |> List.foldBack 
                    (mexpand_basic_o rules (g :: ancestors)) asm cont
            )

mexpand_basic_o
    [
        ([!!"A"], !!"B")
        ([!!"C"], !!"B")
        ([], !!"C")
    ] [] !!"B" id (undefined, 1, 0)

let puremeson_basic_o fm =
    let cls = simpcnf (specialize (pnf fm))
    let rules = List.foldBack ((@) << contrapositives) cls []
    deepen (fun n ->
        mexpand_basic_o rules [] False id (undefined, n, 0)
        |> ignore
        n) 0

let meson_basic_o fm =
    let fm1 = askolemize (Not (generalize fm))
    List.map (puremeson_basic_o << list_conj) (simpdnf fm1)