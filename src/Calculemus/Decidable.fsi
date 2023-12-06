// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Some decidable subsets of first-order logic.
/// </summary>
/// 
/// <category index="6">Decidable subsets and theories</category>
module Decidable = 

    /// <summary>
    /// Tests the validity of a formula in the AE fragment.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input AE formula is valid; otherwise, if the input AE 
    /// formula is invalid, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Not decidable</c> when the input formula doesn't belong to the AE fragment.</exception>
    /// 
    /// <example id="aedecide-1">
    /// <code lang="fsharp">
    /// !! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
    /// (forall u v w x y z.
    /// P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) &lt;=&gt; P(u,z,v)))
    /// ==> forall a b c. P(a,b,c) ==> P(b,a,c)"
    /// |> aedecide
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="aedecide-2">
    /// <code lang="fsharp">
    /// !! @"forall x. f(x) = 0"
    /// |> aedecide
    /// </code>
    /// Throws <c>System.Exception: Not decidable</c>.
    /// </example>
    /// 
    /// <category index="1">The AE fragment</category>
    val aedecide: fm: formula<fol> -> bool

    /// <summary>
    /// Separates in an iterated conjunction those conjuncts with the given 
    /// variable free from those in which is not.
    /// </summary>
    /// 
    /// <remarks>
    /// The intention is to transform an existential formula of the form 
    /// \(\exists x.\ p_1 \land \cdots \land p_n \) in \((\exists x.\ p_i \land 
    /// \cdots \land p_j) \land (p_k \land \cdots \land p_l)\) where the \(p_i 
    /// \land \cdots \land p_j\) are the those with \(x\) free and \(p_k \land 
    /// \cdots \land p_l)\) are the other.
    /// </remarks>
    /// 
    /// <param name="x">The given input variable.</param>
    /// <param name="cjs">The conjuncts in the existential formula.</param>
    /// 
    /// <returns>
    /// The existential formula of the conjunction of the <c>cjs</c> in which 
    /// <c>x</c> is free conjuncted with the <c>cjs</c> in which <c>x</c> is 
    /// not.
    /// </returns>
    /// 
    /// <example id="separate-1">
    /// <code lang="fsharp">
    /// !!>["P(x)"; "Q(y)"; "T(y) /\ R(x,y)"; "S(z,w) ==> Q(i)"]
    /// |> separate "x"
    /// </code>
    /// Evaluates to <c>`(exists x. P(x) /\ T(y) /\ R(x,y)) /\ Q(y) /\ (S(z,w) ==> Q(i))`</c>.
    /// </example>
    /// 
    /// <category index="2">Miniscoping and the monadic fragment</category>
    val separate:
      x: string -> cjs: formula<fol> list -> formula<fol>

    /// <summary>
    /// Given a variable <c>x</c> and a formula <c>p</c> transforms the formula 
    /// <c>exists x. p</c> into an equivalent with the scope of the quantifier 
    /// reduced.
    /// </summary>
    /// 
    /// <param name="x">The input variable.</param>
    /// <param name="p">The input formula.</param>
    /// 
    /// <returns>
    /// The formula <c>exists x. p</c> transformed into an equivalent with the 
    /// scope of the quantifier reduced.
    /// </returns>
    /// 
    /// <example id="pushquant-1">
    /// <code lang="fsharp">
    /// !!"P(x) ==> forall y. Q(y)"
    /// |> pushquant "x"
    /// </code>
    /// Evaluates to <c>`(exists x. ~P(x)) \/ (forall y. Q(y))`</c>.
    /// </example>
    /// 
    /// <category index="2">Miniscoping and the monadic fragment</category>
    val pushquant:
      x: string -> p: formula<fol> -> formula<fol>

    /// <summary>
    /// Minimizes the scope of quantifiers in a NNF formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A formula equivalent to the input with the scope of quantifiers 
    /// minimized.
    /// </returns>
    /// 
    /// <example id="miniscope-1">
    /// <code lang="fsharp">
    /// miniscope(nnf !!"exists y. forall x. P(y) ==> P(x)")
    /// </code>
    /// Evaluates to <c>`(exists y. ~P(y)) \/ (forall x. P(x))`</c>.
    /// </example>
    /// 
    /// <category index="2">Miniscoping and the monadic fragment</category>
    val miniscope: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Tests the validity of a formula that after applying miniscoping belongs 
    /// to the AE fragment.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input is a formula that after applying miniscoping belongs 
    /// to the AE fragment and is valid; otherwise, false.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Not decidable</c> when the input formula, even after applying miniscoping, does not belong to the AE fragment.</exception>
    /// 
    /// <example id="wang-1">
    /// <code lang="fsharp">
    /// wang Pelletier.p20
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="wang-2">
    /// <code lang="fsharp">
    /// wang !!"forall x. f(x) = 0"
    /// </code>
    /// Throws <c>System.Exception: Not decidable</c>.
    /// </example>
    /// 
    /// <category index="2">Miniscoping and the monadic fragment</category>
    val wang: fm: formula<fol> -> bool

    /// <summary>
    /// Constructs an atom of the form <c>p(x)</c>.
    /// </summary>
    /// 
    /// <param name="p">The input monadic predicate.</param>
    /// <param name="x">The input variable.</param>
    /// 
    /// <returns>
    /// The atom <c>p(x)</c>.
    /// </returns>
    /// 
    /// <example id="atom-1">
    /// <code lang="fsharp">
    /// atom "P" "x"
    /// </code>
    /// Evaluates to <c>`P(x)`</c>.
    /// </example>
    /// 
    /// <category index="3">Syllogisms</category>
    val atom: p: string -> x: string -> formula<fol>

    /// <summary>
    /// Constructs an A premiss (universal affirmative) 'all S are P' 
    /// \(\forall x.\ S(x) \Rightarrow P(x)\).
    /// </summary>
    /// 
    /// <param name="p">The first input monadic predicate.</param>
    /// <param name="q">The second input monadic predicate.</param>
    /// 
    /// <returns>
    /// The formula <c>forall x. p(x) ==> q(x)</c>.
    /// </returns>
    /// 
    /// <example id="premiss_A-1">
    /// <code lang="fsharp">
    /// premiss_A ("P", "S")
    /// </code>
    /// Evaluates to <c>`forall x. P(x) ==> S(x)`</c>.
    /// </example>
    /// 
    /// <category index="3">Syllogisms</category>
    val premiss_A: p: string * q: string -> formula<fol>

    /// <summary>
    /// Constructs an E premiss (universal negative) 'no S are P' 
    /// \(\forall x.\ S(x) \Rightarrow \lnot P(x)\).
    /// </summary>
    /// 
    /// <param name="p">The first input monadic predicate.</param>
    /// <param name="q">The second input monadic predicate.</param>
    /// 
    /// <returns>
    /// The formula <c>forall x. p(x) ==> ~q(x)</c>.
    /// </returns>
    /// 
    /// <example id="premiss_E-1">
    /// <code lang="fsharp">
    /// premiss_E ("P", "S")
    /// </code>
    /// Evaluates to <c>`forall x. P(x) ==> ~S(x)`</c>.
    /// </example>
    /// 
    /// <category index="3">Syllogisms</category>
    val premiss_E: p: string * q: string -> formula<fol>

    /// <summary>
    /// Constructs an I premiss (particular affirmative) 'some S are P' 
    /// \(\exists x.\ S(x) \land P(x)\).
    /// </summary>
    /// 
    /// <param name="p">The first input monadic predicate.</param>
    /// <param name="q">The second input monadic predicate.</param>
    /// 
    /// <returns>
    /// The formula <c>exists x. p(x) /\ q(x)</c>.
    /// </returns>
    /// 
    /// <example id="premiss_I-1">
    /// <code lang="fsharp">
    /// premiss_I ("P", "S")
    /// </code>
    /// Evaluates to <c>`exists x. P(x) /\ S(x)`</c>.
    /// </example>
    /// 
    /// <category index="3">Syllogisms</category>
    val premiss_I: p: string * q: string -> formula<fol>

    /// <summary>
    /// Constructs an O premiss (particular negative) 'some S are not P' 
    /// \(\exists x.\ S(x) \land \lnot P(x)\).
    /// </summary>
    /// 
    /// <param name="p">The first input monadic predicate.</param>
    /// <param name="q">The second input monadic predicate.</param>
    /// 
    /// <returns>
    /// The formula <c>exists x. p(x) /\ ~q(x)</c>.
    /// </returns>
    /// 
    /// <example id="premiss_O-1">
    /// <code lang="fsharp">
    /// premiss_O ("P", "S")
    /// </code>
    /// Evaluates to <c>`exists x. P(x) /\ ~S(x)`</c>.
    /// </example>
    /// 
    /// <category index="3">Syllogisms</category>
    val premiss_O: p: string * q: string -> formula<fol>

    /// <summary>
    /// Returns an English reading of a premiss.
    /// </summary>
    /// 
    /// <param name="fm">The input syllogism premiss.</param>
    /// 
    /// <returns>
    /// An English reading of the input syllogism premiss.
    /// </returns>
    /// 
    /// <exception cref="T:System.ArgumentException">Thrown with message <c>anglicize_premiss: not a syllogism premiss (Parameter 'fm')</c> when the input formula is not a syllogism premiss.</exception>
    /// 
    /// <example id="anglicize_premiss-1">
    /// <code lang="fsharp">
    /// premiss_A ("P", "S")
    /// |> anglicize_premiss 
    /// </code>
    /// Evaluates to <c>"all P are S"</c>.
    /// </example>
    /// 
    /// <example id="anglicize_premiss-2">
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> anglicize_premiss 
    /// </code>
    /// Throws <c>System.ArgumentException: anglicize_premiss: not a syllogism premiss (Parameter 'fm')</c>.
    /// </example>
    val anglicize_premiss: fm: formula<fol> -> string

    /// <summary>
    /// Returns an English reading of a syllogism.
    /// </summary>
    /// 
    /// <param name="fm">The input syllogism.</param>
    /// 
    /// <returns>
    /// An English reading of the input syllogism.
    /// </returns>
    /// 
    /// <exception cref="T:System.ArgumentException">Thrown with message <c>anglicize_syllogism: not a syllogism (Parameter 'fm')</c> when the input formula is not a syllogism premiss.</exception>
    /// 
    /// <example id="anglicize_syllogism-1">
    /// <code lang="fsharp">
    /// premiss_A ("M", "P")
    /// |> fun x -> mk_and x (premiss_A ("S", "M"))
    /// |> fun x -> mk_imp x (premiss_A ("S", "P"))
    /// |> anglicize_syllogism 
    /// </code>
    /// Evaluates to <c>"If all M are P and all S are M, then all S are P"</c>.
    /// </example>
    /// 
    /// <example id="anglicize_syllogism-2">
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> anglicize_syllogism 
    /// </code>
    /// Throws <c>System.ArgumentException: anglicize_syllogism: not a syllogism (Parameter 'fm')</c>.
    /// </example>
    val anglicize_syllogism: formula<fol> -> string

    /// <summary>
    /// Returns all 256 possible syllogisms.
    /// </summary>
    val all_possible_syllogisms: formula<fol> list

    /// <summary>
    /// Returns all 256 possible syllogisms together with the assumptions that 
    /// the terms are not empty.
    /// </summary>
    val all_possible_syllogisms': formula<fol> list

    val alltuples: n: int -> l: 'a list -> 'a list list

    val allmappings:
      dom: 'a list -> ran: 'b list -> ('a -> 'b) list when 'a: equality

    val alldepmappings:
      dom: ('a * 'b) list -> ran: ('b -> 'c list) -> ('a -> 'c) list
        when 'a: equality

    val allfunctions:
      dom: 'a list -> n: int -> ('a list -> 'a) list when 'a: equality

    val allpredicates:
      dom: 'a list -> n: int -> ('a list -> bool) list when 'a: equality

    val decide_finite: n: int -> fm: formula<fol> -> bool

    val limmeson:
      n: int ->
        fm: formula<fol> -> func<string,term> * int * int

    val limited_meson:
      n: int ->
        fm: formula<fol> ->
        (func<string,term> * int * int) list

    val decide_fmp: fm: formula<fol> -> bool

    val decide_monadic: fm: formula<fol> -> bool