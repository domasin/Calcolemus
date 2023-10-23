#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Lib.Fpf
open FolAutomReas.Defcnf
open FolAutomReas.Lib.Lexer
open FolAutomReas.Lib.String
open FolAutomReas.Stal
open FolAutomReas.Propexamples

let sprint_bigint (bi:bigint) = sprintf "%OI" bi

// fsi.AddPrinter sprint_prop_formula
// fsi.AddPrinter sprint_bigint

mk_imp !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mk_iff !>"(x <=> q) /\ p" (psubst (P "x" |=> !>"q") !>"p")
|> tautology

mkprop 3I

// `~p \/ q`
maincnf (!> @"p \/ (p \/ q)", undefined, 0I) 
|> fun (fm,defs,counter) -> 
    fm, defs |> graph, counter
// (`p_1`,
//    `p \/ p_0` |-> (`p_1`, `p_1 <=> p \/ p_0`)
//    `p \/ q`   |-> (`p_0`, `p_0 <=> p \/ q`),
//    2I)

let max_varindex pfx s (n : bigint) =
    let m = String.length pfx
    let l = String.length s
    if l <= m || s.Substring(0,m) <> pfx then n else
    let s' = s.Substring(m,(l - m))
    if List.forall numeric (explode s') then
        max n (bigint.Parse(s'))
    else n


let pfx,s = "p_", "p_1"

max_varindex "p_" "p_0" 1I // evaluates to 1I
max_varindex "p_" "p_2" 1I // evaluates to 2I
max_varindex "p_" "x_2" 1I // evaluates to 1I

!>"p ==> q"
|> mk_defcnf maincnf

!>"p ==> q"
|> defcnf01
|> sprint_prop_formula

stalmarck (mk_adder_test 6 3)

stalmarck (mk_adder_test 2 1)

stalmarck (prime 11)

tautology (prime 2)

stalmarck (!> @"a \/ ~a")

List.nth [1;2;3;4]  2

List.item 2 [1;2;3;4]