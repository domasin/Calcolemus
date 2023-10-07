// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// <namespacedoc><summary>
/// Types, functions, operators and procedures for automated/interactive 
/// reasoning in first order logic.
/// </summary></namespacedoc>
/// <summary>A simple algebraic expression example.</summary>
module FolAutomReas.Intro

open FolAutomReas.Lib
open FolAutomReas.Lib.Set

/// Abstract syntax tree of algebraic expressions.
type expression =
    | Var of string
    | Const of int
    | Add of expression * expression
    | Mul of expression * expression

/// Simplify an algebraic expression at the first level.
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

/// Recursively simplifies any immediate subexpressions as much as possible, 
/// then applies `simplify1` to the result.
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

// ------------------------------------------------------------------------- //
// Lexical analysis.                                                         //
// ------------------------------------------------------------------------- //

/// Creates a pattern matching function based on the input `s` as the pattern.
let matches s = 
    let chars = 
        explode s 
    fun c -> mem c chars

/// Classifies string characters as spaces.
let space = matches " \t\n\r"

/// Classifies string characters as punctuation.
let punctuation = matches "()[]{},"

/// Classifies string characters as symbolic.
let symbolic = matches "~`!@#$%^&*-+=|\\:;<>.?/"

/// Classifies string characters as numeric.
let numeric = matches "0123456789"

/// Classifies string characters as alphanumeric.
let alphanumeric = matches "abcdefghijklmnopqrstuvwxyz_'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

/// Takes a property `prop` of characters, such as one of the classifying 
/// predicates (`space`, `punctuation`, `symbolic`, `numeric`, `alphanumeric`), 
/// and a list of input characters `inp`, separating oﬀ as a string the longest 
/// initial sequence of that list of characters satisfying `prop`.
let rec lexwhile prop inp =
    match inp with
    | c :: cs when prop c ->
        let tok, rest = lexwhile prop cs
        c + tok, rest
    | _ -> "", inp

/// Lexical analyser. It maps a list of input characters `inp` into a list of 
/// token strings.
let rec lex inp =
    match snd <| lexwhile space inp with
    | [] -> []
    | c :: cs ->
        let prop =
            if alphanumeric c then alphanumeric
            else if symbolic c then symbolic
            else fun c -> false
        let toktl, rest = lexwhile prop cs
        (c + toktl) :: lex rest

// ------------------------------------------------------------------------- //
// Parsing.                                                                  //
// ------------------------------------------------------------------------- //

/// Recursive descent parsing of expression. It takes a list of tokens `i` and 
/// returns a pair consisting of the parsed expression tree together with any 
/// unparsed input.
let rec parse_expression i =
    match parse_product i with
    | e1, "+" :: i1 ->
        let e2, i2 = parse_expression i1
        Add (e1, e2), i2
    | x -> x
/// Parses a product.
and parse_product i =
    match parse_atom i with
    | e1, "*" :: i1 ->
        let e2, i2 = parse_product i1
        Mul (e1, e2), i2
    | x -> x
/// Parses an atom.
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

// ------------------------------------------------------------------------- //
// Generic function to impose lexing and exhaustion checking on a parser.    //
// ------------------------------------------------------------------------- //

/// A wrapper function that explodes the input string, lexically analyzes it, 
/// parses the sequence of tokens and then ﬁnally checks that no input remains 
/// unparsed.
let make_parser pfn (s : string) =
    let tokens =
        // Replace newlines with spaces so the lexer and parser
        // work correctly on multi-line strings.
        // TODO : This could probably be optimized to make the replacements
        // in a single pass using a Regex.
        s.Replace('\r', ' ')
            .Replace('\n', ' ')
        // Reduce multiple spaces to single spaces to help the parser.
            .Replace("  ", " ")
        |> explode
        |> lex

    match pfn tokens with
    | expr, [] ->
        expr
    | _, rest ->
        failwithf "Unparsed input: %i tokens remaining in buffer."
            <| List.length rest

/// Parses a string into an expression.
let parse_exp =
    make_parser parse_expression
    
// ------------------------------------------------------------------------- //
// Printing                                                                  //
// ------------------------------------------------------------------------- //

/// Reverses transformation, from abstract to concrete syntax keeping brackets. 
/// It puts brackets uniformly round each instance of a binary operator, 
/// which is perfectly correct but sometimes looks cumbersome to a human.
let rec string_of_exp_naive e =
    match e with
    | Var s -> s
    | Const n -> string n
    | Add (e1, e2) ->
        "(" + (string_of_exp_naive e1) + " + " + (string_of_exp_naive e2) + ")"
    | Mul (e1, e2) ->
        "(" + (string_of_exp_naive e1) + " * " + (string_of_exp_naive e2) + ")"

/// Auxiliary function to print expressions without unnecessary brackets. 
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

/// Prints an expression `e` in concrete syntax removing unnecessary brackets. 
/// It omits the outermost brackets, and those that are implicit in rules for 
/// precedence or associativity.
let print_exp e =
    printfn "%O" ("<<" + string_of_exp 0 e + ">>")

/// Returns a string of the concrete syntax of an expression `e` removing 
/// unnecessary brackets. It omits the outermost brackets, and those that are 
/// implicit in rules for precedence or associativity.
let sprint_exp e =
    "<<" + string_of_exp 0 e + ">>"