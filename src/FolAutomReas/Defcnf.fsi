// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Lib.Fpf
open FolAutomReas.Formulas
open FolAutomReas.Prop

/// <summary>Definitional Conjunctive Normal Form.</summary>
/// <category index="3">Propositional logic</category>
module Defcnf = 

    /// <summary>
    /// Generates an indexed variable of the form <c>p_n</c> and the next 
    /// index <c>n+1</c>.
    /// </summary>
    /// 
    /// <param name="n">The index of the variable.</param>
    /// <returns>The indexed variable and the next index</returns>
    /// 
    /// <example id="mkprop-1">
    /// <code lang="fsharp">
    /// mkprop 3I
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// (`p_3`, 4 {IsEven = true;
    ///            IsOne = false;
    ///            IsPowerOfTwo = true;
    ///            IsZero = false;
    ///            Sign = 1;})
    /// </code>
    /// </example>
    /// 
    /// <category index="1">Core definitional CNF procedure</category>
    val mkprop: n: bigint -> formula<prop> * bigint

    /// <summary>
    /// Core definitional CNF procedure.
    /// </summary>
    /// 
    /// <param name="fm">The formula to be transformed (supposed to be already in <see cref='M:FolAutomReas.Prop.nenf``1'/>).</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// <returns>
    /// A triple with the transformed formula, the augmented definitions and a 
    /// new variable index.
    /// </returns>
    /// 
    /// <example id="maincnf-1">
    /// <code lang="fsharp">
    /// maincnf (!> @"p \/ (p \/ q)", undefined, 0I) 
    /// |> fun (fm,defs,counter) -> 
    ///     fm, defs |> graph, counter
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// ("p_1",
    ///  [("p \/ p_0", ("p_1", "p_1 &lt;=&gt; p \/ p_0"));
    ///   ("p \/ q", ("p_0", "p_0 &lt;=&gt; p \/ q"))], 2I)
    /// </code>
    /// </example>
    /// 
    /// <category index="1">Core definitional CNF procedure</category>
    val maincnf:
      fm: formula<prop> *
      defs: func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      n: bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// Used to define <see cref='M:FolAutomReas.Defcnf.maincnf'/>.
    /// </summary>
    /// 
    /// <param name="op">The binary formula constructor received from maincnf.</param>
    /// <param name="p">The left-hand sub-formula.</param>
    /// /// <param name="q">The right-hand sub-formula.</param>
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// 
    /// <returns>
    /// A triple with the transformed formula, the augmented definitions and a 
    /// new variable index.
    /// </returns>
    /// 
    /// <category index="1">Core definitional CNF procedure</category>
    val defstep:
      op: (formula<prop> ->
             formula<prop> -> formula<prop>) ->
        p: formula<prop> * q: formula<prop> ->
          fm: formula<prop> *
          defs: func<formula<prop>,
                             (formula<prop> *
                              formula<prop>)> * n: bigint ->
            formula<prop> *
            func<formula<prop>,
                         (formula<prop> * formula<prop>)    > *
            bigint

    /// <summary>
    /// Return the larger between <c>n</c> and the index <c>m</c> of the 
    /// variable <c>s</c> if this is of the form <c>pfx_m</c>.
    /// </summary>
    /// 
    /// <param name="pfx">The prefix to be checked.</param>
    /// <param name="s">The input variable name.</param>
    /// <param name="n">The value to compare.</param>
    /// 
    /// <returns>
    /// If <c>s</c> is of the form <c>pfx_m</c> returns the larger between 
    /// <c>m</c> and <c>n</c>; otherwise <c>n</c>.
    /// </returns>
    /// 
    /// <example id="max_varindex-1">
    /// <code lang="fsharp">
    /// max_varindex "p_" "p_0" 1I // evaluates to 1I
    /// max_varindex "p_" "p_2" 1I // evaluates to 2I
    /// max_varindex "p_" "x_2" 1I // evaluates to 1I
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val max_varindex: pfx: string -> s: string -> n: bigint -> bigint

    /// <summary>
    /// Returns the result of a specific CNF procedure in a set-of-sets 
    /// representation.
    /// </summary>
    /// 
    /// <remarks>
    /// Transforms a generic propositional formula <c>fm</c> in 
    /// <see cref='M:FolAutomReas.Prop.nenf``1'/> and then it applies to the 
    /// result a specific definitional CNF procedure <c>fn</c> returning an 
    /// equisatisfiable CNF in a set-of-sets representation.
    /// </remarks>
    /// 
    /// <param name="fn">The specific definitional CNF procedure.</param>
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The CNF result of <c>fn</c> in a set-of-sets representation.
    /// </returns>
    /// 
    /// <example id="mk_defcnf-1">
    /// <code lang="fsharp">
    /// !>"p ==> q"
    /// |> mk_defcnf maincnf
    /// </code>
    /// Evaluates to <c>[[`p`; `p_1`]; [`p_1`]; [`p_1`; `~q`]; [`q`; `~p`; `~p_1`]]</c>.
    /// </example>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val mk_defcnf:
      fn: (formula<prop> * func<'a,'b> * bigint ->
             formula<'c> * func<'d,('e * formula<'c>)> *
             'f) ->
        fm: formula<prop> -> formula<'c> list list
        when 'c: comparison and 'd: comparison and 'e: comparison

    /// <summary>
    /// Returns an equisatisfiable CNF of the input formula using a 
    /// definitional procedure, transforming each definition in isolation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// An equisatisfiable CNF of the input formula.
    /// </returns>
    /// 
    /// <example id="defcnf01-1">
    /// <code lang="fsharp">
    /// !>"p ==> q"
    /// |> defcnf01
    /// </code>
    /// Evaluates to <c>`(p \/ p_1) /\ p_1 /\ (p_1 \/ ~q) /\ (q \/ ~p \/ ~p_1)`</c>.
    /// </example>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val defcnf01: fm: formula<prop> -> formula<prop>

    /// <summary>
    /// TBD
    /// </summary>
    /// 
    /// <category index="3">Version tweaked to exploit initial structure</category>
    val subcnf:
      sfn: ('a * 'b * 'c -> 'd * 'b * 'c) ->
        op: ('d -> 'd -> 'e) ->
        p: 'a * q: 'a -> fm: 'f * defs: 'b * n: 'c -> 'e * 'b * 'c

    /// <summary>
    /// TBD
    /// </summary>
    /// 
    /// <category index="3">Version tweaked to exploit initial structure</category>
    val orcnf:
      formula<prop> *
      func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// TBD
    /// </summary>
    /// 
    /// <category index="3">Version tweaked to exploit initial structure</category>
    val andcnf:
      formula<prop> *
      func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// TBD
    /// </summary>
    /// 
    /// <category index="3">Version tweaked to exploit initial structure</category>
    val defcnfs:
      fm: formula<prop> -> formula<prop> list list

    /// <summary>
    /// Version tweaked to exploit initial structure.
    /// </summary>
    /// 
    /// <category index="3">Version tweaked to exploit initial structure</category>
    val defcnf: fm: formula<prop> -> formula<prop>

    /// <summary>
    /// TBD.
    /// </summary>
    /// 
    /// <category index="4">Version that guarantees 3-CNF</category>
    val andcnf3:
      formula<prop> *
      func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// Version that guarantees 3-CNF.
    /// </summary>
    /// 
    /// <category index="4">Version that guarantees 3-CNF</category>
    val defcnf3: fm: formula<prop> -> formula<prop>