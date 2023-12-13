// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Decidable

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Lib.Fpf
open Formulas
open Fol
open Skolem
open Decidable

[<Fact>]
let ``aedecide should return true on a valid AE formula.``() = 
    !! @"(forall x. P(1,x,x)) /\ (forall x. P(x,x,1)) /\
    (forall u v w x y z.
    P(x,y,u) /\ P(y,z,w) ==> (P(x,w,v) <=> P(u,z,v)))
    ==> forall a b c. P(a,b,c) ==> P(b,a,c)"
    |> aedecide
    |> shouldEqual true

[<Fact>]
let ``aedecide should fail with 'Not decidable' if the input isn't an AE formula.``() = 
    (fun () -> 
        !! @"forall x. f(x) = 0"
        |> aedecide
        |> ignore
    )
    |> should (throwWithMessage "Not decidable") typeof<System.Exception>

[<Fact>]
let ``separate should return the existential formula of the conjunction of the cjs in which x is free conjuncted with the cjs in which x is not.``() = 
    !!>["P(x)"; "Q(y)"; "T(y) /\ R(x,y)"; "S(z,w) ==> Q(i)"]
    |> separate "x"
    |> sprint_fol_formula
    |> shouldEqual "`(exists x. P(x) /\ T(y) /\ R(x,y)) /\ Q(y) /\ (S(z,w) ==> Q(i))`"

[<Fact>]
let ``pushquant should return the formula <c>exists x. p</c> transformed into an equivalent with the scope of the quantifier reduced.``() = 
    !!"P(x) ==> forall y. Q(y)"
    |> pushquant "x"
    |> sprint_fol_formula
    |> shouldEqual @"`(exists x. ~P(x)) \/ (forall y. Q(y))`"

[<Fact>]
let ``miniscope should return a formula equivalent to the input with the scope of quantifiers minimized.``() = 
    miniscope(nnf !!"exists y. forall x. P(y) ==> P(x)")
    |> sprint_fol_formula
    |> shouldEqual @"`(exists y. ~P(y)) \/ (forall x. P(x))`"

[<Fact>]
let ``wang should return true on a valid formula that after miniscoping is in AE.``() = 
    wang Pelletier.p20
    |> shouldEqual true

[<Fact>]
let ``wang should fail with 'Not decidable' if the input even after applying miniscoping is not in AE.``() = 
    (fun () -> 
        !! @"forall x. f(x) = 0"
        |> wang
        |> ignore
    )
    |> should (throwWithMessage "Not decidable") typeof<System.Exception>

[<Fact>]
let ``atom should return the atom p(x).``() = 
    atom "P" "x"
    |> sprint_fol_formula
    |> shouldEqual "`P(x)`"

[<Fact>]
let ``premiss_A should return an A premiss.``() = 
    premiss_A ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`forall x. P(x) ==> S(x)`"

[<Fact>]
let ``premiss_E should return an E premiss.``() = 
    premiss_E ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`forall x. P(x) ==> ~S(x)`"

[<Fact>]
let ``premiss_I should return an I premiss.``() = 
    premiss_I ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`exists x. P(x) /\ S(x)`"

[<Fact>]
let ``premiss_O should return an O premiss.``() = 
    premiss_O ("P", "S")
    |> sprint_fol_formula
    |> shouldEqual "`exists x. P(x) /\ ~S(x)`"

[<Fact>]
let ``anglicize_premiss should return an English reading of the input syllogism premiss.``() = 
    premiss_A ("P", "S")
    |> anglicize_premiss 
    |> shouldEqual "all P are S"

[<Fact>]
let ``anglicize_premiss should fail if applied to a formula that is not a syllogism premiss.``() = 
    (fun () -> 
        !!"P(x)"
        |> anglicize_premiss 
        |> ignore
    )
    |> should (throwWithMessage "anglicize_premiss: not a syllogism premiss (Parameter 'fm')") typeof<System.ArgumentException>

[<Fact>]
let ``anglicize_syllogism should return an English reading of the input syllogism.``() = 
    premiss_A ("M", "P")
    |> fun x -> mk_and x (premiss_A ("S", "M"))
    |> fun x -> mk_imp x (premiss_A ("S", "P"))
    |> anglicize_syllogism
    |> shouldEqual "If all M are P and all S are M, then all S are P"

[<Fact>]
let ``anglicize_syllogism should fail if applied to a formula that is not a syllogism.``() = 
    (fun () -> 
        !!"P(x)"
        |> anglicize_syllogism 
        |> ignore
    )
    |> should (throwWithMessage "anglicize_syllogism: not a syllogism (Parameter 'fm')") typeof<System.ArgumentException>

[<Fact>]
let ``alltuples should return all tuples of the given size with member chosen from the given list.``() = 
    [1;2;3;4;5;6;7]
    |> alltuples 3
    |> List.take 77
    |> shouldEqual 
        [
            [1; 1; 1]; [1; 1; 2]; [1; 1; 3]; [1; 1; 4]; [1; 1; 5]; [1; 1; 6]; [1; 1; 7];
            [1; 2; 1]; [1; 2; 2]; [1; 2; 3]; [1; 2; 4]; [1; 2; 5]; [1; 2; 6]; [1; 2; 7];
            [1; 3; 1]; [1; 3; 2]; [1; 3; 3]; [1; 3; 4]; [1; 3; 5]; [1; 3; 6]; [1; 3; 7];
            [1; 4; 1]; [1; 4; 2]; [1; 4; 3]; [1; 4; 4]; [1; 4; 5]; [1; 4; 6]; [1; 4; 7];
            [1; 5; 1]; [1; 5; 2]; [1; 5; 3]; [1; 5; 4]; [1; 5; 5]; [1; 5; 6]; [1; 5; 7];
            [1; 6; 1]; [1; 6; 2]; [1; 6; 3]; [1; 6; 4]; [1; 6; 5]; [1; 6; 6]; [1; 6; 7];
            [1; 7; 1]; [1; 7; 2]; [1; 7; 3]; [1; 7; 4]; [1; 7; 5]; [1; 7; 6]; [1; 7; 7];
            [2; 1; 1]; [2; 1; 2]; [2; 1; 3]; [2; 1; 4]; [2; 1; 5]; [2; 1; 6]; [2; 1; 7];
            [2; 2; 1]; [2; 2; 2]; [2; 2; 3]; [2; 2; 4]; [2; 2; 5]; [2; 2; 6]; [2; 2; 7];
            [2; 3; 1]; [2; 3; 2]; [2; 3; 3]; [2; 3; 4]; [2; 3; 5]; [2; 3; 6]; [2; 3; 7];
            [2; 4; 1]; [2; 4; 2]; [2; 4; 3]; [2; 4; 4]; [2; 4; 5]; [2; 4; 6]; [2; 4; 7]]

[<Fact>]
let ``allmappings should return all possible functions out of the given domain into the given range and undefined outside.``() = 

    let dom,ran = [1..3],[1..3]

    allmappings dom ran
    |> List.mapi (fun i f -> i, dom |> List.map f)
    |> shouldEqual 
        [(0, [1; 1; 1]); (1, [1; 1; 2]); (2, [1; 1; 3]); (3, [1; 2; 1]);
         (4, [1; 2; 2]); (5, [1; 2; 3]); (6, [1; 3; 1]); (7, [1; 3; 2]);
         (8, [1; 3; 3]); (9, [2; 1; 1]); (10, [2; 1; 2]); (11, [2; 1; 3]);
         (12, [2; 2; 1]); (13, [2; 2; 2]); (14, [2; 2; 3]); (15, [2; 3; 1]);
         (16, [2; 3; 2]); (17, [2; 3; 3]); (18, [3; 1; 1]); (19, [3; 1; 2]);
         (20, [3; 1; 3]); (21, [3; 2; 1]); (22, [3; 2; 2]); (23, [3; 2; 3]);
         (24, [3; 3; 1]); (25, [3; 3; 2]); (26, [3; 3; 3])]

[<Fact>]
let ``alldepmappings should return all interpretations of the input function symbols.``() = 

    let dom = [1..3]

    let functionSymbols = [("g",2)]
    let functions = allfunctions [1..3]
    
    alldepmappings functionSymbols functions
    |> List.mapi (fun i f -> 
        i, 
        dom 
        |> alltuples 2
        |> List.map (fun args -> args,f "g" args))
    |> List.take 3
    |> shouldEqual 
        [(0,
             [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
             ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 1)]);
         (1,
             [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
             ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 2)]);
         (2,
             [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
             ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 3)])]

[<Fact>]
let ``allfunctions should return the functions from dom to dom with arity n.``() = 
    let dom = [1..3]

    allfunctions dom 2
    |> List.mapi (fun i f -> 
        i, 
        dom 
        |> alltuples 2
        |> List.map (fun args -> args, f args)
    )
    |> List.take 3
    |> shouldEqual 
        [(0,
          [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
           ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 1)]);
         (1,
          [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
           ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 2)]);
         (2,
          [([1; 1], 1); ([1; 2], 1); ([1; 3], 1); ([2; 1], 1); ([2; 2], 1);
           ([2; 3], 1); ([3; 1], 1); ([3; 2], 1); ([3; 3], 3)])]

[<Fact>]
let ``allpredicates should return the possibile predicates in dom with arity n.``() = 
    let dom = [1..3]

    allpredicates dom 2
    |> List.mapi (fun i f -> 
        i, 
        dom 
        |> alltuples 2
        |> List.map (fun args -> args, f args)
    )
    |> List.take 3
    |> shouldEqual 
        [(0,
          [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
           ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], false);
           ([3; 3], false)]);
         (1,
          [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
           ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], false);
           ([3; 3], true)]);
         (2,
          [([1; 1], false); ([1; 2], false); ([1; 3], false); ([2; 1], false);
           ([2; 2], false); ([2; 3], false); ([3; 1], false); ([3; 2], true);
           ([3; 3], false)])]

[<Fact>]
let ``decide_finite should return true if the input formula holds in all interpretations of the given size.``() = 
    let dom = [1..3]

    !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    |> decide_finite 2 
    |> shouldEqual true

[<Fact>]
let ``limmeson should return the triple with the current instantiation, the depth reached and the number of variables renamed when the goal is solvable.``() = 
    !! @"~R(x,x) /\ (forall x y. R(x,y) \/ R(y,x))"
    |> limmeson 2
    |> fun (inst, n, k) -> (inst |> graph, n, k)
    |> shouldEqual 
        ([("_0", !!!"_1"); ("_1", !!!"_2")], 0, 3)

[<Fact>]
let ``limited_meson should return the triple with the current instantiation, the depth reached and the number of variables renamed returned on the subproblems when the goal is solvable.``() = 
    !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    |> limited_meson 2
    |> List.map (fun (inst, n, k) -> (inst |> graph, n, k))
    |> shouldEqual 
        [([("_0", Fn ("c_x", [])); ("_1", Fn ("c_x", []))], 0, 2)]

[<Fact>]
let ``decide_fmp should return true, if the input formula is valid.``() = 
    !! @"(forall x y. R(x,y) \/ R(y,x)) ==> forall x. R(x,x)"
    |> decide_fmp
    |> shouldEqual true

[<Fact>]
let ``decide_fmp should return false, if the input formula is not valid.``() = 
    !! @"(forall x y z. R(x,y) /\ R(y,z) ==> R(x,z)) ==> forall x. R(x,x)"
    |> decide_fmp
    |> shouldEqual false

[<Fact>]
let ``decide_monadic should return true, if the monadic formula is valid.``() = 
    !! @"((exists x. forall y. P(x) <=> P(y)) <=>
          ((exists x. Q(x)) <=> (forall y. Q(y)))) <=>
         ((exists x. forall y. Q(x) <=> Q(y)) <=>
          ((exists x. P(x)) <=> (forall y. P(y))))"
    |> decide_fmp
    |> shouldEqual true