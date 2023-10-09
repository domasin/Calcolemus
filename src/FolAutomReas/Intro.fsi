// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas

/// <namespacedoc><summary>
/// Types, functions, operators and procedures for automated/interactive 
/// reasoning in first order logic.
/// </summary></namespacedoc>
/// <summary>A simple algebraic expression example.</summary>
module Intro =

    /// <summary>
    /// Abstract syntax tree of algebraic expressions.
    /// </summary>
    type expression =
        /// <summary>
        /// Variable expression.
        /// </summary>
        /// 
        /// <param name="Item">The variable name.</param>
        | Var of string
        /// <summary>
        /// Constant expression.
        /// </summary>
        /// 
        /// <param name="Item">The constant integer.</param>
        | Const of int
        /// <summary>
        /// Addition expression.
        /// </summary>
        /// 
        /// <param name="Item1">The first addendum.</param>
        /// <param name="Item2">The second addendum.</param>
        | Add of expression * expression
        /// <summary>
        /// Multiplication expression.
        /// </summary>
        /// 
        /// <param name="Item1">The first expression to be multiplied.</param>
        /// <param name="Item2">The second expression to be multiplied.</param>
        | Mul of expression * expression    

    /// <summary>
    /// Simplify an algebraic expression at the first level.
    /// </summary>
    val simplify1: expr: expression -> expression   

    /// <summary>
    /// Recursively simplifies any immediate sub-expressions as much as 
    /// possible, then applies <see cref='M:FolAutomReas.Intro.simplify1'/> 
    /// to the result.
    /// </summary>
    val simplify: expr: expression -> expression    

    /// <summary>
    /// Recursive descent parsing of expression. It takes a list of tokens 
    /// <c>i</c> and returns a pair consisting of the parsed expression tree 
    /// together with any unparsed input.
    /// </summary>
    val parse_expression: i: string list -> expression * string list    

    /// <summary>
    /// Parses a product.
    /// </summary>
    val parse_product: i: string list -> expression * string list   

    /// <summary>
    /// Parses an atom.
    /// </summary>
    val parse_atom: i: string list -> expression * string list  

    /// Parses a string into an expression.
    val parse_exp: (string -> expression)   

    /// Reverses transformation, from abstract to concrete syntax keeping brackets. 
    /// It puts brackets uniformly round each instance of a binary operator, 
    /// which is perfectly correct but sometimes looks cumbersome to a human.
    val string_of_exp_naive: e: expression -> string    

    /// Auxiliary function to print expressions without unnecessary brackets. 
    val string_of_exp: pr: int -> e: expression -> string   

    /// Prints an expression `e` in concrete syntax removing unnecessary brackets. 
    /// It omits the outermost brackets, and those that are implicit in rules for 
    /// precedence or associativity.
    val print_exp: e: expression -> unit    

    /// Returns a string of the concrete syntax of an expression `e` removing 
    /// unnecessary brackets. It omits the outermost brackets, and those that are 
    /// implicit in rules for precedence or associativity.
    val sprint_exp: e: expression -> string