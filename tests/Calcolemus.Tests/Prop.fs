// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Prop

open Xunit
open FsUnit.Xunit

open Calcolemus.Lib.Fpf

open Calcolemus.Prop
open Calcolemus.Formulas

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
let ``allvaluations should return all valuations of the formula.``() = 
    let fm = !> @"(p /\ q) \/ s"

    allvaluations fm
    // graphs of all valuations of atoms in fm
    |> List.map (fun v -> 
        atoms fm
        |> List.map (fun a -> (a, v a))
    )
    |> should equal 
        [[(P "p", false); (P "q", false); (P "s", false)];
         [(P "p", false); (P "q", false); (P "s", true)];
         [(P "p", false); (P "q", true); (P "s", false)];
         [(P "p", false); (P "q", true); (P "s", true)];
         [(P "p", true); (P "q", false); (P "s", false)];
         [(P "p", true); (P "q", false); (P "s", true)];
         [(P "p", true); (P "q", true); (P "s", false)];
         [(P "p", true); (P "q", true); (P "s", true)]]

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

[<Fact>]
let ``allsatvaluations should return the valuation for which subfn holds on atoms.``() = 
    let fm = !> "p /\ q"
    let atms = atoms fm
    let satvals = allsatvaluations (eval fm) (fun _ -> false) atms

    Assert.Equal(true, satvals[0] (P"p"))
    Assert.Equal(true,satvals[0] (P"q"))
    Assert.Equal(false,satvals[0] (P"x"))

[<Fact>]
let ``allsatvaluations should return the valuations satisfying a fm if applied to eval fm.``() = 
    let fm = !> @"(p /\ q) \/ (s /\ t)"
    let atms = atoms fm

    allsatvaluations (eval fm) (fun _ -> false) atms
    // graphs of all valuations satisfying fm
    |> List.map (fun v -> 
        atms
        |> List.map (fun a -> (a, v a))
    )
    |> should equal 
        [[(P "p", false); (P "q", false); (P "s", true); (P "t", true)];
         [(P "p", false); (P "q", true); (P "s", true); (P "t", true)];
         [(P "p", true); (P "q", false); (P "s", true); (P "t", true)];
         [(P "p", true); (P "q", true); (P "s", false); (P "t", false)];
         [(P "p", true); (P "q", true); (P "s", false); (P "t", true)];
         [(P "p", true); (P "q", true); (P "s", true); (P "t", false)];
         [(P "p", true); (P "q", true); (P "s", true); (P "t", true)]]

[<Fact>]
let ``dnf_by_truth_tables should return the input formula in dnf.``() = 
    !> @"p ==> q"
    |> dnf_by_truth_tables
    |> sprint_prop_formula
    |> should equal @"`~p /\ ~q \/ ~p /\ q \/ p /\ q`"

[<Fact>]
let ``dnf_by_truth_tables should return the input formula in a dnf equivalent.``() = 
    let fm = !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    let dnf = fm |> dnf_by_truth_tables
    let dnf_string = dnf |> sprint_prop_formula

    Assert.Equal(true, tautology(Iff(fm,dnf)))
    Assert.Equal(@"`~p /\ q /\ r \/ p /\ ~q /\ ~r \/ p /\ q /\ ~r`",dnf_string)

[<Fact>]
let ``distrib_naive should return the formula transformed via the distributive laws, if its immediate subformulas are in DNF.``() = 
    !> @"p /\ (q \/ r)" 
    |> distrib_naive
    |> sprint_prop_formula
    |> should equal @"`p /\ q \/ p /\ r`"

[<Fact>]
let ``distrib_naive should return the input formula unchanged if its immediate subformulas are not in DNF.``() = 
    !> @"p ==> q" 
    |> distrib_naive
    |> sprint_prop_formula
    |> should equal @"`p ==> q`"

[<Fact>]
let ``rawdnf should return a raw DNF if input is in NNF.``() = 
    !> @"p /\ (q \/ r) \/ s" 
    |> rawdnf
    |> sprint_prop_formula
    |> should equal @"`(p /\ q \/ p /\ r) \/ s`"

[<Fact>]
let ``rawdnf should return the input unchanged if it is not in NNF.``() = 
    !> @"p ==> q" 
    |> rawdnf
    |> sprint_prop_formula
    |> should equal @"`p ==> q`"

[<Fact>]
let ``rawdnf-3 book example.``() = 
    !> @"(p \/ q /\ r) /\ (~p \/ ~r)" 
    |> rawdnf
    |> sprint_prop_formula
    |> should equal @"`(p /\ ~p \/ (q /\ r) /\ ~p) \/ p /\ ~r \/ (q /\ r) /\ ~r`"

[<Fact>]
let ``distrib returns the set of the unions of first sets with the latter.``() = 
    distrib [[1;2];[2]] [[3];[4]]
    |> should equal [[1; 2; 3]; [1; 2; 4]; [2; 3]; [2; 4]]

[<Fact>]
let ``purednf should return a set of set dnf if input is in nnf.``() = 
    !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    |> purednf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`p`"; "`~p`"]; ["`p`"; "`~r`"]; ["`q`"; "`r`"; "`~p`"]; ["`q`"; "`r`"; "`~r`"]]

[<Fact>]
let ``purednf should a meaningless list of list of the input itself if it is not in nnf.``() = 
    !> "p ==> q"
    |> purednf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`p ==> q`"]]

[<Fact>]
let ``trivial returns true if there are complementary literals.``() = 
    trivial [!>"p";!>"~p"]
    |> should equal true

[<Fact>]
let ``trivial returns false if there aren't complementary literals.``() = 
    trivial [!>"p";!>"~q"]
    |> should equal false

[<Fact>]
let ``simpdnf should return a set of set dnf of the input.``() = 
    !> @"p ==> q" |> simpdnf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`q`"]; ["`~p`"]]

[<Fact>]
let ``dnf should return a dnf of any kind of formula.``() = 
    !> @"p ==> q" |> dnf
    |> sprint_prop_formula
    |> should equal @"`q \/ ~p`"

[<Fact>]
let ``dnf should return a dnf equivalent of the input.``() = 
    let fm = !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    let dnf = dnf fm
    Assert.Equal(@"`p /\ ~r \/ q /\ r /\ ~p`",dnf |> sprint_prop_formula)
    Assert.Equal(true, tautology(mk_iff fm dnf))

[<Fact>]
let ``purecnf should return a cnf set of sets.``() = 
    !> @"p ==> q" |> purecnf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`q`"; "`~p`"]]

[<Fact>]
let ``purecnf keeps possible superfluous and subsumed conjuncts.``() = 
    !> @"p \/ ~p" |> purecnf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`p`"; "`~p`"]]

[<Fact>]
let ``simpcnf should return a cnf set of sets.``() = 
    !> @"p ==> q" |> simpcnf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal [["`q`"; "`~p`"]]

[<Fact>]
let ``simpcnf removes possible superfluous and subsumed conjuncts.``() = 
    !> @"p \/ ~p" |> simpcnf
    |> List.map (fun xs -> 
        xs
        |> List.map sprint_prop_formula
    )
    |> should equal ([]:string list list)

[<Fact>]
let ``cnf should return a cnf equivalent of the input.``() = 
    let fm = !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    let cnf = cnf fm
    Assert.Equal(@"`(p \/ q) /\ (p \/ r) /\ (~p \/ ~r)`", cnf |> sprint_prop_formula)
    Assert.Equal(true, tautology(mk_iff fm cnf))