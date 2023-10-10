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

    let matches s = 
        let chars = 
            explode s 
        fun c -> mem c chars

    let space s = 
        matches " \t\n\r" s

    let punctuation s = 
        matches "()[]{}," s

    let symbolic s = 
        matches "~`!@#$%^&*-+=|\\:;<>.?/" s

    let numeric s = 
        matches "0123456789" s

    let alphanumeric s = 
        matches  "abcdefghijklmnopqrstuvwxyz_'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" s

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
