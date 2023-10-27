namespace Calcolemus

open Calcolemus.Lib.Fpf
open Calcolemus.Formulas

/// <summary>
/// Basic stuff for first order logic: datatype, parsing and printing, 
/// semantics, syntax operations and substitution.
/// </summary>
/// 
/// <category index="4">First order logic</category>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Fol = 

    /// <summary>Type of first order terms.</summary>
    type term =
        /// <summary>Variable term.</summary>
        | Var of string
        /// <summary>Functional term.</summary>
        | Fn of string * term list

    /// <summary>Type of atomic first order formulas.</summary>
    type fol = | R of string * term list

    /// <summary>
    /// Applies a subfunction <c>f</c> to the top <em>terms</em>.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val onformula:
      f: (term -> term) -> (formula<fol> -> formula<fol>)

    /// <summary>
    /// Checks if a string is a constant term. 
    /// Only numerals and the empty list constant "nil" are considered as 
    /// constants.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val is_const_name: s: string -> bool

    /// <summary>
    /// Parses an atomic term.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val parse_atomic_term: vs: string list -> inp: string list -> term * string list

    /// <summary>
    /// Recursive descent parser of terms built up from an atomic term parser 
    /// by cascading instances of parse infix in order of precedence, following 
    /// the conventions with '^' coming highest and '::' lowest.
    /// <p></p>
    /// It takes a list of string tokens `inp` and returns a pair consisting of 
    /// the parsed term tree together with any unparsed input. 
    /// <p></p>
    /// In order to check whether a name is within the scope of a quanti?er, it 
    /// takes an additional argument `vs` which is the set of bound variables 
    /// in the current scope.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val parse_term: vs: string list -> inp: string list -> term * string list

    /// <summary>
    /// Parses a string into a term.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val parset: (string -> term)

    /// <summary>
    /// A convenient operator to call <c>parset</c>.
    /// </summary>
    /// 
    /// <category index="1">Parsing of terms</category>
    val (!!!) : (string -> term)

    /// <summary>
    /// A special recognizer for 'infix' atomic formulas like s &lt; t.
    /// </summary>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val parse_infix_atom:
      vs: string list -> inp: string list -> formula<fol> * string list

    /// <summary>
    /// Parses atomic fol 
    /// </summary>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val parse_atom:
      vs: string list -> inp: string list -> formula<fol> * string list

    /// <summary>
    /// Parses a fol formula
    /// </summary>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val parse: (string -> formula<fol>)

    /// <summary>
    /// A convenient operator to call parse.
    /// </summary>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val (!!) : (string -> formula<fol>)

    /// <summary>
    /// Prints terms.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val fprint_term: tw: System.IO.TextWriter -> prec: int -> fm: term -> unit

    /// <summary>
    /// Prints a function and its arguments.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val fprint_fargs:
      tw: System.IO.TextWriter -> f: string -> args: term list -> unit

    /// <summary>
    /// Prints an infix operation.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val fprint_infix_term:
      tw: System.IO.TextWriter ->
        isleft: bool ->
        oldprec: int -> newprec: int -> sym: string -> p: term -> q: term -> unit

    /// <summary>
    /// Term printer with TextWriter.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val fprintert: tw: System.IO.TextWriter -> tm: term -> unit

    /// <summary>
    /// Term printer.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val inline print_term: t: term -> unit

    /// <summary>
    /// Return the string of the concrete syntax representation of a term.
    /// </summary>
    /// 
    /// <category index="3">Printing terms</category>
    val inline sprint_term: t: term -> string

    /// <summary>
    /// Printer of atomic fol formulas with TextWriter.
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val fprint_atom: tw: System.IO.TextWriter -> prec: 'a -> fol -> unit

    /// <summary>
    /// Printer of atomic fol 
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline print_atom: prec: 'a -> arg: fol -> unit

    /// <summary>
    /// Returns the concrete syntax representation of an atom.
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline sprint_atom: prec: 'a -> arg: fol -> string

    /// <summary>
    /// Printer of fol formulas with TextWriter.
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val fprint_fol_formula:
      tw: System.IO.TextWriter -> (formula<fol> -> unit)

    /// <summary>
    /// Printer of fol 
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline print_fol_formula: f: formula<fol> -> unit

    /// <summary>
    /// Returns the string of the concrete syntax representation of fol.
    /// </summary>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline sprint_fol_formula: f: formula<fol> -> string

    /// <summary>
    /// Returns the value of a term <c>tm</c> in a particular 
    /// interpretation M (<c>domain</c>, <c>func</c>, <c>pred</c>) and 
    /// valuation <c>v</c>.
    /// </summary>
    /// 
    /// <category index="4">Semantics</category>
    val termval:
      domain: 'a * func: (string -> 'b list -> 'b) * pred: 'c ->
        v: func<string,'b> -> tm: term -> 'b

    /// <summary>
    /// Evaluates a fol formula <c>fm</c> in the interpretation specified
    /// by the triplet <c>domain</c>, <c>func</c>, <c>pred</c> and the 
    /// variables valuation <c>v</c>.
    /// </summary>
    /// 
    /// <category index="4">Semantics</category>
    val holds:
      domain: 'a list * func: (string -> 'a list -> 'a) *
      pred: (string -> 'a list -> bool) ->
        v: func<string,'a> -> fm: formula<fol> -> bool

    /// <summary>
    /// An interpretation a la Boole.
    /// </summary>
    /// 
    /// <category index="4">Semantics</category>
    val bool_interp:
      bool list * (string -> bool list -> bool) * (string -> 'a list -> bool)
        when 'a: equality

    /// <summary>
    /// An arithmetic modulo <c>n</c> interpretation.
    /// </summary>
    /// 
    /// <category index="4">Semantics</category>
    val mod_interp:
      n: int -> int list * (string -> int list -> int) * (string -> 'a list -> bool)
        when 'a: equality

    /// <summary>
    /// Returns the free variables in the term <c>tm</c>.
    /// </summary>
    /// 
    /// <category index="5">Free variables</category>
    val fvt: tm: term -> string list

    /// <summary>
    /// Returns all the variables in the FOL formula <c>fm</c>.
    /// </summary>
    /// 
    /// <category index="5">Free variables</category>
    val var: fm: formula<fol> -> string list

    /// <summary>
    /// Returns the free variables in the FOL formula <c>fm</c>.
    /// </summary>
    /// 
    /// <category index="5">Free variables</category>
    val fv: fm: formula<fol> -> string list

    /// <summary>
    /// Universal closure of a formula.
    /// </summary>
    /// 
    /// <category index="5">Free variables</category>
    val generalize: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Substitution within terms.
    /// </summary>
    /// 
    /// <category index="6">Substitution in terms</category>
    val tsubst: sfn: func<string,term> -> tm: term -> term

    /// <summary>
    /// Creates a 'variant' of a variable name by adding prime characters to it 
    /// until it is distinct from some given list of variables to avoid.
    /// 
    /// <c>variant "x" ["x"; "y"]</c> returns <c>"x'"</c>.
    /// </summary>
    /// 
    /// <category index="7">Substitution in formulas</category>
    val variant: x: string -> vars: string list -> string

    /// <summary>
    /// Given a substitution function <c>sbfn</c> applies it to the input 
    /// formula <c>fm</c>. Bound variables will be renamed if necessary to 
    /// avoid capture.
    /// 
    /// <c>subst ("y" |=> Var "x") ("forall x. x = y" |> parse)</c> returns 
    /// <c>`forall x'. x' = x`</c>.
    /// </summary>
    /// 
    /// <category index="7">Substitution in formulas</category>
    val subst:
      subfn: func<string,term> ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Checks whether there would be variable capture if the bound variable 
    /// <c>x</c> is not renamed.
    /// </summary>
    /// 
    /// <category index="7">Substitution in formulas</category>
    val substq:
      subfn: func<string,term> ->
        quant: (string -> formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>