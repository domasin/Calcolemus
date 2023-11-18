// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus.Lib
open Calculemus.Lib.Set

open Formulas
open Fol
open Skolem

module Equal = 

    // ====================================================================== //
    // First order logic with equality.                                       //
    // ====================================================================== //

    let is_eq fm = 
        match fm with
        | Atom (R ("=", _)) -> true 
        | _ -> false

    let mk_eq s t =
        Atom (R ("=", [s; t]))

    let dest_eq fm =
        match fm with
        | Atom (R ("=", [s;t])) ->
            s, t
        | _ ->
            failwith "dest_eq: not an equation"

    let lhs eq = fst <| dest_eq eq
    let rhs eq = snd <| dest_eq eq

    // ---------------------------------------------------------------------- //
    // The set of predicates in a formula.                                    //
    // ---------------------------------------------------------------------- //

    let rec predicates fm =
        atom_union (fun (R (p, a)) -> [p, List.length a]) fm

    // ---------------------------------------------------------------------- //
    // Code to generate equality axioms for functions.                        //
    // ---------------------------------------------------------------------- //

    let function_congruence (f, n) =
        if n = 0 then [] 
        else
            let argnames_x = List.map (fun n -> "x" + (string n)) [1..n]
            let argnames_y = List.map (fun n -> "y" + (string n)) [1..n]
            let args_x = List.map (fun x -> Var x) argnames_x
            let args_y = List.map (fun x -> Var x) argnames_y
            let ant = List.reduceBack mk_and (List.map2 mk_eq args_x args_y)
            let con = mk_eq (Fn (f, args_x)) (Fn (f, args_y))
            [List.foldBack mk_forall (argnames_x @ argnames_y) (Imp (ant, con))]

    // ---------------------------------------------------------------------- //
    // And for predicates.                                                    //
    // ---------------------------------------------------------------------- //

    let predicate_congruence (p, n) =
        if n = 0 then []
        else
            let argnames_x = List.map (fun n -> "x" + (string n)) [1..n]
            let argnames_y = List.map (fun n -> "y" + (string n)) [1..n]
            let args_x = List.map (fun x -> Var x) argnames_x
            let args_y = List.map (fun x -> Var x) argnames_y
            let ant = List.reduceBack mk_and (List.map2 mk_eq args_x args_y)
            let con = Imp (Atom (R (p, args_x)), Atom (R (p, args_y)))
            [List.foldBack mk_forall (argnames_x @ argnames_y) (Imp (ant, con))]

    // ---------------------------------------------------------------------- //
    // Hence implement logic with equality just by adding equality "axioms".  //
    // ---------------------------------------------------------------------- //

    let equivalence_axioms =
        [
            (parse "forall x. x = x"); 
            (parse "forall x y z. x = y /\ x = z ==> y = z")
        ]

    let equalitize fm =
        let allpreds = predicates fm
        if not (mem ("=", 2) allpreds) then fm
        else
            let preds = subtract allpreds ["=", 2]
            let funcs = functions fm
            let axioms = List.foldBack (union << function_congruence) funcs
                                (List.foldBack (union << predicate_congruence) preds
                                        equivalence_axioms)
            Imp (List.reduceBack mk_and axioms, fm)
