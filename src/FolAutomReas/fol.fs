// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// <summary>
/// Basic stuff for first order logic: datatype, parsing and printing, 
/// semantics, syntax operations and substitution.
/// </summary>
/// 
/// <category index="4">First order logic</category>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FolAutomReas.Fol
 
open FolAutomReas.Lib
open FolAutomReas.Lib.Set
open FolAutomReas.Lib.String
open FolAutomReas.Lib.Fpf
open FolAutomReas.Lib.Lexer
open FolAutomReas.Lib.Parser

open Formulas

/// Type for first order terms.
type term = 
    | Var of string
    | Fn of string * term list

/// Type for atomic first order formulas.
type fol = 
    R of string * term list

/// Applies a subfunction `f` to the top *terms*.
let onformula f =
    onatoms <| fun (R (p, a)) ->
        Atom (R (p, List.map f a))

// ------------------------------------------------------------------------- //
// Parsing of terms.                                                         //
// ------------------------------------------------------------------------- //

/// Checks if a string is a constant term. Only numerals and the empty list constant "nil" are considered as constants.
let is_const_name s =
    s = "nil"
    || List.forall numeric (explode s)

/// Parses an atomic term.
let rec parse_atomic_term vs inp =
    match inp with
    | [] -> failwith "term expected"
    | "(" :: rest ->
        parse_bracketed (parse_term vs) ")" rest
    | "-" :: rest ->
        parse_atomic_term vs rest
        |> papply (fun t -> Fn ("-", [t]))
    | f :: "(" :: ")" :: rest ->
        Fn (f, []), rest
    | f :: "(" :: rest ->
        parse_bracketed (parse_list "," (parse_term vs)) ")" rest
        |> papply (fun args -> Fn (f, args))
    | a :: rest ->
        (if is_const_name a && not (mem a vs) then Fn (a, []) else Var a), rest
/// Recursive descent parser of terms built up from an atomic term parser 
/// by cascading instances of parse infix in order of precedence, following the 
/// conventions with '^' coming highest and '::' lowest.
/// 
/// It takes a list of string tokens `inp` and returns a pair consisting of the 
/// parsed term tree together with any unparsed input. 
/// 
/// In order to check whether a name is within the scope of a quantiﬁer, it 
/// takes an additional argument `vs` which is the set of bound variables in 
/// the current scope.
and parse_term vs inp =
    parse_right_infix "::" (fun (e1,e2) -> Fn ("::",[e1;e2]))
        (parse_right_infix "+" (fun (e1,e2) -> Fn ("+",[e1;e2]))
            (parse_left_infix "-" (fun (e1,e2) -> Fn ("-",[e1;e2]))
                (parse_right_infix "*" (fun (e1,e2) -> Fn ("*",[e1;e2]))
                    (parse_left_infix "/" (fun (e1,e2) -> Fn ("/",[e1;e2]))
                        (parse_left_infix "^" (fun (e1,e2) -> Fn ("^",[e1;e2]))
                            (parse_atomic_term vs)))))) inp

/// Parses a string into a term.
let parset = 
    make_parser (parse_term [])

/// A convenient operator to call `parset`.
let (!!!) = parset

// ------------------------------------------------------------------------- //
// Parsing of fol formulas.                                                  //
// ------------------------------------------------------------------------- //

/// A special recognizer for 'inﬁx' atomic formulas like s < t.
let parse_infix_atom vs inp =
    let tm, rest = parse_term vs inp
    if List.exists (nextin rest) ["="; "<"; "<="; ">"; ">="] then
        papply (fun tm' -> Atom (R (List.head rest, [tm; tm'])))
                (parse_term vs (List.tail rest))
    else failwith ""

/// Parses atomic fol formulas.
let parse_atom vs inp =
    try parse_infix_atom vs inp
    with _ ->
    match inp with
    | p :: "(" :: ")" :: rest ->
        Atom (R (p, [])), rest
    | p :: "(" :: rest ->
        parse_bracketed (parse_list "," (parse_term vs)) ")" rest
        |> papply (fun args -> Atom (R (p, args)))
    | p :: rest when p <> "(" ->
        Atom (R (p, [])), rest
    | _ -> failwith "parse_atom"

/// Parses a fol formula
let parse =
    parse_formula (parse_infix_atom, parse_atom) []
    |> make_parser

/// A convenient operator to call `parse`.
let (!!) = parse

// ------------------------------------------------------------------------- //
// Printing of terms.                                                        //
// ------------------------------------------------------------------------- //

/// Prints terms.
let rec fprint_term tw prec fm =
    match fm with
    | Var x ->
        fprintf tw "%s" x
    | Fn ("^", [tm1; tm2;]) ->
        fprint_infix_term tw true prec 24 "^" tm1 tm2
    | Fn ("/", [tm1; tm2;]) ->
        fprint_infix_term tw true prec 22 "/" tm1 tm2
    | Fn ("*", [tm1; tm2;]) ->
        fprint_infix_term tw false prec 20 "*" tm1 tm2
    | Fn ("-", [tm1; tm2;]) ->
        fprint_infix_term tw true prec 18 "-" tm1 tm2
    | Fn ("+", [tm1; tm2;]) ->
        fprint_infix_term tw false prec 16 "+" tm1 tm2
    | Fn ("::", [tm1; tm2;]) ->
        fprint_infix_term tw false prec 14 "::" tm1 tm2
    | Fn (f, args) ->
        fprint_fargs tw f args
/// Prints a function and its arguments.
and fprint_fargs tw f args =
    fprintf tw "%s" f
    if args <> [] then
        fprintf tw "("
        fprint_term tw 0 (List.head args)
        List.iter (
                    fun t -> 
                    fprintf tw ","
                    fprint_term tw 0 t)
            (List.tail args)
        fprintf tw ")"
/// Prints an infix operation.
and fprint_infix_term tw isleft oldprec newprec sym p q =
    if oldprec > newprec then 
        fprintf tw "("
    fprint_term tw (if isleft then newprec else newprec + 1) p
    fprintf tw "%2s " sym
    fprint_term tw (if isleft then newprec + 1 else newprec) q
    if oldprec > newprec then
        fprintf tw ")"

/// Term printer with TextWriter.
let fprintert tw tm =
    fprintf tw "<<|"
    fprint_term tw 0 tm
    fprintf tw "|>>"

/// Term printer.
let inline print_term t = fprintert stdout t

/// Return the string of the concrete syntax representation of a term.
let inline sprint_term t = writeToString (fun sw -> fprintert sw t)

// ------------------------------------------------------------------------- //
// Printing of fol formulas.                                                 //
// ------------------------------------------------------------------------- //

/// Printer of atomic fol formulas with TextWriter.
let fprint_atom tw prec (R (p, args)) : unit =
    if mem p ["="; "<"; "<="; ">"; ">="] && List.length args = 2 then
        fprint_infix_term tw false 12 12 (" " + p) 
            (args |> List.item 0) (args |> List.item 1)
    else
        fprint_fargs tw p args


/// Printer of atomic fol formulas.
let inline print_atom prec arg = fprint_atom stdout prec arg

/// Returns the concrete syntax representation of an atom.
let inline sprint_atom prec arg = writeToString (fun sw -> fprint_atom sw prec arg)

/// Printer of fol formulas with TextWriter.
let fprint_fol_formula tw =
    fprint_qformula tw (fprint_atom tw)
  
/// Printer of fol formulas.
let inline print_fol_formula f = fprint_fol_formula stdout f

/// Returns the string of the concrete syntax representation of fol formulas.
let inline sprint_fol_formula f = writeToString (fun sw -> fprint_fol_formula sw f)

// ------------------------------------------------------------------------- //
// Semantics, implemented of course for finite domains only.                 //
// ------------------------------------------------------------------------- //

/// Returns the value of a term `tm` in a particular 
/// interpretation M (`domain`, `func`, `pred`) and valuation `v`.
let rec termval (domain, func, pred) v tm =
    let m = domain, func, pred
    match tm with
    | Var x ->
        apply v x
    | Fn (f, args) ->
        func f (List.map (termval m v) args)

/// Evaluates a fol formula `fm` in the interpretation specified
/// by the triplet `domain`, `func`, `pred` and the variables valuation `v`.
let rec holds (domain, func, pred) v fm =
    let m = domain, func, pred
    match fm with
    | False -> false
    | True -> true
    | Atom (R (r, args)) ->
        pred r (List.map (termval m v) args)
    | Not p ->
        not(holds m v p)
    | And (p, q) ->
        (holds m v p) && (holds m v q)
    | Or (p, q) ->
        (holds m v p) || (holds m v q)
    | Imp (p, q) ->
        not(holds m v p) || (holds m v q)
    | Iff (p, q) ->
        (holds m v p = holds m v q)
    | Forall (x, p) ->
        List.forall (fun a -> holds m ((x |-> a) v) p) domain
    | Exists (x, p) ->
        List.exists (fun a -> holds m ((x |-> a) v) p) domain

// ------------------------------------------------------------------------- //
// Examples of particular interpretations.                                   //
// ------------------------------------------------------------------------- //

/// An interpretation à la Boole.
let bool_interp =
    let func f args =
        match f, args with
        | "0", [] -> false
        | "1", [] -> true
        | "+", [x; y] -> not (x = y)
        | "*", [x; y] -> x && y
        | _ -> failwith "uninterpreted function"

    let pred p args =
        match p, args with
        | "=", [x; y] -> x = y
        | _ -> failwith "uninterpreted predicate"

    [false; true], func, pred

/// An arithmetic modulo `n` interpretation.
let mod_interp n =
    let func f args =
        match f, args with
        | "0", [] -> 0
        | "1", [] -> 1 % n
        | "+", [x; y] -> (x + y) % n
        | "*", [x; y] -> (x * y) % n
        | _ -> failwith "uninterpreted function"

    let pred p args =
        match p, args with
        | "=", [x; y] -> x = y
        | _ -> failwith "uninterpreted predicate"

    [0..(n - 1)], func, pred

// ------------------------------------------------------------------------- //
// Free variables in terms and formulas.                                     //
// ------------------------------------------------------------------------- //

/// Returns the free variables in the term `tm`.
let rec fvt tm =
    match tm with
    | Var x -> [x]
    | Fn (f, args) ->
        unions <| List.map fvt args

/// Returns all the variables in the FOL formula `fm`.
let rec var fm =
    match fm with
    | False
    | True -> []
    | Atom (R (p, args)) ->
        unions (List.map fvt args)
    | Not p ->
        var p
    | And (p, q)
    | Or (p, q)
    | Imp (p, q)
    | Iff (p, q) ->
        union (var p) (var q)
    | Forall (x, p)
    | Exists (x, p) ->
        insert x (var p)

/// Returns the free variables in the FOL formula `fm`.
let rec fv fm =
    match fm with
    | False
    | True -> []
    | Atom (R (p, args)) ->
        unions (List.map fvt args)
    | Not p ->
        fv p
    | And (p, q)
    | Or (p, q)
    | Imp (p, q)
    | Iff (p, q) ->
        union (fv p) (fv q)
    | Forall (x, p)
    | Exists (x, p) ->
        subtract (fv p) [x]

/// Universal closure of a formula.
let generalize fm =
    List.foldBack mk_forall (fv fm) fm

/// Substitution within terms.                                                //
let rec tsubst sfn tm =
    match tm with
    | Var x ->
        tryapplyd sfn x tm
    | Fn (f, args) ->
        Fn (f, List.map (tsubst sfn) args)

/// Creates a ‘variant’ of a variable name by adding prime characters to it 
/// until it is distinct from some given list of variables to avoid.
/// 
/// `variant "x" ["x"; "y"]` returns `"x'"`.
let rec variant x vars =
    if mem x vars then
        variant (x + "'") vars 
    else x

/// Given a substitution function `sbfn` applies it to the input formula `fm`.
/// Bound variables will be renamed if necessary to avoid capture.
/// 
/// `subst ("y" |=> Var "x") ("forall x. x = y" |> parse)` returns 
/// `<<forall x'. x' = x>>`.
let rec subst subfn fm =
    match fm with
    | False -> False
    | True -> True
    | Atom (R (p, args)) ->
        Atom (R (p, List.map (tsubst subfn) args))
    | Not p ->
        Not (subst subfn p)
    | And (p, q) ->
        And (subst subfn p, subst subfn q)
    | Or (p, q) ->
        Or (subst subfn p, subst subfn q)
    | Imp (p, q) ->
        Imp (subst subfn p, subst subfn q)
    | Iff (p, q) ->
        Iff (subst subfn p, subst subfn q)
    | Forall (x, p) ->
        substq subfn mk_forall x p
    | Exists (x, p) ->
        substq subfn mk_exists x p
/// Checks whether there would be variable capture if the bound variable 
/// `x` is not renamed.
and substq subfn quant x p =
    let x' =
        if List.exists (fun y -> mem x (fvt (tryapplyd subfn y (Var y)))) (subtract (fv p) [x]) then
            variant x (fv (subst (undefine x subfn) p)) 
        else x
    quant x' (subst ((x |-> Var x') subfn) p)
