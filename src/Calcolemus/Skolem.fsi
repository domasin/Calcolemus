// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Formulas
open Fol

/// <summary>
/// Prenex and Skolem normal forms.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Skolem = 

    /// <summary>
    /// First level simplification routine.
    /// </summary>
    /// 
    /// <remarks>
    /// It performs a simplification routine but just at the first level of the 
    /// input formula <c>fm</c>. It eliminates the basic propositional 
    /// constants <c>False</c> and <c>True</c> and also the vacuous universal 
    /// and existential quantifiers (those applied to variables that does not 
    /// occur free in the body).
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>The simplified formula.</returns>
    /// 
    /// <example id="simplify1-1">
    /// <code lang="fsharp">
    /// simplify1 !!"exists x. P(y)"
    /// </code>
    /// Evaluates to <c>`P(y)`</c>.
    /// </example>
    /// 
    /// <example id="simplify1-2">
    /// <code lang="fsharp">
    /// simplify1 !!"true ==> exists x. P(x)"
    /// </code>
    /// Evaluates to <c>`exists x. P(x)`</c>.
    /// </example>
    /// 
    /// <category index="1">Simplification</category>
    val simplify1: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Simplification routine.
    /// </summary>
    /// 
    /// <remarks>
    /// It performs a simplification eliminating the basic propositional 
    /// constants <c>False</c> and <c>True</c> and also the <em>vacuous 
    /// quantifiers</em> (those applied to variables that do not occur free 
    /// in the body).
    /// <p></p>
    /// It applies <see cref='M:Calcolemus.Skolem.simplify1'/> repeatedly at 
    /// every level of the formula in a recursive bottom-up sweep.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>The simplified formula.</returns>
    /// 
    /// <example id="simplify-1">
    /// <code lang="fsharp">
    /// simplify !!"true ==> (p &lt;=&gt; (p &lt;=&gt;false))"
    /// </code>
    /// Evaluates to <c>`p &lt;=&gt; ~p>`</c>.
    /// </example>
    /// 
    /// <example id="simplify-2">
    /// <code lang="fsharp">
    /// simplify !!"exists x y z. P(x) ==> Q(z) ==> false"
    /// </code>
    /// Evaluates to <c>`exists x z. P(x) ==> ~Q(z)`</c>.
    /// </example>
    /// 
    /// <category index="1">Simplification</category>
    /// 
    val simplify: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Transforms the input formula <c>fm</c> in negation normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// It eliminates implication and equivalence, and pushes down negations 
    /// through quantifiers.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>The formula in negation normal form.</returns>
    /// 
    /// <example id="nnf-1">
    /// <code lang="fsharp">
    /// nnf !!"~ exists x. P(x) &lt;=&gt; Q(x)"
    /// </code>
    /// Evaluates to <c>`forall x. P(x) /\ ~Q(x) \/ ~P(x) /\ Q(x)`</c>.
    /// </example>
    /// 
    /// <category index="2">Negation normal form</category>
    val nnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// It pulls out quantifiers of top level conjunctions or disjunctions.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>The formula with the quantifiers pulled out.</returns>
    /// 
    /// <example id="pullquants-1">
    /// <code lang="fsharp">
    /// !!"(forall x. P(x)) /\ (exists y. P(y))"
    /// |> pullquants
    /// </code>
    /// Evaluates to <c>`forall x. exists y. P(x) /\ P(y)`</c>.
    /// </example>
    /// 
    /// <category index="3">Prenex normal form</category>
    val pullquants: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Auxiliary function to define 
    /// <see cref='M:Calcolemus.Skolem.pullquants'/>.
    /// </summary>
    /// 
    /// <remarks>
    /// It deals with various similar subcases and calls the main 
    /// <see cref='M:Calcolemus.Skolem.pullquants'/> 
    /// function again on the body to pull up further quantifiers.
    /// </remarks>
    /// 
    /// <param name="l">The flag to indicate if the left hand formula should be changed.</param>
    /// <param name="r">The flag to indicate if the right hand formula should be changed.</param>
    /// <param name="fm">The input formula.</param>
    /// <param name="quant">The quantification constructor to apply.</param>
    /// <param name="op">The binary connective constructor to apply.</param>
    /// <param name="x">The  variable to check in the left hand formula to prevent it from being bound.</param>
    /// <param name="y">The  variable to check in the right hand formula to prevent it from being bound.</param>
    /// <param name="p">The left hand formula.</param>
    /// <param name="q">The right hand formula.</param>
    /// <returns>The formula with the quantifiers pulled out.</returns>
    /// 
    /// <example id="pullquants-1">
    /// <code lang="fsharp">
    /// let fm = !!"P(x) /\ exists y. Q(y)"
    /// pullq (false, true) fm mk_exists mk_and "y" "y" !!"P(x)" !!"Q(y)"
    /// </code>
    /// Evaluates to <c>`forall x. exists y. P(x) /\ P(y)`</c>.
    /// </example>
    /// 
    /// <category index="3">Prenex normal form</category>
    val pullq:
      l: bool * r: bool ->
        fm: formula<fol> ->
        quant: (string -> formula<fol> -> formula<fol>) ->
        op: (formula<fol> ->
               formula<fol> -> formula<fol>) ->
        x: string ->
        y: string ->
        p: formula<fol> ->
        q: formula<fol> -> formula<fol>

    /// <summary>
    /// It pulls out quantifiers of a formula supposed to be already simplified 
    /// and in nnf.
    /// </summary>
    /// 
    /// <remarks>
    /// It deals with the subformulas of quantified formulas, and for the 
    /// others (conjunctions and disjunctions) calls 
    /// <see cref='M:Calcolemus.Skolem.pullquants'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>The equivalent of the input (already simplified and in nnf) with quantifiers pulled out.</returns>
    /// 
    /// <example id="prenex-1">
    /// <code lang="fsharp">
    /// !!"forall x. P(X) /\ forall y. Q(x,y)"
    /// |> prenex
    /// </code>
    /// Evaluates to <c>`forall x y. P(X) /\ Q(x,y)`</c>.
    /// </example>
    /// 
    /// <category index="3">Prenex normal form</category>
    val prenex: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Transforms the input formula <c>fm</c> in prenex normal form and 
    /// simplifies it.
    /// </summary>
    /// 
    /// <remarks>
    /// <ul>
    /// <li>simplifies away False, True, vacuous quantification, etc.;</li>
    /// <li>eliminates implication and equivalence, pushes down negations;</li>
    /// <li>pulls out quantifiers.</li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The prenex normal form equivalent of the input formula.
    /// </returns>
    /// 
    /// <example id="pnf-1">
    /// <code lang="fsharp">
    /// pnf !! @"(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P
    /// (z) /\ Q(z))"
    /// </code>
    /// Evaluates to <c>`exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)`</c>.
    /// </example>
    /// 
    /// <category index="3">Prenex normal form</category>
    /// 
    val pnf: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Returns the functions present in the input term <c>tm</c>.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// <returns>
    /// The list of name-arity pairs of the functions in the term.
    /// </returns>
    /// 
    /// <example id="funcs-1">
    /// <code lang="fsharp">
    /// funcs !!!"x + 1"
    /// </code>
    /// Evaluates to <c>[("+", 2); ("1", 0)]</c>.
    /// </example>
    /// 
    /// <category index="4">Get functions in term and formula</category>
    val funcs: tm: term -> (string * int) list

    /// <summary>
    /// Returns the functions present in the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The list of name-arity pairs of the functions in the formula.
    /// </returns>
    /// 
    /// <example id="functions-1">
    /// <code lang="fsharp">
    /// functions !!"x + 1 > 0 /\ f(z) > g(z,i)"
    /// </code>
    /// Evaluates to <c>[("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]</c>.
    /// </example>
    /// 
    /// <category index="4">Get functions in term and formula</category>
    val functions: fm: formula<fol> -> (string * int) list

    /// <summary>
    /// Core Skolemization function specifically intended to be used on 
    /// formulas already simplified and in nnf.
    /// </summary>
    /// 
    /// <remarks>
    /// It simply recursively descends the formula, Skolemizing any existential 
    /// formulas and then proceeding to subformulas using skolem2 for binary 
    /// connectives.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <param name="fns">The list of strings to avoid as names of the Skolem 
    /// functions.</param>
    /// <returns>
    /// The pair of the Skolemized formula together with the updated list of 
    /// strings to avoid as names of the Skolem functions.
    /// </returns>
    /// 
    /// <example id="skolem-1">
    /// <code lang="fsharp">
    /// skolem !!"forall x. exists y. P(x,y)" []
    /// </code>
    /// Evaluates to <c>(`forall x. P(x,f_y(x))`, ["f_y"])</c>.
    /// </example>
    /// 
    /// <category index="5">Core Skolemization</category>
    val skolem:
      fm: formula<fol> ->
        fns: string list -> formula<fol> * string list

    /// <summary>
    /// Auxiliary to <see cref='M:Calcolemus.Skolem.skolem'/> when dealing with 
    /// binary connectives.
    /// </summary>
    /// 
    /// <remarks>
    /// It updates the set of functions to avoid with new Skolem functions 
    /// introduced into one formula before tackling the other.
    /// </remarks>
    /// 
    /// <param name="cons">The binary connective constructor to apply.</param>
    /// <param name="p">The left hand formula.</param>
    /// <param name="q">The right hand formula.</param>
    /// <param name="fns">The list of strings to avoid as names of the Skolem 
    /// functions.</param>
    /// <returns>
    /// The pair of the Skolemized and reconstructed binary formula together 
    /// with the updated list of strings to avoid as names of the Skolem 
    /// functions.
    /// </returns>
    /// 
    /// <example id="skolem2-1">
    /// <code lang="fsharp">
    /// let p,q = !!"forall x. exists y. P(x,y)", !!"forall x. exists y. Q(x,y)"
    /// skolem2 And (p,q) []
    /// </code>
    /// Evaluates to <c>(`(forall x. P(x,f_y(x))) /\ (forall x. Q(x,f_y'(x)))`, ["f_y'"; "f_y"])</c>.
    /// </example>
    /// 
    /// <category index="5">Core Skolemization</category>
    val skolem2:
      cons: (formula<fol> * formula<fol> ->
               formula<fol>) ->
        p: formula<fol> * q: formula<fol> ->
          fns: string list -> formula<fol> * string list

    /// <summary>
    /// Skolemizes the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The input formula with all existential quantifiers replaced by Skolem 
    /// functions.
    /// </returns>
    /// 
    /// <example id="askolemize-1">
    /// <code lang="fsharp">
    /// askolemize !!"forall x. exists y. R(x,y)"
    /// </code>
    /// Evaluates to <c>`forall x. R(x,f_y(x))`</c>.
    /// </example>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val askolemize: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Removes all universal quantifiers from the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The input formula with all universal quantifiers removed.
    /// </returns>
    /// 
    /// <example id="specialize-1">
    /// <code lang="fsharp">
    /// specialize !!"forall x y. P(x) /\ P(y)"
    /// </code>
    /// Evaluates to <c>`P(x) /\ P(y)`</c>.
    /// </example>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val specialize: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Puts the input formula <c>fm</c> into Skolem normal form 
    /// while also removing all universal quantifiers.
    /// </summary>
    /// 
    /// <remarks>
    /// It puts the formula in prenex normal form, substitutes existential 
    /// quantifiers with Skolem functions and also removes all universal 
    /// quantifiers.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// An equisatisfiable Skolem normal form of the input with also the 
    /// universal quantifiers removed.
    /// </returns>
    /// 
    /// <example id="skolemize-1">
    /// <code lang="fsharp">
    /// skolemize !!"forall x. exists y. R(x,y)"
    /// </code>
    /// Evaluates to <c>`R(x,f_y(x))`</c>.
    /// </example>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val skolemize: fm: formula<fol> -> formula<fol>