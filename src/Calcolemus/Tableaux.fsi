// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calcolemus

open Calcolemus.Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Tableaux, seen as an optimized version of a Prawitz-like procedure.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Tableaux = 

    /// <summary>Returns a unifier for a pair of literals.</summary>
    /// 
    /// <remarks>
    /// It also handles the pair <c>False,False</c>.
    /// </remarks>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>
    /// A variable-term mappings that unify <c>eq</c>, if the unification is 
    /// possible and there are no cycles. 
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Can't unify literals' when the input <c>eq</c> is neither a pair of literal or the degenerated case <c>False,False</c></exception>
    /// 
    /// <example id="unify_literals-1">
    /// Successful unification
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x)",!!"P(f(y))")
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-2">
    /// Degenerated case
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"false",!!"false")
    /// |> graph
    /// </code>
    /// Evaluates to <c>Empty</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-3">
    /// Cyclic assignment
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(y)",!!"P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-4">
    /// Impossible unification
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(g(x))",!!"P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    /// 
    /// <example id="unify_literals-5">
    /// Invalid input
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: Can't unify literals</c>.
    /// </example>
    /// 
    /// <category index="1">Unification of literals</category>
    val unify_literals:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    /// <summary>
    /// Returns a unifier for a complementary pair of literals.
    /// </summary>
    /// 
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// <param name="p">The first input literal.</param>
    /// <param name="q">The second input literal.</param>
    /// 
    /// <returns>
    /// The instantiation that makes the input literals become complementary, 
    /// if such an instantiation exists.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'cyclic' when there is a cyclic assignment.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'impossible unification' when the unification is not possible.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Can't unify literals' when the input <c>eq</c> is neither a pair of literal or the degenerated case <c>False,False</c></exception>
    /// 
    /// <example id="unify_complements-1">
    /// Successful unification
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(x)",!!"~P(f(y))")
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-2">
    /// Cyclic assignment
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(y)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: cyclic</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-3">
    /// Impossible unification
    /// <code lang="fsharp">
    /// unify_complements undefined (!!"P(g(x))",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: impossible unification</c>.
    /// </example>
    /// 
    /// <example id="unify_complements-4">
    /// Invalid input
    /// <code lang="fsharp">
    /// unify_literals undefined (!!"P(x) /\ Q(x)",!!"~P(f(y))")
    /// </code>
    /// Throws <c>System.Exception: Can't unify literals</c>.
    /// </example>
    /// 
    /// <category index="1">Unification of literals</category>
    val unify_complements:
      env: func<string,term> ->
        p: formula<fol> * q: formula<fol> ->
          func<string,term>

    /// <summary>
    /// Returns a unifier that causes each clause in the input list to contain 
    /// complementary literals.
    /// </summary>
    /// 
    /// <param name="djs">The input DNF list of clauses</param>
    /// <param name="env">An environment of mappings (represented as a finite partial function) from variables to terms, used as an accumulator for the final result of the unification procedure. </param>
    /// 
    /// <returns>
    /// The variable-term mappings that causes each clause to contain 
    /// complementary literals, if this mapping exists.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'tryfind' when there isn't any such mapping.</exception>
    /// 
    /// <example id="unify_refute-1">
    /// Successful refutation
    /// <code lang="fsharp">
    /// undefined
    /// |> unify_refute !!>>[
    ///         ["P(x)";"~P(f(y))";"R(x,y)"];
    ///         ["Q(x)";"~Q(x)"]
    /// ]
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <example id="unify_refute-2">
    /// Failing refutation
    /// <code lang="fsharp">
    /// undefined
    /// |> unify_refute !!>>[["P(c)"];["Q(c)"]]
    /// </code>
    /// Throws to <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <category index="2">Refutation via unification</category>
    val unify_refute:
      djs: formula<fol> list list ->
        env: func<string,term> -> func<string,term>

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses with a Prawitz-like 
    /// procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The input set of clauses <c>djs0</c> is intended to represent a DNF 
    /// formula and <c>fvs</c> are supposed to be the free variables of the 
    /// formula.
    /// </remarks>
    /// 
    /// <param name="djs0">The input set of clauses.</param>
    /// <param name="fvs">The free variables to unify.</param>
    /// <param name="djs">The accumulator for the substitution instances.</param>
    /// <param name="n">A counter to generate fresh variable names.</param>
    /// 
    /// <returns>
    /// The final instantiation together with the number of instances tried.
    /// </returns>
    /// 
    /// <example id="prawitz_loop-1">
    /// Successful refutation
    /// <code lang="fsharp">
    /// prawitz_loop !!>>[
    ///         ["P(x)";"~P(f(y))";"R(x,y)"];
    ///         ["Q(x)";"~Q(x)"]
    /// ] ["x";"y"] [[]] 0
    /// |> fun (env,nr) -> env |> graph, nr
    /// </code>
    /// Evaluates to <c>[("x", ``f(y)``)]</c>.
    /// </example>
    /// 
    /// <category index="3">Prawitz procedure</category>
    val prawitz_loop:
      djs0: formula<fol> list list ->
        fvs: string list ->
        djs: formula<fol> list list ->
        n: int -> func<string,term> * int

    /// <summary>
    /// Tests the validity of a formula with a Prawitz-like procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The number of instance tried.
    /// </returns>
    /// 
    /// <example id="prawitz_loop-1">
    /// <code lang="fsharp">
    /// prawitz Pelletier.p20
    /// </code>
    /// Evaluates to <c>2</c>.
    /// </example>
    /// 
    /// <category index="3">Prawitz procedure</category>
    val prawitz: fm: formula<fol> -> int

    /// <summary>
    /// Tries to refute a set of <see cref='M:Calcolemus.Skolem.askolemize'/>d 
    /// formulas (assumptions) against a list of (derived) literals given a 
    /// limit of universal variables to be replaced.
    /// </summary>
    /// 
    /// <remarks>
    /// The procedure applies recursively the following rules.
    /// 
    /// If the list is empty, fail: there are no refutations; otherwise 
    /// consider the first formula.
    /// <ul>
    /// <li>
    /// If it is a conjunction, replace it with the two separated 
    /// assumptions of its conjuncts;
    /// </li>
    /// <li>
    /// if it is a disjunction, try to refute the first disjunct and 
    /// 'update' the continuation function to 'remember' to try 
    /// refute the second disjunct in case of failure;
    /// </li>
    /// <li>
    /// if it is a universal formula, (i) replace it with its body with the 
    /// universal variable replaced by a new fresh variable, (ii) move the 
    /// original assumption at the end of the assumptions list, (iii) reduce 
    /// the limit of universal variable that can be replaced and (iv) increment 
    /// by one the counter of universal variables replaced;
    /// </li>
    /// <li>
    /// if it is a literal, try to find a unifiable complement of it in the 
    /// list of literals using the continuation function to iterate 
    /// over each of them. If one complementary literal is found, return with 
    /// the successful instantiation together with the number of universal 
    /// variables replaced; otherwise, add the current formula to the list of 
    /// literals and move on the next formula.
    /// </li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="fms">The set of DNF formulas (assumptions) to be refuted.</param>
    /// <param name="lits">The list of (derived) literals against which to refute the formulas.</param>
    /// <param name="n">The limit of universal variables to be replaced.</param>
    /// <param name="cont">The supporting continuation function.</param>
    /// <param name="env">The given instantiation under which trying refuting.</param>
    /// <param name="k">The counter of universal variables already replaced.</param>
    /// 
    /// <returns>
    /// The pair of the successful instantiation together with the number of 
    /// universal variables replaced, if the procedure succeeds.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'no proof at this level' when <c>n</c> &lt; 0.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'tableau: no proof' when the list of input formulas <c>fms</c> is empty.</exception>
    /// 
    /// <example id="tableau-1">
    /// <code lang="fsharp">
    /// tableau ([!!"P(x)"], [!!"~P(f(y))"], 0) id (undefined, 0)
    /// |> fun (inst,nrInst) -> inst |> graph,nrInst
    /// </code>
    /// Evaluates to <c>([("x", ``f(y)``)], 0)</c>.
    /// </example>
    /// 
    /// <example id="tableau-2">
    /// <code lang="fsharp">
    /// tableau ([!!"P(x)"], [!!"~P(f(y))"], -1) id (undefined, 0)
    /// |> fun (inst,nrInst) -> inst |> graph,nrInst
    /// </code>
    /// Throws <c>System.Exception: no proof at this level</c>.
    /// </example>
    /// 
    /// <example id="tableau-3">
    /// <code lang="fsharp">
    /// tableau ([], [!!"~P(f(x))"], 0) id (undefined, 0)
    /// |> fun (inst,nrInst) -> inst |> graph,nrInst
    /// </code>
    /// Throws <c>System.Exception: no proof</c>.
    /// </example>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tableau:
      fms: formula<fol> list * lits: formula<fol> list *
      n: int ->
        cont: (func<string,term> * int -> func<string,term> * int ) ->
        env: func<string,term> * k: int -> func<string,term> * int 

    /// <summary>
    /// Iterative deepening.
    /// </summary>
    /// 
    /// <remarks>
    /// Calls iteratively a function that accepts an input integer incrementing 
    /// the integer calling it again, if it fails, with the integer incremented.
    /// </remarks>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="n">The input integer.</param>
    /// 
    /// <returns>
    /// The result of the input function call and prints to the <c>stdout</c>
    /// information that the function has been call with the given integer.
    /// </returns>
    /// 
    /// <example id="deepen-1">
    /// <code lang="fsharp">
    /// deepen id 1
    /// </code>
    /// Evaluates to <c>1</c> and prints <c>stdout</c>.
    /// <code lang="fsharp">
    /// Searching with depth limit 1
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val deepen: f: (int -> 'a) -> n: int -> 'a

    /// <summary>
    /// Tries to refute a set of <see cref='M:Calcolemus.Skolem.askolemize'/>d 
    /// formulas using a <em>iterative deepening</em> of the 
    /// <see cref='M:Calcolemus.Tableaux.tableau'/> procedure.
    /// </summary>
    /// 
    /// <param name="fms">The input list of formulas.</param>
    /// 
    /// <returns>
    /// The number of universal variables replaced, if the procedure succeeds, 
    /// and prints to the <c>stdout</c> the depth limits (max number of 
    /// universal variables to replace) tried.
    /// </returns>
    /// 
    /// <note>
    /// Crashes if the list of formulas is satisfiable or the search too long.
    /// </note>
    /// 
    /// <example id="tabrefute-1">
    /// <code lang="fsharp">
    /// [!! "forall x y. P(x) /\ ~P(f(y))"; 
    /// !! "R(x,y) /\ ~R(x,y)"]
    /// |> tabrefute
    /// </code>
    /// Evaluates to <c>2</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tabrefute: fms: formula<fol> list -> int

    /// <summary>
    /// Tests the validity of a formula with the tableau procedure. 
    /// </summary>
    /// 
    /// <param name="fm">The input formula</param>
    /// 
    /// <returns>
    /// The number of universal variables replaced, if the procedure succeeds, 
    /// and prints to the <c>stdout</c> the depth limits (max number of 
    /// universal variables to replace) tried.
    /// </returns>
    /// 
    /// <example id="tabrefute-1">
    /// <code lang="fsharp">
    /// !! @"(forall x.
    ///         P(a) /\ (P(x) ==> (exists y. P(y) /\ R(x,y))) ==>
    ///         (exists z w. P(z) /\ R(x,w) /\ R(w,z))) &lt;=&gt;
    ///         (forall x.
    ///         (~P(a) \/ P(x) \/ (exists z w. P(z) /\ R(x,w) /\ R(w,z))) /\
    ///         (~P(a) \/ ~(exists y. P(y) /\ R(x,y)) \/
    ///         (exists z w. P(z) /\ R(x,w) /\ R(w,z))))"
    /// |> tab
    /// </code>
    /// Evaluates to <c>4</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// Searching with depth limit 3
    /// Searching with depth limit 4
    /// </code>
    /// </example>
    /// 
    /// <category index="4">Tableaux procedure</category>
    val tab: fm: formula<fol> -> int

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and testing 
    /// each of them with the tableau procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The formula is generalized, negated and askolemized. Then 
    /// <see cref='M:Calcolemus.Prop.simpdnf``1'/> is applied and, thus, 
    /// the formula transformed in a sort of a 'DNF set of clauses' with 
    /// possibly some universal formulas in place of literals. Each of these 
    /// 'clauses' is tested separately with 
    /// <see cref='M:Calcolemus.Tableaux.tabrefute'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula</param>
    /// 
    /// <returns>
    /// The numbers of universal variables replaced for each of the 
    /// independently tested subproblems, if the procedure succeeds, 
    /// and prints to the <c>stdout</c> the depth limits (max number of 
    /// universal variables to replace) tried.
    /// </returns>
    /// 
    /// <example id="splittab-1">
    /// <code lang="fsharp">
    /// !!"((exists x. forall y. P(x) &lt;=&gt; P(y)) &lt;=&gt;
    ///     ((exists x. Q(x)) &lt;=&gt; (forall y. Q(y)))) &lt;=&gt;
    ///    ((exists x. forall y. Q(x) &lt;=&gt; Q(y)) &lt;=&gt;
    ///     ((exists x. P(x)) &lt;=&gt; (forall y. P(y))))"
    /// |> splittab
    /// </code>
    /// Evaluates to <c>[5; 4; 5; 3; 3; 3; 2; 4; 6; 2; 3; 3; 4; 3; 3; 3; 3; 2; 2; 3; 6; 3; 2; 4; 3; 3; 3; 3; 3; 4; 4; 4]</c> and prints to the <c>stdout</c>
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// ...
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// ...
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Tableaux procedure optimized</category>
    val splittab: fm: formula<fol> -> int list