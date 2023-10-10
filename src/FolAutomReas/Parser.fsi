// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// <summary>Generic functions for parsing.</summary>
/// <note>
/// The general function for parsing in the original code was in the 
/// <see cref='T:FolAutomReas.Intro'/> module and has been moved here 
/// for isolation purposes.
/// </note>
module Parser = 

    /// <summary>
    /// Generic function to impose lexing and exhaustion checking on a parser. 
    /// </summary>
    /// 
    /// <remarks>
    /// A wrapper function that explodes the input string, lexically analyzes 
    /// it, parses the sequence of tokens and then internally checks that no 
    /// input remains unparsed.
    /// </remarks>
    /// 
    /// <param name="pfn">The input parser.</param>
    /// <param name="s">The input string to be parsed.</param>
    /// 
    /// <returns>
    /// The string parsed based on the input parser logic.
    /// </returns>
    /// 
    /// <example id="make_parser-1">
    /// <code lang="fsharp">
    /// // An oversimplified parser
    /// let rec parseIntList i = 
    ///     match parseInt i with
    ///     | e1, "," :: i1 ->
    ///         let e2, i2 = parseIntList i1
    ///         e1@e2, i2
    ///     | x -> x
    /// and parseInt i = 
    ///     match i with
    ///     | [] -> failwith "eof"
    ///     | tok :: i1 -> [int tok], i1
    /// 
    /// make_parser parseIntList "11,12"
    /// </code>
    /// Evaluates to <c>[11; 12]</c>.
    /// </example>
    val make_parser: pfn: (string list -> 'a * 'b list) -> s: string -> 'a  