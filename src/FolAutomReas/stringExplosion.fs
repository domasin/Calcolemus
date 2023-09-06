// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib.fs)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Explosion and implosion of strings
[<AutoOpen>]
module FolAutomReas.lib.StringExplosion

// ------------------------------------------------------------------------- //
// Explosion and implosion of strings.                                       //
// ------------------------------------------------------------------------- //

// pg. 619
let explode (s : string) =
    let rec exap n l =
        if n < 0 then l
        else exap (n - 1) ((s.Substring(n,1))::l)
    exap ((String.length s) - 1) []
        
// pg. 619
let implode l = List.foldBack (+) l ""