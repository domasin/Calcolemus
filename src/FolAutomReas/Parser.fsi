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
    /// input remains 
    /// unparsed.
    /// </remarks>
    val make_parser: pfn: (string list -> 'a * 'b list) -> s: string -> 'a  