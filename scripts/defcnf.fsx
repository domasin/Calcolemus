#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Formulas
open Calcolemus.Prop
open Calcolemus.Lib.Fpf
open Calcolemus.Defcnf
open Calcolemus.Lib.Lexer
open Calcolemus.Lib.String
open Calcolemus.Stal
open Calcolemus.Propexamples

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

!> @"(p \/ (q /\ ~r)) /\ s"
|> defcnfs

!> @"(p \/ (q /\ ~r)) /\ s"
|> defcnf
// `(p \/ p_1) /\ (p_1 \/ r \/ ~q) /\ (q \/ ~p_1) /\ s /\ (~p_1 \/ ~r)`

!> @"(p \/ (q /\ ~r)) /\ s"
|> defcnf01
// `(p \/ p_1 \/ ~p_2) /\ (p_1 \/ r \/ ~q) /\ (p_2 \/ ~p) /\ (p_2 \/ ~p_1) /\ (p_2 \/ ~p_3) /\ p_3 /\ (p_3 \/ ~p_2 \/ ~s) /\ (q \/ ~p_1) /\ (s \/ ~p_3) /\ (~p_1 \/ ~r)`

!> @"(a \/ b \/ c \/ d) /\ s"
|> defcnf3

!> @"(a \/ b \/ c \/ d) /\ s"
|> defcnf