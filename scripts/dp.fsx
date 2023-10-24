#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Prop
open FolAutomReas.DP
open FolAutomReas.Lib.Set

// fsi.AddPrinter sprint_prop_formula

!>> [["p"];["p";"~q"]]
|> hasUnitClause

!>> [["p";"q"]]
|> hasUnitClause

!>> [["p"];["s";"t"];["q"]] 
|> one_literal_rule

!>> [["p"];["s";"~p"];["~p";"t"]] 
|> one_literal_rule

!>> [["p"];["s";"p"];["q";"t"]] 
|> one_literal_rule

!>> [["s";"p"];["q";"t"]] 
|> one_literal_rule

!>> [["p";"q"];["~p";"~q"]]
|> hasPureLiteral

!>> [["p";"q"];["~p";"q"]]
|> hasPureLiteral


// exists pure literal

!>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
|> pureLiterals

!>> [["p";"q";"~t"];["~p";"q"];["p";"t"]]
|> affirmative_negative_rule

// not exists pure literal

!>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]]
|> pureLiterals

!>> [["p";"~q";"~t"];["~p";"q"];["p";"t"]]
|> affirmative_negative_rule

!>> [["p";"c1";"c2"];
     ["~p";"d1";"d2";"d3";"d4"];
     ["q";"t"];
     ["p";"e1";"e2"]]
|> resolve_on !>"p"

!>> [["a"];["b"]]
|> resolve_on !>"p"