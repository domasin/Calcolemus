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
    /// 
    /// <category index="3">Syllogisms</category>
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
    /// 
    /// <category index="3">Syllogisms</category>
    val anglicize_syllogism: formula<fol> -> string

    /// <summary>
    /// Returns all 256 possible syllogisms.
    /// </summary>
    /// 
    /// <category index="3">Syllogisms</category>
    val all_possible_syllogisms: formula<fol> list

    /// <summary>
    /// Returns all 256 possible syllogisms together with the assumptions that 
    /// the terms are not empty.
    /// </summary>
    /// 
    /// <category index="3">Syllogisms</category>
    val all_possible_syllogisms': formula<fol> list

    /// <summary>
    /// Generates all tuples of a given size with members chosen from a given 
    /// list.
    /// </summary>
    /// 
    /// <param name="n">The size of the resulting tuples.</param>
    /// <param name="l">The input list.</param>
    /// 
    /// <returns>
    /// All tuples of size <c>n</c> with members chosen from the list <c>l</c>.
    /// </returns>
    /// 
    /// <example id="alltuples-1">
    /// <code lang="fsharp">
    /// [1;2;3;4;5;6;7]
    /// |> alltuples 3
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// [[1; 1; 1]; [1; 1; 2]; [1; 1; 3]; [1; 1; 4]; [1; 1; 5]; [1; 1; 6]; [1; 1; 7];
    ///  [1; 2; 1]; [1; 2; 2]; [1; 2; 3]; [1; 2; 4]; [1; 2; 5]; [1; 2; 6]; [1; 2; 7];
    ///  [1; 3; 1]; [1; 3; 2]; [1; 3; 3]; [1; 3; 4]; [1; 3; 5]; [1; 3; 6]; [1; 3; 7];
    ///  [1; 4; 1]; [1; 4; 2]; [1; 4; 3]; [1; 4; 4]; [1; 4; 5]; [1; 4; 6]; [1; 4; 7];
    ///  [1; 5; 1]; [1; 5; 2]; [1; 5; 3]; [1; 5; 4]; [1; 5; 5]; [1; 5; 6]; [1; 5; 7];
    ///  [1; 6; 1]; [1; 6; 2]; [1; 6; 3]; [1; 6; 4]; [1; 6; 5]; [1; 6; 6]; [1; 6; 7];
    ///  [1; 7; 1]; [1; 7; 2]; [1; 7; 3]; [1; 7; 4]; [1; 7; 5]; [1; 7; 6]; [1; 7; 7];
    ///  [2; 1; 1]; [2; 1; 2]; [2; 1; 3]; [2; 1; 4]; [2; 1; 5]; [2; 1; 6]; [2; 1; 7];
    ///  [2; 2; 1]; [2; 2; 2]; [2; 2; 3]; [2; 2; 4]; [2; 2; 5]; [2; 2; 6]; [2; 2; 7];
    ///  [2; 3; 1]; [2; 3; 2]; [2; 3; 3]; [2; 3; 4]; [2; 3; 5]; [2; 3; 6]; [2; 3; 7];
    ///  [2; 4; 1]; [2; 4; 2]; [2; 4; 3]; [2; 4; 4]; [2; 4; 5]; [2; 4; 6]; [2; 4; 7];
    ///  [2; 5; 1]; [2; 5; 2]; [2; 5; 3]; [2; 5; 4]; [2; 5; 5]; [2; 5; 6]; [2; 5; 7];
    ///  [2; 6; 1]; [2; 6; 2]; [2; 6; 3]; [2; 6; 4]; [2; 6; 5]; [2; 6; 6]; [2; 6; 7];
    ///  [2; 7; 1]; [2; 7; 2]; [2; 7; 3]; [2; 7; 4]; [2; 7; 5]; [2; 7; 6]; [2; 7; 7];
    ///  [3; 1; 1]; [3; 1; 2]; ...]
    /// </code>
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val alltuples: n: int -> l: 'a list -> 'a list list

    /// <summary>
    /// Generates all possible functions out of a finite domain into a finite range.
    /// </summary>
    /// 
    /// <param name="dom">The input domain.</param>
    /// <param name="ran">The input range.</param>
    /// 
    /// <returns>
    /// All possible functions out of the domain <c>dom</c> into range 
    /// <c>ran</c>, and undefined outside <c>dom</c>.
    /// </returns>
    /// 
    /// <example id="allmappings-1">
    /// <code lang="fsharp">
    /// let dom,ran = [1..3],[1..3]
    /// 
    /// allmappings dom ran
    /// |> List.mapi (fun i f -> i, dom |> List.map f)
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// [(0, [1; 1; 1]); (1, [1; 1; 2]); (2, [1; 1; 3]); (3, [1; 2; 1]);
    ///  (4, [1; 2; 2]); (5, [1; 2; 3]); (6, [1; 3; 1]); (7, [1; 3; 2]);
    ///  (8, [1; 3; 3]); (9, [2; 1; 1]); (10, [2; 1; 2]); (11, [2; 1; 3]);
    ///  (12, [2; 2; 1]); (13, [2; 2; 2]); (14, [2; 2; 3]); (15, [2; 3; 1]);
    ///  (16, [2; 3; 2]); (17, [2; 3; 3]); (18, [3; 1; 1]); (19, [3; 1; 2]);
    ///  (20, [3; 1; 3]); (21, [3; 2; 1]); (22, [3; 2; 2]); (23, [3; 2; 3]);
    ///  (24, [3; 3; 1]); (25, [3; 3; 2]); (26, [3; 3; 3])]
    /// </code>
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val allmappings:
      dom: 'a list -> ran: 'b list -> ('a -> 'b) list when 'a: equality

    /// <summary>
    /// Enumerates all ways to interpreting function symbols.
    /// </summary>
    /// 
    /// <param name="dom">The input list of function name-arity pairs.</param>
    /// <param name="ran">The list of all possible functions in the domain.</param>
    /// 
    /// <returns>
    /// All interpretations of the input function symbols, i.e. all mappings 
    /// from the function symbols in <c>dom</c> to the domain functions in 
    /// <c>ran</c>.
    /// </returns>
    /// 
    /// <example id="alldepmappings-1">
    /// <code lang="fsharp">
    /// let dom = [1..3]
    /// 
    /// let functionSymbols = [("g",2)]
    /// let functions = allfunctions [1..3]
    /// 
    /// alldepmappings functionSymbols functions
    /// |> List.mapi (fun i f -> 
    ///     i, 
    ///     dom 
    ///     |> alltuples 2
    ///     |> List.map (fun args -> args,f "g" args))
    /// |> List.take 3
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// val it: (int * (int list * int) list) list =
    ///   [(0,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 1)]);
    ///    (1,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 2)]);
    ///    (2,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 3)])]
    /// </code>
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val alldepmappings:
      dom: ('a * 'b) list -> ran: ('b -> 'c list) -> ('a -> 'c) list
        when 'a: equality

    /// <summary>
    /// Generates of the functions of a given finite domain with a given arity.
    /// </summary>
    /// 
    /// <param name="dom">The input domain.</param>
    /// <param name="n">The input arity.</param>
    /// 
    /// <returns>
    /// All the functions from <c>dom</c> to <c>dom</c> with arity <c>n</c>.
    /// </returns>
    /// 
    /// <example id="allfunctions-1">
    /// <code lang="fsharp">
    /// let dom = [1..3]
    /// 
    /// allfunctions dom 2
    /// |> List.mapi (fun i f -> 
    ///     i, 
    ///     dom 
    ///     |> alltuples 2
    ///     |> List.map (fun args -> args, f args)
    /// )
    /// |> List.take 3
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// val it: (int * (int list * int) list) list =
    ///   [(0,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 1)]);
    ///    (1,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 2)]);
    ///    (2,
    ///     [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
    ///      ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 3)])]
    /// </code>
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val allfunctions:
      dom: 'a list -> n: int -> ('a list -> 'a) list when 'a: equality

    /// <summary>
    /// Generates of the predicates of a given finite domain with a given arity.
    /// </summary>
    /// 
    /// <param name="dom">The input domain.</param>
    /// <param name="n">The input arity.</param>
    /// 
    /// <returns>
    /// All the possibile predicates in <c>dom</c> with arity <c>n</c>.
    /// </returns>
    /// 
    /// <example id="allpredicates-1">
    /// <code lang="fsharp">
    /// let dom = [1..3]
    /// 
    /// allpredicates dom 2
    /// |> List.mapi (fun i f -> 
    ///     i, 
    ///     dom 
    ///     |> alltuples 2
    ///     |> List.map (fun args -> args, f args)
    /// )
    /// |> List.take 3
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// val it: (int * (int list * bool) list) list =
    ///   [(0,
    ///     [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
    ///      ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], false);
    ///      ([3; 3], false)]);
    ///    (1,
    ///     [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
    ///      ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], false);
    ///      ([3; 3], true)]);
    ///    (2,
    ///     [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
    ///      ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], true);
    ///      ([3; 3], false)])]
    /// </code>
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val allpredicates:
      dom: 'a list -> n: int -> ('a list -> bool) list when 'a: equality

    /// <summary>
    /// Tests if a formula holds in all interpretations of size <c>n</c>.
    /// </summary>
    /// 
    /// <param name="n">The input size of the interpretation.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if <c>fm</c> holds in all interpretations of size <c>n</c>.
    /// </returns>
    /// 
    /// <example id="decide_finite-1">
    /// <code lang="fsharp">
    /// !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    /// |> decide_finite 2 
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val decide_finite: n: int -> fm: formula<fol> -> bool

    /// <summary>
    /// Tests the unsatisfiability of a formula using the core MESON procedure 
    /// optimized with a fixed limit of rules application.
    /// </summary>
    /// 
    /// <param name="n">The limit of rules application.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The triple with the current instantiation, the depth reached and the 
    /// number of variables renamed.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when the goals are not solvable, at least at the current limit.</exception>
    /// 
    /// <example id="limmeson-1">
    /// <code lang="fsharp">
    /// !! @"~R(x,x) /\ (forall x y. R(x,y) \/ R(y,x))"
    /// |> limmeson 2
    /// |> fun (inst, n, k) -> (inst |> graph, n, k)
    /// </code>
    /// Evaluates to <c>([("_0", ``_1``); ("_1", ``_2``)], 0, 3)</c>.
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val limmeson:
      n: int ->
        fm: formula<fol> -> func<string,term> * int * int

    /// <summary>
    /// Tests the validity of a formula by negating it and splitting in 
    /// subproblems to be refuted with the core MESON procedure with a fixed 
    /// limit of rules application. 
    /// </summary>
    /// 
    /// <param name="n">The limit of rules application.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of triples with current instantiation, depth reached and 
    /// number of variables renamed returned on the subproblems.
    /// </returns>
    /// 
    /// <example id="limited_meson-1">
    /// <code lang="fsharp">
    /// !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    /// |> limited_meson 2
    /// |> List.map (fun (inst, n, k) -> (inst |> graph, n, k))
    /// </code>
    /// Evaluates to <c>[([("_0", ``c_x``); ("_1", ``c_x``)], 0, 2)]</c>.
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val limited_meson:
      n: int ->
        fm: formula<fol> ->
        (func<string,term> * int * int) list

    /// <summary>
    /// Tests if a formula (with the finite model property) is valid or invalid.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is valid; otherwise, false.
    /// </returns>
    /// 
    /// <note>
    /// Termination is guaranteed only for formulas with the finite model 
    /// property.
    /// </note>
    /// 
    /// <example id="decide_fmp-1">
    /// <code lang="fsharp">
    /// !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    /// |> decide_fmp
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="decide_fmp-2">
    /// <code lang="fsharp">
    /// !! @"(forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)) ==> forall x. R(x,x)"
    /// |> decide_fmp
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="decide_fmp-3">
    /// <code lang="fsharp">
    /// !! @"~((forall x. ~R(x,x)) /\
    ///        (forall x. exists z. R(x,z)) /\
    ///        (forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)))"
    /// |> decide_fmp
    /// </code>
    /// Crashes.
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val decide_fmp: fm: formula<fol> -> bool

    /// <summary>
    /// Tests if a monadic formula without function symbols is valid or invalid 
    /// testing only interpretations of size \(2^k\) where \(k\) is the number 
    /// of monadic predicates in the formula.
    /// </summary>
    /// 
    /// <param name="fm">The input monadic formula.</param>
    /// 
    /// <returns>
    /// true, if the input monadic formula is valid; otherwise, false.
    /// </returns>
    /// 
    /// <example id="decide_monadic-1">
    /// <code lang="fsharp">
    /// !! @"((exists x. forall y. P(x) &lt;=&gt; P(y)) &lt;=&gt;
    ///       ((exists x. Q(x)) &lt;=&gt; (forall y. Q(y)))) &lt;=&gt;
    ///      ((exists x. forall y. Q(x) &lt;=&gt; Q(y)) &lt;=&gt;
    ///       ((exists x. P(x)) &lt;=&gt; (forall y. P(y))))"
    /// |> decide_monadic
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="4">The finite model property</category>
    val decide_monadic: fm: formula<fol> -> bool