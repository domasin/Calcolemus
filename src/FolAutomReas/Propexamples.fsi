// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

open FolAutomReas.Formulas
open FolAutomReas.Prop

/// <summary>
/// Some propositional formulas to test, and functions to generate classes.
/// </summary>
/// 
/// <remarks>
/// This module, while showing some applications of propositional 
/// logic, is used above all to generate interesting classes of problems in 
/// order to have a stock of non-trivial formulas on which to test the 
/// efficiency of propositional logic algorithms.
/// </remarks>
/// 
/// <category index="3">Propositional logic</category>
module Propexamples = 

    /// <summary>
    /// Generates an assertion equivalent to R(<c>s</c>,<c>t</c>) &lt;= 
    /// <c>n</c> for the Ramsey number R(<c>s</c>,<c>t</c>).
    /// </summary>
    /// 
    /// <remarks>
    /// In terms of graph theory, the ramsey number R(<c>s</c>,<c>t</c>) is the 
    /// minimum number x such that all graphs of x vertices have a completely 
    /// connected subgraph of size <c>s</c> or a completely disconnected 
    /// subgraph of size <c>t</c>. The Ramsey's theorem proves that for every 
    /// pair of \(s,t \in \mathbb{N}\) such a number exists.
    /// </remarks>
    /// 
    /// <param name="s">The number of connected vertices.</param>
    /// <param name="t">The number of disconnected vertices.</param>
    /// <param name="n">The number supposed to be greater or equal to the Ramsey number.</param>
    /// 
    /// <returns>
    /// A propositional formula that is a tautology if if <c>n</c> is greater 
    /// or equal than the Ramsey number R(<c>s</c>,<c>t</c>).
    /// </returns>
    /// 
    /// <example id="ramsey-1">
    /// <code lang="fsharp">
    /// ramsey 3 3 4
    /// </code>
    /// Evaluates to <c>(p_1_2 /\ p_1_3 /\ p_2_3 \/ p_1_2 /\ p_1_4 /\ p_2_4 \/ p_1_3 /\ p_1_4 /\ p_3_4 \/ p_2_3 /\ p_2_4 /\ p_3_4) \/ ~p_1_2 /\ ~p_1_3 /\ ~p_2_3 \/ ~p_1_2 /\ ~p_1_4 /\ ~p_2_4 \/ ~p_1_3 /\ ~p_1_4 /\ ~p_3_4 \/ ~p_2_3 /\ ~p_2_4 /\ ~p_3_4</c>.
    /// </example>
    /// 
    /// <example id="ramsey-2">
    /// <code lang="fsharp">
    /// tautology(ramsey 3 3 5)
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="ramsey-3">
    /// <code lang="fsharp">
    /// tautology(ramsey 3 3 6)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="1">Ramsey numbers</category>
    val ramsey: s: int -> t: int -> n: int -> formula<prop>

    /// <summary>
    /// Sum of an half adder: <c>x &lt;=&gt; ~ y</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates the propositional formula whose truth value corresponds to 
    /// the sum of an half adder, given the <c>x</c> and <c>y</c> digits to 
    /// be summed also represented as prop formulas: <c>False</c> for 0, 
    /// <c>True</c> for 1.
    /// </remarks>
    /// 
    /// <param name="x">The first digit to be summed.</param>
    /// <param name="y">The second digit to be summed.</param>
    /// <returns>The half adder's sum of <c>x</c> + <c>y</c></returns>
    /// 
    /// <example id="halfsum-1">
    /// <code lang="fsharp">
    /// halfsum (True:prop formula) False
    /// </code>
    /// Evaluates to <c>`true &lt;=&gt; ~false`</c>.
    /// </example>
    /// 
    /// <example id="halfsum-2">
    /// The following shows the results of the function for each possible 
    /// combination of the intended inputs:
    /// <code lang="fsharp">
    /// let to01 fm = 
    ///   match eval fm (fun _ -> false) with
    ///   | false  -> 0
    ///   | true  -> 1
    /// 
    /// printfn "-------------"
    /// printfn "| x | y | s |"
    /// printfn "-------------"
    /// for x in [False;True] do 
    ///     for y in [False;True] do 
    ///         printfn "| %i | %i | %i |" 
    ///             (x |> to01) 
    ///             (y |> to01) 
    ///             (halfsum x y |> to01)
    /// printfn "-------------"
    /// </code>
    /// After evaluation the following is printed to the to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -------------
    /// | x | y | s |
    /// -------------
    /// | 0 | 0 | 0 |
    /// | 0 | 1 | 1 |
    /// | 1 | 0 | 1 |
    /// | 1 | 1 | 0 |
    /// -------------
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Half adder</category>
    val halfsum:
      x: formula<'a> -> y: formula<'a> -> formula<'a>

    /// <summary>
    /// Carry of an half adder: <c>x /\ y</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates the propositional formulas whose truth value corresponds to 
    /// the carry of an half adder, given the <c>x</c> and <c>y</c> digits to 
    /// be summed also represented as prop formulas: <c>False</c> for 0 
    /// <c>True</c> for 1.
    /// </remarks>
    /// 
    /// <param name="x">The first digit to be summed.</param>
    /// <param name="y">The second digit to be summed.</param>
    /// <returns>The half adder's carry of <c>x</c> + <c>y</c></returns>
    /// 
    /// <example id="halfcarry-1">
    /// <code lang="fsharp">
    /// halfcarry (True:prop formula) False
    /// </code>
    /// Evaluates to <c>`true /\ false`</c>.
    /// </example>
    /// 
    /// <example id="halfcarry-2">
    /// The following shows the results of the function for each possible 
    /// combination of the intended inputs:
    /// <code lang="fsharp">
    /// let to01 fm = 
    ///   match eval fm (fun _ -> false) with
    ///   | false  -> 0
    ///   | true  -> 1
    /// 
    /// printfn "-------------"
    /// printfn "| x | y | s |"
    /// printfn "-------------"
    /// for x in [False;True] do 
    ///     for y in [False;True] do 
    ///         printfn "| %i | %i | %i |" 
    ///             (x |> to01) 
    ///             (y |> to01) 
    ///             (halfcarry x y |> to01)
    /// printfn "-------------"
    /// </code>
    /// After evaluation the following is printed to the to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -------------
    /// | x | y | s |
    /// -------------
    /// | 0 | 0 | 0 |
    /// | 0 | 1 | 0 |
    /// | 1 | 0 | 0 |
    /// | 1 | 1 | 1 |
    /// -------------
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Half adder</category>
    val halfcarry:
      x: formula<'a> -> y: formula<'a> -> formula<'a>

    /// <summary>
    /// Half adder function.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates a propositional formula that is a tautology if the input 
    /// formulas represent respectively two digits <c>x</c> and <c>y</c> to be 
    /// summed, the resulting sum <c>s</c> and the carry <c>c</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first digit to be summed.</param>
    /// <param name="y">The second digit to be summed.</param>
    /// <param name="s">The supposed sum.</param>
    /// <param name="c">The supposed carry.</param>
    /// <returns>
    /// The propositional formula that represents the intended relations 
    /// between the input. In other words, a formula that is a tautology 
    /// if <c>s</c> and <c>c</c> are the sum and carry of an half adder for 
    /// the input digits <c>x</c> and <c>y</c>.
    /// </returns>
    /// 
    /// <example id="ha-1">
    /// <code lang="fsharp">
    /// let fm = ha (True:prop formula) True False True
    /// // evaluates to: `(false &lt;=&gt; true &lt;=&gt; ~true) /\ (true &lt;=&gt; true /\ true)`
    /// tautology(fm)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="ha-2">
    /// Taking only the tautologies returned by the function 
    /// gives an half adder:
    /// <code lang="fsharp">
    /// let to01 fm = 
    ///   match eval fm (fun _ -> false) with
    ///   | false  -> 0
    ///   | true  -> 1
    /// 
    /// printfn "-----------------"
    /// printfn "| x | y | c | s |"
    /// printfn "-----------------"
    /// for x in [False;True] do 
    ///     for y in [False;True] do 
    ///         for c in [False;True] do 
    ///             for s in [False;True] do 
    ///                 if tautology(ha x y s c) then 
    ///                     printfn "| %i | %i | %i | %i |" 
    ///                       (x |> to01) (y |> to01) (c |> to01) (s |> to01)
    /// printfn "-----------------"
    /// </code>
    /// After evaluation the following is printed to the to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -----------------
    /// | x | y | c | s |
    /// -----------------
    /// | 0 | 0 | 0 | 0 |
    /// | 0 | 1 | 0 | 1 |
    /// | 1 | 0 | 0 | 1 |
    /// | 1 | 1 | 1 | 0 |
    /// -----------------
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Half adder</category>
    val ha:
      x: formula<'a> ->
        y: formula<'a> ->
        s: formula<'a> -> c: formula<'a> -> formula<'a>

    /// <summary>
    /// Carry of a full adder: <c>(x /\ y) \/ ((x \/ y) /\ z)</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates the propositional formula whose truth value corresponds to 
    /// the carry of a full adder, given the <c>x</c> and <c>y</c> digits to 
    /// be summed also represented as prop formulas: <c>False</c> for 0 
    /// <c>True</c> for 1.
    /// </remarks>
    /// 
    /// <category index="3">Full adder</category>
    val carry:
      x: formula<'a> ->
        y: formula<'a> -> z: formula<'a> -> formula<'a>

    /// <summary>
    /// Sum of a full adder: <c>(x &lt;=&gt; ~ y) &lt;=&gt; ~ z</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates the propositional formula whose truth value corresponds to 
    /// the sum of a full adder, given the <c>x</c> and <c>y</c> digits to 
    /// be summed also represented as prop formulas: <c>False</c> for 0 
    /// <c>True</c> for 1.
    /// </remarks>
    /// 
    /// <category index="3">Full adder</category>
    val sum:
      x: formula<'a> ->
        y: formula<'a> -> z: formula<'a> -> formula<'a>

    /// <summary>
    /// Full adder function.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates a propositional formula that is a tautology if the input 
    /// formulas represent respectively two digits <c>x</c> and <c>y</c> to be 
    /// summed, the <c>z</c> carry from a previous sum, the resulting sum 
    /// <c>s</c> and the carry <c>c</c>.
    /// </remarks>
    /// 
    /// <category index="3">Full adder</category>
    val fa:
      x: formula<'a> ->
        y: formula<'a> ->
        z: formula<'a> ->
        s: formula<'a> -> c: formula<'a> -> formula<'a>

    /// <summary>
    /// Conjoins multiple full-adders.
    /// </summary>
    /// 
    /// <remarks>
    /// Given a function that creates a propositional formula from an index and 
    /// a list of indexes, it puts multiple full-adders together into an n-bit 
    /// adder.
    /// <p></p>
    /// It is an auxiliary function to define ripplecarry.
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val conjoin:
      f: ('a -> formula<'b>) -> l: 'a list -> formula<'b>
        when 'b: equality

    /// <summary>
    /// N-bit ripple carry adder with carry c(0) propagated in and c(n) out.  
    /// </summary>
    /// 
    /// <remarks>
    /// Generates a propositional formula that represent a ripple-carry adder 
    /// circuit. Filtering the true rows of its truth table gives the sum and 
    /// carry values for each digits.
    /// 
    /// It expects the user to supply functions <c>x</c>, <c>y</c>, <c>out</c> 
    /// and <c>c</c> that, when given an index, generates an appropriate new 
    /// variable. Use <c>mk_index</c> to generate such functions.
    /// 
    /// For example, 
    /// 
    /// <c>let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]</c>
    /// 
    /// <c>ripplecarry x y c out 2</c>
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Generates input for ripplecarry.
    /// </summary>
    /// 
    /// <remarks>
    /// Given a prop formula <c>x</c> and an index <c>i</c>, it generates a 
    /// propositional variable <c>P "x_i"</c>.
    /// 
    /// <c>let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]</c> 
    /// generates the x, y, out and c functions that can be given 
    /// as input to ripplecarry
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val mk_index: x: string -> i: int -> formula<prop>

    /// <summary>
    /// Generates input for ripplecarry.
    /// </summary>
    /// 
    /// <remarks>
    /// Similar to <c>mk_index</c>. 
    /// 
    /// Given a prop formula <c>x</c> and an indexes <c>i</c> and <c>j</c>, it 
    /// generates a propositional variable <c>P "x_i_j"</c>.
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val mk_index2: x: string -> i: 'a -> j: 'b -> formula<prop>

    /// <summary>
    /// N-bit ripple carry adder with carry c(0) forced to 0.
    /// </summary>
    /// 
    /// <remarks>
    /// It can be used when we are not interested in a carry in at the low end.
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry0:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// N-bit ripple carry adder with carry c(0) forced at 1.
    /// </summary>
    /// 
    /// <remarks>
    /// It is used to define the carry-select adder. In a carry-select adder 
    /// the n-bit inputs are split into several blocks of k, and corresponding 
    /// k-bit blocks are added twice, once assuming a carry-in of 0 and once 
    /// assuming a carry-in of 1.
    /// </remarks>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry1:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Multiplexer 
    /// </summary>
    /// 
    /// <remarks>
    /// Used to define the carry-select adder. We will use it to select between 
    /// the two alternatives (carry-in of 0 or 1) when we do carry propagation.
    /// </remarks>
    /// 
    /// <category index="5">Carry select adder</category>
    val mux:
      sel: formula<'a> ->
        in0: formula<'a> ->
        in1: formula<'a> -> formula<'a>

    /// <summary>
    /// Offsets the indices in an array of bits.
    /// </summary>
    /// 
    /// <remarks>
    /// It is used to define the carry-select adder.
    /// </remarks>
    /// 
    /// <category index="5">Carry select adder</category>
    val offset: n: int -> x: (int -> 'a) -> i: int -> 'a

    /// <summary>Carry select adder</summary>
    /// 
    /// <category index="5">Carry select adder</category>
    val carryselect:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c0: (int -> formula<'a>) ->
        c1: (int -> formula<'a>) ->
        s0: (int -> formula<'a>) ->
        s1: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        s: (int -> formula<'a>) -> n: int -> k: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Tests equivalence of ripple carry and carry select.
    /// </summary>
    /// 
    /// <remarks>
    /// Generates propositions that state the equivalence of various 
    /// ripplecarry and carryselect circuits based on the input <c>n</c> 
    /// (number of bit to be added) and <c>k</c> (number of blocks in the 
    /// carryselect circuit).
    /// <p></p>
    /// If the proposition generated is a tautology, the equivalence between 
    /// the two circuit is proved.
    /// </remarks>
    /// 
    /// <category index="5">Carry select adder</category>
    val mk_adder_test: n: int -> k: int -> formula<prop>

    /// <summary>
    /// Ripple carry stage that separates off the final result of a 
    /// multiplication.
    /// </summary>
    /// 
    /// <category index="6">Multiplier circuit</category>
    val rippleshift:
      u: (int -> formula<'a>) ->
        v: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        z: formula<'a> ->
        w: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Naive multiplier based on repeated ripple carry. 
    /// </summary>
    /// 
    /// <category index="6">Multiplier circuit</category>
    val multiplier:
      x: (int -> int -> formula<'a>) ->
        u: (int -> int -> formula<'a>) ->
        v: (int -> int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Returns the number of bit needed to represent x in binary notation.
    /// </summary>
    /// 
    /// <category index="7">Prime numbers</category>
    val bitlength: x: int -> int

    /// <summary>
    /// Extract the <c>n</c>th bit (as a boolean value) of a nonnegative 
    /// integer <c>x</c>.
    /// </summary>
    /// 
    /// <category index="7">Prime numbers</category>
    val bit: n: int -> x: int -> bool

    /// <summary>
    /// Produces a propositional formula asserting that the atoms <c>x</c>(i) 
    /// encode the bits of a value <c>m</c>, at least modulo 2^<c>n</c>.
    /// </summary>
    /// 
    /// <category index="7">Prime numbers</category>
    val congruent_to:
      x: (int -> formula<'a>) -> m: int -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Applied to a positive integer <c>p</c> generates a propositional 
    /// formula that is a tautology precisely if <c>p</c> is prime.
    /// </summary>
    /// 
    /// <category index="7">Prime numbers</category>
    val prime: p: int -> formula<prop>