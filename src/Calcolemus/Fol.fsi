namespace Calcolemus

open Calcolemus.Lib.Fpf
open Calcolemus.Formulas

/// <summary>
/// Basic stuff for first order logic: datatype, parsing and printing, 
/// semantics, syntax operations and substitution.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Fol = 

    /// <summary>Type of first order terms.</summary>
    /// 
    /// <example id="term-1">
    /// A formalization of the expression \(\sqrt{1- cos^2(x+y)}\):
    /// <code lang="fsharp">
    /// Fn("sqrt",[Fn("-",[Fn("1",[]);
    ///                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
    ///                                         Fn("2",[])])])])])
    /// </code>
    /// </example>
    type term =
      /// <summary>Variable.</summary>
      /// 
      /// <param name="Item">The variable name.</param>
      | Var of string
      /// <summary>Function.</summary>
      /// 
      /// <param name="Item1">The function name.</param>
      /// <param name="Item2">The function arguments.</param>
      | Fn of string * term list

    /// <summary>Type of atomic first order formulas.</summary>
    /// 
    /// <example id="fol-1">
    /// A formalization of the expression \(x + y &lt; z\):
    /// <code lang="fsharp">
    /// Atom(R("&lt;",[Fn("+",[Var "x"; Var "y"]); Var "z"]))
    /// </code>
    /// </example>
    type fol = 
      /// <summary>Predicate or relation.</summary>
      /// 
      /// <param name="Item1">The relation name.</param>
      /// <param name="Item2">The relation arguments.</param>
      | R of string * term list

    /// <summary>
    /// Applies a function <c>f</c> to all the top <em>terms</em> in a 
    /// fol formula <c>fm</c>, but otherwise leaves the structure unchanged.
    /// </summary>
    /// 
    /// <remarks>
    /// It is similar to <see cref='M:Calcolemus.Formulas.onatoms``1'/> 
    /// but specific for <see cref='T:Calcolemus.Fol.fol'/> formulas.
    /// </remarks>
    /// 
    /// <param name="f">The function to apply.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The result of applying <c>f</c>.
    /// </returns>
    /// 
    /// <example id="onformula-1">
    /// <code lang="fsharp">
    /// !! "P(x,f(z)) ==> Q(x)"
    /// |> onformula (function 
    ///     | Var x -> Var (x + "_1") 
    ///     | tm -> tm
    /// )
    /// </code>
    /// Evaluates to <c>`P(x_1,f(z)) ==> Q(x_1)`</c>.
    /// </example>
    /// 
    /// <category index="7">Syntax operations</category>
    val onformula:
      f: (term -> term) -> fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Checks if a string is a constant term. 
    /// </summary>
    /// 
    /// <remarks>
    /// Only numerals and the empty list constant <c>nil</c> are considered as 
    /// constants.
    /// </remarks>
    /// 
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// true, if the input is a constant term, otherwise false.
    /// </returns>
    /// 
    /// <example id="is_const_name-1">
    /// <code lang="fsharp">
    /// is_const_name "nil" // evaluates to true
    /// is_const_name "123" // evaluates to true
    /// is_const_name "x"   // evaluates to false
    /// </code>
    /// </example>
    /// 
    /// <category index="7">Syntax operations</category>
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
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// The term corresponding to the input, if this is valid.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown when the input string is not a valid term.</exception>
    /// 
    /// <example id="parset-1">
    /// <code lang="fsharp">
    /// parset "sqrt(1 - power(cos(x + y,2)))"
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// Fn("sqrt",[Fn("-",[Fn("1",[]);
    ///                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
    ///                                         Fn("2",[])])])])])
    /// </code>
    /// </example>
    /// 
    /// <example id="parset-2">
    /// <code lang="fsharp">
    /// parset "sqrt(1 - power(cos(x + y,2"
    /// </code>
    /// Throws <c>System.Exception: Closing bracket expected</c>.
    /// </example>
    /// 
    /// <category index="1">Parsing of terms</category>
    val parset: s: string -> term

    /// <summary>
    /// A convenient operator to make it easier to parse terms.
    /// </summary>
    /// 
    /// <remarks>
    /// It is just a shortcut to call <see cref='P:Calcolemus.Fol.parset'/>.
    /// </remarks>
    /// 
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// The term corresponding to the input, if this is valid.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown when the input string is not a valid term.</exception>
    /// 
    /// <example id="exclamation-exclamation-exclamation-1">
    /// <code lang="fsharp">
    /// !!! "sqrt(1 - power(cos(x + y,2)))"
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// Fn("sqrt",[Fn("-",[Fn("1",[]);
    ///                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
    ///                                         Fn("2",[])])])])])
    /// </code>
    /// </example>
    /// 
    /// <example id="exclamation-exclamation-exclamation-2">
    /// <code lang="fsharp">
    /// !!! "sqrt(1 - power(cos(x + y,2"
    /// </code>
    /// Throws <c>System.Exception: Closing bracket expected</c>.
    /// </example>
    /// 
    /// <category index="1">Parsing of terms</category>
    val (!!!) : s: string -> term

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
    /// Parses string into a fol formula.
    /// </summary>
    /// 
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// The fol formula corresponding to the input, if this is valid.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown when the input string is not a syntactically valid fol formula.</exception>
    /// 
    /// <example id="parse-1">
    /// <code lang="fsharp">
    /// parse "x + y &lt; z"
    /// </code>
    /// Evaluates to <c>Atom (R ("&lt;", [Fn ("+", [Var "x"; Var "y"]); Var "z"]))</c>.
    /// </example>
    /// 
    /// <example id="parse-2">
    /// <code lang="fsharp">
    /// parse "x + y"
    /// </code>
    /// Throws <c>System.Exception: Unparsed input: 2 tokens remaining in buffer.</c>
    /// </example>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val parse: s: string -> formula<fol>

    /// <summary>
    /// A convenient operator to make it easier to parse terms.
    /// </summary>
    /// 
    /// <remarks>
    /// It is just a shortcut to call <see cref='P:Calcolemus.Fol.parse'/>. 
    /// </remarks>
    /// 
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// The fol formula corresponding to the input, if this is valid.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown when the input string is not a syntactically valid fol formula.</exception>
    /// 
    /// <example id="exclamation-exclamation-1">
    /// <code lang="fsharp">
    /// parse "x + y &lt; z"
    /// </code>
    /// Evaluates to <c>Atom (R ("&lt;", [Fn ("+", [Var "x"; Var "y"]); Var "z"]))</c>.
    /// </example>
    /// 
    /// <example id="exclamation-exclamation-2">
    /// <code lang="fsharp">
    /// parse "x + y"
    /// </code>
    /// Throws <c>System.Exception: Unparsed input: 2 tokens remaining in buffer.</c>
    /// </example>
    /// 
    /// <category index="2">Parsing of formulas</category>
    val (!!) : s: string -> formula<fol>

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
    /// <category index="7">Syntax operations</category>
    val tsubst: sfn: func<string,term> -> tm: term -> term

    /// <summary>
    /// Creates a 'variant' of a variable name by adding prime characters to it 
    /// until it is distinct from some given list of variables to avoid.
    /// 
    /// <c>variant "x" ["x"; "y"]</c> returns <c>"x'"</c>.
    /// </summary>
    /// 
    /// <category index="7">Syntax operations</category>
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
    /// <category index="7">Syntax operations</category>
    val subst:
      subfn: func<string,term> ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Checks whether there would be variable capture if the bound variable 
    /// <c>x</c> is not renamed.
    /// </summary>
    /// 
    /// <category index="7">Syntax operations</category>
    val substq:
      subfn: func<string,term> ->
        quant: (string -> formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>