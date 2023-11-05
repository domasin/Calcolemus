// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus.Lib

/// <summary>Generic functions for parsing.</summary>
/// 
/// <note>
/// The general function for parsing in the original code was in the 
/// <see cref='T:Calculemus.Intro'/> module and has been moved here 
/// for isolation purposes.
/// </note>
/// 
/// <category index="6">Lexer, parser and prettyprinter</category>
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

    // ---------------------------------------------------------------------- //
    // General parsing of infixes.                                            //
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// General parsing of iterated infixes.
    /// </summary>
    /// 
    /// <param name="opupdate">Modifies the function appropriately when a new item is parsed.</param>
    /// <param name="sof">Takes the current input and combines it in some way with the items arrived at so far.</param>
    val parse_ginfix:
      opsym: 'a ->
        opupdate: (('b -> 'c) -> 'b -> 'b -> 'c) ->
        sof: ('b -> 'c) ->
        subparser: ('a list -> 'b * 'a list) -> inp: 'a list -> 'c * 'a list
        when 'a: equality

    /// Parses left infix.
    val parse_left_infix:
      opsym: 'a ->
        opcon: ('b * 'b -> 'b) ->
        (('a list -> 'b * 'a list) -> 'a list -> 'b * 'a list) when 'a: equality

    /// Parses right infix.
    val parse_right_infix:
      opsym: 'a ->
        opcon: ('b * 'b -> 'b) ->
        (('a list -> 'b * 'a list) -> 'a list -> 'b * 'a list) when 'a: equality

    /// Parses a list: used to parse the list of arguments to a function.
    val parse_list:
      opsym: 'a -> (('a list -> 'b * 'a list) -> 'a list -> 'b list * 'a list)
        when 'a: equality

    // ---------------------------------------------------------------------- //
    // Other general parsing combinators.                                     //
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// Applies a function to the first element of a pair, the idea being to 
    /// modify the returned abstract syntax tree while leaving the 'unparsed 
    /// input' alone.
    /// </summary>
    val inline papply: f: ('a -> 'b) -> ast: 'a * rest: 'c -> 'b * 'c

    /// Checks if the head of a list (typically the list of unparsed input) is some 
    /// particular item, but also ?rst checks that the list is nonempty before 
    /// looking at its head.
    val nextin: inp: 'a list -> tok: 'a -> bool when 'a: equality

    /// Deals with the common situation of syntactic items enclosed in brackets. It 
    /// simply calls the subparser and then checks and eliminates the closing 
    /// bracket. In principle, the terminating character can be anything, so this 
    /// function could equally be used for other purposes, but we will always use 
    /// ')' for the cbra ('closing bracket') argument.
    val parse_bracketed:
      subparser: ('a -> 'b * 'c list) -> cbra: 'c -> inp: 'a -> 'b * 'c list
        when 'c: equality