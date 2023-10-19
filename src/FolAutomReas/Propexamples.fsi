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
/// <note>
/// <b>This document is still in progress but already a pretty good version.</b>
/// </note>
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
    /// Sum of an half adder.
    /// <br />
    /// <c>x &lt;=&gt; ~ y</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The truth-value of the generated propositional formula corresponds to 
    /// the sum of an half adder of two one-bit-numbers <c>x</c> and <c>y</c> 
    /// also represented as prop formulas. In this context the truth-values 
    /// <c>false</c> and <c>true</c> should be read as the two digits of the 
    /// binary system: <c>0</c> and <c>1</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <returns>
    /// The prop formula that represents the half adder's sum of <c>x</c> + 
    /// <c>y</c>.
    /// </returns>
    /// 
    /// <example id="halfsum-1">
    /// <code lang="fsharp">
    /// halfsum (True:prop formula) False
    /// </code>
    /// Evaluates to <c>`true &lt;=&gt; ~false`</c>.
    /// </example>
    /// 
    /// <example id="halfsum-2">
    /// The truth-table of the output formula, with truth-values converted in 
    /// integers, shows how it calculates the half-adder sum:
    /// <code lang="fsharp">
    /// let fm = halfsum (!>"x") (!>"y")
    /// // evaluates to `x &lt;=&gt; ~y`
    /// 
    /// printfn "-------------"
    /// printfn "| x | y | s |"
    /// printfn "-------------"
    /// 
    /// // for each valuation:
    /// allvaluations fm
    /// |> List.iter (fun v -> 
    ///     // for each atom:
    ///     atoms fm
    ///     |> List.iter (fun atm -> 
    ///         // print the truth-value of the atom in the valuation;
    ///         printf "| %A " (v atm |> System.Convert.ToInt32)
    ///     )
    ///     // and print the truth-value of the formula in the valuation.
    ///     printfn "| %A |" 
    ///         (eval fm v |> System.Convert.ToInt32)
    /// )
    /// printfn "-------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
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
    /// Carry of an half adder.
    /// <br />
    /// <c>x /\ y</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The truth-value of the generated propositional formula corresponds to 
    /// the carry of an half adder of two one-bit-numbers <c>x</c> and <c>y</c> 
    /// also represented as prop formulas. In this context the truth-values 
    /// <c>false</c> and <c>true</c> should be read as the two digits of the 
    /// binary system: <c>0</c> and <c>1</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <returns>
    /// The prop formula that represents the half adder's carry of <c>x</c> + 
    /// <c>y</c>.
    /// </returns>
    /// 
    /// <example id="halfcarry-1">
    /// <code lang="fsharp">
    /// halfcarry (True:prop formula) False
    /// </code>
    /// Evaluates to <c>`true /\ false``</c>.
    /// </example>
    /// 
    /// <example id="halfcarry-2">
    /// The truth-table of the output formula, with truth-values converted in 
    /// integers, shows how it calculates the half-adder carry:
    /// <code lang="fsharp">
    /// let fm = halfcarry (!>"x") (!>"y")
    /// // evaluates to `x /\ y`
    /// 
    /// printfn "-------------"
    /// printfn "| x | y | c |"
    /// printfn "-------------"
    /// 
    /// // for each valuation:
    /// allvaluations fm
    /// |> List.iter (fun v -> 
    ///     // for each atom:
    ///     atoms fm
    ///     |> List.iter (fun atm -> 
    ///         // print the truth-value of the atom in the valuation;
    ///         printf "| %A " (v atm |> System.Convert.ToInt32)
    ///     )
    ///     // and print the truth-value of the formula in the valuation.
    ///     printfn "| %A |" 
    ///         (eval fm v |> System.Convert.ToInt32)
    /// )
    /// printfn "-------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -------------
    /// | x | y | c |
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
    /// <br />
    /// <c>(s &lt;=&gt; x &lt;=&gt; ~y) /\ (c &lt;=&gt; x /\ y)</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// An half adder calculates the sum and carry for only two 
    /// one-bit-numbers.
    /// <p></p>
    /// <c>ha</c> generates a propositional formula that is true in those 
    /// valuations in which <c>s</c> and <c>c</c> are, respectively, the sum 
    /// and carry calculated by an half-adder for the sum of <c>x</c> and 
    /// <c>y</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <param name="s">The supposed sum.</param>
    /// <param name="c">The supposed carry.</param>
    /// <returns>
    /// The propositional formula that is true in those valuations in which 
    /// <c>s</c> and <c>c</c> are, respectively, the sum and carry calculated 
    /// by an half-adder for the sum of <c>x</c> and <c>y</c>.
    /// </returns>
    /// 
    /// <example id="ha-1">
    /// <code lang="fsharp">
    /// ha (True:prop formula) True False True
    /// </code>
    /// Evaluates to <c>`(false &lt;=&gt; true &lt;=&gt; ~true) /\ (true &lt;=&gt; true /\ true)`</c>.
    /// </example>
    /// 
    /// <example id="ha-2">
    /// All the valuations satisfying the formula represent the correct 
    /// relation between input and output of an half adder.
    /// <code lang="fsharp">
    /// let fm = ha (!>"x") (!>"y") (!>"s") (!>"c")
    /// // `(s &lt;=&gt; x &lt;=&gt; ~y) /\ (c &lt;=&gt; x /\ y)`
    /// 
    /// printfn "-----------------"
    /// printfn "| x | y | c | s |"
    /// printfn "-----------------"
    /// 
    /// (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    /// |> List.iter (fun v -> 
    ///     printfn "| %A | %A | %A | %A |" 
    ///         (v (P "x") |> System.Convert.ToInt32)
    ///         (v (P "y") |> System.Convert.ToInt32)
    ///         (v (P "c") |> System.Convert.ToInt32)
    ///         (v (P "s") |> System.Convert.ToInt32)
    /// )
    /// printfn "-----------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
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
    /// Carry of a full adder.
    /// <br />
    /// <c>(x /\ y) \/ ((x \/ y) /\ z)</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The truth-value of the generated propositional formula corresponds to 
    /// the carry of a full adder of two one-bit-numbers <c>x</c> and <c>y</c> 
    /// plus a carry <c>z</c> from a previous sum also represented as prop 
    /// formulas. In this context the truth-values <c>false</c> and <c>true</c> 
    /// should be read as the two digits of the binary system: <c>0</c> and 
    /// <c>1</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <param name="z">The possible carry of a previous sum.</param>
    /// <returns>
    /// The full adder's carry of <c>x</c> + <c>y</c> + <c>z</c>
    /// </returns>
    /// 
    /// <example id="carry-1">
    /// <code lang="fsharp">
    /// carry (True:prop formula) False True
    /// </code>
    /// Evaluates to <c>`true /\ false \/ (true \/ false) /\ true`</c>.
    /// </example>
    /// 
    /// <example id="carry-2">
    /// The truth-table of the output formula, with truth-values converted in 
    /// integers, shows how it calculates the half-adder carry: 
    /// <code lang="fsharp">
    /// let fm = carry (!>"x") (!>"y") (!>"z")
    /// // evaluates to `x /\ y \/ (x \/ y) /\ z`
    /// 
    /// printfn "-----------------"
    /// printfn "| x | y | z | c |"
    /// printfn "-----------------"
    /// 
    /// // for each valuation:
    /// allvaluations fm
    /// |> List.iter (fun v -> 
    ///     // for each atom:
    ///     atoms fm
    ///     |> List.iter (fun atm -> 
    ///         // print the truth-value of the atom in the valuation;
    ///         printf "| %A " (v atm |> System.Convert.ToInt32)
    ///     )
    ///     // and print the truth-value of the formula in the valuation.
    ///     printfn "| %A |" 
    ///         (eval fm v |> System.Convert.ToInt32)
    /// )
    /// printfn "-----------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -----------------
    /// | x | y | z | c |
    /// -----------------
    /// | 0 | 0 | 0 | 0 |
    /// | 0 | 0 | 1 | 0 |
    /// | 0 | 1 | 0 | 0 |
    /// | 0 | 1 | 1 | 1 |
    /// | 1 | 0 | 0 | 0 |
    /// | 1 | 0 | 1 | 1 |
    /// | 1 | 1 | 0 | 1 |
    /// | 1 | 1 | 1 | 1 |
    /// -----------------
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Full adder</category>
    val carry:
      x: formula<'a> ->
        y: formula<'a> -> z: formula<'a> -> formula<'a>

    /// <summary>
    /// Sum of a full adder.
    /// <br />
    /// <c>(x &lt;=&gt; ~ y) &lt;=&gt; ~ z</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// The truth-value of the generated propositional formula corresponds to 
    /// the sum of a full adder of two one-bit-numbers <c>x</c> and <c>y</c> 
    /// plus a carry <c>z</c> from a previous sum also represented as prop 
    /// formulas. In this context the truth-values <c>false</c> and <c>true</c> 
    /// should be read as the two digits of the binary system: <c>0</c> and 
    /// <c>1</c>.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <param name="z">The possibile carry of a previous sum.</param>
    /// <returns>
    /// The full adder's sum of <c>x</c> + <c>y</c> + <c>z</c>.
    /// </returns>
    /// 
    /// <example id="sum-1">
    /// <code lang="fsharp">
    /// sum (True:prop formula) False True
    /// </code>
    /// Evaluates to <c>`(true &lt;=&gt; ~false) &lt;=&gt; ~true`</c>.
    /// </example>
    /// 
    /// <example id="sum-2">
    /// The truth-table of the output formula, with truth-values converted in 
    /// integers, shows how it calculates the half-adder carry: 
    /// <code lang="fsharp">
    /// let fm = sum (!>"x") (!>"y") (!>"z")
    /// // evaluates to `(x &lt;=&gt; ~y) &lt;=&gt; ~z`
    /// 
    /// printfn "-----------------"
    /// printfn "| x | y | z | c |"
    /// printfn "-----------------"
    /// 
    /// // for each valuation:
    /// allvaluations fm
    /// |> List.iter (fun v -> 
    ///     // for each atom:
    ///     atoms fm
    ///     |> List.iter (fun atm -> 
    ///         // print the truth-value of the atom in the valuation;
    ///         printf "| %A " (v atm |> System.Convert.ToInt32)
    ///     )
    ///     // and print the truth-value of the formula in the valuation.
    ///     printfn "| %A |" 
    ///         (eval fm v |> System.Convert.ToInt32)
    /// )
    /// printfn "-----------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// -----------------
    /// | x | y | z | c |
    /// -----------------
    /// | 0 | 0 | 0 | 0 |
    /// | 0 | 0 | 1 | 1 |
    /// | 0 | 1 | 0 | 1 |
    /// | 0 | 1 | 1 | 0 |
    /// | 1 | 0 | 0 | 1 |
    /// | 1 | 0 | 1 | 0 |
    /// | 1 | 1 | 0 | 0 |
    /// | 1 | 1 | 1 | 1 |
    /// -----------------
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Full adder</category>
    val sum:
      x: formula<'a> ->
        y: formula<'a> -> z: formula<'a> -> formula<'a>

    /// <summary>
    /// Full adder function.
    /// <br />
    /// <c>(s &lt;=&gt; (x &lt;=&gt; ~y) &lt;=&gt; ~z) /\ (c &lt;=&gt; x /\ y \/ (x \/ y) /\ z)</c>
    /// </summary>
    /// 
    /// <remarks>
    /// A full adder is a one-bit adder that adds three one-bit-numbers: two 
    /// operands <c>x</c> and <c>y</c> plus <c>z</c> that represent the 
    /// carry from a previous sum.
    /// <p></p>
    /// <c>fa</c> generates a propositional formula that is true in those 
    /// valuations in which <c>s</c> and <c>c</c> are, respectively, the sum 
    /// and carry calculated by a full-adder for the sum of <c>x</c> and 
    /// <c>y</c> plus the carry <c>z</c> for a previous sum.
    /// </remarks>
    /// 
    /// <param name="x">The first one-bit-number to be added.</param>
    /// <param name="y">The second one-bit-number to be added.</param>
    /// <param name="z">The carry from a previous sum.</param>
    /// <param name="s">The supposed sum.</param>
    /// <param name="c">The supposed carry.</param>
    /// <returns>
    /// The propositional formula that is true in those valuations in which 
    /// <c>s</c> and <c>c</c> are, respectively, the sum and carry calculated 
    /// by a full-adder for the sum of <c>x</c> and <c>y</c> plus the carry 
    /// <c>z</c> for a previous sum.
    /// </returns>
    /// 
    /// <example id="ha-2">
    /// All the valuations satisfying the formula represent the correct 
    /// relation between input and output of an half adder. 
    /// <code lang="fsharp">
    /// printfn "---------------------"
    /// printfn "| x | y | z | c | s |"
    /// printfn "---------------------"
    /// 
    /// (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    /// |> List.iter (fun v -> 
    ///     printfn "| %A | %A | %A | %A | %A |" 
    ///         (v (P "x") |> System.Convert.ToInt32)
    ///         (v (P "y") |> System.Convert.ToInt32)
    ///         (v (P "z") |> System.Convert.ToInt32)
    ///         (v (P "c") |> System.Convert.ToInt32)
    ///         (v (P "s") |> System.Convert.ToInt32)
    /// )
    /// printfn "---------------------"
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// ---------------------
    /// | x | y | z | c | s |
    /// ---------------------
    /// | 0 | 0 | 0 | 0 | 0 |
    /// | 0 | 0 | 1 | 0 | 1 |
    /// | 0 | 1 | 0 | 0 | 1 |
    /// | 1 | 0 | 0 | 0 | 1 |
    /// | 0 | 1 | 1 | 1 | 0 |
    /// | 1 | 0 | 1 | 1 | 0 |
    /// | 1 | 1 | 0 | 1 | 0 |
    /// | 1 | 1 | 1 | 1 | 1 |
    /// ---------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Full adder</category>
    val fa:
      x: formula<'a> ->
        y: formula<'a> ->
        z: formula<'a> ->
        s: formula<'a> -> c: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a conjunction of the formulas obtained by applying a 
    /// function (from indexes to formulas) to the elements of a list of 
    /// indexes.
    /// </summary>
    /// 
    /// <remarks>
    /// Its intended use, in our context, is to put multiple 1-bit adders 
    /// together into an n-bit adder. Indexes in this case point to the 
    /// n-bit positions of the two n-bit numbers to be added. For example, 
    /// <see cref='M:FolAutomReas.Propexamples.ripplecarry``1'/> use this 
    /// function to 'conjoin' n 
    /// <see cref='M:FolAutomReas.Propexamples.fa``1'/>'s in order to obtain an 
    /// n-bit adder.
    /// </remarks>
    /// 
    /// <param name="f">A function from indexes to formulas.</param>
    /// <param name="l">A list of indexes.</param>
    /// 
    /// <returns>
    /// The conjunctions of the formulas obtained by applying <c>f</c>
    /// to the elements of <c>l</c>.
    /// </returns>
    /// 
    /// <example id="conjoin-1">
    /// <code lang="fsharp">
    /// conjoin Atom [1;2;3]
    /// </code>
    /// Evaluates to <c>And (Atom 1, And (Atom 2, Atom 3))</c>.
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val conjoin:
      f: ('a -> formula<'b>) -> l: 'a list -> formula<'b>
        when 'b: equality

    /// <summary>
    /// Ripple carry adder with carry c(0) propagated in and c(n) out.
    /// <br />
    /// <c>(s_0 &lt;=&gt; (x_0 &lt;=&gt; ~y_0) &lt;=&gt; ~c_0) /\ (c_1 &lt;=&gt; x_0 /\ y_0 \/ (x_0 \/ y_0) /\ c_0) /\ 
    /// ... /\ 
    /// (s_n &lt;=&gt; (x_n &lt;=&gt; ~y_n) &lt;=&gt; ~c_n) /\ (c_(n+1) &lt;=&gt; x_n /\ y_n \/ (x_n \/ y_n) /\ c_n)</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Conjoins (see <see cref='M:FolAutomReas.Propexamples.conjoin``2'/>) 
    /// <c>n</c> one-bit full adders to obtain an <c>n</c>-bit adder.
    /// <p></p>
    /// The generated propositional formula represents the correct relations 
    /// between the input and output of a ripple carry adder, just as 
    /// <see cref='M:FolAutomReas.Propexamples.ha``1'/> and 
    /// <see cref='M:FolAutomReas.Propexamples.fa``1'/>, respectively, 
    /// represent the correct relations between input and output for an 
    /// half-adder and a full-adder.
    /// <p></p>
    /// It expects the user to supply functions <c>x</c>, <c>y</c>, <c>out</c> 
    /// and <c>c</c> that, when given an index, generates an appropriate new 
    /// variable. <see cref='M:FolAutomReas.Propexamples.mk_index'/> is 
    /// supplied to generate such functions.
    /// </remarks>
    /// 
    /// <param name="x">A function that, given an index, returns a variable for the value of the first addend at that bit (index).</param>
    /// <param name="y">A function that, given an index, returns a variable for the value of the second addend at that bit (index).</param>
    /// <param name="c">A function that, given an index, returns a variable for the value of the carry (in and out) at that bit (index).</param>
    /// <param name="out">A function that, given an index, returns a variable for the value of the sum at that bit (index).</param>
    /// <param name="n">The number of bits added by the ripplecarry adder.</param>
    /// <returns>
    /// The conjunction of the formulas that represent full adders for each 
    /// bit position.
    /// </returns>
    /// 
    /// <example id="ripplecarry-0">
    /// Ripple carry adder of two 2-bit numbers: printing all the valuations 
    /// satisfying the formula shows all the possible correct relations between 
    /// input.
    /// <code lang="fsharp">
    /// let x, y, s, c = 
    ///     mk_index "x",
    ///     mk_index "y",
    ///     mk_index "s",
    ///     mk_index "c"
    /// 
    /// let fm = ripplecarry x y c s 2
    /// // ((s_0 &lt;=&gt; (x_0 &lt;=&gt; ~y_0) &lt;=&gt; ~c_0) /\ (c_1 &lt;=&gt; x_0 /\ y_0 \/ 
    /// // (x_0 \/ y_0) /\ c_0)) /\ 
    /// // (s_1 &lt;=&gt; (x_1 &lt;=&gt; ~y_1) &lt;=&gt; ~c_1) /\ (c_2 &lt;=&gt; x_1 /\ y_1 \/ 
    /// // (x_1 \/ y_1) /\ c_1)
    /// 
    /// // eval the atom at the input valuations and convert to int
    /// let toInt (v: prop -> bool) x =
    ///     v (P x) |> System.Convert.ToInt32
    /// 
    /// allsatvaluations (eval fm) (fun _ -> false) (atoms fm)
    /// |> List.iteri (fun i v -> 
    ///     printfn "carry |   %A %A |" (toInt v "c_1") (toInt v "c_0")
    ///     printfn "---------------"
    ///     printfn "x     |   %A %A |" (toInt v "x_1") (toInt v "x_0")
    ///     printfn "y     |   %A %A |" (toInt v "y_1") (toInt v "y_0")
    ///     printfn "==============="
    ///     printfn "sum   | %A %A %A |" 
    ///         (toInt v "c_2")
    ///         (toInt v "s_1")
    ///         (toInt v "s_0")
    ///     printfn ""
    /// )
    /// </code>
    /// Some output omitted for brevity:
    /// <code lang="fsharp">
    /// carry |   0 0 |
    /// ---------------
    /// x     |   0 0 |
    /// y     |   0 0 |
    /// ===============
    /// sum   | 0 0 0 |
    /// ...
    /// carry |   0 0 |
    /// ---------------
    /// x     |   1 0 |
    /// y     |   1 1 |
    /// ===============
    /// sum   | 1 0 1 |
    /// ...
    /// carry |   1 1 |
    /// ---------------
    /// x     |   1 1 |
    /// y     |   1 1 |
    /// ===============
    /// sum   | 1 1 1 |
    /// </code>
    /// Note how this version admit a carry c(0) different from 0 propagated in 
    /// at the first bit.
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Returns indexed propositional variables.
    /// </summary>
    /// 
    /// <remarks>
    /// Given a string <c>"x"</c> and an integer <c>i</c>, generates a 
    /// propositional variable <c>Atom (P x_i)</c>.
    /// <p></p>
    /// Its intended use is to generate input for the ripplecarry functions: 
    /// partially applying <c>mk_index</c>, just to the first string argument, 
    /// generates the type of functions (from integers to propositional 
    /// variables) expected by ripplecarry functions.
    /// </remarks>
    /// 
    /// <param name="x">The variable name.</param>
    /// <param name="i">The variable index.</param>
    /// <returns>The indexed prop variable <c>Atom (P x_i)</c></returns>
    /// 
    /// <example id="mk_index-1">
    /// <code lang="fsharp">
    /// mk_index "x" 0
    /// </code>
    /// Evaluates to <c>Atom (P "x_0")</c>.
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val mk_index: x: string -> i: int -> formula<prop>

    /// <summary>
    /// Returns double indexed propositional variables.
    /// </summary>
    /// 
    /// <remarks>
    /// Similar to <see cref='M:FolAutomReas.Propexamples.mk_index'/> 
    /// <p></p>
    /// Given a string <c>"x"</c> and indexes <c>i</c> and <c>j</c>, it 
    /// generates a propositional variable <c>Atom (P x_i_j)</c>.
    /// </remarks>
    /// 
    /// <param name="x">The variable name.</param>
    /// <param name="i">The first variable index.</param>
    /// <param name="j">The second variable index.</param>
    /// <returns>
    /// The double indexed prop variable <c>Atom (P x_i_j)</c>.
    /// </returns>
    /// 
    /// <example id="mk_index2-1">
    /// <code lang="fsharp">
    /// mk_index2 "x" 0 1
    /// </code>
    /// Evaluates to <c>Atom (P "x_0_1")</c>.
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val mk_index2: x: string -> i: 'a -> j: 'b -> formula<prop>

    /// <summary>
    /// Ripple carry adder with carry c(0) forced to 0.
    /// <br />
    /// <c>((s_0 &lt;=&gt; x_0 &lt;=&gt; ~y_0) /\ (c_1 &lt;=&gt; x_0 /\ y_0)) /\ ... /\ (s_n &lt;=&gt; (x_n &lt;=&gt; ~y_n) &lt;=&gt; ~c_n) /\ (c_(n+1) &lt;=&gt; x_n /\ y_n \/ (x_n \/ y_n) /\ c_n)</c>
    /// </summary>
    /// 
    /// <remarks>
    /// It can be used when we are not interested in a carry in at the low end.
    /// </remarks>
    /// 
    /// <param name="x">A function that, given an index, returns a variable for the value of the first addend at that bit (index).</param>
    /// <param name="y">A function that, given an index, returns a variable for the value of the second addend at that bit (index).</param>
    /// <param name="c">A function that, given an index, returns a variable for the value of the carry (in and out) at that bit (index).</param>
    /// <param name="out">A function that, given an index, returns a variable for the value of the sum at that bit (index).</param>
    /// <param name="n">The number of bits added by the ripplecarry adder.</param>
    /// <returns>
    /// The conjunction of the formulas that represent full adders for each 
    /// bit position, except for the first position that is represented by an 
    /// half adder.
    /// </returns>
    /// 
    /// <example id="ripplecarry0-1">
    /// Filtering the satisfying valuations with x_1=0,x_0=0,y_1=1,y_0=1 
    /// gives the ripplecarry0 for the sum of 01+11:
    /// <code lang="fsharp">
    /// let rc0 = ripplecarry0 x y c s 2
    /// // ((s_0 &lt;=&gt; x_0 &lt;=&gt; ~y_0) /\ 
    /// //      (c_1 &lt;=&gt; x_0 /\ y_0)) /\ 
    /// // (s_1 &lt;=&gt; (x_1 &lt;=&gt; ~y_1) &lt;=&gt; ~c_1) /\ 
    /// //      (c_2 &lt;=&gt; x_1 /\ y_1 \/ (x_1 \/ y_1) /\ c_1)
    /// 
    /// // eval the atom at the input valuations and convert to int
    /// let toInt (v: prop -> bool) x =
    ///     v (P x) |> System.Convert.ToInt32
    /// 
    /// allsatvaluations (eval rc0) (fun _ -> false) (atoms rc0)
    /// |> List.filter (fun v -> 
    ///     (toInt v "x_1") = 0 &amp;&amp; (toInt v "x_0") = 1      // x = 01
    ///     &amp;&amp; (toInt v "y_1") = 1 &amp;&amp; (toInt v "y_0") = 1   // y = 11
    /// )
    /// |> List.iteri (fun i v -> 
    ///     printfn "carry |   %A %A |" (toInt v "c_1") (toInt v "c_0")
    ///     printfn "---------------"
    ///     printfn "x     |   %A %A |" (toInt v "x_1") (toInt v "x_0")
    ///     printfn "y     |   %A %A |" (toInt v "y_1") (toInt v "y_0")
    ///     printfn "==============="
    ///     printfn "sum   | %A %A %A |" 
    ///         (toInt v "c_2")
    ///         (toInt v "s_1")
    ///         (toInt v "s_0")
    ///     printfn ""
    /// )
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// carry |   1 0 |
    /// ---------------
    /// x     |   0 1 |
    /// y     |   1 1 |
    /// ===============
    /// sum   | 1 0 0 |
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry0:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Ripple carry adder with carry c(0) forced to 1.
    /// <br />
    /// <c>((s_0 &lt;=&gt; ~(x_0 &lt;=&gt; ~y_0)) /\ (c_1 &lt;=&gt; x_0 /\ y_0 \/ x_0 \/ y_0)) /\ ... /\(s_n &lt;=&gt; (x_n &lt;=&gt; ~y_n) &lt;=&gt; ~c_n) /\ (c_(n+1) &lt;=&gt; x_n /\ y_n \/ (x_n \/ y_n) /\ c_n)</c>
    /// </summary>
    /// 
    /// <remarks>
    /// Together with <see cref='M:FolAutomReas.Propexamples.ripplecarry0``1'/> 
    /// is used to define <see cref='M:FolAutomReas.Propexamples.carryselect``1'/>.
    /// </remarks>
    /// 
    /// <param name="x">A function that, given an index, returns a variable for the value of the first addend at that bit (index).</param>
    /// <param name="y">A function that, given an index, returns a variable for the value of the second addend at that bit (index).</param>
    /// <param name="c">A function that, given an index, returns a variable for the value of the carry (in and out) at that bit (index).</param>
    /// <param name="out">A function that, given an index, returns a variable for the value of the sum at that bit (index).</param>
    /// <param name="n">The number of bits added by the ripplecarry adder.</param>
    /// <returns>
    /// The conjunction of the formulas that represent full adders for each 
    /// bit position, except for the first position in which a carry-in of 1 is 
    /// forced.
    /// </returns>
    /// 
    /// <example id="ripplecarry1-1">
    /// Filtering the satisfying valuations with x_1=0,x_0=0,y_1=1,y_0=1 
    /// gives the ripplecarry1 for the sum of 01+11:
    /// <code lang="fsharp">
    /// let rc1 = ripplecarry1 x y c s 2
    /// // ((s_0 &lt;=&gt; ~(x_0 &lt;=&gt; ~y_0)) /\ 
    /// //    (c_1 &lt;=&gt; x_0 /\ y_0 \/ x_0 \/ y_0)) /\ 
    /// // (s_1 &lt;=&gt; (x_1 &lt;=&gt; ~y_1) &lt;=&gt; ~c_1) /\ 
    /// //    (c_2 &lt;=&gt; x_1 /\ y_1 \/ (x_1 \/ y_1) /\ c_1)
    /// 
    /// // eval the atom at the input valuations and convert to int
    /// let toInt (v: prop -> bool) x =
    ///     v (P x) |> System.Convert.ToInt32
    /// 
    /// allsatvaluations (eval rc1) (fun _ -> false) (atoms rc1)
    /// |> List.filter (fun v -> 
    ///     (toInt v "x_1") = 0 &amp;&amp; (toInt v "x_0") = 1      // x = 01
    ///     &amp;&amp; (toInt v "y_1") = 1 &amp;&amp; (toInt v "y_0") = 1   // y = 11
    /// )
    /// |> List.iteri (fun i v -> 
    ///     printfn "carry |   %A %A |" (toInt v "c_1") (toInt v "c_0")
    ///     printfn "---------------"
    ///     printfn "x     |   %A %A |" (toInt v "x_1") (toInt v "x_0")
    ///     printfn "y     |   %A %A |" (toInt v "y_1") (toInt v "y_0")
    ///     printfn "==============="
    ///     printfn "sum   | %A %A %A |" 
    ///         (toInt v "c_2")
    ///         (toInt v "s_1")
    ///         (toInt v "s_0")
    ///     printfn ""
    /// )
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// carry |   1 0 |
    /// ---------------
    /// x     |   0 1 |
    /// y     |   1 1 |
    /// ===============
    /// sum   | 1 0 1 |
    /// </code>
    /// Note that while there is no <c>c_0</c> in the formula the sum is 
    /// calculated as if <c>c_0</c> were equal to 1.
    /// </example>
    /// 
    /// <category index="4">Ripple carry adder</category>
    val ripplecarry1:
      x: (int -> formula<'a>) ->
        y: (int -> formula<'a>) ->
        c: (int -> formula<'a>) ->
        out: (int -> formula<'a>) -> n: int -> formula<'a>
        when 'a: equality

    /// <summary>
    /// Multiplexer.
    /// <br />
    /// <c>~sel /\ in0 \/ sel /\ in1</c>
    /// </summary>
    /// 
    /// <remarks>
    /// Selects between two alternatives: if <c>sel</c> selects <c>in1</c>, 
    /// otherwise selects <c>in0</c>.
    /// <p></p>
    /// It is used in the the carry-select adder's implementation to select 
    /// between carry-in of 0 or 1 when we do carry propagation.
    /// </remarks>
    /// 
    /// <param name="sel">The formula that drives selection.</param>
    /// <param name="in0">The first option.</param>
    /// <param name="in1">The second option.</param>
    /// <returns>
    /// The formula that when <c>sel</c> is true, corresponds to 
    /// <c>in1</c> and otherwise to <c>in0</c>.
    /// </returns>
    /// 
    /// <example id="mux-1">
    /// If <c>sel=true</c>, the formula corresponds to <c>in1</c>.
    /// <code lang="fsharp">
    /// mux !>"true" !>"in0" !>"in1"
    /// |> print_truthtable
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>.
    /// <code lang="fsharp">
    /// in0   in1   |   formula
    /// ---------------------
    /// false false | false 
    /// false true  | true  
    /// true  false | false 
    /// true  true  | true  
    /// ---------------------
    /// </code>
    /// </example>
    /// 
    /// <example id="mux-2">
    /// If <c>sel=false</c>, the formula corresponds to <c>in0</c>.
    /// <code lang="fsharp">
    /// mux !>"false" !>"in0" !>"in1"
    /// |> print_truthtable
    /// </code>
    /// After evaluation the following is printed to the <c>stdout</c>.
    /// <code lang="fsharp">
    /// in0   in1   |   formula
    /// ---------------------
    /// false false | false 
    /// false true  | false 
    /// true  false | true  
    /// true  true  | true  
    /// ---------------------
    /// </code>
    /// </example>
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