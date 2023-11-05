// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus.Lib

module String = 

    let explode (s : string) =
        s |> Seq.map string |> Seq.toList
        // let rec exap n l =
        //     if n < 0 then l
        //     else exap (n - 1) ((s.Substring(n,1))::l)
        // exap ((String.length s) - 1) []

    let implode (l:string list) = 
        // l |> String.concat ""
        List.foldBack (+) l "" // seems better to be checked

    let writeToString fn = 
        use sw = new System.IO.StringWriter()
        fn sw
        sw.ToString()