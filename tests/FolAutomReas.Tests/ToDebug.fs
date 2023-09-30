module FolAutomReas.Tests.ToDebug

open Xunit
open FsUnit.Xunit

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand

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