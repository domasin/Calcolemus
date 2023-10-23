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
    /// The triple with the transformed formula, the augmented definitions 
    /// and a new variable index.
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
    /// The triple with the transformed formula, the augmented definitions 
    /// and a new variable index.
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
    /// Links the definitional transformations produced by <c>sfn</c> in the 
    /// different parts of the formula.
    /// </summary>
    /// 
    /// <param name="sfn">The specific definitional CNF procedure.</param>
    /// <param name="op">The binary formula constructor received from <c>sfn</c>.</param>
    /// <param name="p">The left-hand sub-formula.</param>
    /// /// <param name="q">The right-hand sub-formula.</param>
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// 
    /// <returns>
    /// The triple with the transformed formula, the augmented definitions 
    /// and a new variable index.
    /// </returns>
    /// 
    /// <category index="3">Optimized definitional CNF</category>
    val subcnf:
      sfn: ('a * 'b * 'c -> 'd * 'b * 'c) ->
        op: ('d -> 'd -> 'e) ->
        p: 'a * q: 'a -> fm: 'f * defs: 'b * n: 'c -> 'e * 'b * 'c

    /// <summary>
    /// Performs the definitional transformation of the disjuncts.
    /// </summary>
    /// 
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// 
    /// <returns>
    /// The triple with the transformed formula, the augmented definitions 
    /// and a new variable index.
    /// </returns>
    /// 
    /// <category index="3">Optimized definitional CNF</category>
    val orcnf:
      fm: formula<prop> *
      defs: func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      n: bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// Performs the definitional transformation of the conjuncts.
    /// </summary>
    /// 
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// 
    /// <returns>
    /// The triple with the transformed formula, the augmented definitions 
    /// and a new variable index.
    /// </returns>
    /// 
    /// <category index="3">Optimized definitional CNF</category>
    val andcnf:
      fm: formula<prop> *
      defs: func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      n: bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// Optimized definitional CNF in set-of-sets representation.
    /// </summary>
    /// 
    /// <remarks>
    /// It returns an equisatisfiable CNF of the input formula in a set-of-sets 
    /// representation avoiding some redundant definitions.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// An equisatisfiable CNF of the input formula in a set-of-sets 
    /// representation.
    /// </returns>
    /// 
    /// <example id="defcnfs-1">
    /// <code lang="fsharp">
    /// !> @"(p \/ (q /\ ~r)) /\ s"
    /// |> defcnfs
    /// </code>
    /// Evaluates to <c>[[`p`; `p_1`]; [`p_1`; `r`; `~q`]; [`q`; `~p_1`]; [`s`]; [`~p_1`; `~r`]]</c>.
    /// </example>
    /// 
    /// <category index="3">Optimized definitional CNF</category>
    val defcnfs:
      fm: formula<prop> -> formula<prop> list list

    /// <summary>
    /// Optimized definitional CNF.
    /// </summary>
    /// 
    /// <remarks>
    /// It returns an equisatisfiable CNF of the input formula avoiding some 
    /// redundant definitions. The optimization is obtained, when dealing with 
    /// an iterated conjunction,  by 
    /// <ul>
    /// <li>putting the conjuncts in CNF separately</li>
    /// <li>
    /// and if they in turn are already disjunctions of literals leave them 
    /// unchanged.
    /// </li>
    /// </ul>
    /// 
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// An equisatisfiable CNF of the input formula.
    /// </returns>
    /// 
    /// <example id="defcnf-1">
    /// <code lang="fsharp">
    /// !> @"(p \/ (q /\ ~r)) /\ s"
    /// |> defcnf
    /// </code>
    /// Evaluates to <c>`(p \/ p_1) /\ (p_1 \/ r \/ ~q) /\ (q \/ ~p_1) /\ s /\ (~p_1 \/ ~r)`</c>.
    /// </example>
    /// 
    /// <category index="3">Optimized definitional CNF</category>
    val defcnf: fm: formula<prop> -> formula<prop>

    /// <summary>
    /// Performs the definitional transformation of the conjuncts.
    /// </summary>
    /// 
    /// <remarks>
    /// It keeps the optimization of putting the conjuncts in CNF separately, 
    /// but removes the second optimization of leaving unchanged conjuncts 
    /// that are already a disjunction of literals. In this way guarantees that 
    /// the result is in 3-CNF.
    /// </remarks>
    /// 
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
    /// 
    /// <category index="4">3-CNF</category>
    val andcnf3:
      fm: formula<prop> *
      defs: func<formula<prop>,
                   (formula<prop> * formula<prop>)> *
      n: bigint ->
        formula<prop> *
        func<formula<prop>,
                     (formula<prop> * formula<prop>)> *
        bigint

    /// <summary>
    /// Optimized definitional CNF that also guarantees 3-CNF in the result.
    /// </summary>
    /// 
    /// <remarks>
    /// 3-CNF means that each conjunct contains a disjunction of **at most** 
    /// three literals.
    /// </remarks>
    /// 
    /// <example id="defcnf3-1">
    /// <code lang="fsharp">
    /// !> @"(a \/ b \/ c \/ d) /\ s"
    /// |> defcnf3
    /// </code>
    /// Evaluates to <c>`(a \/ p_2 \/ ~p_3) /\ (b \/ p_1 \/ ~p_2) /\ (c \/ d \/ ~p_1) /\ (p_1 \/ ~c) /\ (p_1 \/ ~d) /\ (p_2 \/ ~b) /\ (p_2 \/ ~p_1) /\ p_3 /\ (p_3 \/ ~a) /\ (p_3 \/ ~p_2) /\ s`</c>.
    /// </example>
    /// 
    /// <category index="4">3-CNF</category>
    val defcnf3: fm: formula<prop> -> formula<prop>