// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Rewrite

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Lib.Partition

open Fol
open Rewrite

[<Fact>]
let ``rewrite1 should return the input term rewritten at the top level based on the first equation that succeeds to match.``() = 
    rewrite1 !!>["g(c) = 0"; "f(f(x)) = x"] !!!"f(f(f(x)))"
    |> sprint_term
    |> shouldEqual "``f(x)``"

[<Fact>]
let ``rewrite should return the input term with all its subterms rewritten at all and repeatedly w.r.t the input list of equations.``() = 
    !!!"S(S(S(0))) * S(S(0)) + S(S(S(S(0))))"
    |> rewrite !!>[
        "0 + x = x"; 
        "S(x) + y = S(x + y)";
        "0 * x = 0"; 
        "S(x) * y = y + x * y"
    ]
    |> sprint_term
    |> shouldEqual "``S(S(S(S(S(S(S(S(S(S(0))))))))))``"