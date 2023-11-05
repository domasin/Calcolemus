// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.ToDebug

open Xunit
open FsUnit.Xunit

open Calculemus.Formulas
open Calculemus.Fol
open Calculemus.Herbrand

let p42Parsed = 
    Not
      (Exists
         ("y",
          Forall
            ("x",
             Iff
               (Atom (R ("P", [Var "x"; Var "y"])),
                Not
                  (Exists
                     ("z",
                      And
                        (Atom (R ("P", [Var "x"; Var "z"])),
                         Atom (R ("P", [Var "z"; Var "x"])))))))))

[<Fact>]
let ``davisputnam should succeed on p42 after trying 3 ground instances.``() = 
    davisputnam p42Parsed
    |> should equal 3