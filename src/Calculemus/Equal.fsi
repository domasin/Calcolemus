// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Formulas
open Fol

/// <summary>
/// Naive equality axiomatization.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Equal = 

    /// <summary>
    /// Tests if a formula is an equation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is an equation; otherwise, false.
    /// </returns>
    /// 
    /// <example id="is_eq-1">
    /// <code lang="fsharp">
    /// !!"x = y"
    /// |> is_eq
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="is_eq-2">
    /// Logical equations are not equations.
    /// <code lang="fsharp">
    /// !!"P(x) &lt;=&gt; Q(x)"
    /// |> is_eq
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val is_eq: fm: formula<fol> -> bool

    /// <summary>
    /// Constructs an equation.
    /// </summary>
    /// 
    /// <param name="s">The first input term.</param>
    /// <param name="t">The second input term.</param>
    /// 
    /// <returns>
    /// The equation of the input terms.
    /// </returns>
    /// 
    /// <example id="mk_eq-1">
    /// <code lang="fsharp">
    /// mk_eq !!!"f(x)" !!!"y"
    /// </code>
    /// Evaluates to <c>`f(x) = y`</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val mk_eq: s: term -> t: term -> formula<fol>

    /// <summary>
    /// Formula destructor for equations.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The pair of the LHS and RHS terms of the equation.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_eq: not an equation</c> when the input formula is not an equation.</exception>
    /// 
    /// <example id="dest_eq-1">
    /// <code lang="fsharp">
    /// dest_eq !!"f(x) = y"
    /// </code>
    /// Evaluates to <c>(``f(x)``,``y``)</c>.
    /// </example>
    /// 
    /// <example id="dest_eq-2">
    /// <code lang="fsharp">
    /// dest_eq !!"P(x) &lt;=&gt; Q(y)"
    /// </code>
    /// Throws <c>System.Exception: dest_eq: not an equation</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val dest_eq: fm: formula<fol> -> term * term

    /// <summary>
    /// Returns the LHS term of an equation.
    /// </summary>
    /// 
    /// <param name="eq">The input equation.</param>
    /// 
    /// <returns>
    /// The LHS term of the equation.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_eq: not an equation</c> when the input formula is not an equation.</exception>
    /// 
    /// <example id="lhs-1">
    /// <code lang="fsharp">
    /// lhs !!"f(x) = y"
    /// </code>
    /// Evaluates to <c>``f(x)``</c>.
    /// </example>
    /// 
    /// <example id="lhs-2">
    /// <code lang="fsharp">
    /// lhs !!"P(x) &lt;=&gt; Q(y)"
    /// </code>
    /// Throws <c>System.Exception: dest_eq: not an equation</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val lhs: eq: formula<fol> -> term

    /// <summary>
    /// Returns the RHS term of an equation.
    /// </summary>
    /// 
    /// <param name="eq">The input equation.</param>
    /// 
    /// <returns>
    /// The RHS term of the equation.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>dest_eq: not an equation</c> when the input formula is not an equation.</exception>
    /// 
    /// <example id="rhs-1">
    /// <code lang="fsharp">
    /// rhs !!"f(x) = y"
    /// </code>
    /// Evaluates to <c>``y``</c>.
    /// </example>
    /// 
    /// <example id="rhs-2">
    /// <code lang="fsharp">
    /// rhs !!"P(x) &lt;=&gt; Q(y)"
    /// </code>
    /// Throws <c>System.Exception: dest_eq: not an equation</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val rhs: eq: formula<fol> -> term

    /// <summary>
    /// Returns the predicates present in the input formula <c>fm</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// The list of name-arity pairs of the predicates in the formula.
    /// </returns>
    /// 
    /// <example id="predicates-1">
    /// <code lang="fsharp">
    /// predicates !!"x + 1 > 0 /\ f(z) > g(z,i)"
    /// </code>
    /// Evaluates to <c>[(">", 2)]</c>.
    /// </example>
    /// 
    /// <category index="1">Syntax operation</category>
    val predicates: fm: formula<fol> -> (string * int) list

    /// <summary>
    /// Returns the congruence axiom for the given function.
    /// </summary>
    /// 
    /// <param name="f">The function symbol name.</param>
    /// <param name="n">The function arity.</param>
    /// <returns>
    /// The list with the congruence axiom for the given function as the only 
    /// element, if <c>n > 0</c>; otherwise, the empty list.
    /// </returns>
    /// 
    /// <example id="function_congruence-1">
    /// <code lang="fsharp">
    /// function_congruence ("f",2)
    /// </code>
    /// Evaluates to <c>[`forall x1 x2 y1 y2. x1 = y1 /\ x2 = y2 ==> f(x1,x2) = f(y1,y2)`]</c>.
    /// </example>
    /// 
    /// <example id="function_congruence-1">
    /// <code lang="fsharp">
    /// function_congruence ("f",0)
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    /// 
    /// <category index="2">Equality axioms</category>
    val function_congruence: f: string * n: int -> formula<fol> list

    /// <summary>
    /// Returns the congruence axiom for the given predicate.
    /// </summary>
    /// 
    /// <param name="p">The predicate symbol name.</param>
    /// <param name="n">The predicate arity.</param>
    /// <returns>
    /// The list with the congruence axiom for the given predicate as the only 
    /// element, if <c>n > 0</c>; otherwise, the empty list.
    /// </returns>
    /// 
    /// <example id="predicate_congruence-1">
    /// <code lang="fsharp">
    /// predicate_congruence ("P",3)
    /// </code>
    /// Evaluates to <c>[`forall x1 x2 x3 y1 y2 y3. x1 = y1 /\ x2 = y2 /\ x3 = y3 ==> P(x1,x2,x3) ==> P(y1,y2,y3)`]</c>.
    /// </example>
    /// 
    /// <example id="predicate_congruence-2">
    /// <code lang="fsharp">
    /// predicate_congruence ("P",0)
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    /// 
    /// <category index="2">Equality axioms</category>
    val predicate_congruence: p: string * n: int -> formula<fol> list

    /// <summary>
    /// Returns the list of equivalence axioms.
    /// </summary>
    /// 
    /// <remarks>
    /// Reflexivity and a variation of transitivity:
    /// \begin{align*}
    /// &amp;\forall x.\ x = x \\
    /// &amp;\forall x\ y\ z.\ x = y \land x = z \Rightarrow y = z
    /// \end{align*}
    /// Symmetry follows by instantiating that axiom so that \(x\) and \(z\) 
    /// are the same, then using reflexivity.
    /// </remarks>
    /// 
    /// <returns>
    /// The list of the equivalence axioms.
    /// </returns>
    /// 
    /// <category index="2">Equality axioms</category>
    val equivalence_axioms: formula<fol> list

    /// <summary>
    /// Returns the implication of the input formula from its equality axioms.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula itself if it doesn't involve equality at all; 
    /// otherwise, the implication of the input formula from its equality 
    /// axioms.
    /// </returns>
    /// 
    /// <example id="equalitize-1">
    /// <code lang="fsharp">
    /// !! @"(forall x. f(x) ==> g(x)) /\
    ///      (exists x. f(x)) /\
    ///      (forall x y. g(x) /\ g(y) ==> x = y)
    ///      ==> forall y. g(y) ==> f(y)"
    /// |> equalitize
    /// |> sprint_fol_formula
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// "(forall x. x = x) /\ 
    ///  (forall x y z. x = y /\ x = z ==> y = z) /\ 
    ///  (forall x1 y1. x1 = y1 ==> f(x1) ==> f(y1)) /\ 
    ///  (forall x1 y1. x1 = y1 ==> g(x1) ==> g(y1)) ==> 
    ///  (forall x. f(x) ==> g(x)) /\ 
    ///  (exists x. f(x)) /\ 
    ///  (forall x y. g(x) /\ g(y) ==> x = y) 
    ///  ==> (forall y. g(y) ==> f(y))"
    /// </code>
    /// </example>
    /// 
    /// <example id="equalitize-2">
    /// <code lang="fsharp">
    /// equalitize !!"P(x) &lt;=&gt; Q(y)"
    /// </code>
    /// Evaluates to <c>`P(x) &lt;=&gt; Q(y)`</c>.
    /// </example>
    /// 
    /// <category index="2">Equality axioms</category>
    val equalitize: fm: formula<fol> -> formula<fol>