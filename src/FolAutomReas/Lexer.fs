// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (moved from intro for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

open FolAutomReas.Lib.String
open FolAutomReas.Lib.Set

module Lexer = 

    // Lexical analysis

    let matches s = 
        let chars = 
            explode s 
        fun c -> mem c chars

    let space = matches " \t\n\r"

    let punctuation = matches "()[]{},"

    let symbolic = matches "~`!@#$%^&*-+=|\\:;<>.?/"

    let numeric = matches "0123456789"

    let alphanumeric = matches  "abcdefghijklmnopqrstuvwxyz_'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

    let rec lexwhile prop inp =
        match inp with
        | c :: cs when prop c ->
            let tok, rest = lexwhile prop cs
            c + tok, rest
        | _ -> "", inp

    let rec lex inp =
        match snd <| lexwhile space inp with
        | [] -> []
        | c :: cs ->
            let prop =
                if alphanumeric c then alphanumeric
                else if symbolic c then symbolic
                else fun c -> false
            let toktl, rest = lexwhile prop cs
            (c + toktl) :: lex rest
    
    // Generic function to impose lexing and exhaustion checking on a parser

    let make_parser pfn (s : string) =
        let tokens =
            // Replace newlines with spaces so the lexer and parser
            // work correctly on multi-line strings.
            // TODO : This could probably be optimized to make the replacements
            // in a single pass using a Regex.
            s.Replace('\r', ' ')
                .Replace('\n', ' ')
            // Reduce multiple spaces to single spaces to help the parser.
                .Replace("  ", " ")
            |> explode
            |> lex

        match pfn tokens with
        | expr, [] ->
            expr
        | _, rest ->
            failwithf "Unparsed input: %i tokens remaining in buffer."
                <| List.length rest