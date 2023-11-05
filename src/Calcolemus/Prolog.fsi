// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calcolemus

open Calcolemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Backchaining procedure for Horn clauses, and toy Prolog implementation.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Prolog = 

    /// <summary>
    /// Renames a rule.
    /// </summary>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val renamerule:
      k: int ->
        asm: formula<fol> list * c: formula<fol> ->
          (formula<fol> list * formula<fol>) * int

    /// <summary>
    /// Basic prover for Horn clauses.
    /// </summary>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val backchain:
      rules: (formula<fol> list * formula<fol>) list ->
        n: int ->
        k: int ->
        env: func<string,term> ->
        goals: formula<fol> list -> func<string,term>

    /// <summary>
    /// Converts a raw Horn clause into a rule.
    /// </summary>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val hornify:
      cls: formula<'a> list ->
        formula<'a> list * formula<'a> when 'a: equality

    /// <summary>
    /// Automated prover for formulas convertible into Horn clauses.
    /// </summary>
    /// 
    /// <category index="1">Automated prover for Horn clauses</category>
    val hornprove:
      fm: formula<fol> -> func<string,term> * int

    /// <summary>
    /// Parses rules in a Prolog-like syntax.
    /// </summary>
    /// 
    /// <category index="2">Prolog</category>
    val parserule:
      s: string -> formula<fol> list * formula<fol>

    /// <summary>
    /// Prolog interpreter without clear variable binding output.
    /// </summary>
    /// 
    /// <category index="2">Prolog</category>
    val simpleprolog:
      rules: string list -> gl: string -> func<string,term>

    /// <summary>
    /// Prolog interpreter.
    /// </summary>
    /// 
    /// <category index="2">Prolog</category>
    val prolog: rules: string list -> gl: string -> formula<fol> list