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

    // ---------------------------------------------------------------------- //
    // General parsing of infixes.                                            //
    // ---------------------------------------------------------------------- //

    let rec parse_ginfix opsym opupdate sof subparser inp =
        let e1, inp1 = subparser inp
        match inp1 with
        | hd :: tl when hd = opsym ->
            parse_ginfix opsym opupdate (opupdate sof e1) subparser tl
        | _ ->
            sof e1, inp1

    let parse_left_infix opsym opcon =
        parse_ginfix opsym (fun f e1 e2 -> opcon (f e1, e2)) id

    let parse_right_infix opsym opcon =
        parse_ginfix opsym (fun f e1 e2 -> f <| opcon (e1, e2)) id

    let parse_list opsym =
        parse_ginfix opsym (fun f e1 e2 -> (f e1) @ [e2]) (fun x -> [x])

    // ---------------------------------------------------------------------- //
    // Other general parsing combinators.                                     //
    // ---------------------------------------------------------------------- //

    let inline papply f (ast, rest) =
        f ast, rest

    let nextin inp tok =
        match inp with
        | hd :: _ when hd = tok -> true
        | _ -> false

    let parse_bracketed subparser cbra inp =
        let ast, rest = subparser inp
        if nextin rest cbra then
            ast, List.tail rest
        else failwith "Closing bracket expected"