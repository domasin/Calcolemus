// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Equality elimination: Brand transform etc.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Eqelim = 

    /// <summary>
    /// Applies Brand's S-modification to a clause.
    /// </summary>
    /// 
    /// <remarks>
    /// It allows to eliminate the symmetry axiom of equality.
    /// </remarks>
    /// 
    /// <param name="cl">The input clause.</param>
    /// 
    /// <returns>
    /// The list of all the clauses that arise from all the possible 
    /// combinations of forward and backward positive equations in the original 
    /// clause. If \(n\) is the number of positive equation in the input 
    /// clause, \(2^n\) clauses are returned.
    /// </returns>
    /// 
    /// <example id="modify_S-1">
    /// <code lang="fsharp">
    /// !!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
    /// |> modify_S
    /// |> List.map (List.map sprint_fol_formula)
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [["`s1 = t1`"; "`s2 = t2`"; "`s3 = t3`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s1 = t1`"; "`s2 = t2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s1 = t1`"; "`s3 = t3`"; "`t2 = s2`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s1 = t1`"; "`t2 = s2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s2 = t2`"; "`s3 = t3`"; "`t1 = s1`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s2 = t2`"; "`t1 = s1`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`s3 = t3`"; "`t1 = s1`"; "`t2 = s2`"; "`~s1 = t1`"; "`~s2 = t2`"];
    ///  ["`t1 = s1`"; "`t2 = s2`"; "`t3 = s3`"; "`~s1 = t1`"; "`~s2 = t2`"]]
    /// </code>
    /// </example>
    /// 
    /// <category index="1">Brand's S- and T-modifications</category>
    val modify_S:
      cl: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Applies Brand's T-modification to a clause.
    /// </summary>
    /// 
    /// <remarks>
    /// It allows to eliminate the transitivity axiom of equality.
    /// </remarks>
    /// 
    /// <param name="cl">The input clause.</param>
    /// 
    /// <returns>
    /// A new clause with all the negative equation untouched and each positive 
    /// equation <c>s = t</c> replaced by <c>~t = w1 \/ s = w1</c> creating 
    /// fresh new variables <c>wi</c> for each of them.
    /// </returns>
    /// 
    /// <example id="modify_T-1">
    /// <code lang="fsharp">
    /// !!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
    /// |> modify_T
    /// </code>
    /// Evaluates to <c>[`~t1 = w''`; `s1 = w''`; `~t2 = w'`; `s2 = w'`; `~t3 = w`; `s3 = w`; `~s1 = t1`; `~s2 = t2`]</c>.
    /// </example>
    /// 
    /// <category index="1">Brand's S- and T-modifications</category>
    val modify_T:
      cl: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Checks if the input is a non-variable term.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// true, if the input is a non-variable term; otherwise, false.
    /// </returns>
    /// 
    /// <example id="is_nonvar-1">
    /// <code lang="fsharp">
    /// !!!"f(x)"
    /// |> is_nonvar
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="is_nonvar-2">
    /// <code lang="fsharp">
    /// !!!"x"
    /// |> is_nonvar
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val is_nonvar: tm: term -> bool

    /// <summary>
    /// Tries to find a nested non-variable subterm inside a term.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The first non-variable subterm of the input, if it exists.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>findnvsubt</c> when the input term is just a variable.</exception>
    /// 
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">Thrown with message <c>An index satisfying the predicate was not found in the collection.</c> when the input is not a simple variable but there isn't any non-variable subterm.</exception>
    /// 
    /// <example id="find_nestnonvar-1">
    /// <code lang="fsharp">
    /// !!!"f(0,1)"
    /// |> find_nestnonvar
    /// </code>
    /// Evaluates to <c>``0``</c>.
    /// </example>
    /// 
    /// <example id="find_nestnonvar-2">
    /// <code lang="fsharp">
    /// !!!"x"
    /// |> find_nestnonvar
    /// </code>
    /// Throws <c>System.Exception: findnvsubt</c>.
    /// </example>
    /// 
    /// <example id="find_nestnonvar-3">
    /// <code lang="fsharp">
    /// !!!"f(x,y)"
    /// |> find_nestnonvar
    /// </code>
    /// Throws <c>System.Collections.Generic.KeyNotFoundException: An index satisfying the predicate was not found in the collection.</c>
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val find_nestnonvar: tm: term -> term

    /// <summary>
    /// Tries to find a non-variable term inside a formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the first non-variable subterm, if it exists and the input formula is a 
    /// literal. In case of equality the result must be a <em>nested</em> 
    /// subterm, while for other predicate symbols it is any non-variable 
    /// subterm.
    /// </returns>
    /// 
    /// <exception cref="T:System.ArgumentException">Thrown with message <c>find_nvsubterm: not a literal (Parameter 'fm')</c> when the input formula is not a literal.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>tryfind</c> when there isn't any non-variable subterm in the formula.</exception>
    /// 
    /// <example id="find_nvsubterm-1">
    /// <code lang="fsharp">
    /// !!"R(0,1)"
    /// |> find_nvsubterm
    /// </code>
    /// Evaluates to <c>``0``</c>.
    /// </example>
    /// 
    /// <example id="find_nvsubterm-2">
    /// <code lang="fsharp">
    /// !!"~x = f(0)"
    /// |> find_nvsubterm
    /// </code>
    /// Evaluates to <c>``0``</c>.
    /// </example>
    /// 
    /// <example id="find_nvsubterm-3">
    /// <code lang="fsharp">
    /// !!"~x = f(0) /\ P(x)"
    /// |> find_nvsubterm
    /// </code>
    /// Throws <c>System.ArgumentException: find_nvsubterm: not a literal 
    /// (Parameter 'fm')</c>
    /// </example>
    /// 
    /// <example id="find_nvsubterm-4">
    /// <code lang="fsharp">
    /// !!"~x = f(y)"
    /// |> find_nvsubterm
    /// </code>
    /// Throws <c>System.Exception: tryfind</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val find_nvsubterm: fm: formula<fol> -> term

    /// <summary>
    /// Replaces subterms in a term based on the given substitution.
    /// </summary>
    /// 
    /// <param name="rfn">The input substitution fpf.</param>
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The term with subterms replaced based on the given substitution 
    /// function.
    /// </returns>
    /// 
    /// <example id="replacet-1">
    /// <code lang="fsharp">
    /// !!!"f(0,1)"
    /// |> replacet ((!!!"0" |-> !!!"1")undefined)
    /// </code>
    /// Evaluates to <c>``f(1,1)``</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val replacet: rfn: func<term,term> -> tm: term -> term

    /// <summary>
    /// Replaces terms in a formula based on the given substitution.
    /// </summary>
    /// 
    /// <param name="rfn">The input substitution fpf.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula with terms replaced based on the given substitution 
    /// function.
    /// </returns>
    /// 
    /// <example id="replace-1">
    /// <code lang="fsharp">
    /// !!"f(0,1) = 0"
    /// |> replace ((!!!"0" |-> !!!"1")undefined)
    /// </code>
    /// Evaluates to <c>``f(1,1) = 1``</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val replace:
      rfn: func<term,term> ->
        fm: formula<fol> -> formula<fol>

    /// <summary>
    /// E-modifies a clause avoiding the variables names indicated in the input 
    /// list.
    /// </summary>
    /// 
    /// <remarks>
    /// E-modification allows to eliminate the congruence axioms of equality.
    /// 
    /// The function repeatedly pulls out non-variable immediate subterms \(t\) 
    /// of function and predicate symbols (other than equality) using the 
    /// following formulas, which are equivalences in the presence of the 
    /// congruence and reflexivity axioms:
    /// \begin{align*}
    ///     f (\ldots , t, \ldots) = s \ &amp;\Leftrightarrow \ \forall w.\ t = w  \Rightarrow f (\ldots , w, \ldots) = s, \\
    ///     s = f (\ldots , t, \ldots) \ &amp;\Leftrightarrow \ \forall w.\ t = w   \Rightarrow s = f (\ldots , w, \ldots), \\
    ///     P (\ldots , t, \ldots) \ &amp;\Leftrightarrow \ \forall w.\ t = w \Rightarrow P(\ldots , w, \ldots).
    /// \end{align*}
    /// <br />
    /// The input and overall result are in clausal form.
    /// </remarks>
    /// 
    /// <param name="fvs">The list of variable names to avoid.</param>
    /// <param name="cls">The input clause.</param>
    /// 
    /// <returns>
    /// The list of clauses that represents the e-modification of the input 
    /// clause.
    /// </returns>
    /// 
    /// <example id="emodify-1">
    /// <code lang="fsharp">
    /// !!>["(x * y) * z = x * (y * z)"]
    /// |> emodify ["x";"y";"z"]
    /// </code>
    /// Evaluates to <c>[`~y * z = w'`; `~x * y = w`; `w * z = x * w'`]</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val emodify:
      fvs: string list ->
        cls: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Brand's E-modification
    /// </summary>
    /// 
    /// <remarks>
    /// E-modification allows to eliminate the congruence axioms of equality.
    /// 
    /// The function repeatedly pulls out non-variable immediate subterms \(t\) 
    /// of function and predicate symbols (other than equality) using the 
    /// following formulas, which are equivalences in the presence of the 
    /// congruence and reflexivity axioms:
    /// \begin{align*}
    ///     f (\ldots , t, \ldots) = s \ &amp;\Leftrightarrow \ \forall w.\ t = w  \Rightarrow f (\ldots , w, \ldots) = s, \\
    ///     s = f (\ldots , t, \ldots) \ &amp;\Leftrightarrow \ \forall w.\ t = w   \Rightarrow s = f (\ldots , w, \ldots), \\
    ///     P (\ldots , t, \ldots) \ &amp;\Leftrightarrow \ \forall w.\ t = w \Rightarrow P(\ldots , w, \ldots).
    /// \end{align*}
    /// <br />
    /// The input and overall result are in clausal form.
    /// </remarks>
    /// 
    /// <param name="cls">The input clause.</param>
    /// 
    /// <returns>
    /// The list of clauses that represents the e-modification of the input 
    /// clause.
    /// </returns>
    /// 
    /// <example id="modify_E-1">
    /// <code lang="fsharp">
    /// !!>["(x * y) * z = x * (y * z)"]
    /// |> modify_E
    /// </code>
    /// Evaluates to <c>[`~y * z = w'`; `~x * y = w`; `w * z = x * w'`]</c>.
    /// </example>
    /// 
    /// <category index="2">Brand's E-modification</category>
    val modify_E:
      cls: formula<fol> list -> formula<fol> list

    /// <summary>
    /// Applies E-modification, then S-modification and T-modification; finally 
    /// it includes the reflexive clause <c>x = x</c>.
    /// </summary>
    /// 
    /// <param name="cls">The input set of clauses.</param>
    /// 
    /// <returns>
    /// The input clauses E- S- and T-modified plus the reflexive clause 
    /// <c>x = x</c>.
    /// </returns>
    /// 
    /// <example id="brand-1">
    /// <code lang="fsharp">
    /// !!>>[["x = f(0)"]]
    /// |> brand
    /// </code>
    /// Evaluates to <c>[[`x = x`]; [`~f(w) = w'`; `x = w'`; `~0 = w`]; [`~x = w'`; `f(w) = w'`; `~0 = w`]]</c>.
    /// </example>
    /// 
    /// <category index="3">Overall Brand transformation</category>
    val brand:
      cls: formula<fol> list list ->
        formula<fol> list list

    /// <summary>
    /// Tests the unsatisfiability of a formula using the MESON procedure 
    /// with Brand's transformations incorporated.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the number of depth limit reached trying refuting the formula with 
    /// MESON, if the input formula is unsatisfiable after applying Brand's 
    /// transformation on equations and a refutation could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="bpuremeson-1">
    /// <code lang="fsharp">
    /// !!"~ x = x"
    /// |> bpuremeson
    /// </code>
    /// Evaluates to <c>1</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Overall Brand transformation</category>
    val bpuremeson: fm: formula<fol> -> int

    /// <summary>
    /// Tests the validity of a formula by negating it and splitting in 
    /// subproblems to be refuted by the MESON procedure with Brand's 
    /// transformations incorporated. 
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the list of depth limits reached trying to refute the subproblems, if 
    /// the formula is valid after applying Brand's transformation on equations.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="bmeson-1">
    /// <code lang="fsharp">
    /// !! @"(forall x y z. x * (y * z) = (x * y) * z) /\
    /// (forall x. e * x = x) /\
    /// (forall x. i(x) * x = e)
    /// ==> forall x. x * i(x) = e"
    /// |> bmeson
    /// </code>
    /// Evaluates to <c>[19]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// ...
    /// Searching with depth limit 15
    /// Searching with depth limit 16
    /// Searching with depth limit 17
    /// Searching with depth limit 18
    /// Searching with depth limit 19
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Overall Brand transformation</category>
    val bmeson: fm: formula<fol> -> int list

    /// <summary>
    /// MESON procedure with equality axioms (see 
    /// <see cref='T:Calculemus.Equal'/>) incorporated.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// the list of depth limits reached trying to refute the subproblems, if 
    /// the formula is valid in presence of the equality axioms.
    /// </returns>
    /// 
    /// <note>
    /// Prints the depth limits tried to the <c>stdout</c>.
    /// Crashes if the input formula is not unsatisfiable.
    /// </note>
    /// 
    /// <example id="emeson-1">
    /// <code lang="fsharp">
    /// !! @"(forall x. f(x) ==> g(x)) /\
    /// (exists x. f(x)) /\
    /// (forall x y. g(x) /\ g(y) ==> x = y)
    /// ==> forall y. g(y) ==> f(y)"
    /// |> emeson
    /// </code>
    /// Evaluates to <c>[6]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// Searching with depth limit 3
    /// Searching with depth limit 4
    /// Searching with depth limit 5
    /// Searching with depth limit 6
    /// </code>
    /// </example>
    /// 
    /// <category index="3">Overall Brand transformation</category>
    val emeson: fm: formula<fol> -> int list