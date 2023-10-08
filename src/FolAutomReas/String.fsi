// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// Explosion and implosion of strings.
module String = 

    /// <summary>
    /// Converts a string into a list of single-character strings.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>explode s</c> returns the list of single-character strings that make 
    /// up <c>s</c>, in the order in which they appear in <c>s</c>. If <c>s</c> 
    /// is the empty string, then an empty list is returned.
    /// </remarks>
    /// 
    /// <param name="s">The input string.</param>
    /// 
    /// <returns>
    /// The string exploded in it single-character strings.
    /// </returns>
    /// 
    /// <example id="explode-1">
    /// <code lang="fsharp">
    /// explode "The Quick fox."
    /// </code>
    /// Evaluates to <c>["T"; "h"; "e"; " "; "Q"; "u"; "i"; "c"; "k"; " "; "f"; "o"; "x"; "."]</c>.
    /// </example>
    /// 
    /// <example id="explode-2">
    /// <code lang="fsharp">
    /// explode ""
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    val explode: s: string -> string list

    /// <summary>
    /// Concatenates a list of strings into one string.
    /// </summary>
    /// 
    /// <remarks>
    /// <c>implode [s1;...;sn]</c> returns the string formed by concatenating 
    /// the strings <c>s1</c> ... <c>sn</c>. If n is zero (the list is empty), 
    /// then the empty string is returned.
    /// 
    /// <p>
    /// Never fails and accepts empty or multi-character component strings.
    /// </p>
    /// </remarks>
    /// 
    /// <param name="l">The input string list.</param>
    /// 
    /// <returns>
    /// The concatenation of input strings.
    /// </returns>
    /// 
    /// <example id="implode-1">
    /// <code lang="fsharp">
    /// implode ["e";"x";"a";"m";"p";"l";"e"]
    /// </code>
    /// Evaluates to <c>"example"</c>.
    /// </example>
    /// 
    /// <example id="implode-2">
    /// <code lang="fsharp">
    /// implode ["ex";"a";"mpl";"";"e"]
    /// </code>
    /// Evaluates to <c>"example"</c>.
    /// </example>
    /// 
    /// <example id="implode-3">
    /// <code lang="fsharp">
    /// implode []
    /// </code>
    /// Evaluates to <c>""</c>.
    /// </example>
    val implode: l: string list -> string

    /// Write from a StringWriter to a string
    val writeToString: fn: (System.IO.StringWriter -> unit) -> string