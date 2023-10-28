// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Lib.Fpf
open Formulas

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
    /// <category index="8">Other syntax operations</category>
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
    /// <category index="8">Other syntax operations</category>
    val is_const_name: s: string -> bool

    /// <summary>
    /// Parses an atomic term.
    /// </summary>
    /// 
    /// <category index="1">Parsing terms</category>
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
    /// <category index="1">Parsing terms</category>
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
    /// <category index="1">Parsing terms</category>
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
    /// <category index="1">Parsing terms</category>
    val (!!!) : s: string -> term

    /// <summary>
    /// A special recognizer for 'infix' atomic formulas like s &lt; t.
    /// </summary>
    /// 
    /// <category index="2">Parsing formulas</category>
    val parse_infix_atom:
      vs: string list -> inp: string list -> formula<fol> * string list

    /// <summary>
    /// Parses atomic fol 
    /// </summary>
    /// 
    /// <category index="2">Parsing formulas</category>
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
    /// <category index="2">Parsing formulas</category>
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
    /// <category index="2">Parsing formulas</category>
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
    /// Prints to the <c>stdout</c> the concrete syntax representation of a 
    /// term.
    /// </summary>
    /// 
    /// <param name="t">The input term.</param>
    /// 
    /// <example id="print_term-1">
    /// <code lang="fsharp">
    /// Fn("sqrt",[Fn("-",[Fn("1",[]);
    ///                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
    ///                                         Fn("2",[])])])])])
    /// |> print_term
    /// </code>
    /// After evaluation the text <c>``sqrt(1 - power(cos(x + y,2)))``</c> is 
    /// printed to the <c>stdout</c>.
    /// </example>
    /// 
    /// <category index="3">Printing terms</category>
    val inline print_term: t: term -> unit

    /// <summary>
    /// Returns the concrete syntax representation of a term.
    /// </summary>
    /// 
    /// <remarks>
    /// Use the interactive option
    /// <code lang="fsharp">
    /// fsi.AddPrinter sprint_term
    /// </code>
    /// to automatically return the concrete syntax representation of terms 
    /// when working in an F# interactive session.
    /// </remarks>
    /// 
    /// <param name="t">The input term.</param>
    /// <returns>
    /// The terms's concrete syntax representation.
    /// </returns>
    /// 
    /// <example id="sprint_term-1">
    /// <code lang="fsharp">
    /// Fn("sqrt",[Fn("-",[Fn("1",[]);
    ///                    Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
    ///                                         Fn("2",[])])])])])
    /// |> sprint_term
    /// </code>
    /// Evaluates to <c>"``sqrt(1 - power(cos(x + y,2)))``"</c>.
    /// </example>
    /// 
    /// <note>
    /// The opening and closing quotation symbols <c>&lt;&lt;||&gt;&gt;</c> 
    /// used in the Handbook for terms have been here replaced with 
    /// <c>````</c>.
    /// </note>
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
    /// Prints to the <c>stdout</c> the concrete syntax representation of a 
    /// fol formula.
    /// </summary>
    /// 
    /// <param name="f">The input formula.</param>
    /// 
    /// <example id="print_term-1">
    /// <code lang="fsharp">
    /// Forall
    /// ("x",
    ///  Forall
    ///    ("y",
    ///     Exists
    ///       ("z",
    ///        And
    ///          (Atom (R ("&lt;", [Var "x"; Var "z"])),
    ///           Atom (R ("&lt;", [Var "y"; Var "z"]))))))
    /// </code>
    /// After evaluation the text 
    /// <c>`forall x y. exists z. x &lt; z /\ y &lt; z`</c> is 
    /// printed to the <c>stdout</c>.
    /// </example>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline print_fol_formula: f: formula<fol> -> unit

    /// <summary>
    /// Returns the concrete syntax representation of a fol formula.
    /// </summary>
    /// 
    /// <remarks>
    /// Use the interactive option
    /// <code lang="fsharp">
    /// fsi.AddPrinter sprint_fol_formula
    /// </code>
    /// to automatically return the concrete syntax representation of fol 
    /// formulas when working in an F# interactive session.
    /// </remarks>
    /// 
    /// <param name="f">The input formula.</param>
    /// <returns>
    /// The formula's concrete syntax representation.
    /// </returns>
    /// 
    /// <example id="sprint_fol_formula-1">
    /// <code lang="fsharp">
    /// Forall
    /// ("x",
    ///  Forall
    ///    ("y",
    ///     Exists
    ///       ("z",
    ///        And
    ///          (Atom (R ("&lt;", [Var "x"; Var "z"])),
    ///           Atom (R ("&lt;", [Var "y"; Var "z"]))))))
    /// |> sprint_fol_formula
    /// </code>
    /// Evaluates to <c>"`forall x y. exists z. x &lt; z /\ y &lt; z`"</c>.
    /// </example>
    /// 
    /// <note>
    /// The opening and closing quotation symbols <c>&lt;&lt;||&gt;&gt;</c> 
    /// used in the Handbook for terms have been here replaced with 
    /// <c>``</c>.
    /// </note>
    /// 
    /// <category index="4">Printing formulas</category>
    val inline sprint_fol_formula: f: formula<fol> -> string

    /// <summary>
    /// Returns the value of a term <c>tm</c> in the interpretation (say \(M\)) 
    /// specified by the triplet <c>domain</c>, <c>func</c>, <c>pred</c> and 
    /// valuation <c>v</c>.
    /// </summary>
    /// 
    /// <param name="domain">The domain of the interpretation: the non-empty set \(D\) in which the value of each term lies.</param>
    /// <param name="func">The mapping of each \(n\)-ary function symbol to a function \(f_M : D^n \rightarrow D\).</param>
    /// <param name="pred">The mapping of each \(n\)-ary predicate symbol to a boolean function \(P_M  : D^n \rightarrow \{falso,vero\}\).</param>
    /// <param name="v">The valuation of the variables: an fpf that maps each variable to an element of the domain.</param>
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The element of the domain that corresponds to the term value in the 
    /// given interpretation and valuation.
    /// </returns>
    /// 
    /// <example id="termval-1">
    /// <code lang="fsharp">
    /// !!! "0" |> termval bool_interp undefined    // evaluates to false
    /// !!! "0" |> termval (mod_interp 3) undefined // evaluates to 0
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Semantics</category>
    val termval:
      domain: 'a * func: (string -> 'b list -> 'b) * pred: 'c ->
        v: func<string,'b> -> tm: term -> 'b

    /// <summary>
    /// Evaluates a fol formula <c>fm</c> in the interpretation (say \(M\)) 
    /// specified by the triplet <c>domain</c>, <c>func</c>, <c>pred</c> and 
    /// the variables valuation <c>v</c>.
    /// </summary>
    /// 
    /// <param name="domain">The domain of the interpretation: the non-empty set \(D\) in which the value of each term lies.</param>
    /// <param name="func">The mapping of each \(n\)-ary function symbol to a function \(f_M : D^n \rightarrow D\).</param>
    /// <param name="pred">The mapping of each \(n\)-ary predicate symbol to a boolean function \(P_M  : D^n \rightarrow \{falso,vero\}\).</param>
    /// <param name="v">The valuation of the variables: an fpf that maps each variable to an element of the domain.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The truth-value of the formula in the given interpretation and 
    /// valuation.
    /// </returns>
    /// 
    /// <example id="holds-1">
    /// <code lang="fsharp">
    /// let fm = !! @"forall x. (x = 0) \/ (x = 1)" 
    ///   
    /// holds bool_interp undefined fm    // evaluates to true
    /// holds (mod_interp 2) undefined fm // evaluates to true
    /// holds (mod_interp 3) undefined fm // evaluates to false
    /// </code>
    /// </example>
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
    /// <returns>
    /// <ul>
    /// <li>Domain \(\{true, false\}\)</li>
    /// <li>Function symbols mapping: 
    /// \(\text{0} \mapsto false, \text{1} \mapsto true, \text{+} \mapsto \lor, * \mapsto \land\)
    /// </li>
    /// <li>Predicate symbols mapping: \(\text{=} \mapsto =\)</li>
    /// </ul>
    /// </returns>
    /// 
    /// <category index="5">Sample interpretations</category>
    val bool_interp:
      bool list * (string -> bool list -> bool) * (string -> 'a list -> bool)
        when 'a: equality

    /// <summary>
    /// An arithmetic modulo <c>n</c> interpretation.
    /// </summary>
    /// 
    /// <param name="n">The input modulo.</param>
    /// 
    /// <returns>
    /// <ul>
    /// <li>Domain \(\{0,\ldots, n-1\}\)</li>
    /// <li>Function symbols mapping: \(\text{0} \mapsto 0, \text{1} \mapsto 1 \mod n, \text{+} \mapsto (x + y) \mod n, * \mapsto (x * y) \mod n\)
    /// </li>
    /// <li>Predicate symbols mapping: \(\text{=} \mapsto =\)</li>
    /// </ul>
    /// </returns>
    /// 
    /// <example id="mod_interp-1">
    /// <code lang="fsharp">
    /// !!! "0" |> termval (mod_interp 3) undefined                 // evaluates to 0
    /// !!! "1" |> termval (mod_interp 3) undefined                 // evaluates to 1
    /// !!! "1 + 1" |> termval (mod_interp 3) undefined             // evaluates to 2
    /// !!! "1 + 1 + 1" |> termval (mod_interp 3) undefined         // evaluates to 0
    /// !!! "1 + 1 + 1 + 1" |> termval (mod_interp 3) undefined     // evaluates to 1
    /// !!! "1 + 1 + 1 + 1 + 1" |> termval (mod_interp 3) undefined // evaluates to 2
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Sample interpretations</category>
    val mod_interp:
      n: int -> int list * (string -> int list -> int) * (string -> 'a list -> bool)
        when 'a: equality

    /// <summary>
    /// Returns the free variables in the term <c>tm</c>.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The list of free variables in the term <c>tm</c>
    /// </returns>
    /// 
    /// <example id="fvt-1">
    /// <code lang="fsharp">
    /// fvt !!!"x + f(y,z)"
    /// </code>
    /// Evaluates to <c>["x"; "y"; "z"]</c>.
    /// </example>
    /// 
    /// <category index="6">Free variables</category>
    val fvt: tm: term -> string list

    /// <summary>
    /// Returns all the variables in the fol formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of all the variables in the formula <c>fm</c>
    /// </returns>
    /// 
    /// <example id="var-1">
    /// <code lang="fsharp">
    /// var !!"forall x. x + f(y,z) > w"
    /// </code>
    /// Evaluates to <c>["w"; "x"; "y"; "z"]</c>.
    /// </example>
    /// 
    /// <category index="6">Free variables</category>
    val var: fm: formula<fol> -> string list

    /// <summary>
    /// Returns the free variables in the fol formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the free variables in the formula <c>fm</c>
    /// </returns>
    /// 
    /// <example id="fv-1">
    /// <code lang="fsharp">
    /// fv !!"forall x. x + f(y,z) > w"
    /// </code>
    /// Evaluates to <c>["w"; "y"; "z"]</c>.
    /// </example>
    /// 
    /// <category index="6">Free variables</category>
    val fv: fm: formula<fol> -> string list

    /// <summary>
    /// Universal closure of a formula.
    /// </summary>
    /// 
    /// <remarks>
    /// Binds every free variable in the formula with a universal 
    /// quantifier.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The universal closure of <c>fm</c>.
    /// </returns>
    /// 
    /// <example id="generalize-1">
    /// <code lang="fsharp">
    /// generalize !!"x + f(y,z) > w"
    /// </code>
    /// Evaluates to <c>`forall w x y z. x + f(y,z) > w`</c>.
    /// </example>
    /// 
    /// <category index="8">Other syntax operations</category>
    val generalize: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Applies the substitution function <c>subfn</c> to <c>tm</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Replaces every variable in <c>tm</c> that matches an argument of 
    /// <c>sfn</c> with its value.
    /// </remarks>
    /// 
    /// <param name="subfn">The fpf that contains the mapping for the variables to replace.</param>
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The term obtained from <c>tm</c> by replacing its variables with the 
    /// given mappings.
    /// </returns>
    /// 
    /// <example id="tsubst-1">
    /// <code lang="fsharp">
    /// !!!"x + f(y,z)" 
    /// |> tsubst (fpf ["x";"z"] [!!!"1";!!!"2"])
    /// </code>
    /// Evaluates to <c>``1 + f(y,2)``</c>.
    /// </example>
    /// 
    /// <category index="7">Substitution</category>
    val tsubst: subfn: func<string,term> -> tm: term -> term

    /// <summary>
    /// Creates a variant of the variable name <c>x</c> given a list of names 
    /// (<c>vars</c>) to avoid.
    /// </summary>
    /// 
    /// <remarks>
    /// Creates a 'variant' of a variable name by adding prime characters to it 
    /// until it is distinct from every element of <c>vars</c>.
    /// </remarks>
    /// 
    /// <param name="x">The input variable name.</param>
    /// <param name="vars">The list of names to avoid.</param>
    /// 
    /// <returns>
    /// The <c>x</c> itself it if is not contained in <c>vars</c>; otherwise, 
    /// the string obtained from <c>x</c> adding prime characters to it until 
    /// it is distinct from every element of <c>vars</c>.
    /// </returns>
    /// 
    /// <example id="variant-1">
    /// <code lang="fsharp">
    /// variant "x" ["y"; "z"]  // evaluates to "x"
    /// variant "x" ["x"; "y"]  // evaluates to "x'"
    /// variant "x" ["x"; "x'"] // evaluates to "x''"
    /// </code>
    /// </example>
    /// 
    /// <category index="7">Substitution</category>
    val variant: x: string -> vars: string list -> string

    /// <summary>
    /// Applies the substitution function <c>subfn</c> to <c>fm</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Bound variables will be renamed if necessary to avoid capture.
    /// </remarks>
    /// 
    /// <param name="subfn">The fpf that contains the mapping for the variables to replace.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula obtained from <c>fm</c> by replacing its variables with the 
    /// given mappings.
    /// </returns>
    /// 
    /// <example id="subst-1">
    /// <code lang="fsharp">
    /// subst ("y" |=> Var "x") !!"forall x. x = y"
    /// </code>
    /// Evaluates to <c>`forall x'. x' = x`</c>.
    /// </example>
    /// 
    /// <example id="subst-2">
    /// <code lang="fsharp">
    /// !!"forall x x'. x = y ==> x = x'"
    /// |> subst ("y" |=> Var "x")
    /// </code>
    /// Evaluates to <c>`forall x' x''. x' = x ==> x' = x''`</c>.
    /// </example>
    /// 
    /// <category index="7">Substitution</category>
    val subst:
      subfn: func<string,term> ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Checks for variable captures in quantified formulas substitutions.
    /// </summary>
    /// 
    /// <remarks>
    /// Checks whether there would be variable capture if the bound variable 
    /// <c>x</c> is not renamed and, if so, creates the appropriate 
    /// variants.
    /// <p></p>
    /// It is use to define the <see cref='M:Calcolemus.Fol.subst'/> quantified 
    /// formulas steps.
    /// </remarks>
    /// 
    /// <param name="subfn">The fpf that contains the mapping for the variables to replace.</param>
    /// <param name="quant">The quantification constructor to apply when reconstructing a quantified formula.</param>
    /// <param name="x">The variable to check.</param>
    /// <param name="p">The input formula.</param>
    /// 
    /// <returns>
    /// The reconstructed quantified formula with appropriate variables 
    /// variants if needed.
    /// </returns>
    /// 
    /// <example id="substq-1">
    /// <code lang="fsharp">
    /// substq ("y" |=> Var "x") mk_forall "x" !!"x = y"
    /// </code>
    /// Evaluates to <c>`forall x'. x' = x`</c>.
    /// </example>
    /// 
    /// <category index="7">Substitution</category>
    val substq:
      subfn: func<string,term> ->
        quant: (string -> formula<fol> -> formula<fol>) ->
        x: string -> p: formula<fol> -> formula<fol>