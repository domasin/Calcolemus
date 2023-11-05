// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Herbrand

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Lib.Fpf

open Fol
open Clause
open Herbrand

[<Fact>]
let ``pholds should return true if the quantifier-free formula is true in the given valuations.``() = 
    !!"P(x) /\ Q(x)"
    |> pholds (function 
        | x when x = !!"P(x)" -> true 
        | x when x = !!"Q(x)" -> true 
        | _ -> false
    )
    |> should equal true

[<Fact>]
let ``pholds should return false if the quantifier-free formula is true in the given valuations.``() = 
    !!"P(x) /\ Q(x)"
    |> pholds (function 
        | x when x = !!"P(x)" -> true 
        | x when x = !!"Q(x)" -> false 
        | _ -> false
    )
    |> should equal false

[<Fact>]
let ``pholds should fail if applied to a formula that contains quantifiers.``() = 
    (fun () -> 
        !!"forall x. P(x) /\ Q(x)"
        |> pholds (function 
            | x when x = !!"P(x)" -> true 
            | x when x = !!"Q(x)" -> false 
            | _ -> false
        )
        |> ignore
    )
    |> should (throwWithMessage "Not part of propositional logic.") typeof<System.Exception>

[<Fact>]
let ``herbfuns should return the pair of the lists of functions in a formula separated in nullary and non.``() = 
    !!"forall x. P(x) /\ (Q(f(y)) ==> R(g(x,y),z))"
    |> herbfuns
    |> should equal ([("c", 0)], [("f", 1); ("g", 2)])

[<Fact>]
let ``groundterms-1.``() = 
    groundterms !!!>["0";"1"] [("f",1);("g",2)] 0
    |> sprint_termList
    |> shouldEqual ["``0``";"``1``"]

[<Fact>]
let ``groundterms-2.``() = 
    groundterms !!!>["0";"1"] [("f",1);("g",2)] 1
    |> sprint_termList
    |> shouldEqual 
        ["``f(0)``"; "``f(1)``"; "``g(0,0)``"; 
          "``g(0,1)``"; "``g(1,0)``"; "``g(1,1)``"]

[<Fact>]
let ``groundterms-3.``() = 
    groundterms !!!>["0";"1"] [("f",1);("g",2)] 2
    |> sprint_termList
    |> shouldEqual 
        ["``f(f(0))``"; "``f(f(1))``"; "``f(g(0,0))``"; "``f(g(0,1))``";
         "``f(g(1,0))``"; "``f(g(1,1))``"; "``g(0,f(0))``"; "``g(0,f(1))``";
         "``g(0,g(0,0))``"; "``g(0,g(0,1))``"; "``g(0,g(1,0))``"; "``g(0,g(1,1))``";
         "``g(1,f(0))``"; "``g(1,f(1))``"; "``g(1,g(0,0))``"; "``g(1,g(0,1))``";
         "``g(1,g(1,0))``"; "``g(1,g(1,1))``"; "``g(f(0),0)``"; "``g(f(0),1)``";
         "``g(f(1),0)``"; "``g(f(1),1)``"; "``g(g(0,0),0)``"; "``g(g(0,0),1)``";
         "``g(g(0,1),0)``"; "``g(g(0,1),1)``"; "``g(g(1,0),0)``"; "``g(g(1,0),1)``";
         "``g(g(1,1),0)``"; "``g(g(1,1),1)``"]

let ``groundtuples-1.``() = 
    groundtuples !!!>["0";"1"] [("f",1);("g",2)] 0 2
    |> sprint_termListList
    |> shouldEqual 
        [["``0``"; "``0``"]; ["``0``"; "``1``"]; ["``1``"; "``0``"];
         ["``1``"; "``1``"]]

[<Fact>]
let ``gilmore_mfn should return the update ground instance of the input formula.``() = 
    gilmore_mfn !!>>[["P(f(x))"]; ["~P(y)"]] 
       (subst (fpf ["x"; "y"] !!!>["c";"f(c)"])) 
       !!>>[["P(f(c))"]; ["~P(c)"]] 
    |> sprint_clauses
    |> shouldEqual 
        [["`P(f(c))`"]; ["`P(f(c))`"; "`~P(c)`"]; ["`~P(c)`"; "`~P(f(c))`"]]

[<Fact>]
let ``gilmore_tfn should return true if the input list is nonempty.``() = 
    !!>>[["P(f(c))"]; ["P(f(c))"; "~P(c)"]; ["P(f(c))"; "~P(f(c))"];
           ["~P(c)"; "~P(f(c))"]]
    |> gilmore_tfn
    |> shouldEqual true

[<Fact>]
let ``gilmore_tfn should return false if the input list is empty.``() = 
    !!>>[]
    |> gilmore_tfn
    |> shouldEqual false

[<Fact>]
let ``gilmore_loop should return the set of ground tuples that generate the unsatisfiable ground instances of an unsatisfiable set of clauses, if it succeeds.``() = 
    gilmore_loop !!>>[["Q(f_z(x))"; "~Q(y)"]] 
        !!!>["c"] [("f_z",1);("f_y",1)] ["x";"y"] 0 [[]] [] []
    |> sprint_termListList
    |> shouldEqual 
        [["``c``"; "``f_z(c)``"]; ["``c``"; "``c``"]]

[<Fact>]
let ``gilmore should succeed on p24 after trying 1 ground instance.``() = 
    gilmore Pelletier.p24
    |> should equal 1

[<Fact>]
let ``gilmore should succeed on p45 after trying 5 ground instances.``() = 
    gilmore Pelletier.p45
    |> should equal 5

[<Fact>]
let ``dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"c"])) [] should return [[P(c)]; [~P(f_y(c))]].``() = 
    dp_mfn 
        [[!!"P(x)"]; [!!"~P(f_y(x))"]] 
        (subst (fpf ["x"] [!!!"c"])) 
        [] 
    |> should equal [[!!"P(c)"]; [!!"~P(f_y(c))"]]

[<Fact>]
let ``dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"f_y(c)"])) [[!!"P(c)"]; [!!"~P(f_y(c))"]] should return [[P(c)]; [P(f_y(c))]; [~P(f_y(c))]; [~P(f_y(f_y(c)))]].``() = 
    dp_mfn 
        [[!!"P(x)"]; [!!"~P(f_y(x))"]] 
        (subst (fpf ["x"] [!!!"f_y(c)"])) 
        [[!!"P(c)"]; [!!"~P(f_y(c))"]]
    |> should equal 
        [[!!"P(c)"]; [!!"P(f_y(c))"]; [!!"~P(f_y(c))"]; [!!"~P(f_y(f_y(c)))"]]

[<Fact>]
let ``davisputnam should succeed on p20 after trying 19 ground instances.``() = 
    davisputnam Pelletier.p20
    |> should equal 19

[<Fact>]
let ``davisputnam002 should succeed on p20 after trying 2 ground instances.``() = 
    davisputnam002 Pelletier.p20
    |> should equal 2

[<Fact>]
let ``davisputnam should succeed on p36 after trying 40 ground instances.``() = 
    davisputnam Pelletier.p36
    |> should equal 40

[<Fact>]
let ``davisputnam002 should succeed on p36 after trying 3 ground instances.``() = 
    davisputnam002 Pelletier.p36
    |> should equal 3