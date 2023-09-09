// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Timing; useful for documentation but not logically necessary. 
[<AutoOpen>]
module FolAutomReas.Lib.Timing

// ------------------------------------------------------------------------- //
// Timing; useful for documentation but not logically necessary.             //
// ------------------------------------------------------------------------- //

// pg. 617
/// Timing; useful for documentation but not logically necessary. 
let time f x =
    let timer = System.Diagnostics.Stopwatch.StartNew ()
    let result = f x
    timer.Stop ()
    printfn "CPU time (user): %f" timer.Elapsed.TotalSeconds
    result



