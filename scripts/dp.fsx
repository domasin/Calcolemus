#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Prop
open FolAutomReas.DP
open FolAutomReas.Lib.Set
open FolAutomReas.Lib.Search
open FolAutomReas.Propexamples

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

let cls = !>> [
     ["p";"c"];["~p";"d"]
     ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
]

resolution_blowup cls !>"c" // evaluates to -1
resolution_blowup cls !>"d" // evaluates to -1
resolution_blowup cls !>"e" // evaluates to -1
resolution_blowup cls !>"p" // evaluates to -1
resolution_blowup cls !>"q" // evaluates to 1

resolve_on !>"c" cls 
// [[`p`; `q`]; [`q`; `~d`]; [`q`; `~e`]; [`~p`; `d`]; [`~q`; `e`]; [`~q`; `~d`]]

resolve_on !>"d" cls 
// [[`p`; `c`]; [`q`; `~c`]; [`q`; `~e`]; [`q`; `~p`]; [`~p`; `~q`]; [`~q`; `e`]]

resolve_on !>"e" cls 
// [[`p`; `c`]; [`q`; `~c`]; [`q`; `~d`]; [`~p`; `d`]; [`~q`; `~d`]]

resolve_on !>"p" cls 
// [`c`; `d`]; [`q`; `e`]; [`q`; `~c`]; [`q`; `~d`]; [`~q`; `e`]; [`~q`; `~d`]]

resolve_on !>"q" cls 
// [[`e`]; [`e`; `~c`]; [`e`; `~d`]; [`p`; `c`]; [`~c`; `~d`]; [`~d`]; [`~p`; `d`]]

resolution_rule cls

let pvs = List.filter positive (unions cls)
let p = minimize (resolution_blowup cls) pvs
resolve_on p cls

!>> [
     ["p";"c"];["~p";"d"]
     ["q";"~c"];["q";"~d"];["q";"~e"];["~q";"~d"];["~q";"e"]
]
|> resolution_rule

tautology(prime 15)
dptaut(prime 15)