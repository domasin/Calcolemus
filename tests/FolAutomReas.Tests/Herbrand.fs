module FolAutomReas.Tests.Herbrand

open Xunit
open FsUnit.Xunit

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand
open FolAutomReas.Pelletier
open FolAutomReas.Lib

[<Fact>]
let ``pholds (function Atom (R ("P", [Var "x"])) -> true) (parse "P(x)") returns true.``() = 
    pholds (function Atom (R ("P", [Var "x"])) -> true | _ -> false) (parse "P(x)")
    |> should equal true

[<Fact>]
let ``pholds (function Atom (R ("P", [Var "x"])) -> true | Atom (R ("Q", [Var "x"])) -> true) (parse "P(x) /\ Q(x)") returns true.``() = 
    parse @"P(x) /\ Q(x)"
    |> pholds (function 
                Atom (R ("P", [Var "x"])) -> true 
                | Atom (R ("Q", [Var "x"])) -> true 
                | _ -> false)
    |> should equal true

[<Fact>]
let ``groundterms [!!!"0";!!!"1"] ["f",1;"g",1] 0 returns [!!!"0";!!!"1"].``() = 
    groundterms [!!!"0";!!!"1"] ["f",1;"g",1] 0
    |> should equal [!!!"0";!!!"1"]

[<Fact>]
let ``groundterms [!!!"0";!!!"1"] ["f",1;"g",1] 1 returns [!!!"f(0)"; !!!"f(1)"; !!!"g(0)"; !!!"|g(1)"].``() = 
    groundterms [!!!"0";!!!"1"] ["f",1;"g",1] 1
    |> should equal [!!!"f(0)"; !!!"f(1)"; !!!"g(0)"; !!!"g(1)"]

[<Fact>]
let ``gilmore should succeed on p24 after trying 1 ground instance.``() = 
    gilmore p24
    |> should equal 1

[<Fact>]
let ``gilmore should succeed on p45 after trying 5 ground instances.``() = 
    gilmore p45
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
    davisputnam p20
    |> should equal 19

[<Fact>]
let ``davisputnam002 should succeed on p20 after trying 2 ground instances.``() = 
    davisputnam002 p20
    |> should equal 2

[<Fact>]
let ``davisputnam should succeed on p36 after trying 40 ground instances.``() = 
    davisputnam p36
    |> should equal 40

[<Fact>]
let ``davisputnam002 should succeed on p36 after trying 3 ground instances.``() = 
    davisputnam002 p36
    |> should equal 3