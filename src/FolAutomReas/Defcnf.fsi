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
    /// <param name="fm">The formula to be transformed.</param>
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
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// (`p_1`,
    ///   `p \/ p_0` |-> (`p_1`, `p_1 &lt;=&gt; p \/ p_0`)
    ///   `p \/ q`   |-> (`p_0`, `p_0 &lt;=&gt; p \/ q`),
    ///   2I) 
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
    /// <param name="fm">The formula to be transformed.</param>
    /// <param name="defs">The definitions made so far.</param>
    /// <param name="n">The current variable index.</param>
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
    /// Make n large enough that "v_m" won't clash with s for any m >= n.
    /// </summary>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val max_varindex: pfx: string -> s: string -> n: bigint -> bigint

    /// <summary>
    /// TBD
    /// </summary>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val mk_defcnf:
      fn: (formula<prop> * func<'a,'b> * bigint ->
             formula<'c> * func<'d,('e * formula<'c>)> *
             'f) ->
        fm: formula<prop> -> formula<'c> list list
        when 'c: comparison and 'd: comparison and 'e: comparison

    /// <summary>
    /// Overall definitional CNF.
    /// </summary>
    /// 
    /// <category index="2">Overall definitional CNF</category>
    val defcnfOrig: fm: formula<prop> -> formula<prop>

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