// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (modified for documentation)           //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open LanguagePrimitives

open Calcolemus.Lib.Set
open Calcolemus.Lib.String
open Calcolemus.Lib.Fpf
open Calcolemus.Lib.Lexer

open Formulas
open Prop


module Defcnf = 

    // ---------------------------------------------------------------------- //
    // Make a stylized variable and update the index.                         //
    // ---------------------------------------------------------------------- //

    let mkprop (n : bigint) =
        let name = sprintf "p_%O" n
        Atom (P name), n + (1I)

    // ---------------------------------------------------------------------- //
    // Core definitional CNF procedure.                                       //
    // ---------------------------------------------------------------------- //

    let rec maincnf (fm, defs, n) =
        let trip = fm, defs, n 
        match fm with
        | And (p, q) ->
            defstep mk_and (p, q) trip
        | Or (p, q) ->
            defstep mk_or (p, q) trip
        | Iff (p, q) ->
            defstep mk_iff (p, q) trip
        | _ -> trip

    and defstep op (p, q) (fm, defs, n) =
        let fm1, defs1, n1 = maincnf (p, defs, n)
        let fm2, defs2, n2 = maincnf (q, defs1, n1)
        let fm' = op fm1 fm2
        try fst(apply defs2 fm'), defs2, n2
        with _ ->
            let v, n3 = mkprop n2
            v, (fm' |-> (v, Iff (v, fm'))) defs2, n3

    // ---------------------------------------------------------------------- //
    // Make n large enough that "v_m" won't clash with s for any m >= n       //
    // ---------------------------------------------------------------------- //

    let max_varindex pfx s (n : bigint) =
        let m = String.length pfx
        let l = String.length s
        if l <= m || s.Substring(0,m) <> pfx then n else
        let s' = s.Substring(m,(l - m))
        if List.forall numeric (explode s') then
            max n (bigint.Parse(s'))
        else n

    // ---------------------------------------------------------------------- //
    // Overall definitional CNF.                                              //
    // ---------------------------------------------------------------------- //

    let mk_defcnf fn fm =
        let fm' = nenf fm
        let n = 
            GenericOne + overatoms (max_varindex "p_" << pname) fm' GenericZero
        let fm'', defs, _ = fn (fm', undefined, n)
        let deflist = List.map (snd << snd) (graph defs)
        unions <| simpcnf fm'' :: List.map simpcnf deflist

    let defcnf01 fm =
        mk_defcnf maincnf fm
        |> List.map list_disj
        |> list_conj

    // ---------------------------------------------------------------------- //
    // Version tweaked to exploit initial structure.                          //
    // ---------------------------------------------------------------------- //

    let subcnf sfn op (p, q) (fm, defs, n) =
        let fm1, defs1, n1 = sfn (p, defs, n)
        let fm2, defs2, n2 = sfn (q, defs1, n1) 
        op fm1 fm2, defs2, n2

    let rec orcnf (fm, defs, n) =
        let trip = fm, defs, n 
        match fm with
        | Or (p, q) ->
            subcnf orcnf mk_or (p,q) trip
        | _ -> maincnf trip

    let rec andcnf (fm, defs, n) =
        let trip = fm, defs, n
        match fm with
        | And (p, q) ->
            subcnf andcnf mk_and (p,q) trip
        | _ -> orcnf trip

    let defcnfs fm =
        mk_defcnf andcnf fm

    let defcnf fm =
        defcnfs fm
        |> List.map list_disj
        |> list_conj

    // ---------------------------------------------------------------------- //
    // Version that guarantees 3-CNF.                                         //
    // ---------------------------------------------------------------------- //

    let rec andcnf3 (fm, defs, n) =
        let trip = fm, defs, n
        match fm with
        | And (p, q) ->
            subcnf andcnf3 mk_and (p, q) trip
        | _ -> maincnf trip

    let defcnf3 fm =
        mk_defcnf andcnf3 fm
        |> List.map list_disj
        |> list_conj

