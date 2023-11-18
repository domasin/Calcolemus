// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Calculemus

open Formulas
open Fol

/// <summary>
/// Skolemizing a set of 
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Skolems = 

    /// <summary>
    /// Renames all the function symbols in a term.
    /// </summary>
    /// 
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The term with an <c>old_</c> prefix added to each function symbols.
    /// </returns>
    /// 
    /// <example id="rename_term-1">
    /// <code lang="fsharp">
    /// !!!"f(g(x),z)"
    /// |> rename_term
    /// </code>
    /// Evaluates to <c>``old_f(old_g(x),z)``</c>.
    /// </example>
    val rename_term: tm: term -> term

    /// <summary>
    /// Renames all the function symbols in a formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula with an <c>old_</c> prefix added to each function symbols.
    /// </returns>
    /// 
    /// <example id="rename_form-1">
    /// <code lang="fsharp">
    /// !!!"f(g(x),z)"
    /// |> rename_form
    /// </code>
    /// Evaluates to <c>`P(old_f(old_g(x),z))`</c>.
    /// </example>
    val rename_form: fm: formula<fol> -> formula<fol>

    /// <summary>
    /// Core Skolemization function for set of formulas. 
    /// </summary>
    /// 
    /// <remarks>
    /// It is specifically intended to be used on formulas already simplified 
    /// and in nnf.
    /// </remarks>
    /// 
    /// <param name="fms">The input set of formulas.</param>
    /// <param name="corr">The list of strings to avoid as names of the Skolem functions.</param>
    /// 
    /// <returns>
    /// The pair of the Skolemized formulas set together with the updated list 
    /// of strings to avoid as names of the Skolem functions.
    /// </returns>
    /// 
    /// <example id="skolems-1">
    /// <code lang="fsharp">
    /// skolems !!>["exists x. P(f(g(x),z))"; "forall x. exists y. P(x,y)"] []
    /// </code>
    /// Evaluates to <c>([`P(old_f(old_g(f_x(z)),z))`; `forall x. P(x,f_y(x))`], ["f_y"; "f_x"])</c>.
    /// </example>
    val skolems:
      fms: formula<fol> list ->
        corr: string list -> formula<fol> list * string list

    /// <summary>
    /// Skolemizes a set of formulas. 
    /// </summary>
    /// 
    /// <param name="fms">The input set of formulas.</param>
    /// 
    /// <returns>
    /// The set of Skolemized formulas.
    /// </returns>
    /// 
    /// <example id="skolemizes-1">
    /// <code lang="fsharp">
    /// skolemizes !!>["exists x. P(f(g(x),z))"; "forall x. exists y. P(x,y)"] 
    /// </code>
    /// Evaluates to <c>[`P(old_f(old_g(f_x(z)),z))`; `forall x. P(x,f_y(x))`]</c>.
    /// </example>
    val skolemizes:
      fms: formula<fol> list -> formula<fol> list