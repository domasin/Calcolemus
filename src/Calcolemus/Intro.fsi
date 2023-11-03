// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

/// <namespacedoc><summary>
/// Types, functions, operators and procedures for automated/interactive 
/// reasoning in first order logic.
/// </summary></namespacedoc>
/// <summary>A simple algebraic expressions example to demonstrate the basic 
/// concepts of abstract syntax tree, symbolic computation, parsing and 
/// prettyprinting.
/// </summary>
/// 
/// <category index="1">Algebraic Example</category>
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
        /// Product expression.
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
    /// <see cref='M:Calcolemus.Intro.simplify'/> that applies the rules at 
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
    /// 
    /// <category index="1">Symbolic computation</category>
    val simplify1: expr: expression -> expression   

    /// <summary>
    /// Simplifies an algebraic expression completely.
    /// </summary>
    /// 
    /// <remarks>
    /// Completes the work of <see cref='M:Calcolemus.Intro.simplify1'/>.
    /// 
    /// Recursively simplifies any immediate sub-expressions as much as 
    /// possible, then applies <see cref='M:Calcolemus.Intro.simplify1'/> 
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
    /// 
    /// <category index="1">Symbolic computation</category>
    val simplify: expr: expression -> expression    

    /// <summary>
    /// Parses an atomic expression.
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
    /// An atomic expression is either a constant, a variable or an arbitrary 
    /// expression enclosed in brackets.
    /// 
    /// See also: <see cref='M:Calcolemus.Intro.parse_expression'/>; 
    /// <see cref='M:Calcolemus.Intro.parse_product'/>.
    /// </remarks>
    /// 
    /// <param name="i">The tokenized string list to be parsed.</param>
    /// 
    /// <returns>
    /// The pair consisting of the parsed expression tree together with any 
    /// unparsed input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected an expression at end of input</c>, when applied to an empty list.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected closing bracket</c>, when applied to a list with an initial opening bracket but without a closing one.</exception>
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
    /// 
    /// <category index="2">Parsing</category>
    val parse_atom: i: string list -> expression * string list  

    /// <summary>
    /// Parses a product expression.
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
    /// A product expression is a sequence of 'atomic expressions' (see <see 
    /// cref='M:Calcolemus.Intro.parse_atom'/>) separated by <c>*</c>.
    /// 
    /// See also: <see cref='M:Calcolemus.Intro.parse_expression'/>.
    /// </remarks>
    /// 
    /// <param name="i">The tokenized string list to be parsed.</param>
    /// 
    /// <returns>
    /// The pair consisting of the parsed expression tree together with any 
    /// unparsed input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected an expression at end of input</c>, when applied to an empty list.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected closing bracket</c>, when applied to a list with an initial opening bracket but without a closing one.</exception>
    /// 
    /// <example id="parse_product-1">
    /// It parses just the first atom if applied to an addition:
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
    /// 
    /// <note>
    /// The grammar is right-associative (\(atom * product \)). 
    /// So repeated operations that lacks disambiguation are interpreted 
    /// as right-associative: \(x * y * z = x * (y * z)\) .
    /// </note>
    /// 
    /// <category index="2">Parsing</category>
    val parse_product: i: string list -> expression * string list 

    /// <summary>
    /// Parses an expression.
    /// </summary>
    /// 
    /// <remarks>
    /// Implements the addition part of the expression's recursive 
    /// descent parsing:
    /// 
    /// \begin{eqnarray*} 
    ///  expression &amp; \longrightarrow &amp; product \\
    ///             &amp; |               &amp; product + expression \\
    /// \end{eqnarray*}
    /// 
    /// An expression is a sequence of 'product expressions' (see <see 
    /// cref='M:Calcolemus.Intro.parse_product'/>) separated by <c>+</c>.
    /// 
    /// See also: <see cref='M:Calcolemus.Intro.parse_atom'/>.
    /// </remarks>
    /// 
    /// <param name="i">The tokenized string list to be parsed.</param>
    /// 
    /// <returns>
    /// The pair consisting of the parsed expression tree together with any 
    /// unparsed input.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected an expression at end of input</c>, when applied to an incomplete expression.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected closing bracket</c>, when applied to a list with an initial opening bracket but without a closing one.</exception>
    /// 
    /// <example id="parse_expression-1">
    /// <code lang="fsharp">
    /// parse_expression ["x"; "+"; "3"]
    /// </code>
    /// Evaluates to <c>(Add (Var "x", Const 3), [])</c>.
    /// </example>
    /// 
    /// <example id="parse_expression-2">
    /// <code lang="fsharp">
    /// parse_expression ["x"; "+";]
    /// </code>
    /// Throws <c>System.Exception: Expected an expression at end of input</c>.
    /// </example>
    /// 
    /// <example id="parse_expression-5">
    /// <code lang="fsharp">
    /// parse_expression ["(";"12"; "+"; "3"]
    /// </code>
    /// Throws <c>System.Exception: Expected closing bracket</c>.
    /// </example>
    /// 
    /// <note>
    /// The grammar is right-associative (\(product + expression \)). 
    /// So repeated operations that lacks disambiguation are interpreted 
    /// as right-associative: \(x + y + z = x + (y + z)\) .
    /// </note>
    /// 
    /// <category index="2">Parsing</category>
    val parse_expression: i: string list -> expression * string list    

    /// <summary>
    /// Parses a string into an expression.
    /// </summary>
    /// 
    /// <param name="s">The input string to be parsed.</param>
    /// 
    /// <returns>
    /// The expression corresponding to the input string.
    /// </returns>
    /// 
    /// <remarks>
    /// See also: <see cref='M:Calcolemus.Intro.parse_atom'/>; 
    /// <see cref='M:Calcolemus.Intro.parse_product'/>; 
    /// <see cref='M:Calcolemus.Intro.parse_expression'/>
    /// </remarks>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected an expression at end of input</c>, when applied to an incomplete string expression.</exception>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>Expected closing bracket</c>, when applied to a string with an initial opening bracket but without a closing one.</exception>
    /// 
    /// <example id="parse_exp-1">
    /// <code lang="fsharp">
    /// parse_exp "(x1 + x2 + x3) * (1 + 2 + 3 * x + y)"
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// Mul
    ///     (Add (Var "x1", Add (Var "x2", Var "x3")),
    ///      Add (Const 1, Add (Const 2, Add (Mul (Const 3, Var "x"), Var "y"))))
    /// </code>
    /// </example>
    /// 
    /// <example id="parse_exp-2">
    /// <code lang="fsharp">
    /// parse_exp "x +"
    /// </code>
    /// Throws <c>System.Exception: Expected an expression at end of input</c>.
    /// </example>
    /// 
    /// <example id="parse_exp-5">
    /// <code lang="fsharp">
    /// parse_exp "(12 + 3"
    /// </code>
    /// Throws <c>System.Exception: Expected closing bracket</c>.
    /// </example>
    /// 
    /// <category index="2">Parsing</category>
    val parse_exp: s: string -> expression

    /// <summary>
    /// Returns a naive concrete syntax representation of an expression.
    /// </summary>
    /// 
    /// <remarks>
    /// Reverses transformation, from abstract to concrete syntax keeping 
    /// brackets. 
    /// 
    /// This is a naive version that puts brackets uniformly round each 
    /// instance of a binary operator, which is perfectly correct but sometimes 
    /// looks cumbersome to a human.
    /// 
    /// Seealso: 
    /// <see cref='M:Calcolemus.Intro.sprint_exp'/>; 
    /// <see cref='M:Calcolemus.Intro.print_exp'/>;
    /// for clever versions.
    /// </remarks>
    /// 
    /// <param name="e">The expression to be translated.</param>
    /// 
    /// <returns>The expression's concrete syntax representation.</returns>
    /// 
    /// <example id="string_of_exp_naive-1">
    /// <code lang="fsharp">
    /// Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
    /// |> string_of_exp_naive
    /// </code>
    /// Evaluates to <c>"((0 + 1) * (0 + 0))"</c>
    /// </example>
    /// 
    /// <category index="3">Prettyprinting</category>
    val string_of_exp_naive: e: expression -> string    

    /// <summary>
    /// Returns a concrete syntax representation of an expression considering 
    /// the precedence level of the operator of which the expression is an 
    /// immediate sub-expression.
    /// </summary>
    /// 
    /// <remarks>
    /// It is an auxiliary function used to define 
    /// <see cref='M:Calcolemus.Intro.sprint_exp'/> and 
    /// <see cref='M:Calcolemus.Intro.print_exp'/>.
    /// and to calculate whether or not additional brackets can be omitted.
    /// 
    /// <p>
    /// The allocated precedences are as follows:
    /// </p>
    /// 
    /// <ul>
    /// <li>2 to addition;</li>
    /// <li>4 to multiplication;</li>
    /// <li>0 at the outermost level.</li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="pr">The precedence level of the operator of which the expression is an immediate sub-expression .</param>
    /// <param name="e">The expression to be translated.</param>
    /// 
    /// <returns>
    /// The expression's concrete syntax representation with 
    /// additional brackets based on <c>pr</c>.
    /// </returns>
    /// 
    /// <example id="string_of_exp-1">
    /// <code lang="fsharp">
    /// "x + 1"
    /// |> parse_exp
    /// |> string_of_exp 2
    /// </code>
    /// Evaluates to <c>"x + 1"</c>
    /// </example>
    /// 
    /// <example id="string_of_exp-2">
    /// <code lang="fsharp">
    /// "x + 1"
    /// |> parse_exp
    /// |> string_of_exp 3
    /// </code>
    /// Evaluates to <c>"(x + 1)"</c>
    /// </example>
    /// 
    /// <category index="3">Prettyprinting</category>
    val string_of_exp: pr: int -> e: expression -> string   

    /// <summary>
    /// Prints to the <c>stdout</c> the concrete syntax representation of an 
    /// expression.
    /// </summary>
    /// 
    /// <remarks>
    /// Calculates the concrete syntax of an expression <c>e</c> 
    /// removing unnecessary brackets and writes it to the <c>stdout</c>. 
    /// 
    /// It omits the outermost brackets, and those that are implicit in rules 
    /// for precedence or associativity.
    /// 
    /// Seealso: 
    /// <see cref='M:Calcolemus.Intro.string_of_exp'/>;
    /// <see cref='M:Calcolemus.Intro.sprint_exp'/>.
    /// </remarks>
    /// 
    /// <param name="e">The expression to be translated.</param>
    /// 
    /// <example id="print_exp-1">
    /// <code lang="fsharp">
    /// Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
    /// |> print_exp
    /// </code>
    /// After evaluation the text <c>"&lt;&lt;(0 + 1) * (0 + 0)&gt;&gt;"</c> is 
    /// written to the <c>stdout</c>.
    /// </example>
    /// 
    /// <category index="3">Prettyprinting</category>
    val print_exp: e: expression -> unit    

    /// <summary>
    /// Returns the concrete syntax representation of an expression.
    /// </summary>
    /// 
    /// <remarks>
    /// Returns a string of the concrete syntax of an expression <c>e</c> 
    /// removing unnecessary brackets. It omits the outermost brackets, and 
    /// those that are implicit in rules for precedence or associativity.
    /// 
    /// Seealso: 
    /// <see cref='M:Calcolemus.Intro.string_of_exp'/>;
    /// <see cref='M:Calcolemus.Intro.print_exp'/>.
    /// </remarks>
    /// 
    /// <param name="e">The expression to be translated.</param>
    /// 
    /// <returns>The expression's concrete syntax representation.</returns>
    /// 
    /// <example id="sprint_exp-1">
    /// <code lang="fsharp">
    /// Mul (Add(Const 0, Const 1), Add(Const 0, Const 0))
    /// |> sprint_exp
    /// </code>
    /// Evaluates to <c>"&lt;&lt;(0 + 1) * (0 + 0)&gt;&gt;"</c>
    /// </example>
    /// 
    /// <category index="3">Prettyprinting</category>
    val sprint_exp: e: expression -> string