// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (adapted from lib for documentation)   //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

module Time = 

    let time f x =
        let timer = System.Diagnostics.Stopwatch.StartNew ()
        let result = f x
        timer.Stop ()
        printfn "CPU time (user): %f" timer.Elapsed.TotalSeconds
        result



