// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Formulas
open Fol

/// <summary>
/// Rewriting.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Rewrite = 

    /// <summary>
    /// Rewrites a term at the top level with the first element of an equation 
    /// list that succeeds to match.
    /// </summary>
    /// 
    /// <param name="eqs">The input list of equation.</param>
    /// <param name="t">The input term.</param>
    /// 
    /// <returns>
    /// The input term rewritten at the top level based on the first equation 
    /// that succeeds to match.
    /// </returns>
    /// 
    /// <example id="rewrite1-1">
    /// <code lang="fsharp">
    /// rewrite1 !!>["g(c) = 0"; "f(f(x)) = x"] !!!"f(f(f(x)))"
    /// </code>
    /// Evaluates to <c>``f(x)``</c>.
    /// </example>
    val rewrite1: eqs: formula<fol> list -> t: term -> term

    /// <summary>
    /// Normalize a term w.r.t. a set of equations.
    /// </summary>
    /// 
    /// <param name="eqs">The input list of equation.</param>
    /// <param name="tm">The input term.</param>
    /// 
    /// <returns>
    /// The input term with all its subterms rewritten at all and repeatedly 
    /// w.r.t the input list of equations.
    /// </returns>
    /// 
    /// <example id="rewrite-1">
    /// <code lang="fsharp">
    /// !!!"S(S(S(0))) * S(S(0)) + S(S(S(S(0))))"
    /// |> rewrite !!>[
    ///     "0 + x = x"; 
    ///     "S(x) + y = S(x + y)";
    ///     "0 * x = 0"; 
    ///     "S(x) * y = y + x * y"
    /// ]
    /// </code>
    /// Evaluates to <c>``S(S(S(S(S(S(S(S(S(S(0))))))))))``</c>.
    /// </example>
    val rewrite: eqs: formula<fol> list -> tm: term -> term