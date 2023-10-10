// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (moved from intro for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

open FolAutomReas.Lib.String
open FolAutomReas.Lib.Lexer

module Parser = 

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