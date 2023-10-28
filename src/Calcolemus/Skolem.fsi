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
    /// and existential quanti?ers (those applied to variables that does not 
    /// occur free in the body).
    /// <p></p>
    /// Whenever <c>False</c> and <c>True</c> occur in combination, there is 
    /// always a a tautology justifying the equivalence with a simpler formula, 
    /// e.g. <c>False /\ p &lt;=&gt; False</c>, <c>True \/ p &lt;=&gt; p</c>, 
    /// <c>p ==> False &lt;=&gt; ~p</c>. At he same time, it also eliminates double 
    /// negations <c>~~p</c>.
    /// <p></p>
    /// If <c>x</c> not in <c>fv p</c> then <c>forall x. p</c> and 
    /// <c>exists x. p are</c> logically equivalent to <c>p</c>.
    /// </remarks>
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
    /// While simplify1 performs the transformation just at the first level, 
    /// simplify performs it at every levels in a recursive bottom-up sweep.
    /// </remarks>
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
    /// through quanti?ers.
    /// </remarks>
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
    /// It pulls out quantifiers.
    /// </summary>
    /// 
    /// <category index="3">Prenex normal form</category>
    val pullquants: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// calls the main pullquants functions again on the body to pull up 
    /// further quanti?ers
    /// </summary>
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
    /// leaves quanti?ed formulas alone, and for conjunctions and disjunctions 
    /// recursively prenexes the immediate subformulas and then uses pullquants
    /// </summary>
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
    /// <li>simplifies away False, True, vacuous quanti?cation, etc.;</li>
    /// <li>eliminates implication and equivalence, push down negations;</li>
    /// <li>pulls out quanti?ers.</li>
    /// </ul>
    /// </remarks>
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
    /// <example id="functions-1">
    /// <code lang="fsharp">
    /// functions !!!"x + 1 > 0 /\ f(z) > g(z,i)"
    /// </code>
    /// Evaluates to <c>[("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]</c>.
    /// </example>
    /// 
    /// <category index="4">Get functions in term and formula</category>
    val functions: fm: formula<fol> -> (string * int) list

    /// <summary>
    /// Core Skolemization function specifically intended to be used on NNF.
    /// </summary>
    /// 
    /// <remarks>
    /// It simply recursively descends the formula, Skolemizing any existential 
    /// formulas and then proceeding to subformulas using skolem2 for binary 
    /// connectives.
    /// </remarks>
    /// 
    /// <category index="5">Core Skolemization</category>
    val skolem:
      fm: formula<fol> ->
        fns: string list -> formula<fol> * string list

    /// <summary>
    /// Auxiliary to skolem when dealing with binary connectives. 
    /// </summary>
    /// 
    /// <remarks>
    /// It updates the set of functions to avoid with new Skolem functions 
    /// introduced into one formula before tackling the other.
    /// </remarks>
    /// 
    /// <category index="5">Core Skolemization</category>
    val skolem2:
      cons: (formula<fol> * formula<fol> ->
               formula<fol>) ->
        p: formula<fol> * q: formula<fol> ->
          fns: string list -> formula<fol> * string list

    /// <summary>
    /// Overall Skolemization function, intended to be used with any type of 
    /// initial fol formula.
    /// </summary>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val askolemize: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Removes all universale quantifiers from the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <example id="specialize-1">
    /// <code lang="fsharp">
    /// specialize !!!"forall x y. P(x) /\ P(y)"
    /// </code>
    /// Evaluates to <c>`P(x) /\ P(y)`</c>.
    /// </example>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val specialize: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Puts the input formula <c>fm</c> into skolem normal form 
    /// while also removing all universal quantifiers.
    /// </summary>
    /// 
    /// <remarks>
    /// It puts the formula in prenex normal form, substitutes existential 
    /// quantifiers with skolem functions and also removes all universal 
    /// quantifiers.
    /// </remarks>
    /// 
    /// <example id="skolemize-1">
    /// <code lang="fsharp">
    /// skolemize !!!"forall x. exists y. R(x,y)"
    /// </code>
    /// Evaluates to <c>`R(x,f_y(x))`</c>.
    /// </example>
    /// 
    /// <category index="5">Overall Skolemization</category>
    val skolemize: fm: formula<fol> -> formula<fol>