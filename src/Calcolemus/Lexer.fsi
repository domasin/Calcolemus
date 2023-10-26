// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

/// <summary>Lexical analysis.</summary>
/// 
/// <note>
/// The functions for lexical analysis in the original code where in the 
/// <see cref='T:Calcolemus.Intro'/> module and have been moved here 
/// for isolation purposes.
/// </note>
/// 
/// <category index="6">Lexer, parser and prettyprinter</category>
module Lexer = 

    /// <summary>
    /// Creates a pattern matching function based on the input string <c>s</c> 
    /// as the pattern.
    /// </summary>
    /// 
    /// <param name="s">The string of all characters to be matched.</param>
    /// 
    /// <returns>
    /// A function that applied to a single character string checks 
    /// if it matches the given pattern.
    /// </returns>
    /// 
    /// <example id="matches-1">
    /// <code lang="fsharp">
    /// matches "abc" "a"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="matches-2">
    /// <code lang="fsharp">
    /// matches "abc" "d"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val matches: s: string -> (string -> bool)  

    /// <summary>
    /// Classifies single character strings as spaces.
    /// </summary>
    /// 
    /// <remarks>
    /// Tabs and new lines are also considered spaces.
    /// </remarks>
    /// 
    /// <param name="s">The single character string to be classified.</param>
    /// 
    /// <returns>
    /// true if the single character string it is considered a space, 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="space-1">
    /// <code lang="fsharp">
    /// space " "
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="space-2">
    /// <code lang="fsharp">
    /// space "."
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val space: s: string -> bool

    /// <summary>
    /// Classifies single character strings as punctuation symbols: 
    /// <c>()[]{},</c>.
    /// </summary>
    ///
    /// <param name="s">The single character string to be classified.</param>
    /// 
    /// <returns>
    /// true if the single character string it is considered a 
    /// punctuation symbol, otherwise false.
    /// </returns>
    /// 
    /// <example id="punctuation-1">
    /// <code lang="fsharp">
    /// punctuation ","
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="punctuation-2">
    /// <code lang="fsharp">
    /// punctuation "."
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val punctuation: s: string -> bool

    /// <summary>
    /// Classifies single character strings as symbolic: 
    /// <c>~`!@#$%^&amp;*-+=|\\:;&lt;&gt;.?/</c>.
    /// </summary>
    ///
    /// <param name="s">The single character string to be classified.</param>
    /// 
    /// <returns>
    /// true if the single character string it is considered symbolic, 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="symbolic-1">
    /// <code lang="fsharp">
    /// symbolic "."
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="symbolic-2">
    /// <code lang="fsharp">
    /// symbolic "1"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val symbolic: s: string -> bool

    ///  <summary>
    /// Classifies single character strings as numeric.
    /// </summary>
    ///
    /// <param name="s">The single character string to be classified.</param>
    /// 
    /// <returns>
    /// true if the single character string it is considered 
    /// numeric, otherwise false.
    /// </returns>
    /// 
    /// <example id="numeric-1">
    /// <code lang="fsharp">
    /// numeric "1"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="numeric-2">
    /// <code lang="fsharp">
    /// numeric "z"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val numeric: s: string -> bool

    ///  <summary>
    /// Classifies single character strings as alphanumeric.
    /// </summary>
    ///
    /// <param name="s">The single character string to be classified.</param>
    /// 
    /// <returns>
    /// true if the single character string it is considered 
    /// alphanumeric, otherwise false.
    /// </returns>
    /// 
    /// <example id="alphanumeric-1">
    /// <code lang="fsharp">
    /// alphanumeric "1"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="alphanumeric-2">
    /// <code lang="fsharp">
    /// alphanumeric "z"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="alphanumeric-3">
    /// <code lang="fsharp">
    /// alphanumeric "."
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val alphanumeric: s: string -> bool

    /// <summary>
    /// Takes a property <c>prop</c> of characters (such as one of the 
    /// classifying predicates: <see cref='P:Calcolemus.Lib.Lexer.space'/>, 
    /// <see cref='P:Calcolemus.Lib.Lexer.punctuation'/>, 
    /// <see cref='P:Calcolemus.Lib.Lexer.symbolic'/>, 
    /// <see cref='P:Calcolemus.Lib.Lexer.numeric'/>, 
    /// <see cref='P:Calcolemus.Lib.Lexer.alphanumeric'/>) and a list of 
    /// single character strings <c>inp</c>, separating off as a string 
    /// the longest initial sequence of that list of characters 
    /// satisfying <c>prop</c>.
    /// </summary>
    /// 
    /// <param name="prop">The predicate to identify tokens.</param>
    /// <param name="inp">The input list of single character strings.</param>
    /// 
    /// <returns>
    /// A pair with the longest initial sequence of elements of 
    /// <c>inp</c> classifiable as satisfying <c>prop</c> as the first 
    /// component, and the remaining characters as the second.
    /// </returns>
    /// 
    /// <example id="lexwhile-1">
    /// <code lang="fsharp">
    /// "((1 + 2) * x_1)"
    /// |> explode
    /// |> lexwhile punctuation 
    /// </code>
    /// Evaluates to <c>("((", ["1"; " "; "+"; " "; "2"; ")"; " "; "*"; " "; "x"; "_"; "1"; ")"])</c>.
    /// </example>
    val lexwhile: prop: (string -> bool) -> inp: string list -> string * string list    

    /// <summary>
    /// Lexical analyser. 
    /// </summary>
    /// 
    /// <remarks>
    /// It maps a list of input characters <c>inp</c> into a 
    /// list of token strings.
    /// </remarks>
    /// 
    /// <param name="inp">The input list of single character strings to be tokenized.</param>
    /// 
    /// <returns>
    /// The input list of single character strings tokenized.
    /// </returns>
    /// 
    /// <example id="lexwhile-1">
    /// <code lang="fsharp">
    /// "((11 + 2) * x_1)"
    /// |> explode
    /// |> lex
    /// </code>
    /// Evaluates to <c>["("; "("; "11"; "+"; "2"; ")"; "*"; "x_1"; ")"]</c>.
    /// Note how <c>11</c> and <c>x_1</c> are analyzed as a single tokens.
    /// </example>
    val lex: inp: string list -> string list   

    