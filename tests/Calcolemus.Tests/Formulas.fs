// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calcolemus.Tests.Formulas

open Xunit
open FsUnit.Xunit

open Calcolemus.Formulas

[<Fact>]
let ``mk_and should return the conjunction of the input formulas.``() = 
    mk_and (Atom "p") (Atom "q")
    |> should equal (And (Atom "p", Atom "q"))

[<Fact>]
let ``mk_or should return the disjunction of the input formulas.``() = 
    mk_or (Atom "p") (Atom "q")
    |> should equal (Or (Atom "p", Atom "q"))

[<Fact>]
let ``mk_imp should return the implication of p on q.``() = 
    mk_imp (Atom "p") (Atom "q")
    |> should equal (Imp (Atom "p", Atom "q"))

[<Fact>]
let ``mk_iff should return the equivalence of p with q.``() = 
    mk_iff (Atom "p") (Atom "q")
    |> should equal (Iff (Atom "p", Atom "q"))

[<Fact>]
let ``mk_forall should return the p formula universally quantified on x.``() = 
    mk_forall "x" (Atom "p")
    |> should equal (Forall ("x", Atom "p"))

[<Fact>]
let ``mk_exists should return the p formula existentially quantified on x.``() = 
    mk_exists "x" (Atom "p")
    |> should equal (Exists ("x", Atom "p"))

[<Fact>]
let ``dest_iff should return the pair of its members if the input formula is a logical equivalence.``() = 
    Iff (Atom "p", Atom "q") 
    |> dest_iff
    |> should equal (Atom "p", Atom "q")

[<Fact>]
let ``dest_iff should fail with 'dest_iff' if the input formula is not a logical equivalence.``() = 
    (fun () -> 
        Imp (Atom "p", Atom "q") 
        |> dest_iff
        |> ignore
    )
    |> should (throwWithMessage "dest_iff") typeof<System.Exception>

[<Fact>]
let ``dest_and should return the conjuncts if the input formula is a conjunction.``() = 
    And (Atom "p", Atom "q") 
    |> dest_and
    |> should equal (Atom "p", Atom "q")

[<Fact>]
let ``dest_and should fail with 'dest_and' if the input formula is not a conjunction.``() = 
    (fun () -> 
        Imp (Atom "p", Atom "q") 
        |> dest_and
        |> ignore
    )
    |> should (throwWithMessage "dest_and") typeof<System.Exception>

[<Fact>]
let ``conjuncts should return the list of all the conjuncts if the input formula is a (repeated) conjunction.``() = 
    And (And (Atom "p", Atom "q"), Atom "r") 
    |> conjuncts
    |> should equal [Atom "p"; Atom "q"; Atom "r"]

[<Fact>]
let ``conjuncts should return just the list of the input unchanged if it is not a conjunction.``() = 
    Imp (Atom "a", Atom "b")
    |> conjuncts
    |> should equal [Imp (Atom "a", Atom "b")]

let ``dest_or should return the disjuncts if the input formula is a disjunction.``() = 
    Or (Atom "p", Atom "q") 
    |> dest_or
    |> should equal (Atom "p", Atom "q")

[<Fact>]
let ``dest_or should fail with 'dest_or' if the input formula is not a disjunction.``() = 
    (fun () -> 
        Imp (Atom "p", Atom "q") 
        |> dest_or
        |> ignore
    )
    |> should (throwWithMessage "dest_or") typeof<System.Exception>

[<Fact>]
let ``disjuncts should return the list of all the disjuncts if the input formula is a (repeated) disjunction.``() = 
    Or (Or (Atom "p", Atom "q"), Atom "r") 
    |> disjuncts
    |> should equal [Atom "p"; Atom "q"; Atom "r"]

[<Fact>]
let ``disjuncts should return just the list of the input unchanged if it is not a disjunction.``() = 
    Imp (Atom "a", Atom "b")
    |> disjuncts
    |> should equal [Imp (Atom "a", Atom "b")]

let ``dest_imp should return the pair of the antecedent and consequent if the input formula is an implication.``() = 
    Imp (Atom "p", Atom "q") 
    |> dest_imp
    |> should equal (Atom "p", Atom "q")

[<Fact>]
let ``dest_imp should fail with 'dest_imp' if the input formula is not an implication.``() = 
    (fun () -> 
        And (Atom "p", Atom "q") 
        |> dest_imp
        |> ignore
    )
    |> should (throwWithMessage "dest_imp") typeof<System.Exception>

let ``antecedent should return the antecedent if the input formula is an implication.``() = 
    Imp (Atom "p", Atom "q") 
    |> antecedent
    |> should equal (Atom "p")

[<Fact>]
let ``antecedent should fail with 'dest_imp' if the input formula is not an implication.``() = 
    (fun () -> 
        And (Atom "p", Atom "q") 
        |> antecedent
        |> ignore
    )
    |> should (throwWithMessage "dest_imp") typeof<System.Exception>

let ``consequent should return the consequent if the input formula is an implication.``() = 
    Imp (Atom "p", Atom "q") 
    |> consequent
    |> should equal (Atom "q")

[<Fact>]
let ``consequent should fail with 'dest_imp' if the input formula is not an implication.``() = 
    (fun () -> 
        And (Atom "p", Atom "q") 
        |> consequent
        |> ignore
    )
    |> should (throwWithMessage "dest_imp") typeof<System.Exception>

let ``strip_quant should return a pair with the list of the quantified variables and body if the input is a universally quantified formula.``() = 
    Forall ("y", Forall ("x", Atom "p")) 
    |> strip_quant
    |> should equal (["y"; "x"], Atom "p")

let ``strip_quant should return a pair with an empty list and the input formula unchanged if it is not a universally quantified formula.``() = 
    Atom "p"
    |> strip_quant
    |> should equal (([]:string list), Atom "p")

let ``overatoms should iterate input function over all the atoms of the formula.``() = 
    And (Atom 1, Atom 2)
    |> fun fm -> overatoms (fun acc x -> x + acc) fm 0
    |> should equal 3

let ``onatoms should return the formula with the atoms remapped by the function.``() = 
    And (Atom 1, Atom 2)
    |> onatoms (fun x -> Atom (x * 5))
    |> should equal 3

let ``atom_union should collect atoms' attributes remapped based on function.``() = 
   And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
    |> atom_union (fun x -> [x])
    |> should equal [1; 2; 3; 4]

let ``atom_union should remove duplicates.``() = 
   And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
    |> atom_union (fun x -> [1..x])
    |> should equal [1; 2; 3; 4]