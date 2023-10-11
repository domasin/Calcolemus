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
    /// Simplifies an algebraic expression at the first level.
    /// </summary>
    /// 
    /// <remarks>
    /// This is an example of symbolic computation.
    /// 
    /// It applies the following transformation rules
    /// <ul>
    /// <li>
    /// <c>Const 0 * Var x</c>, <c>Var x * Const 0</c> \(\longrightarrow\)
    /// <c>Const 0</c>
    /// </li>
    /// <li>
    /// <c>Const 0 + Var x</c>, <c>Var x + Const 0</c>, <c>Const 1 * Var x</c>, 
    /// <c>Var x * Const 1</c> \(\longrightarrow\) <c> Var x</c></li>
    /// <li><c>Const m + Const n</c> \(\longrightarrow\) <c>Const (m+n)</c></li>
    /// <li><c>Const m * Const n</c> \(\longrightarrow\) <c>Const (m*n)</c></li>
    /// </ul>
    /// 
    /// This function applies the rules only if they are applicable directly at 
    /// the first level of the expression's structure. It is an auxiliary 
    /// function used to define the complete function 
    /// <see cref='M:FolAutomReas.Intro.simplify'/> that applies the rules at 
    /// every level of the expression.
    /// </remarks>
    /// 
    /// <param name="expr">The input expression.</param>
    /// 
    /// <returns>
    /// The simplified expression if simplifiable; otherwise the input itself.
    /// </returns>
    /// 
    /// <example id="simplify1-1">
    /// <code lang="fsharp">
    /// Add(Const 0, Const 1) |> simplify1
    /// </code>
    /// Evaluates to <c>Const 1</c>.
    /// </example>
    /// 
    /// <example id="simplify1-2">
    /// <code lang="fsharp">
    /// Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify1 
    /// </code>
    /// Evaluates to <c>Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))</c>.
    /// <p>
    /// The input is returned unchanged, because even if the rules are 
    /// applicable to its sub-expressions, they cannot be applied directly 
    /// to the expression itself.
    /// </p>
    /// </example>
    val simplify1: expr: expression -> expression   

    /// <summary>
    /// Simplifies an algebraic expression completely.
    /// </summary>
    /// 
    /// <remarks>
    /// Completes the work of <see cref='M:FolAutomReas.Intro.simplify1'/>.
    /// 
    /// Recursively simplifies any immediate sub-expressions as much as 
    /// possible, then applies <see cref='M:FolAutomReas.Intro.simplify1'/> 
    /// to the result.
    /// </remarks>
    /// 
    /// <param name="expr">The input expression.</param>
    /// 
    /// <returns>
    /// The simplified expression if simplifiable; otherwise the input itself.
    /// </returns>
    /// 
    /// <example id="simplify-1">
    /// <code lang="fsharp">
    /// Mul (Add(Const 0, Const 1), Add(Const 0, Const 0)) |> simplify
    /// </code>
    /// Evaluates to <c>Const 0</c>.
    /// </example>
    /// 
    /// <example id="simplify-2">
    /// <code lang="fsharp">
    /// Add (Mul (Add (Mul (Const 0, Var "x"), Const 1), Const 3), Const 12)
    /// |> simplify
    /// </code>
    /// Evaluates to <c>Const 15</c>.
    /// </example>
    val simplify: expr: expression -> expression    

    /// <summary>
    /// Parses an atom.
    /// </summary>
    /// 
    /// <remarks>
    /// Implements the atoms part of the expression's recursive 
    /// descent parsing:
    /// 
    /// \begin{eqnarray*} 
    ///  atoms &amp; \longrightarrow &amp; (expression) \\
    ///        &amp; |               &amp; constant \\
    ///        &amp; |               &amp; variable 
    /// \end{eqnarray*}
    /// 
    /// It parses constants and variables and calls 
    /// <see cref='M:FolAutomReas.Intro.parse_expression'/> if applied to an 
    /// expression enclosed in brackets.
    /// </remarks>
    /// 
    /// <param name="i">The tokenized string list to be parsed.</param>
    /// 
    /// <returns>
    /// The pair consisting of the parsed expression tree together with any 
    /// unparsed input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Expected an expression at end of input', when applied to an empty list.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Expected closing bracket', when applied to a list with an initial opening bracket but without a closing one.</exception>
    /// 
    /// <example id="parse_atom-1">
    /// Parsing of a variable:
    /// <code lang="fsharp">
    /// parse_atom ["x"; "+"; "3"]
    /// </code>
    /// Evaluates to <c>(Var "x", ["+"; "3"])</c>.
    /// </example>
    /// 
    /// <example id="parse_atom-2">
    /// Parsing of a constant:
    /// <code lang="fsharp">
    /// parse_atom ["12"; "+"; "3"]
    /// </code>
    /// Evaluates to <c>(Const 12, ["+"; "3"])</c>.
    /// </example>
    /// 
    /// <example id="parse_atom-3">
    /// Parsing of an expression enclosed in brackets:
    /// <code lang="fsharp">
    /// parse_atom ["(";"12"; "+"; "3";")"]
    /// </code>
    /// Evaluates to <c>(Add (Const 12, Const 3), [])</c>.
    /// </example>
    /// 
    /// <example id="parse_atom-4">
    /// <code lang="fsharp">
    /// parse_atom []
    /// </code>
    /// Throws <c>System.Exception: Expected an expression at end of input</c>.
    /// </example>
    /// 
    /// <example id="parse_atom-5">
    /// <code lang="fsharp">
    /// parse_atom ["(";"12"; "+"; "3"]
    /// </code>
    /// Throws <c>System.Exception: Expected closing bracket</c>.
    /// </example>
    val parse_atom: i: string list -> expression * string list  

    /// <summary>
    /// Parses a product.
    /// </summary>
    /// 
    /// <remarks>
    /// Implements the products part of the expression's recursive 
    /// descent parsing:
    /// 
    /// \begin{eqnarray*} 
    ///  product &amp; \longrightarrow &amp; atom \\
    ///          &amp; |               &amp; atom * product \\
    /// \end{eqnarray*}
    /// 
    /// It calls 
    /// <see cref='M:FolAutomReas.Intro.parse_atom'/> to parse the first part of
    /// the expression and tries to parse the rest as a product.
    /// </remarks>
    /// 
    /// <param name="i">The tokenized string list to be parsed.</param>
    /// 
    /// <returns>
    /// The pair consisting of the parsed expression tree together with any 
    /// unparsed input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Expected an expression at end of input', when applied to an empty list.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Expected closing bracket', when applied to a list with an initial opening bracket but without a closing one.</exception>
    /// 
    /// <example id="parse_product-1">
    /// It parses an atom:
    /// <code lang="fsharp">
    /// parse_product ["x"; "+"; "3"]
    /// </code>
    /// Evaluates to <c>(Var "x", ["+"; "3"])</c>.
    /// </example>
    /// 
    /// <example id="parse_product-2">
    /// It parses a product completely:
    /// <code lang="fsharp">
    /// parse_product ["x"; "*"; "3"]
    /// </code>
    /// Evaluates to <c>(Mul (Var "x", Const 3), [])</c>.
    /// </example>
    /// 
    /// <example id="parse_product-3">
    /// It parses expressions enclosed in brackets:
    /// <code lang="fsharp">
    /// parse_product ["(";"12"; "+"; "3";")"]
    /// </code>
    /// Evaluates to <c>(Add (Const 12, Const 3), [])</c>.
    /// </example>
    /// 
    /// <example id="parse_product-4">
    /// <code lang="fsharp">
    /// parse_product []
    /// </code>
    /// Throws <c>System.Exception: Expected an expression at end of input</c>.
    /// </example>
    /// 
    /// <example id="parse_product-5">
    /// <code lang="fsharp">
    /// parse_product ["(";"12"; "+"; "3"]
    /// </code>
    /// Throws <c>System.Exception: Expected closing bracket</c>.
    /// </example>
    val parse_product: i: string list -> expression * string list 

    /// <summary>
    /// Recursive descent parsing of expression. It takes a list of tokens 
    /// <c>i</c> and returns a pair consisting of the parsed expression tree 
    /// together with any unparsed input.
    /// </summary>
    val parse_expression: i: string list -> expression * string list    

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