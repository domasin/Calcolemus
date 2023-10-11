// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

/// <summary>
/// Polymorphic type of formulas with parser and printer. 
/// </summary>
module Formulas =

    /// <summary>
    /// Abstract syntax tree of polymorphic type of formulas.
    /// </summary>
    type formula<'a> =
        ///<summary>
        /// Constant formula <c>False</c>.
        /// </summary>
        | False
        ///<summary>
        /// Constant formula <c>True</c>.
        /// </summary>
        | True
        ///<summary>
        /// Atomic formula.
        /// </summary>
        /// 
        /// <param name="Item">The type of the formula: see <see cref='T:FolAutomReas.Prop.prop'/> and <see cref='T:FolAutomReas.FolModule.fol'/>.</param>
        | Atom of 'a
        ///<summary>
        /// Negation.
        /// </summary>
        /// 
        /// <param name="Item">The formula being negated.</param>
        | Not of formula<'a>
        ///<summary>
        /// Conjunction.
        /// </summary>
        /// 
        /// <param name="Item1">The first conjunct.</param>
        /// <param name="Item2">The second conjunct.</param>
        | And of formula<'a> * formula<'a>
        ///<summary>
        /// Disjunction.
        /// </summary>
        /// 
        /// <param name="Item1">The first disjunct.</param>
        /// <param name="Item2">The second disjunct.</param>
        | Or of formula<'a> * formula<'a>
        ///<summary>
        /// Implication.
        /// </summary>
        /// 
        /// <param name="Item1">The antecedent of the implication.</param>
        /// <param name="Item2">The conclusion of the implication.</param>
        | Imp of formula<'a> * formula<'a>
        ///<summary>
        /// Logical Equivalence.
        /// </summary>
        /// 
        /// <param name="Item1">The first member of the equivalence.</param>
        /// <param name="Item2">The second member of the equivalence.</param>
        | Iff of formula<'a> * formula<'a>
        ///<summary>
        /// Universally quantified formula.
        /// </summary>
        /// 
        /// <param name="Item1">The name of the universally quantified variable.</param>
        /// <param name="Item2">The scope of the universal quantification.</param>
        | Forall of string * formula<'a>
        ///<summary>
        /// Existentially quantified formula.
        /// </summary>
        /// 
        /// /// <param name="Item1">The name of the existentially quantified variable.</param>
        /// <param name="Item2">The scope of the existential quantification.</param>
        | Exists of string * formula<'a>
    
    /// <summary>Parses atomic formulas.</summary>
    /// 
    /// <remarks>
    /// It attempts to parse an atomic formula as a term followed by an infix 
    /// predicate symbol and only if that fails proceed to considering other 
    /// kinds of formulas.
    /// </remarks>
    /// 
    /// <param name="ifn">A restricted parser for infix atoms.</param>
    /// <param name="afn">A general parser for arbitrary atoms.</param>
    /// <param name="inp">The list of string tokens</param>
    /// <param name="vs">The set of bound variables in the current scope.</param>
    /// 
    /// <returns>
    /// A pair consisting of the parsed formula tree together with 
    /// any unparsed input. 
    /// </returns>
    val parse_atomic_formula:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list -> inp: string list -> formula<'a> * string list

    /// <summary>
    /// Parses quantified formulas.
    /// </summary>
    /// 
    /// <param name="ifn">A restricted parser for infix atoms.</param>
    /// <param name="afn">A general parser for arbitrary atoms.</param>
    /// <param name="inp">The list of string tokens</param>
    /// <param name="vs">The set of bound variables in the current scope.</param>
    /// 
    /// <returns>
    /// A pair consisting of the parsed formula tree together with 
    /// any unparsed input. 
    /// </returns>
    val parse_quant:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list ->
        qcon: (string * formula<'a> -> formula<'a>) ->
        x: string -> inp: string list -> formula<'a> * string list

    /// <summary>
    /// Recursive descent parser of polymorphic formulas built up from an 
    /// atomic formula parser by cascading instances of parse infix in order of 
    /// precedence, following the conventions with <c>/\</c> coming highest and 
    /// <c>&lt;=&gt;</c> lowest.
    /// </summary>
    /// 
    /// <remarks>
    /// It takes a list of string tokens <c>inp</c> and in order to check 
    /// whether a name is within the scope of a quantifier, it takes an 
    /// additional argument <c>vs</c> which is the set of bound variables in 
    /// the current scope.
    /// </remarks>
    /// 
    /// <param name="ifn">A restricted parser for infix atoms.</param>
    /// <param name="afn">A general parser for arbitrary atoms.</param>
    /// <param name="inp">The list of string tokens</param>
    /// <param name="vs">The set of bound variables in the current scope.</param>
    /// 
    /// <returns>
    /// A pair consisting of the parsed formula tree together with 
    /// any unparsed input. 
    /// </returns>
    val parse_formula:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list -> inp: string list -> formula<'a> * string list

    /// Modifies a basic printer to have a kind of boxing 'wrapped' around it.
    val fbracket:
      tw: System.IO.TextWriter ->
        p: bool -> n: 'a -> f: ('b -> 'c -> unit) -> x: 'b -> y: 'c -> unit

    /// Breaks up a quantified term into its quanti?ed variables and body.
    val strip_quant: fm: formula<'a> -> string list * formula<'a>

    /// Printing parametrized by a function <c>pfn</c> to print atoms.
    val fprint_formula:
      tw: System.IO.TextWriter -> pfn: (int -> 'a -> unit) -> (formula<'a> -> unit)

    /// Main toplevel printer. It just adds the guillemot-style quotations 
    /// round the formula so that it looks like the quoted formulas we parse.
    val fprint_qformula:
      tw: System.IO.TextWriter ->
        pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Prints a formula <c>fm</c> using a function <c>pfn</c> to print atoms.
    val inline print_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Returns a string representation of a formula <c>fm</c> using a function 
    /// <c>pfn</c> to print atoms.
    val inline sprint_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

    /// Prints a formula <c>fm</c> using a function <c>pfn</c> to print atoms.
    val inline print_qformula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// Returns a string representation of a formula <c>fm</c> using a function 
    /// <c>pfn</c> to print atoms.
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
    /// the structure unchanged. It can be used, for example, to perform 
    /// systematic replacement of one particular atomic proposition by another 
    /// formula.
    val onatoms: f: ('a -> formula<'a>) -> fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Formula analog of list iterator 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.FoldBack``2'/>. 
    /// It iterates a binary function <c>f</c> over all the atoms of a formula 
    /// (as the first argument) using the input <c>b</c> as the second argument.
    /// </summary>
    val overatoms: f: ('a -> 'b -> 'b) -> fm: formula<'a> -> b: 'b -> 'b

    /// Collects together some set of attributes associated with the atoms; in 
    /// the simplest case just returning the set of all atoms. It does this by 
    /// iterating a function <c>f</c> together with an <c>append</c> over all 
    /// the atoms, and finally converting the result to a set to remove 
    /// duplicates.
    val atom_union:
      f: ('a -> 'b list) -> fm: formula<'a> -> 'b list when 'b: comparison