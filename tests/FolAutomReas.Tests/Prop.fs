module FolAutomReas.Tests.Prop

open Xunit
open FsUnit.Xunit

open FolAutomReas.Lib.Fpf

open FolAutomReas.Prop
open FolAutomReas.Formulas

[<Fact>]
let ``pname should return the name of the proposition.``() = 
    P "x" |> pname
    |> should equal "x"

[<Fact>]
let ``!> should return the parsed prop formula.``() = 
    !> "p /\ q ==> q /\ r"
    |> should equal (Imp (And (Atom (P "p"), Atom (P "q")), And (Atom (P "q"), Atom (P "r"))))

[<Fact>]
let ``eval should return the truth-value of the formula in the given valuation.``() = 
    eval (!>"p /\ q ==> q /\ r") 
        (function P"p" -> true | P"q" -> false | P"r" -> true | _ -> failwith "undefined")
    |> should equal true

[<Fact>]
let ``atoms should return the atoms of the formula.``() = 
    !>"p /\ q ==> q /\ r" 
    |> atoms
    |> should equal [P "p"; P "q"; P "r"]

[<Fact>]
let ``onallvaluations should return true if subfn returns true for each atoms on all valuations.``() = 
    onallvaluations (eval True) (fun _ -> false) []
    |> should equal true

[<Fact>]
let ``sprint_truthtable should return a string representation of the truth table of the input formula.``() = 
    sprint_truthtable !>"p ==> q"
    |> should equal 
        "p     q     |   formula
---------------------
false false | true  
false true  | true  
true  false | false 
true  true  | true  
---------------------

"

[<Fact>]
let ``tautology-1 should return false.``() = 
    !> @"p \/ ~p"
    |> tautology
    |> should equal true

[<Fact>]
let ``tautology-2 should return false.``() = 
    !> @"p \/ q ==> p"
    |> tautology
    |> should equal false

[<Fact>]
let ``tautology-3 should return false.``() = 
    !> @"p \/ q ==> q \/ (p <=> q)"
    |> tautology
    |> should equal false

[<Fact>]
let ``tautology-4 should return true.``() = 
    !> @"(p \/ q) /\ ~(p /\ q) ==> (~p <=> q)"
    |> tautology
    |> should equal true

[<Fact>]
let ``unsatisfiable should return true if the formula is unsatisfiable.``() = 
    !> "p /\ ~p"
    |> unsatisfiable
    |> should equal true

[<Fact>]
let ``unsatisfiable should return false if the formula is satisfiable.``() = 
    !> "p"
    |> unsatisfiable
    |> should equal false

[<Fact>]
let ``satisfiable should return false if the formula is unsatisfiable.``() = 
    !> "p /\ ~p"
    |> unsatisfiable
    |> should equal true

[<Fact>]
let ``satisfiable should return true if the formula is satisfiable.``() = 
    !> "p"
    |> unsatisfiable
    |> should equal false

[<Fact>]
let ``psubst should replace atoms with formulas based on fpf mapping.``() = 
    !> "p /\ q /\ p /\ q"
    |> psubst (P"p" |=> !>"p /\ q")
    |> sprint_prop_formula
    |> should equal "`(p /\ q) /\ q /\ (p /\ q) /\ q`"

[<Fact>]
let ``dual should return the dual of the input formula.``() = 
    !> @"p \/ ~p"
    |> dual
    |> sprint_prop_formula
    |> should equal "`p /\ ~p`"

[<Fact>]
let ``psimplify1 should simplify at the first level.``() = 
    !> "false /\ p"
    |> psimplify1
    |> sprint_prop_formula
    |> should equal "`false`"

[<Fact>]
let ``psimplify should simplify also on subformulas.``() = 
    !> @"((x ==> y) ==> true) \/ ~false"
    |> psimplify
    |> sprint_prop_formula
    |> should equal "`true`"

[<Fact>]
let ``negative should return true if applied to a negative literal.``() = 
    !> "~p"
    |> negative
    |> should equal true

[<Fact>]
let ``negative should return false if applied to a literal that is not negative.``() = 
    !> "p"
    |> negative
    |> should equal false

[<Fact>]
let ``positive should return true if applied to a positive literal.``() = 
    !> "p"
    |> positive
    |> should equal true

[<Fact>]
let ``positive should return false if applied to a literal that is not positive.``() = 
    !> "~p"
    |> positive
    |> should equal false

[<Fact>]
let ``negate should return the negated literal if applied to a positive literal.``() = 
    !> "p"
    |> negate
    |> sprint_prop_formula
    |> should equal "`~p`"

[<Fact>]
let ``negate should return the not negated literal  if applied to a negative literal.``() = 
    !> "~p"
    |> negate
    |> sprint_prop_formula
    |> should equal "`p`"

[<Fact>]
let ``nnf_naive should return the input formula transformed in a naive nnf.``() = 
    !> "~ (p ==> false)"
    |> nnf_naive
    |> sprint_prop_formula
    |> should equal "`p /\ ~false`"

[<Fact>]
let ``nnf should return the input formula transformed in a complete nnf.``() = 
    !> "~ (p ==> false)"
    |> nnf
    |> sprint_prop_formula
    |> should equal "`p`"

[<Fact>]
let ``nnf-2.``() = 
    !> "(p <=> q) <=> ~(r ==> s)"
    |> nnf
    |> sprint_prop_formula
    |> should equal @"`(p /\ q \/ ~p /\ ~q) /\ r /\ ~s \/ (p /\ ~q \/ ~p /\ q) /\ (~r \/ s)`"

[<Fact>]
let ``nenf_naive should return nnf but keeping logical equivalences.``() = 
    !> "~ (p <=> q)"
    |> nenf_naive
    |> sprint_prop_formula
    |> should equal "`p <=> ~q`"

[<Fact>]
let ``nenf_naive should return nnf but keeping logical constants mixed with other formulas.``() = 
    !> "~ (false <=> q)"
    |> nenf_naive
    |> sprint_prop_formula
    |> should equal "`false <=> ~q`"

[<Fact>]
let ``nenf should return nnf but keeping logical equivalences.``() = 
    !> "~ (p <=> q)"
    |> nenf
    |> sprint_prop_formula
    |> should equal "`p <=> ~q`"

[<Fact>]
let ``nenf_naive should apply simplification.``() = 
    !> "~ (false <=> q)"
    |> nenf
    |> sprint_prop_formula
    |> should equal "`q`"

[<Fact>]
let ``list_conj should return the conjunction of the input formulas.``() = 
    list_conj [!>"p";!>"q";!>"r"]
    |> sprint_prop_formula
    |> should equal "`p /\ q /\ r`"

[<Fact>]
let ``list_disj should return the disjunction of the input formulas.``() = 
    list_disj [!>"p";!>"q";!>"r"]
    |> sprint_prop_formula
    |> should equal @"`p \/ q \/ r`"

[<Fact>]
let ``mk_lits should return the conjunction of the input formulas positive or negative depending on valuation.``() = 
    mk_lits [!>"p";!>"q"] 
        (function P"p" -> true | P"q" -> false | _ -> failwith "")
    |> sprint_prop_formula
    |> should equal "`p /\ ~q`"