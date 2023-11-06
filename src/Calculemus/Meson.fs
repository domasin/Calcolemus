// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus

open Lib.List
open Lib.Set
open Lib.Fpf
open Lib.Search

open Formulas
open Prop
open Fol
open Skolem
open Tableaux
open Prolog

module Meson = 

    // ====================================================================== //
    // Model elimination procedure (MESON version, based on Stickel's PTTP).  //
    // ====================================================================== //

    // ---------------------------------------------------------------------- //
    // Generation of contrapositives.                                         //
    // ---------------------------------------------------------------------- //

    let contrapositives cls =
        let baseClause = List.map (fun c -> List.map negate (subtract cls [c]), c)  cls
        if List.forall negative cls then
            (List.map negate cls, False) :: baseClause 
        else baseClause

    // ---------------------------------------------------------------------- //
    // The core of MESON: ancestor unification or Prolog-style extension.     //
    // ---------------------------------------------------------------------- //

    let rec mexpand_basic rules ancestors g cont (env, n, k) 
        : func<string,term> * int * int =
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
                        (mexpand_basic rules (g :: ancestors)) asm cont
                )

    // ---------------------------------------------------------------------- //
    // Full MESON procedure.                                                  //
    // ---------------------------------------------------------------------- //

    let puremeson_basic fm =
        let cls = simpcnf (specialize (pnf fm))
        let rules = List.foldBack ((@) << contrapositives) cls []
        deepen (fun n ->
            mexpand_basic rules [] False id (undefined, n, 0)
            |> ignore
            n) 0

    let meson_basic fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (puremeson_basic << list_conj) (simpdnf fm1)

    // ---------------------------------------------------------------------- //
    // With repetition checking and divide-and-conquer search.                //
    // ---------------------------------------------------------------------- //

    let rec equal env fm1 fm2 =
        try unify_literals env (fm1, fm2) = env
        with _ -> false

    let expand2 expfn (goals1:formula<fol> list) n1 goals2 n2 n3 cont env k 
        : func<string,term> * int * int =

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

    let rec mexpand rules ancestors g cont (env, n, k) 
        : func<string,term> * int * int =

        let rec mexpands rules ancestors gs cont (env, n, k) =
            if n < 0 then failwith "Too deep" 
            else
                let m = List.length gs
                if m <= 1 then List.foldBack (mexpand rules ancestors) gs cont   (env, n, k) 
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
                    cont (unify_literals env (g, negate a), n, k)
                )     
            with Failure _ ->
                rules
                |> tryfind (fun r ->
                    let (asm, c), k' = renamerule k r
                    
                    (unify_literals env (g, c), n - List.length asm, k')
                    |> mexpands rules (g :: ancestors) asm cont 
                )

    let puremeson fm =   
        let cls = simpcnf (specialize (pnf fm))
        let rules = List.foldBack ((@) << contrapositives) cls []
        deepen (fun n ->
            mexpand rules [] False id (undefined, n, 0)
            |> ignore
            n) 0

    let meson fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (puremeson << list_conj) (simpdnf fm1)
