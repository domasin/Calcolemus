// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus.Lib.Set

open Formulas
open Prop
open Fol

module Clause = 

    let rec literals fm =
        match fm with
        | True -> [True]
        | False -> [False]
        | Atom a -> [fm]
        | Not p -> [fm]
        | And (p, q)
        | Or (p, q)
        | Imp (p, q)
        | Iff (p, q) ->
            (literals p)@(literals q)
        | Forall (x, p)
        | Exists (x, p) -> literals p

    let opposites lits =
        let pos, neg = List.partition positive lits
        intersect pos (image negate neg)

    let (!!>>) xs = 
        xs |> List.map (List.map (!!))

    let sprint_clauses clauses = 
        clauses
        |> List.map (List.map sprint_fol_formula)

    let rec djsToClauses fm = 
        match fm with
        | Or (p,q) -> (literals p)::(djsToClauses q)
        | fm -> [literals fm]

    let rec cjsToClauses fm = 
        match fm with
        | And (p,q) -> (literals p)::(cjsToClauses q)
        | fm -> [literals fm]

    let clausesToDnf cls = 
        cls
        |> List.map list_conj
        |> list_disj