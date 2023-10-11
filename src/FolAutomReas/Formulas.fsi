// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

/// <summary>A simple algebraic expressions example
/// Polymorphic type of formulas with parser and printer. 
/// </summary>
module Formulas =

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
    
    /// Attempts to parse an atomic formula as a term followed by an in?x predicate 
    /// symbol and only if that fails proceed to considering other kinds of 
    /// formulas.
    val parse_atomic_formula:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list -> inp: string list -> formula<'a> * string list

    /// Parses quantifiers.
    val parse_quant:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list ->
        qcon: (string * formula<'a> -> formula<'a>) ->
        x: string -> inp: string list -> formula<'a> * string list

    /// Recursive descent parser of polymorphic formulas built up from an 
    /// atomic formula parser by cascading instances of parse infix in order of 
    /// precedence, following the conventions with '/\' coming highest and '<=>' 
    /// lowest.
    /// 
    /// It takes a list of string tokens `inp` and In order to check whether a name 
    /// is within the scope of a quanti?er, it takes an additional argument `vs` 
    /// which is the set of bound variables in the current scope.
    /// 
    /// It returns a pair consisting of the parsed formula tree together with 
    /// any unparsed input. 
    val parse_formula:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list -> inp: string list -> formula<'a> * string list

    /// Modi?es a basic printer to have a kind of boxing 'wrapped' around it.
    val fbracket:
      tw: System.IO.TextWriter ->
        p: bool -> n: 'a -> f: ('b -> 'c -> unit) -> x: 'b -> y: 'c -> unit

    /// Breaks up a quanti?ed term into its quanti?ed variables and body.
    val strip_quant: fm: formula<'a> -> string list * formula<'a>

    /// Printing parametrized by a function `pfn` to print atoms.
    val fprint_formula:
      tw: System.IO.TextWriter -> pfn: (int -> 'a -> unit) -> (formula<'a> -> unit)

    /// Main toplevel printer. It just adds the guillemot-style quotations round 
    /// the formula so that it looks like the quoted formulas we parse.
    val fprint_qformula:
      tw: System.IO.TextWriter ->
        pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Prints a formula `fm` using a function `pfn` to print atoms.
    val inline print_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Returns a string representation of a formula `fm` using a function `pfn` to 
    /// print atoms.
    val inline sprint_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

    /// Prints a formula `fm` using a function `pfn` to print atoms.
    val inline print_qformula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Returns a string representation of a formula `fm` using a function `pfn` to 
    /// print atoms.
    val inline sprint_qformula:
      pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

    /// Constructs a conjunction.
    val inline mk_and: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// Constructs a disjunction.
    val inline mk_or: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// Constructs an implication.
    val inline mk_imp: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// Constructs a logical equivalence.
    val inline mk_iff: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// Constructs a universal quantification.
    val inline mk_forall: x: string -> p: formula<'a> -> formula<'a>

    /// Constructs an existential quantification.
    val inline mk_exists: x: string -> p: formula<'a> -> formula<'a>

    /// Formula destructor for logical equivalence.
    val dest_iff: _arg1: formula<'a> -> formula<'a> * formula<'a>

    /// Formula destructor for conjunction into the two conjuncts.
    val dest_and: _arg1: formula<'a> -> formula<'a> * formula<'a>

    /// Iteratively breaks apart a conjunction.
    val conjuncts: _arg1: formula<'a> -> formula<'a> list

    /// Breaks apart a disjunction into the two disjuncts.
    val dest_or: _arg1: formula<'a> -> formula<'a> * formula<'a>

    /// Iteratively breaks apart a disjunction.
    val disjuncts: _arg1: formula<'a> -> formula<'a> list

    /// Breaks apart an implication into antecedent and consequent.
    val dest_imp: _arg1: formula<'a> -> formula<'a> * formula<'a>

    /// Returns the antecedent of an implication.
    val inline antecedent: fm: formula<'a> -> formula<'a>

    /// Returns the consequent of an implication.
    val inline consequent: fm: formula<'a> -> formula<'a>

    /// Applies a function to all the atoms in a formula, but otherwise leaves 
    /// the structure unchanged. It can be used, for example, to perform systematic 
    /// replacement of one particular atomic proposition by another formula.
    val onatoms: f: ('a -> formula<'a>) -> fm: formula<'a> -> formula<'a>

    /// Formula analog of list iterator `List.foldBack`. It iterates a binary 
    /// function `f` over all the atoms of a formula (as the first argument) using 
    /// the input `b` as the second argument.
    val overatoms: f: ('a -> 'b -> 'b) -> fm: formula<'a> -> b: 'b -> 'b

    /// Collects together some set of attributes associated with the atoms; in the 
    /// simplest case just returning the set of all atoms. It does this by 
    /// iterating a function `f` together with an ‘append’ over all the atoms, and 
    /// ?nally converting the result to a set to remove duplicates.
    val atom_union:
      f: ('a -> 'b list) -> fm: formula<'a> -> 'b list when 'b: comparison