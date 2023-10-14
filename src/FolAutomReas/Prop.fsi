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
    /// <remarks>
    /// This function is used to define both truth-table (see 
    /// <see cref='M:FolAutomReas.Prop.print_truthtable'/>) and 
    /// <see cref='M:FolAutomReas.Prop.tautology``1'/>
    /// </remarks>
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
    /// Prints the truth table of the prop formula <c>fm</c> to a TextWriter 
    /// <c>sw</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// See also <see cref='M:FolAutomReas.Prop.print_truthtable'/>
    /// </remarks>
    /// 
    /// <param name="sw">The TextWriter to print to.</param>
    /// <param name="fm">The input prop formula.</param>
    /// 
    /// <example id="fprint_truthtable-1">
    /// <code lang="fsharp">
    /// let file = System.IO.File.CreateText("out.txt")
    /// fprint_truthtable file (!>"p ==> q")
    /// file.Close()
    /// </code>
    /// After evaluation the file contains the text
    /// <code lang="fsharp">
    /// p     q     |   formula
    /// ---------------------
    /// false false | true  
    /// false true  | true  
    /// true  false | false 
    /// true  true  | true  
    /// ---------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val fprint_truthtable:
      sw: System.IO.TextWriter -> fm: formula<prop> -> unit

    /// <summary>
    /// Prints the truth table of the prop formula <c>fm</c> to the 
    /// <c>stdout</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// <p>Each logical connective is interpreted by a corresponding bool 
    /// operator of the metalanguage.</p>
    /// 
    /// <p>A truth-table shows how the truth-value assigned to a formula is 
    /// determined by those of its atoms, based on the interpretation of its 
    /// logical connectives.</p>
    /// 
    /// For binary connective we have:
    /// 
    /// \begin{array}{|c|c||c|c|c|c|}
    /// \hline
    /// p &amp; q &amp; p \land q &amp; p \lor q &amp; p \Rightarrow q &amp; p \Leftrightarrow q \\
    /// \hline
    /// false &amp; false &amp; false &amp; false &amp; true &amp; true \\
    /// \hline
    /// false &amp; true &amp; false &amp; true &amp; true &amp; false \\
    /// \hline
    /// true &amp; false &amp; false &amp; true &amp; false &amp; false \\
    /// \hline
    /// true &amp; true &amp; true &amp; true &amp; true &amp; true \\
    /// \hline
    /// \end{array}
    /// 
    /// while for the unary negation:
    /// 
    /// \begin{array}{|c||c|}
    /// 	\hline
    /// 	p &amp; \neg p \\
    /// 	\hline
    /// 	false &amp; true\\
    /// 	\hline
    /// 	true &amp; false\\
    /// 	\hline
    /// \end{array} 
    /// 
    /// <p>A truth table has one column for each propositional variable and one 
    /// final column showing all of the possible results of the logical 
    /// operation that the table represents. Each row of the truth table 
    /// contains one possible configuration of the propositional variables, 
    /// and the result of the operation for those values.</p>
    /// 
    /// In particular, truth-tables can be used to show whether (i) a prop 
    /// formula is logically valid (i.e. a tautology: see 
    /// <see cref='M:FolAutomReas.Prop.tautology``1'/> that, as this 
    /// function, is based on 
    /// <see cref='M:FolAutomReas.Prop.onallvaluations``1'/>) when the result 
    /// column has <c>true</c> in each rows of the table; (ii) 
    /// <see cref='M:FolAutomReas.Prop.satisfiable``1'/>, when the result colum 
    /// has <c>true</c> at least in one row; (iii) 
    /// <see cref='M:FolAutomReas.Prop.unsatisfiable``1'/> when has 
    /// <c>false</c> in each rows.
    /// </remarks>
    /// 
    /// <param name="fm">The input prop formula.</param>
    /// 
    /// <example id="print_truthtable-1">
    /// <code lang="fsharp">
    /// print_truthtable !>"p /\ q ==> q /\ r"
    /// </code>
    /// After evaluation the following text is printed to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// p     q     r     |   formula
    /// ---------------------------
    /// false false false | true  
    /// false false true  | true  
    /// false true  false | true  
    /// false true  true  | true  
    /// true  false false | true  
    /// true  false true  | true  
    /// true  true  false | false 
    /// true  true  true  | true  
    /// ---------------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val inline print_truthtable: fm: formula<prop> -> unit

    /// <summary>
    /// Returns a string representation of the truth table of the prop formula 
    /// <c>fm</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// See also <see cref='M:FolAutomReas.Prop.print_truthtable'/>
    /// </remarks>
    /// 
    /// <param name="fm">The input prop formula.</param>
    /// <returns>
    /// The string representation of the truth table of the formula.
    /// </returns>
    /// 
    /// <example id="sprint_truthtable-1">
    /// <code lang="fsharp">
    /// sprint_truthtable !>"p ==> q"
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// "p     q     |   formula
    /// ---------------------
    /// false false | true  
    /// false true  | true  
    /// true  false | false 
    /// true  true  | true  
    /// ---------------------
    /// 
    /// "
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val inline sprint_truthtable: fm: formula<prop> -> string

    /// <summary>
    /// Checks if a formula is a tautology at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a tautology at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="tautology-1">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="tautology-2">
    /// <code lang="fsharp">
    /// !> @"p \/ q ==> p"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="tautology-3">
    /// <code lang="fsharp">
    /// !> @"p \/ q ==> q \/ (p &lt;=&gt; q)"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="tautology-4">
    /// <code lang="fsharp">
    /// !> @"(p \/ q) /\ ~(p /\ q) ==> (~p &lt;=&gt; q)"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val tautology: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is unsatisfiable at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a unsatisfiable at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="unsatisfiable-1">
    /// <code lang="fsharp">
    /// !> "p /\ ~p"
    /// |> unsatisfiable
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="unsatisfiable-2">
    /// <code lang="fsharp">
    /// !> @"p"
    /// |> unsatisfiable
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val unsatisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is satisfiable at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a satisfiable at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="satisfiable-1">
    /// <code lang="fsharp">
    /// !> "p /\ ~p"
    /// |> satisfiable
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="satisfiable-2">
    /// <code lang="fsharp">
    /// !> @"p"
    /// |> satisfiable
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val satisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Substitutes atoms in a formula with other formulas based on an fpf.
    /// </summary>
    /// 
    /// <param name="subfn">The fpf mapping atoms to formulas.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula resulting from substituting atoms of <c>fm</c> with the 
    /// formulas based on the mapping defined in <c>sbfn</c>.
    /// </returns>
    /// 
    /// <example id="psubst-1">
    /// <code lang="fsharp">
    /// !> "p /\ q /\ p /\ q"
    /// |> psubst (P"p" |=> !>"p /\ q")
    /// </code>
    /// Evaluates to <c>`(p /\ q) /\ q /\ (p /\ q) /\ q`</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val psubst:
      subfn: func<'a,formula<'a>> ->
        fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Returns the dual of the input formula: i.e. the result of 
    /// systematically exchanging <c>/\</c> with <c>\/</c> and also 
    /// <c>True</c> with <c>False</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The dual of the input formula.</returns>
    /// 
    /// <example id="dual-1">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p"
    /// |> dual
    /// </code>
    /// Evaluates to <c>`p /\ ~p`</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val dual: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine (but just at the first 
    /// level) of the input formula, eliminating the basic 
    /// propositional constants <c>False</c> and <c>True</c> and the double 
    /// negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// This function applies to formulas a simplification routine similar to 
    /// the one <see cref='M:FolAutomReas.Intro.simplify1'/> applies on 
    /// algebraic expressions.
    /// 
    /// It eliminates the basic propositional 
    /// constants \(\bot\) and \(\top\) based on the equivalences similar to 
    /// the following (see the implementation for details):
    /// <ul>
    /// <li>\(\bot \land p \Leftrightarrow \bot\)</li>
    /// <li>\(\top \lor p \Leftrightarrow p\)</li>
    /// <li>\(p \Rightarrow \bot \Leftrightarrow \neg p\)</li>
    /// <li>...</li>
    /// </ul>
    /// 
    /// At the same time, it also eliminates double negations \(\neg \neg p\).
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The formula simplified.</returns>
    /// 
    /// <example id="psimplify1-1">
    /// <code lang="fsharp">
    /// !> "false /\ p"
    /// |> psimplify1
    /// </code>
    /// Evaluates to <c>`false`</c>.
    /// </example>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify1: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine eliminating 
    /// the basic propositional constants <c>False</c> and <c>True</c> and the 
    /// double negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Completes the simplification routine psimplify1 applying it at every 
    /// level of the formula.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The formula simplified.</returns>
    /// 
    /// <example id="psimplify-1">
    /// <code lang="fsharp">
    /// !> "((x ==> y) ==> true) \/ ~false"
    /// |> psimplify
    /// </code>
    /// Evaluates to <c>`true`</c>.
    /// </example>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Checks if a literal is negative.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>true, if the literal is negative; otherwise false.</returns>
    /// 
    /// <example id="negative-1">
    /// <code lang="fsharp">
    /// !> "~p" |> negative
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="negative-2">
    /// <code lang="fsharp">
    /// !> "p" |> negative
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val negative: lit: formula<'a> -> bool

    /// <summary>
    /// Checks if a literal is positive.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>true, if the literal is positive; otherwise false.</returns>
    /// 
    /// <example id="positive-1">
    /// <code lang="fsharp">
    /// !> "p" |> positive
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="positive-2">
    /// <code lang="fsharp">
    /// !> "~p" |> positive
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val positive: lit: formula<'a> -> bool

    /// <summary>Changes a literal into its contrary.</summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>
    /// The negated literal if the input is positive and vice versa.
    /// </returns>
    /// 
    /// <example id="negate-1">
    /// <code lang="fsharp">
    /// !> "p" |> negate
    /// </code>
    /// Evaluates to <c>`~p`</c>.
    /// </example>
    /// 
    /// <example id="negate-2">
    /// <code lang="fsharp">
    /// !> "~p" |> negate
    /// </code>
    /// Evaluates to <c>`p`</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val negate: lit: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into a naive negation normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in <em>negation normal form</em> (NNF) if it is 
    /// constructed from literals using only the binary connectives \(\land\) 
    /// and \(\lor\), or else is one of the degenerate cases \(\top\) and 
    /// \(\bot\).
    /// 
    /// <c>nnf_naive</c> implements an incomplete transformation of NNF which 
    /// pushes down negation on atoms and removes the binary connective 
    /// \(\Rightarrow\) and \(\Leftrightarrow\) but keeps \(\top\) and 
    /// \(\bot\) mixed with other formulas.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a naive negation normal form.
    /// </returns>
    /// 
    /// <example id="nnf_naive-1">
    /// <code lang="fsharp">
    /// !> "~ (p ==> false)"
    /// |> nnf_naive
    /// </code>
    /// Evaluates to <c>`p /\ ~false`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nnf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into a naive negation normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in <em>negation normal form</em> (NNF) if it is 
    /// constructed from literals using only the binary connectives \(\land\) 
    /// and \(\lor\), or else is one of the degenerate cases \(\top\) and 
    /// \(\bot\).
    /// 
    /// <c>nnf</c> implements a complete transformation in NNF 
    /// applying <see cref='M:FolAutomReas.Prop.psimplify``1'/> first 
    /// and then <see cref='M:FolAutomReas.Prop.nnf_naive``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form.
    /// </returns>
    /// 
    /// <example id="nnf-1">
    /// <code lang="fsharp">
    /// !> "~ (p ==> false)"
    /// |> nnf
    /// </code>
    /// Evaluates to <c>`p`</c>.
    /// </example>
    /// 
    /// <example id="nnf-2">
    /// <code lang="fsharp">
    /// !> "(p &lt;=&gt; q) &lt;=&gt; ~(r ==> s)"
    /// |> nnf
    /// </code>
    /// Evaluates to <c>`(p /\ q \/ ~p /\ ~q) /\ r /\ ~s \/ (p /\ ~q \/ ~p /\ q) /\ (~r \/ s)`</c>.
    /// </example>
    /// 
    /// <note>
    /// Negation normal form is not a canonical form: for example, 
    /// \(a \land (b \lor \lnot c)\) and \((a \land b) \lor (a \land \lnot c)\) 
    /// are equivalent, and are both in negation normal form.
    /// (from 
    /// <a href="https://en.wikipedia.org/wiki/Negation_normal_form">https://en.wikipedia.org/wiki/Negation_normal_form</a>)
    /// </note>
    /// 
    /// <category index="9">Negation Normal Form</category> 
    val nnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into negation normal form but keeps logical 
    /// equivalences and <c>False</c> and <c>True</c> mixed with other 
    /// formulas.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form that keeps 
    /// equivalences and logical constants mixed with other formulas.
    /// </returns>
    /// 
    /// <example id="nenf_naive-1">
    /// <code lang="fsharp">
    /// !> "~ (p &lt;=&gt; q)"
    /// |> nenf_naive
    /// </code>
    /// Evaluates to <c>`p &lt;=&gt; ~q`</c>.
    /// </example>
    /// 
    /// <example id="nenf_naive-2">
    /// <code lang="fsharp">
    /// !> "~ (false &lt;=&gt; q)"
    /// |> nenf_naive
    /// </code>
    /// Evaluates to <c>`false &lt;=&gt; ~q`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into negation normal form but keeps logical 
    /// equivalences.
    /// </summary>
    /// 
    /// <remarks>
    /// Applies <see cref='M:FolAutomReas.Prop.psimplify``1'/> first 
    /// and then <see cref='M:FolAutomReas.Prop.nenf_naive``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form that keeps 
    /// equivalences.
    /// </returns>
    /// 
    /// <example id="nenf-1">
    /// <code lang="fsharp">
    /// !> "~ (p &lt;=&gt; q)"
    /// |> nenf
    /// </code>
    /// Evaluates to <c>`p &lt;=&gt; ~q)`</c>.
    /// </example>
    /// 
    /// <example id="nenf-2">
    /// <code lang="fsharp">
    /// !> "~ (false &lt;=&gt; q)"
    /// |> nenf
    /// </code>
    /// Evaluates to <c>`q`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a conjunction from a list of formulas.
    /// </summary>
    /// 
    /// <param name="l">The list of formulas to conjunct.</param>
    /// 
    /// <returns>The conjunction of the input formulas.</returns>
    /// 
    /// <example id="list_conj-1">
    /// <code lang="fsharp">
    /// list_conj [!>"p";!>"q";!>"r"]
    /// </code>
    /// Evaluates to <c>`p /\ q /\ r`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_conj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Constructs a disjunction from a list of formulas.
    /// </summary>
    /// 
    /// <param name="l">The list of formulas to disjunct.</param>
    /// 
    /// <returns>The disjunction of the input formulas.</returns>
    /// 
    /// <example id="list_disj-1">
    /// <code lang="fsharp">
    /// list_disj [!>"p";!>"q";!>"r"]
    /// </code>
    /// Evaluates to <c>`p \/ q \/ r`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_disj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Constructs a conjunction from a list of formulas <c>pvs</c> and their 
    /// negations, according to whether each is satisfied by a valuation 
    /// <c>v</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// As the name suggest, it is intended to be used on literals and 
    /// actually is used just to define 
    /// <see cref='M:FolAutomReas.Prop.dnf_by_truth_tables``1'/>
    /// </remarks>
    /// 
    /// <param name="pvs">The input list of formulas.</param>
    /// <param name="v">The input valuation.</param>
    /// 
    /// <returns>
    /// The conjunction of the <c>pvs</c> formulas (positive or negated 
    /// depending on <c>v</c>).
    /// </returns>
    /// 
    /// <example id="mk_lits-1">
    /// <code lang="fsharp">
    /// mk_lits [!>"p";!>"q"] 
    ///     (function P"p" -> true | P"q" -> false)
    /// </code>
    /// Evaluates to <c>`p /\ ~q`</c>.
    /// </example>
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
