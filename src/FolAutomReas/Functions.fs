// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

module Functions = 

    let inline non p x = 
        not (p x)

    let check p x = 
        if p(x) then 
            x 
        else 
            failwith "check"

    let rec funpow n f x =
        if n < 1 then 
            x 
        else 
            funpow (n-1) f (f x)

    let can f x = 
        try 
            f x |> ignore; 
            true 
        with _ -> 
            false

    let rec repeat f x = 
        try 
            repeat f (f x)
        with _ -> 
            x