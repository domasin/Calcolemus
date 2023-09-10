// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Polymorphic type of formulas with parser and printer. 
module FolAutomReas.Formulas

open FolAutomReas.Lib

/// Abstract syntax tree of polymorphic type of formulas.
type formula<'a> =
    | False
    | True
    | Atom of 'a
    | Not of formula<'a>
    | And of formula<'a> * formula<'a>
    | Or of formula<'a> * formula<'a>
    | Imp of formula<'a> * formula<'a>
    | Iff of formula<'a> * formula<'a>
    | Forall of string * formula<'a>
    | Exists of string * formula<'a>

// ------------------------------------------------------------------------- //
// General parsing of infixes.                                               //
// ------------------------------------------------------------------------- //

/// General parsing of iterated infixes. 
let rec parse_ginfix opsym opupdate sof subparser inp =
    let e1, inp1 = subparser inp
    match inp1 with
    | hd :: tl when hd = opsym ->
        parse_ginfix opsym opupdate (opupdate sof e1) subparser tl
    | _ ->
        sof e1, inp1

/// Parses left infix.
let parse_left_infix opsym opcon =
    parse_ginfix opsym (fun f e1 e2 -> opcon (f e1, e2)) id

/// Parses right infix.
let parse_right_infix opsym opcon =
    parse_ginfix opsym (fun f e1 e2 -> f <| opcon (e1, e2)) id

/// Parses a list: used to parse the list of arguments to a function.
let parse_list opsym =
    parse_ginfix opsym (fun f e1 e2 -> (f e1) @ [e2]) (fun x -> [x])

// ------------------------------------------------------------------------- //
// Other general parsing combinators.                                        //
// ------------------------------------------------------------------------- //

/// Applies a function to the ﬁrst element of a pair, the idea being to modify 
/// the returned abstract syntax tree while leaving the ‘unparsed input’ alone.
let inline papply f (ast, rest) =
    f ast, rest

/// Checks if the head of a list (typically the list of unparsed input) is some 
/// particular item, but also ﬁrst checks that the list is nonempty before 
/// looking at its head.
let nextin inp tok =
    match inp with
    | hd :: _ when hd = tok -> true
    | _ -> false

/// Deals with the common situation of syntactic items enclosed in brackets. It 
/// simply calls the subparser and then checks and eliminates the closing 
/// bracket. In principle, the terminating character can be anything, so this 
/// function could equally be used for other purposes, but we will always use 
/// ')' for the cbra (‘closing bracket’) argument.
let parse_bracketed subparser cbra inp =
    let ast, rest = subparser inp
    if nextin rest cbra then
        ast, List.tail rest
    else failwith "Closing bracket expected"

// ------------------------------------------------------------------------- //
// Parsing of formulas.                                                      //
// ------------------------------------------------------------------------- //

/// Attempts to parse an atomic formula as a term followed by an inﬁx predicate 
/// symbol and only if that fails proceed to considering other kinds of 
/// formulas.
let rec parse_atomic_formula (ifn, afn) vs inp =
    match inp with
    | [] ->
        failwith "formula expected"
    | "false" :: rest ->
        False, rest
    | "true" :: rest ->
        True, rest
    | "(" :: rest -> 
        try ifn vs inp
        with _ ->
            parse_bracketed (parse_formula (ifn, afn) vs) ")" rest
    | "~" :: rest ->
        papply Not (parse_atomic_formula (ifn, afn) vs rest)
    | "forall" :: x :: rest ->
        parse_quant (ifn, afn) (x :: vs) Forall x rest
    | "exists" :: x :: rest ->
        parse_quant (ifn, afn) (x :: vs) Exists x rest
    | _ -> afn vs inp
/// Parses quantifiers.
and parse_quant (ifn, afn) vs qcon x inp =
    match inp with
    | [] ->
        failwith "Body of quantified term expected"
    | y :: rest ->
        if y = "." then
            parse_formula (ifn, afn) vs rest
        else
            parse_quant (ifn, afn) (y :: vs) qcon y rest
        |> papply (fun fm ->
            qcon (x, fm))
/// Recursive descent parser of polymorphic formulas built up from an 
/// atomic formula parser by cascading instances of parse infix in order of 
/// precedence, following the conventions with '/\' coming highest and '<=>' 
/// lowest.
/// 
/// It takes a list of string tokens `inp` and In order to check whether a name 
/// is within the scope of a quantiﬁer, it takes an additional argument `vs` 
/// which is the set of bound variables in the current scope.
/// 
/// It returns a pair consisting of the parsed formula tree together with 
/// any unparsed input. 
and parse_formula (ifn, afn) vs inp =
    parse_right_infix "<=>" Iff
        (parse_right_infix "==>" Imp
            (parse_right_infix "\\/" Or
                (parse_right_infix "/\\" And
                    (parse_atomic_formula (ifn, afn) vs)))) inp

// ------------------------------------------------------------------------- //
// Printing of formulas.                                                     //
// ------------------------------------------------------------------------- //

/// Modiﬁes a basic printer to have a kind of boxing 'wrapped' around it.
let fbracket tw p n f x y =
    if p then fprintf tw "("
    f x y
    if p then fprintf tw ")"
    
/// Breaks up a quantiﬁed term into its quantiﬁed variables and body.
let rec strip_quant fm =
    match fm with
    | Forall (x, (Forall (y, p) as yp))
    | Exists (x, (Exists (y, p) as yp)) ->
        let xs, q = strip_quant yp
        (x :: xs), q
    | Forall (x, p)
    | Exists (x, p) ->
        [x], p
    | _ ->
        [], fm

/// Printing parametrized by a function `pfn` to print atoms.
let fprint_formula tw pfn =
    let rec print_formula pr fm =
        match fm with
        | False ->
            fprintf tw "false"
        | True ->
            fprintf tw "true"
        | Atom pargs ->
            pfn pr pargs
        | Not p ->
            fbracket tw (pr > 10) 1 (print_prefix 10) "~" p
        | And (p, q) ->
            fbracket tw (pr > 8) 0 (print_infix 8 "/\\") p q
        | Or (p, q) ->
            fbracket tw (pr > 6) 0 (print_infix  6 "\\/") p q
        | Imp (p, q) ->
            fbracket tw (pr > 4) 0 (print_infix 4 "==>") p q
        | Iff (p, q) ->
            fbracket tw (pr > 2) 0 (print_infix 2 "<=>") p q
        | Forall (x, p) ->
            fbracket tw (pr > 0) 2 print_qnt "forall" (strip_quant fm)
        | Exists (x, p) ->
            fbracket tw (pr > 0) 2 print_qnt "exists" (strip_quant fm)
    /// Prints quantifiers.
    and print_qnt qname (bvs, bod) =
        fprintf tw "%s" qname
        List.iter (fprintf tw " %s") bvs
        fprintf tw ". "
        print_formula 0 bod
    /// Prints iterated preﬁx operations without multiple brackets.
    and print_prefix newpr sym p =
        fprintf tw "%s" sym
        print_formula (newpr + 1) p
    /// Prints instances of inﬁx operators.
    and print_infix newpr sym p q =
        print_formula (newpr + 1) p
        fprintf tw " %s " sym
        print_formula newpr q

    print_formula 0

/// Main toplevel printer. It just adds the guillemot-style quotations round 
/// the formula so that it looks like the quoted formulas we parse.
let fprint_qformula tw pfn fm =
    fprintf tw "<<" // it renders better in notebooks
    fprint_formula tw pfn fm
    fprintf tw ">>" // it renders better in notebooks

/// Prints a formula `fm` using a function `pfn` to print atoms.
let inline print_formula pfn fm = fprint_formula stdout pfn fm

/// Returns a string representation of a formula `fm` using a function `pfn` to 
/// print atoms.
let inline sprint_formula pfn fm = writeToString (fun sw -> fprint_formula sw pfn fm)
    
/// Prints a formula `fm` using a function `pfn` to print atoms.
let inline print_qformula pfn fm = fprint_qformula stdout pfn fm

/// Returns a string representation of a formula `fm` using a function `pfn` to 
/// print atoms.
let inline sprint_qformula pfn fm = writeToString (fun sw -> fprint_qformula sw pfn fm)

// ------------------------------------------------------------------------- //
// Formula Constructors.                                                     //
// ------------------------------------------------------------------------- //

/// Constructs a conjunction.
let inline mk_and p q = And (p, q)

/// Constructs a disjunction.
let inline mk_or p q = Or (p, q)

/// Constructs an implication.
let inline mk_imp p q = Imp (p, q)

/// Constructs a logical equivalence.
let inline mk_iff p q = Iff (p, q)

/// Constructs a universal quantification.
let inline mk_forall x p = Forall (x, p)

/// Constructs an existential quantification.
let inline mk_exists x p = Exists (x, p)

// ------------------------------------------------------------------------- //
// Formula Destructors.                                                      //
// ------------------------------------------------------------------------- //

/// Formula destructor for logical equivalence.
let dest_iff = function
    | Iff (p, q) ->
        p, q
    | _ ->
        failwith "dest_iff"

/// Formula destructor for conjunction into the two conjuncts.
let dest_and = function
    | And (p, q) ->
        p, q
    | _ ->
        failwith "dest_and"

/// Iteratively breaks apart a conjunction.
let rec conjuncts = function
    | And (p, q) ->
        conjuncts p @ conjuncts q 
    | fm -> [fm]

/// Breaks apart a disjunction into the two disjuncts.
let dest_or = function
    | Or (p, q) ->
        p, q
    | _ ->
        failwith "dest_or"

/// Iteratively breaks apart a disjunction.
let rec disjuncts = function
    | Or (p, q) ->
        disjuncts p @ disjuncts q 
    | fm -> [fm]

/// Breaks apart an implication into antecedent and consequent.
let dest_imp = function
    | Imp (p, q) ->
        p, q
    | _ ->
        failwith "dest_imp"

/// Returns the antecedent of an implication.
let inline antecedent fm =
    fst <| dest_imp fm

/// Returns the consequent of an implication.
let inline consequent fm =
    snd <| dest_imp fm

/// Applies a function to all the atoms in a formula, but otherwise leaves 
/// the structure unchanged. It can be used, for example, to perform systematic 
/// replacement of one particular atomic proposition by another formula.
let rec onatoms f fm =
    match fm with
    | Atom a ->
        f a
    | Not p ->
        Not (onatoms f p)
    | And (p, q) ->
        And (onatoms f p, onatoms f q)
    | Or (p, q) ->
        Or (onatoms f p,onatoms f q)
    | Imp (p, q) ->
        Imp (onatoms f p, onatoms f q)
    | Iff (p, q) ->
        Iff (onatoms f p, onatoms f q)
    | Forall (x, p) ->
        Forall (x, onatoms f p)
    | Exists (x, p) ->
        Exists (x, onatoms f p)
    | _ -> fm

/// Formula analog of list iterator `List.foldBack`. It iterates a binary 
/// function `f` over all the atoms of a formula (as the first argument) using 
/// the input `b` as the second argument.
let rec overatoms f fm b =
    match fm with
    | Atom a ->
        f a b
    | Not p ->
        overatoms f p b
    | And (p, q)
    | Or (p, q)
    | Imp (p, q)
    | Iff (p, q) ->
        overatoms f p (overatoms f q b)
    | Forall (x, p)
    | Exists (x, p) ->
        overatoms f p b
    | _ -> b

/// Collects together some set of attributes associated with the atoms; in the 
/// simplest case just returning the set of all atoms. It does this by 
/// iterating a function `f` together with an ‘append’ over all the atoms, and 
/// ﬁnally converting the result to a set to remove duplicates.
let atom_union f fm =
    (fm, [])
    ||> overatoms (fun h t ->
        (f h) @ t)
    |> setify


    

