// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.Fpf
open FolAutomReas.Formulas

/// <summary>
/// Basic stuff for propositional logic: datatype, parsing and prettyprinting, 
/// syntax and semantics, normal forms.
/// </summary>
/// 
/// <note>
/// Many functions defined for propositional logic apply generically 
/// to any kind of <see cref='T:FolAutomReas.Formulas.formula`1'/> (and in 
/// particular also to <see cref='T:FolAutomReas.FolModule.fol'/> formulas).
/// <br />
/// A defined type for propositional variables 
/// (<see cref='T:FolAutomReas.Prop.prop'/>) is fixed here just to make 
/// experimentation with some of the operations easier.
/// </note>
/// 
/// <category index="3">Propositional logic</category>
module Prop = 

    /// <summary>
    /// Type of propositional variables.
    /// </summary>
    type prop = 
        /// <summary>
        /// Propositional variable.
        /// </summary>
        /// 
        /// <param name="Item">Name of the propositional variable.</param>
        | P of string

    /// <summary>
    /// Returns the name of a propositional variable.
    /// </summary>
    /// 
    /// <param name="p">The input propositional variable.</param>
    /// <returns>The name of the propositional variable.</returns>
    /// 
    /// <example id="pname-1">
    /// <code lang="fsharp">
    /// P "x" |> pname
    /// </code>
    /// Evaluates to <c>"x"</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val inline pname: p: prop -> string

    /// <summary>
    /// Parses atomic propositional variables.
    /// </summary>
    /// 
    /// <category index="1">Parsing</category>
    val parse_propvar:
      vs: 'a -> inp: string list -> formula<prop> * string list

    /// <summary>
    /// Parses a string in a propositional formula.
    /// </summary>
    /// 
    /// <category index="1">Parsing</category>
    val parse_prop_formula: (string -> formula<prop>)

    /// <summary>
    /// A convenient parsing operator to make it easier to parse prop formulas
    /// </summary>
    /// 
    /// <param name="s">The string to be parsed.</param>
    /// <returns>The parsed prop formula.</returns>
    /// 
    /// <example id="exclamation-greater-1">
    /// <code lang="fsharp">
    /// !> "p /\ q ==> q /\ r"
    /// </code>
    /// Evaluates to <c>Imp (And (Atom (P "p"), Atom (P "q")), And (Atom (P "q"), Atom (P "r")))</c>.
    /// </example>
    /// 
    /// <category index="1">Parsing</category>
    val (!>): s: string -> formula<prop>

    /// <summary>
    /// Prints a propositional variable using a TextWriter.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val fprint_propvar: sw: System.IO.TextWriter -> prec: 'a -> p: prop -> unit

    /// <summary>
    /// Prints a propositional variable.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline print_propvar: prec: 'a -> p: prop -> unit

    /// <summary>
    /// Returns a string representation of a propositional variable.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline sprint_propvar: prec: 'a -> p: prop -> string

    /// <summary>
    /// Prints a propositional formula using a TextWriter.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val fprint_prop_formula:
      sw: System.IO.TextWriter -> (formula<prop> -> unit)

    /// <summary>
    /// Prints a propositional formula.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline print_prop_formula: f: formula<prop> -> unit

    /// <summary>
    /// Returns a string representation of a propositional formula instead of 
    /// its abstract syntax tree.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline sprint_prop_formula: f: formula<prop> -> string

    /// <summary>
    /// Evaluates the truth-value of a formula given a valuation.
    /// </summary>
    /// 
    /// <remarks>
    /// A valuation is a function from the set of atoms to the set of 
    /// truth-values {<c>false</c>, <c>true</c>} (note that these are elements 
    /// of the metalanguage, in this case F#, that represent the semantic 
    /// concepts of truth-values and are not the same thing as <c>False</c> and 
    /// <c>True</c> which are element of the object language and so syntactic 
    /// elements).
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <param name="v">The input valuation.</param>
    /// <returns>
    /// <c>true</c>, if the formula is true in the give valuation; otherwise 
    /// <c>false</c>.
    /// </returns>
    /// 
    /// <example id="eval-1">
    /// The following evaluates the formula given a valuation that evaluates 
    /// <c>p</c> and <c>r</c> to <c>true</c> and <c>q</c> to <c>false</c>:
    /// <code lang="fsharp">
    /// eval (!>"p /\ q ==> q /\ r") 
    ///     (function P"p" -> true | P"q" -> false | P"r" -> true)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="4">Semantics</category>
    val eval: fm: formula<'a> -> v: ('a -> bool) -> bool

    /// <summary>
    /// Returns the set of atoms in a formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The set of atoms in the formula</returns>
    /// 
    /// <example id="atoms-1">
    /// <code lang="fsharp">
    /// !>"p /\ q ==> q /\ r" |> atoms
    /// </code>
    /// Evaluates to <c>[P "p"; P "q"; P "r"]</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val atoms: fm: formula<'a> -> 'a list when 'a: comparison

    /// <summary>
    /// Tests whether a function <c>subfn</c> returns <c>true</c> on all 
    /// possible valuations of the atoms <c>ats</c>, using an existing 
    /// valuation <c>v</c> for all other atoms.
    /// </summary>
    /// 
    /// <param name="subfn">A function that given a valuation return true or false.</param>
    /// <param name="v">The default valuation for other atoms.</param>
    /// <param name="ats">The list of atoms on which to test all possibile valuations.</param>
    /// 
    /// <returns>true, if <c>subfn</c> returns <c>true</c> on all 
    /// possible valuations of the atoms <c>ats</c> with 
    /// valuation <c>v</c> for all other atoms; otherwise false.</returns>
    /// 
    /// <example id="onallvaluations-1">
    /// <c>eval True</c> returns <c>true</c> on all valuations:
    /// <code lang="fsharp">
    /// onallvaluations (eval True) (fun _ -> false) []
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="4">Semantics</category>
    val onallvaluations:
      subfn: (('a -> bool) -> bool) -> v: ('a -> bool) -> ats: 'a list -> bool
        when 'a: equality

    /// <summary>
    /// Prints the truth table of a formula <c>fm</c> using a TextWriter.
    /// </summary>
    /// 
    /// <category index="5">Truth tables</category>
    val fprint_truthtable:
      sw: System.IO.TextWriter -> fm: formula<prop> -> unit

    /// <summary>
    /// Prints the truth table of the propositional formula <c>f</c>.
    /// </summary>
    /// 
    /// <category index="5">Truth tables</category>
    val inline print_truthtable: f: formula<prop> -> unit

    /// <summary>
    /// Returns a string representation of the truth table of the propositional 
    /// formula <c>f</c>.
    /// </summary>
    /// 
    /// <category index="5">Truth tables</category>
    val inline sprint_truthtable: f: formula<prop> -> string

    /// <summary>
    /// Checks if a formula is a tautology at the propositional level.
    /// </summary>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val tautology: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is unsatisfiable at the propositional level.
    /// </summary>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val unsatisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is satisfiable at the propositional level.
    /// </summary>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val satisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Returns the formula resulting from applying the substitution 
    /// <c>sbfn</c> to the input formula.
    /// </summary>
    /// 
    /// <category index="3">Syntax operations</category>
    val psubst:
      subfn: func<'a,formula<'a>> ->
        (formula<'a> -> formula<'a>) when 'a: comparison

    /// <summary>
    /// Returns the dual of the input formula: i.e. the result of 
    /// systematically exchanging \(\land\) with \(\lor\) and also 
    /// \(\top\) with \(\bot\).
    /// </summary>
    /// 
    /// <category index="3">Syntax operations</category>
    val dual: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine (but just at the first 
    /// level of the input formula) eliminating eliminating the basic 
    /// propositional constants <c>False</c> and <c>True</c> and the double 
    /// negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It eliminates the basic propositional 
    /// constants \(\bot\) and \(\top\). 
    /// (Whenever \(\bot\) and \(\top\) occur in combination, there is always a 
    /// a tautology justifying the equivalence with a simpler formula, e.g. 
    /// \(\bot \land p \Leftrightarrow \bot\), 
    /// \(\top \lor p \Leftrightarrow p\), 
    /// \(p \Rightarrow \bot \Leftrightarrow \neg p\).)
    /// 
    /// At the same time, it also eliminates double negations \(\neg \neg p\).
    /// </remarks>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify1: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine eliminating eliminating 
    /// the basic propositional constants <c>False</c> and <c>True</c> and the 
    /// double negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Completes the simplification routine psimplify1 applying it at every 
    /// level of the formula.
    /// </remarks>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Checks if a literal is negative.
    /// </summary>
    /// 
    /// <category index="8">Litterals</category>
    val negative: _arg1: formula<'a> -> bool

    /// <summary>
    /// Checks if a literal is positive.
    /// </summary>
    /// 
    /// <category index="8">Litterals</category>
    val positive: lit: formula<'a> -> bool

    /// <summary>Changes a literal into its contrary.</summary>
    /// 
    /// <category index="8">Litterals</category>
    val negate: _arg1: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into its negation normal form without simplifying it.
    /// </summary>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nnf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into its negation normal and applies it the routine 
    /// simplification <see cref='M:FolAutomReas.Prop.psimplify``1'/>.
    /// </summary>
    /// 
    /// <category index="9">Negation Normal Form</category> 
    val nnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Simply pushes negations in the input formula <c>fm</c> down to the 
    /// level of  atoms without simplifying it.
    /// </summary>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Simply pushes negations in the input formula <c>fm</c> down to the 
    /// level of atoms and applies it the routine simplification 
    /// <see cref='M:FolAutomReas.Prop.psimplify``1'/>.
    /// </summary>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Creates a conjunction of all the formulas in the input list <c>l</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_conj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Creates a disjunction of all the formulas in the input list <c>l</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_disj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Given a list of formulas <c>pvs</c>, makes a conjunction of these 
    /// formulas and their negations according to whether each is satisfied by 
    /// the valuation <c>v</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val mk_lits:
      pvs: formula<'a> list -> v: ('a -> bool) -> formula<'a>
        when 'a: equality

    /// <summary>
    /// A close analogue of 
    /// <see cref='M:FolAutomReas.Prop.onallvaluations``1'/> that collects 
    /// into a list the valuations for which <c>subfn</c> holds.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val allsatvaluations:
      subfn: (('a -> bool) -> bool) ->
        v: ('a -> bool) -> pvs: 'a list -> ('a -> bool) list when 'a: equality

    /// <summary>
    /// Transforms a formula <c>fm</c> in disjunctive normal form using truth 
    /// tables.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val dnf_by_truth_tables:
      fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Applies the distributive laws to the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val distrib_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Transforms the input formula <c>fm</c> in disjunctive normal form.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val rawdnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Applies the distributive laws of propositional connectives \(\land\) 
    /// and \(\lor\) using a list representation of the input formulas 
    /// <c>s1</c> and <c>s2</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val distrib:
      s1: 'a list list -> s2: 'a list list -> 'a list list when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in disjunctive normal form 
    /// using a list representation of the formula as a set of sets: 
    /// <c>p /\ q \/ ~ p /\ r</c> as <c>[[p; q]; [~ p; r]]</c>.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val purednf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Filters out trivial disjuncts (in this guise, contradictory).
    /// </summary>
    /// 
    /// <remarks>
    /// Check if there are complementary literals of the form p and ~ p 
    /// in the same list.
    /// </remarks>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val trivial: lits: formula<'a> list -> bool when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in disjunctive normal form 
    /// using a list representation of the formula as a set of sets: 
    /// <c>p /\ q \/ ~ p /\ r</c> as <c>[[p; q]; [~ p; r]]</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It exploits the list of list representation filtering out trivial 
    /// complementary literals and subsumed ones (with subsumption checking, 
    /// done very naively: quadratic).
    /// </remarks>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val simpdnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in disjunctive normal form.
    /// </summary>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val dnf: fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in conjunctive normal form 
    /// by using <see cref='M:FolAutomReas.Prop.purednf``1'/>.
    /// </summary>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val purecnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in conjunctive normal form 
    /// using a list representation of the formula as a set of sets: 
    /// <c>p \/ q /\ ~ p \/ r</c> as <c>[[p; q]; [~ p; r]]</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// It exploits the list of list representation filtering out trivial 
    /// complementary literals and subsumed ones.
    /// </remarks>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val simpcnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms the input formula <c>fm</c> in conjunctive normal form.
    /// </summary>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val cnf: fm: formula<'a> -> formula<'a> when 'a: comparison
