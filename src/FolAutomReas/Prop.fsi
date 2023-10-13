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

    /// Type of primitive propositions indexed by names.
    type prop = | P of string

    /// Returns constant or variable name of a propositional formula.
    val inline pname: prop -> string

    /// Parses atomic propositions.
    val parse_propvar:
      vs: 'a -> inp: string list -> formula<prop> * string list

    /// Parses a string in a propositional formula.
    val parse_prop_formula: (string -> formula<prop>)

    /// Prints a prop variable using a TextWriter.
    val fprint_propvar: sw: System.IO.TextWriter -> prec: 'a -> p: prop -> unit

    /// Prints a prop variable
    val inline print_propvar: prec: 'a -> p: prop -> unit

    /// Returns a string representation of a prop variable.
    val inline sprint_propvar: prec: 'a -> p: prop -> string

    /// Prints a prop formula using a TextWriter.
    val fprint_prop_formula:
      sw: System.IO.TextWriter -> (formula<prop> -> unit)

    /// Prints a prop formula
    val inline print_prop_formula: f: formula<prop> -> unit

    /// Returns a string representation of a propositional formula instead of 
    /// its abstract syntax tree..
    val inline sprint_prop_formula: f: formula<prop> -> string

    /// Interpretation of  
    val eval: fm: formula<'a> -> v: ('a -> bool) -> bool

    /// Return the set of propositional variables in a formula.
    val atoms: fm: formula<'a> -> 'a list when 'a: comparison

    /// Tests whether a function `subfn` returns `true` on all possible valuations 
    /// of the atoms `ats`, using an existing valuation `v` for all other atoms.
    val onallvaluations:
      subfn: (('a -> bool) -> bool) -> v: ('a -> bool) -> ats: 'a list -> bool
        when 'a: equality

    /// Prints the truth table of a formula `fm` using a TextWriter.
    val fprint_truthtable:
      sw: System.IO.TextWriter -> fm: formula<prop> -> unit

    /// Prints the truth table of the propositional formula `f`.
    val inline print_truthtable: f: formula<prop> -> unit

    /// Returns a string representation of the truth table of the propositional 
    /// formula `f`.
    val inline sprint_truthtable: f: formula<prop> -> string

    /// Checks if a propositional formula is a tautology.
    val tautology: fm: formula<'a> -> bool when 'a: comparison

    /// Checks if a propositional formula is unsatisfiable.
    val unsatisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// Checks if a propositional formula is satisfiable.
    val satisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// Returns the formula resulting from applying the substitution `sbfn` 
    /// to the input formula.
    val psubst:
      subfn: func<'a,formula<'a>> ->
        (formula<'a> -> formula<'a>) when 'a: comparison

    /// Returns the dual of the input formula `fm`: i.e. the result of 
    /// systematically exchanging `/\`s with `\/`s and also `True` with 
    /// `False`.
    val dual: fm: formula<'a> -> formula<'a>

    /// Performs a simplification routine but just at the first level of the input 
    /// formula `fm`. It eliminates the basic propositional constants `False` and 
    /// `True`. 
    /// 
    /// Whenever `False` and `True` occur in combination, there is always a a 
    /// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
    /// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
    /// also eliminates double negations `~~p`.
    val psimplify1: fm: formula<'a> -> formula<'a>

    /// Performs a simplification routine on the input formula 
    /// `fm` eliminating the basic propositional constants `False` and `True`. 
    /// 
    /// Whenever `False` and `True` occur in combination, there is always a a 
    /// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
    /// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
    /// also eliminates double negations `~~p`.
    /// 
    /// While `psimplify1` performs the transformation just at the first level, 
    /// `psimplify` performs it at every levels in a recursive bottom-up sweep.
    val psimplify: fm: formula<'a> -> formula<'a>

    /// Checks if a literal is negative.
    val negative: _arg1: formula<'a> -> bool

    /// Checks if a literal is positive.
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
