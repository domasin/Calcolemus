// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (adapted from lib for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.String
open FolAutomReas.Lib.Lexer
open FolAutomReas.Lib.Parser

module Intro = 

    // Abstract syntax tree

    type expression =
        | Var of string
        | Const of int
        | Add of expression * expression
        | Mul of expression * expression

    // Symbolic computation

    let simplify1 expr =
        match expr with
        | Mul (Const 0, x)
        | Mul (x, Const 0) ->
            Const 0
        | Add (Const 0, x)
        | Add (x, Const 0)        
        | Mul (Const 1, x)
        | Mul (x, Const 1) ->
            x
        | Add (Const m, Const n) ->
            Const (m + n)
        | Mul (Const m, Const n) ->
            Const (m * n)
        | _ -> expr

    let rec simplify expr =
        match expr with
        | Add (e1, e2) ->
            Add (simplify e1, simplify e2)
            |> simplify1
        | Mul (e1, e2) ->
            Mul (simplify e1, simplify e2)
            |> simplify1
        | _ ->
            simplify1 expr

    // Parsing

    let rec parse_expression i =
        match parse_product i with
        // right associative
        | e1, "+" :: i1 ->
            let e2, i2 = parse_expression i1
            Add (e1, e2), i2
        | x -> x
    and parse_product i =
        match parse_atom i with
        // right associative
        | e1, "*" :: i1 ->
            let e2, i2 = parse_product i1
            Mul (e1, e2), i2
        | x -> x
    and parse_atom i =
        match i with
        | [] ->
            failwith "Expected an expression at end of input"
        | "(" :: i1 ->
            match parse_expression i1 with
            | e2, ")" :: i2 -> e2, i2
            | _ -> failwith "Expected closing bracket"
        | tok :: i1 ->
            if List.forall numeric (explode tok) then
                Const (int tok), i1
            else Var tok, i1

    let parse_exp s =
        make_parser parse_expression s

    // Printing

    let rec string_of_exp_naive e =
        match e with
        | Var s -> s
        | Const n -> string n
        | Add (e1, e2) ->
            "(" + (string_of_exp_naive e1) + " + " + (string_of_exp_naive e2) + ")"
        | Mul (e1, e2) ->
            "(" + (string_of_exp_naive e1) + " * " + (string_of_exp_naive e2) + ")"

    let rec string_of_exp pr e =
        match e with
        | Var s -> s
        | Const n -> string n
        | Add (e1, e2) ->
            let s = (string_of_exp 3 e1) + " + " + (string_of_exp 2 e2)
            if 2 < pr then "(" + s + ")" 
            else s
        | Mul (e1, e2) ->
            let s = (string_of_exp 5 e1) + " * " + (string_of_exp 4 e2)
            if 4 < pr then "(" + s + ")" 
            else s

    let print_exp e =
        printfn "%O" ("<<" + string_of_exp 0 e + ">>")

    let sprint_exp e =
        "<<" + string_of_exp 0 e + ">>"