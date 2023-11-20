// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Lib.Fpf

open Formulas
open Fol
open Resolution

/// <summary>
/// Rewriting.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Rewrite = 

    // ====================================================================== //
    // Rewriting.                                                             //
    // ====================================================================== //

    // ---------------------------------------------------------------------- //
    // Rewriting at the top level with first of list of equations.            //
    // ---------------------------------------------------------------------- //

    let rec rewrite1 eqs t =
        match eqs with
        | Atom (R ("=", [l; r])) :: oeqs -> 
            try 
                tsubst (term_match undefined [l, t]) r
            with _ ->
                rewrite1 oeqs t
        | _ -> failwith "rewrite1"

    // ---------------------------------------------------------------------- //
    // Rewriting repeatedly and at depth (top-down).                          //
    // ---------------------------------------------------------------------- //

    let rec rewrite eqs tm =
        try rewrite eqs (rewrite1 eqs tm)
        with _ ->
            match tm with
            | Var x -> tm
            | Fn (f, args) -> 
                let tm' = Fn (f, List.map (rewrite eqs) args)
                if tm' = tm then tm 
                else rewrite eqs tm'


