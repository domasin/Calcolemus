// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// <summary>Lexical analysis.</summary>
/// <note>
/// The functions for lexical analysis in the original code where in the 
/// <see cref='T:FolAutomReas.Intro'/> module and have been moved here 
/// for isolation purposes.
/// </note>
module Lexer = 

    /// <summary>
    /// Creates a pattern matching function based on the input string <c>s</c> 
    /// as the pattern.
    /// </summary>
    val matches: s: string -> (string -> bool)  

    /// <summary>
    /// Classifies string characters as spaces.
    /// </summary>
    val space: (string -> bool) 

    /// <summary>
    /// Classifies string characters as punctuation.
    /// </summary>
    val punctuation: (string -> bool)   

    /// <summary>
    /// Classifies string characters as symbolic.
    /// </summary>
    val symbolic: (string -> bool)  

    /// <summary>
    /// Classifies string characters as numeric.
    /// </summary>
    val numeric: (string -> bool)   

    /// <summary>
    /// Classifies string characters as alphanumeric.
    /// </summary>
    val alphanumeric: (string -> bool)  

    /// <summary>
    /// Takes a property <c>prop</c> of characters, such as one of the 
    /// classifying predicates (<see cref='P:FolAutomReas.Lib.Lexer.space'/>, 
    /// <see cref='P:FolAutomReas.Lib.Lexer.punctuation'/>, 
    /// <see cref='P:FolAutomReas.Lib.Lexer.symbolic'/>, 
    /// <see cref='P:FolAutomReas.Lib.Lexer.numeric'/>, 
    /// <see cref='P:FolAutomReas.Lib.Lexer.alphanumeric'/>), and a list of 
    /// input characters <c>inp</c>, separating off as a string the longest 
    /// initial sequence of that list of characters 
    /// satisfying <c>prop</c>.
    /// </summary>
    val lexwhile: prop: (string -> bool) -> inp: string list -> string * string list    

    /// <summary>
    /// Lexical analyser. It maps a list of input characters <c>inp</c> into a 
    /// list of token strings.
    /// </summary>
    val lex: inp: string list -> string list   

    /// <summary>
    /// Generic function to impose lexing and exhaustion checking on a parser. 
    /// </summary>
    /// 
    /// <remarks>
    /// A wrapper function that explodes the input string, lexically analyzes 
    /// it, parses the sequence of tokens and then internally checks that no 
    /// input remains 
    /// unparsed.
    /// </remarks>
    val make_parser: pfn: (string list -> 'a * 'b list) -> s: string -> 'a  