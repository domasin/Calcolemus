module Tests.Herbrand

open Xunit
open FsUnit.Xunit

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand

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
let ``groundterms [!|"0";!|"1"] ["f",1;"g",1] 0 returns [<<|0|>>; <<|1|>>].``() = 
    groundterms [!|"0";!|"1"] ["f",1;"g",1] 0
    |> List.map sprint_term
    |> should equal ["<<|0|>>"; "<<|1|>>"]

[<Fact>]
let ``groundterms [!|"0";!|"1"] ["f",1;"g",1] 1 returns [<<|f(0)|>>; <<|f(1)|>>; <<|g(0)|>>; <<|g(1)|>>].``() = 
    groundterms [!|"0";!|"1"] ["f",1;"g",1] 1
    |> List.map sprint_term
    |> should equal ["<<|f(0)|>>"; "<<|f(1)|>>"; "<<|g(0)|>>"; "<<|g(1)|>>"]