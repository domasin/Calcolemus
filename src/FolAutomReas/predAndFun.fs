// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// <namespacedoc><summary>
/// Misc library functions to set up a nice environment. 
/// </summary></namespacedoc>
/// 
/// <summary>
/// Functions over predicates and functions.
/// </summary>
[<AutoOpen>]
module FolAutomReas.Lib.PredAndFun

/// Revereses a predicate
/// 
/// (non f) equals (fun x -> not (f x)). 
/// 4 |> non (fun x -> x = 2) gives true.
/// 2 |> non (fun x -> x = 2) gives false.
let inline non p x = not (p x)

/// Checks that a value satisfies a predicate.
/// 
/// check p x returns x if the application p x yields true. Otherwise, check p x fails.
/// 
/// check p x fails with Failure "check" if the predicate p yields false when applied 
/// to the value x.
let check p x = if p(x) then x else failwith "check"

/// Iterates a function a fixed number of times.
/// 
/// funpow n f x applies f to x, n times, giving the result f (f ... (f x)...) where the
/// number of f’s is n. funpow 0 f x returns x. If n is negative, it is treated as zero.
/// 
/// funpow n f x fails if any of the n applications of f fail.
let rec funpow n f x =
    if n < 1 then x else funpow (n-1) f (f x)

/// Tests for failure.
/// 
/// can f x evaluates to true if the application of f to x succeeds. It evaluates to false if
/// the application fails with a Failure _ exception.
/// 
/// Never fails on Failure _ exceptions.
let can f x = try f x |> ignore; true with _ -> false

/// Repeatedly apply a function until it fails.
/// 
/// The call repeat f x successively applies f over and over again starting with x, and stops
/// at the first point when a Failure _ exception occurs.
/// 
/// Never fails. If f fails at once it returns x.
let rec repeat f x = 
    try repeat f (f x)
    with _ -> x