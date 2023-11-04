// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

/// <summary>
/// Polymorphic type of formulas with parser and printer. 
/// </summary>
/// 
/// <category index="2">Polymorphic formulas</category>
module Formulas =

    /// <summary>
    /// Abstract syntax tree of polymorphic type of formulas.
    /// </summary>
    /// 
    /// <category index="1">Abstract Syntax Tree</category>
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
        /// <param name="Item">The type of the formula: see <see cref='T:Calcolemus.Prop.prop'/> and <see cref='T:Calcolemus.FolModule.fol'/>.</param>
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
    /// 
    /// <category index="2">Parsing</category>
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
    /// 
    /// <category index="2">Parsing</category>
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
    /// 
    /// <category index="2">Parsing</category>
    val parse_formula:
      ifn: (string list -> string list -> formula<'a> * string list) *
      afn: (string list -> string list -> formula<'a> * string list) ->
        vs: string list -> inp: string list -> formula<'a> * string list

    /// <summary>
    /// Modifies a basic printer to have a kind of boxing 'wrapped' around it.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val fbracket:
      tw: System.IO.TextWriter ->
        p: bool -> n: 'a -> f: ('b -> 'c -> unit) -> x: 'b -> y: 'c -> unit

    /// <summary>
    /// Formula destructor for quantified formulas.
    /// </summary>
    /// 
    /// <remarks>
    /// It breaks apart a quantified formula \(\forall xy\ldots z.\ p\) into 
    /// a pair of the list of its quantified variables and body 
    /// \(([x,y,\ldots,z], p)\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed quantified formula.</param>
    /// 
    /// <returns>
    /// The pair of the list of its quantified variables and body, if the input 
    /// formula is a universally quantified formula. If the formula is not a 
    /// universally quantified one, the list of variables is empty.
    /// </returns>
    /// 
    /// <example id="strip_quant-1">
    /// <code lang="fsharp">
    /// Forall ("y", Forall ("x", Atom "p")) |> strip_quant
    /// </code>
    /// Evaluates to <c>(["y"; "x"], Atom "p")</c>.
    /// </example>
    /// 
    /// <example id="strip_quant-2">
    /// <code lang="fsharp">
    /// Atom "p" |> strip_quant
    /// </code>
    /// Evaluates to <c>([], Atom "p")</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val strip_quant: fm: formula<'a> -> string list * formula<'a>

    /// <summary>
    /// Printing parametrized by a function <c>pfn</c> to print atoms.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val fprint_formula:
      tw: System.IO.TextWriter -> pfn: (int -> 'a -> unit) -> (formula<'a> -> unit)

    /// <summary>
    /// Main toplevel printer. It just adds the guillemot-style quotations 
    /// round the formula so that it looks like the quoted formulas we parse.
    /// </summary>
    /// 
    /// <note>
    /// The quotations symbols <c>&lt;&lt;&gt;&gt;</c> for formulas have been 
    /// replaced with <c>``</c> that are more concise and cause less problems 
    /// in writing documentation.
    /// 
    /// Even if in F# there isn't an equivalent of OCaml's quotations 
    /// expansions, it is convenient to keep quotation symbols to delimit 
    /// formulas and terms to make it easier identifying.
    /// </note>
    /// 
    /// <category index="3">Prettyprinting</category>
    val fprint_qformula:
      tw: System.IO.TextWriter ->
        pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// <summary>
    /// Main toplevel latex printer.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val fprint_latex_qformula:
      tw: System.IO.TextWriter ->
        pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// <summary>
    /// Prints a formula <c>fm</c> using a function <c>pfn</c> to print atoms.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val inline print_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// <summary>
    /// Returns a string representation of a formula <c>fm</c> using a function 
    /// <c>pfn</c> to print atoms.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val inline sprint_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

    /// <summary>
    /// Prints a formula <c>fm</c> using a function <c>pfn</c> to print atoms.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val inline print_qformula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

    /// <summary>
    /// Returns a string representation of a formula <c>fm</c> using a function 
    /// <c>pfn</c> to print atoms.
    /// </summary>
    /// 
    /// <category index="3">Prettyprinting</category>
    val inline sprint_qformula:
      pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

    /// <summary>
    /// Constructs a conjunction.
    /// </summary>
    /// 
    /// <param name="p">The first formula to conjunct.</param>
    /// <param name="q">The second formula to conjunct.</param>
    /// <returns>The conjunction of the input formulas.</returns>
    /// 
    /// <example id="mk_and-1">
    /// <code lang="fsharp">
    /// mk_and (Atom "a") (Atom "b")
    /// </code>
    /// Evaluates to <c>And (Atom "a", Atom "b")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_and: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a disjunction.
    /// </summary>
    /// 
    /// <param name="p">The first formula to disjunct.</param>
    /// <param name="q">The second formula to disjunct.</param>
    /// <returns>The disjunction of the input formulas.</returns>
    /// 
    /// <example id="mk_or-1">
    /// <code lang="fsharp">
    /// mk_or (Atom "a") (Atom "b")
    /// </code>
    /// Evaluates to <c>Or (Atom "a", Atom "b")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_or: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs an implication.
    /// </summary>
    /// 
    /// <param name="p">The antecedent of the implication to construct.</param>
    /// <param name="q">The consequent of the implication to construct.</param>
    /// <returns>The implication with <c>p</c> as antecedent and <c>q</c> as consequent.</returns>
    /// 
    /// <example id="mk_imp-1">
    /// <code lang="fsharp">
    /// mk_imp (Atom "a") (Atom "b")
    /// </code>
    /// Evaluates to <c>Imp (Atom "a", Atom "b")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_imp: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a logical equivalence.
    /// </summary>
    /// 
    /// <param name="p">The first member of the equivalence to construct.</param>
    /// <param name="q">The second member of the equivalence to construct.</param>
    /// <returns>The equivalence of <c>p</c> with <c>q</c>.</returns>
    /// 
    /// <example id="mk_iff-1">
    /// <code lang="fsharp">
    /// mk_iff (Atom "a") (Atom "b")
    /// </code>
    /// Evaluates to <c>Iff (Atom "a", Atom "b")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_iff: p: formula<'a> -> q: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a universal quantification.
    /// </summary>
    /// 
    /// <param name="x">The variable to bind.</param>
    /// <param name="p">The formula to bind.</param>
    /// 
    /// <returns>The <c>p</c> formula universally quantified on <c>x</c>.</returns>
    /// 
    /// <example id="mk_forall-1">
    /// <code lang="fsharp">
    /// mk_forall "x" (Atom "a")
    /// </code>
    /// Evaluates to <c>Forall ("x", Atom "a")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_forall: x: string -> p: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs an existential quantification.
    /// </summary>
    /// 
    /// <param name="x">The variable to bind.</param>
    /// <param name="p">The formula to bind.</param>
    /// 
    /// <returns>The <c>p</c> formula existentially quantified on <c>x</c>.</returns>
    /// 
    /// <example id="mk_exists-1">
    /// <code lang="fsharp">
    /// mk_exists "x" (Atom "a")
    /// </code>
    /// Evaluates to <c>Exists ("x", Atom "a")</c>.
    /// </example>
    /// 
    /// <category index="5">Constructors</category>
    val inline mk_exists: x: string -> p: formula<'a> -> formula<'a>

    /// <summary>
    /// Formula destructor for logical equivalences.
    /// </summary>
    /// 
    /// <remarks>
    /// It breaks apart a logical equivalence \(p \Leftrightarrow q\) into 
    /// the pair of its members \((p, q)\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed logical equivalence.</param>
    /// 
    /// <returns>
    /// The pair of the equivalence members, if the input formula is a logical 
    /// equivalence.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_imp</c>, when <c>fm</c> is not a logical equivalence.</exception>
    /// 
    /// <example id="dest_iff-1">
    /// <code lang="fsharp">
    /// Iff (Atom "p", Atom "q") |> dest_iff
    /// </code>
    /// Evaluates to <c>(Atom "p", Atom "q")</c>.
    /// </example>
    /// 
    /// <example id="dest_iff-2">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> dest_iff
    /// </code>
    /// Throws to <c>System.Exception: dest_iff</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val dest_iff: fm: formula<'a> -> formula<'a> * formula<'a>

    /// <summary>
    /// Formula destructor for conjunctions.
    /// </summary>
    /// 
    /// <remarks>
    /// It breaks apart a conjunction \(p \land q\) into 
    /// the pair of its conjuncts \((p, q)\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed conjunction.</param>
    /// 
    /// <returns>
    /// The pair of the conjuncts, if the input formula is a conjunction.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_and</c>, when <c>fm</c> is not a conjunction.</exception>
    /// 
    /// <example id="dest_and-1">
    /// <code lang="fsharp">
    /// And (Atom "p", Atom "q") |> dest_and
    /// </code>
    /// Evaluates to <c>(Atom "p", Atom "q")</c>.
    /// </example>
    /// 
    /// <example id="dest_and-2">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> dest_and
    /// </code>
    /// Throws to <c>System.Exception: dest_and</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val dest_and: fm: formula<'a> -> formula<'a> * formula<'a>

    /// <summary>
    /// Formula destructor for repeated conjunctions.
    /// </summary>
    /// 
    /// <remarks>
    /// Recursively breaks apart repeated conjunctions 
    /// \(p \land q \land r \cdots \) into the list of all the conjuncts 
    /// \([p, q, r, \ldots]\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed conjunction.</param>
    /// 
    /// <returns>
    /// The list of the conjuncts, if the input formula is a conjunction; 
    /// otherwise a list containing the input formula unchanged.
    /// </returns>
    /// 
    /// <example id="conjuncts-1">
    /// <code lang="fsharp">
    /// And (And (Atom "p", Atom "q"), Atom "r")
    /// |> conjuncts
    /// </code>
    /// Evaluates to <c>[Atom "p"; Atom "q"; Atom "r"]</c>.
    /// </example>
    /// 
    /// <example id="conjuncts-2">
    /// <code lang="fsharp">
    /// Imp (Atom "a", Atom "b")
    /// |> conjuncts
    /// </code>
    /// Evaluates to <c>[Imp (Atom "a", Atom "b")]</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val conjuncts: fm: formula<'a> -> formula<'a> list

    /// <summary>
    /// Formula destructor for disjunctions.
    /// </summary>
    /// 
    /// <remarks>
    /// It breaks apart a disjunction \(p \lor q\) into 
    /// the pair of its conjuncts \((p, q)\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed disjunction.</param>
    /// 
    /// <returns>
    /// The pair of the disjuncts, if the input formula is a disjunction.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_or</c>, when <c>fm</c> is not a disjunction.</exception>
    /// 
    /// <example id="dest_or-1">
    /// <code lang="fsharp">
    /// Or (Atom "p", Atom "q") |> dest_or
    /// </code>
    /// Evaluates to <c>(Atom "p", Atom "q")</c>.
    /// </example>
    /// 
    /// <example id="dest_or-2">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> dest_or
    /// </code>
    /// Throws to <c>System.Exception: dest_or</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val dest_or: fm: formula<'a> -> formula<'a> * formula<'a>

    /// <summary>
    /// Formula destructor for repeated disjunctions.
    /// </summary>
    /// 
    /// <remarks>
    /// Recursively breaks apart repeated disjunctions 
    /// \(p \lor q \lor r \cdots \) into the list of all the disjuncts 
    /// \([p, q, r, \ldots]\).
    /// </remarks>
    /// 
    /// <param name="fm">The supposed disjunction.</param>
    /// 
    /// <returns>
    /// The list of the disjuncts, if the input formula is a disjunction; 
    /// otherwise a list containing the input formula unchanged.
    /// </returns>
    /// 
    /// <example id="disjuncts-1">
    /// <code lang="fsharp">
    /// Or (Or (Atom "p", Atom "q"), Atom "r")
    /// |> disjuncts
    /// </code>
    /// Evaluates to <c>[Atom "p"; Atom "q"; Atom "r"]</c>.
    /// </example>
    /// 
    /// <example id="disjuncts-2">
    /// <code lang="fsharp">
    /// Imp (Atom "a", Atom "b")
    /// |> disjuncts
    /// </code>
    /// Evaluates to <c>[Imp (Atom "a", Atom "b")]</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val disjuncts: fm: formula<'a> -> formula<'a> list

    /// <summary>
    /// Formula destructor for implications.
    /// </summary>
    /// 
    /// <remarks>
    /// It breaks apart an implications \(p \Rightarrow q\) into 
    /// the pair of its antecedent and consequent \((p, q)\). 
    /// 
    /// See also: <see cref='M:Calcolemus.Formulas.antecedent``1'/>, 
    /// <see cref='M:Calcolemus.Formulas.consequent``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The supposed implication.</param>
    /// 
    /// <returns>
    /// The pair of the antecedent and consequent, if the input formula is 
    /// an implication.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_imp</c>, when <c>fm</c> is not an implication.</exception>
    /// 
    /// <example id="dest_imp-1">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> dest_imp
    /// </code>
    /// Evaluates to <c>(Atom "p", Atom "q")</c>.
    /// </example>
    /// 
    /// <example id="dest_imp-2">
    /// <code lang="fsharp">
    /// And (Atom "p", Atom "q") |> dest_imp
    /// </code>
    /// Throws to <c>System.Exception: dest_imp</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val dest_imp: fm: formula<'a> -> formula<'a> * formula<'a>

    /// <summary>
    /// Returns the antecedent of an implication.
    /// </summary>
    /// 
    /// <remarks>
    /// See also: <see cref='M:Calcolemus.Formulas.dest_imp``1'/>, 
    /// <see cref='M:Calcolemus.Formulas.consequent``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The antecedent, if the input formula is an implication.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_imp</c>, when <c>fm</c> is not an implication.</exception>
    /// 
    /// <example id="antecedent-1">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> antecedent
    /// </code>
    /// Evaluates to <c>Atom "p"</c>.
    /// </example>
    /// 
    /// <example id="antecedent-2">
    /// <code lang="fsharp">
    /// And (Atom "p", Atom "q") |> antecedent
    /// </code>
    /// Throws to <c>System.Exception: dest_imp</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val inline antecedent: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Returns the consequent of an implication.
    /// </summary>
    /// 
    /// <remarks>
    /// See also: <see cref='M:Calcolemus.Formulas.dest_imp``1'/>, 
    /// <see cref='M:Calcolemus.Formulas.antecedent``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The consequent, if the input formula is an implication.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_imp</c>, when <c>fm</c> is not an implication.</exception>
    /// 
    /// <example id="consequent-1">
    /// <code lang="fsharp">
    /// Imp (Atom "p", Atom "q") |> consequent
    /// </code>
    /// Evaluates to <c>Atom "q"</c>.
    /// </example>
    /// 
    /// <example id="consequent-2">
    /// <code lang="fsharp">
    /// And (Atom "p", Atom "q") |> consequent
    /// </code>
    /// Throws to <c>System.Exception: dest_imp</c>.
    /// </example>
    /// 
    /// <category index="4">Destructors</category>
    val inline consequent: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Applies a function <c>f</c> to all the attributes of the atoms in a 
    /// formula <c>fm</c>, but otherwise leaves the structure unchanged. It can 
    /// be used, for example, to perform systematic replacement of one 
    /// particular atomic proposition by another formula.
    /// </summary>
    /// 
    /// <remarks>
    /// It is for <see cref='T:Calcolemus.Formulas.formula`1'/> what 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.map``2'/> is 
    /// for <see cref='T:Microsoft.FSharp.Collections.list`1'/>.
    /// </remarks>
    /// 
    /// <param name="f">The function to apply to the attributes of the formula's atoms.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The formula with the atoms remapped by the function.</returns>
    /// 
    /// <example id="onatoms-1">
    /// <code lang="fsharp">
    /// And (Atom 1, Atom 2)
    /// |> onatoms (fun x -> Atom (x * 5))
    /// </code>
    /// Evaluates to <c>And (Atom 5, Atom 10)</c>.
    /// </example>
    /// 
    /// <category index="6">Syntax operations</category>
    val onatoms: f: ('a -> formula<'a>) -> fm: formula<'a> -> formula<'a>

    /// <summary>Applies a function to each attributes of formula's atoms, 
    /// threading an accumulator argument through the computation. Take the 
    /// third argument, and apply the function to it and the attribute of the 
    /// first atom of the formula. Then feed this result into the function 
    /// along with the second atom and so on. Return the final result. If the 
    /// input function is <c>f</c> and the atoms are <c>Atom i0...Atom iN</c> 
    /// then computes <c>f (... (f s i0) i1 ...) iN</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It is for <see cref='T:Calcolemus.Formulas.formula`1'/> what 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.fold``2'/> is 
    /// for <see cref='T:Microsoft.FSharp.Collections.list`1'/>.
    /// </remarks>
    /// 
    /// <note>
    /// Although the inversion between the second and third arguments would 
    /// seem to make this function similar to 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.foldBack``2'/>, 
    /// it actually corresponds to 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.fold``2'/> 
    /// because the folder function takes the accumulator as the first argument 
    /// and the atom's argument as the second.
    /// </note>
    /// 
    ///
    /// <param name="folder">The function to update the state given the input elements.</param>
    /// <param name="fm">The input formula.</param>
    /// <param name="state">The initial state.</param>
    ///
    /// <returns>The final state value.</returns>
    /// 
    /// <example id="overatoms-1">
    /// <code lang="fsharp">
    /// And (Atom 1, Atom 2)
    /// |> fun fm -> overatoms (fun acc x -> acc + x) fm 0
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    /// 
    /// <category index="6">Syntax operations</category>
    val overatoms: folder: ('T -> 'State -> 'State) -> fm: formula<'T> -> state: 'State -> 'State

    /// <summary>
    /// For each attributes of the formula's atoms, applies the given function. 
    /// Concatenates all the results and converts them to a set to remove 
    /// duplicates.
    /// </summary>
    /// 
    /// <remarks>
    /// Except for the final removing of duplicates, it is for 
    /// <see cref='T:Calcolemus.Formulas.formula`1'/> what 
    /// <see cref='M:Microsoft.FSharp.Collections.ListModule.collect``2'/> is 
    /// for <see cref='T:Microsoft.FSharp.Collections.list`1'/>.
    /// </remarks>
    ///
    /// <param name="mapping">The function to transform each attributes of the input formula's atoms into a sublist to be concatenated.</param>
    /// <param name="fm">The input formula.</param>
    ///
    /// <returns>The concatenation without duplicates of the transformed sublists.</returns>
    /// 
    /// <example id="atom_union-1">
    /// <code lang="fsharp">
    /// And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
    /// |> atom_union (fun x -> [x])
    /// </code>
    /// Evaluates to <c>[1; 2; 3; 4]</c>.
    /// </example>
    /// 
    /// <example id="atom_union-2">
    /// <code lang="fsharp">
    /// And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
    /// |> atom_union (fun x -> [1..x]
    /// </code>
    /// The sample evaluates to <c>[1; 2; 3; 4]</c> and not to <c>[1;   1; 2;   1; 2; 3;   1; 2; 3; 4]</c> since duplicates are removed;
    /// </example>
    /// 
    /// <category index="6">Syntax operations</category>
    val atom_union:
      mapping: ('T -> 'U list) -> fm: formula<'T> -> 'U list when 'U: comparison