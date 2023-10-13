// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.Fpf
open FolAutomReas.Formulas

/// <summary>
/// Basic stuff for propositional logic: datatype, parsing and printing. 
/// </summary>
/// 
/// <category index="3">Propositional logic</category>
module Prop = 

    /// <summary>
    /// Type of propositional variables.
    /// </summary>
    /// 
    /// <note>
    /// Many functions defined for propositional logic apply generically 
    /// to any kind of <see cref='T:FolAutomReas.Formulas.formula`1'/> and in 
    /// particular for <see cref='T:FolAutomReas.FolModule.fol'/> formulas. 
    /// 
    /// A defined type for primitive propositions is fixed here to make 
    /// experimentation with some of the operations easier.
    /// </note>
    type prop = 
        /// <summary>
        /// Propositional variable.
        /// </summary>
        /// 
        /// <param name="Item">Name of the propositional variable.</param>
        | P of string

    /// <summary>
    /// Returns then name of a propositional variable.
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
    /// Interpretation of  formulas
    /// </summary>
    /// 
    /// <category index="4">Semantics</category>
    val eval: fm: formula<'a> -> v: ('a -> bool) -> bool

    /// <summary>
    /// Return the set of atoms in a formula (regardless of whether it is a 
    /// propositional or first-order formula)
    /// </summary>
    /// 
    /// <category index="3">Syntax operations</category>
    val atoms: fm: formula<'a> -> 'a list when 'a: comparison

    /// <summary>
    /// Tests whether a function <c>subfn</c> returns <c>true</c> on all 
    /// possible valuations of the atoms <c>ats</c>, using an existing 
    /// valuation <c>v</c> for all other atoms.
    /// </summary>
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
    /// Returns the dual of the input formula <c>fm</c>: i.e. the result of 
    /// systematically exchanging \(\land\)'s with \(\lor\)'s and also 
    /// \(\top\) 's with \(\bot\)'s.
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

    /// Changes a literal into its contrary.
    val negate: _arg1: formula<'a> -> formula<'a>

    /// Changes a formula into its negation normal form without simplifying it.
    val nnf_naive: fm: formula<'a> -> formula<'a>

    /// Changes a formula into its negation normal and applies it the routine 
    /// simplification `psimplify`.
    val nnf: fm: formula<'a> -> formula<'a>

    /// Simply pushes negations in the input formula `fm` down to the level of  atoms without simplifying it.
    val nenf_naive: fm: formula<'a> -> formula<'a>

    /// Simply pushes negations in the input formula `fm` down to the level of 
    /// atoms and applies it the routine simplification `psimplify`.
    val nenf: fm: formula<'a> -> formula<'a>

    /// Creates a conjunction of all the formulas in the input list `l`.
    val list_conj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// Creates a disjunction of all the formulas in the input list `l`.
    val list_disj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// Given a list of formulas `pvs`, makes a conjunction of these formulas and 
    /// their negations according to whether each is satis?ed by the valuation `v`.
    val mk_lits:
      pvs: formula<'a> list -> v: ('a -> bool) -> formula<'a>
        when 'a: equality

    /// A close analogue of `onallvaluations` that collects the valuations for 
    /// which `subfn` holds into a list.
    val allsatvaluations:
      subfn: (('a -> bool) -> bool) ->
        v: ('a -> bool) -> pvs: 'a list -> ('a -> bool) list when 'a: equality

    /// Transforms a formula `fm` in disjunctive normal form using truth tables.
    val dnf_by_truth_tables:
      fm: formula<'a> -> formula<'a> when 'a: comparison

    /// Applies the distributive laws to the input formula `fm`.
    val distrib_naive: fm: formula<'a> -> formula<'a>

    /// Transforms the input formula `fm` in disjunctive normal form.
    val rawdnf: fm: formula<'a> -> formula<'a>

    /// Applies the distributive laws of propositional connectives `/\` and 
    /// `\/` using a list representation of the formulas `s1` and `s2` on which 
    /// to operate.
    val distrib:
      s1: 'a list list -> s2: 'a list list -> 'a list list when 'a: comparison

    /// Transforms the input formula `fm` in disjunctive normal form using 
    /// (internally) a list representation of the formula as a set of sets. 
    /// `p /\ q \/ ~ p /\ r` as `[[p; q]; [~ p; r]]`
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
    val trivial: lits: formula<'a> list -> bool when 'a: comparison

    /// Transforms the input formula `fm` in a list of list representation of  
    /// disjunctive normal form. It exploits the list of list representation 
    /// filtering out trivial complementary literals and subsumed ones.
    /// 
    /// With subsumption checking, done very naively (quadratic). 
    val simpdnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// Transforms the input formula `fm` in disjunctive normal form.
    val dnf: fm: formula<'a> -> formula<'a> when 'a: comparison

    /// Transforms the input formula `fm` in conjunctive normal form 
    /// by using `purednf`.
    val purecnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// Transforms the input formula `fm` in a list of list representation of 
    /// conjunctive normal form. It exploits the list of list representation 
    /// filtering out trivial complementary literals and subsumed ones.
    val simpcnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// Transforms the input formula `fm` in conjunctive normal form.
    val cnf: fm: formula<'a> -> formula<'a> when 'a: comparison
