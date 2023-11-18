// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Equal

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Fol
open Equal

[<Fact>]
let ``is_eq should return true if the formula is an equation.``() = 
    !!"x = y"
    |> is_eq
    |> shouldEqual true

[<Fact>]
let ``is_eq should return formula if the formula is not an equation.``() = 
    !!"P(x) <=> Q(x)"
    |> is_eq
    |> shouldEqual false

[<Fact>]
let ``mk_eq should return the equation of the input terms.``() = 
    mk_eq !!!"f(x)" !!!"y"
    |> sprint_fol_formula
    |> shouldEqual "`f(x) = y`"

[<Fact>]
let ``dest_eq should return the pair of the LHS and RHS terms of the equation.``() = 
    dest_eq !!"f(x) = y"
    |> fun (l,r) -> l |> sprint_term, r |> sprint_term
    |> shouldEqual ("``f(x)``","``y``")

[<Fact>]
let ``dest_eq should fail with 'dest_eq: not an equation' when the input formula is not an equation.``() = 
    (fun () -> 
        dest_eq !!"P(x) <=> Q(y)"
        |> ignore
    )
    |> should (throwWithMessage "dest_eq: not an equation") typeof<System.Exception>

[<Fact>]
let ``lhs should return the LHS term of the equation.``() = 
    lhs !!"f(x) = y"
    |> sprint_term
    |> shouldEqual "``f(x)``"

[<Fact>]
let ``lhs should fail with 'dest_eq: not an equation' when the input formula is not an equation.``() = 
    (fun () -> 
        lhs !!"P(x) <=> Q(y)"
        |> ignore
    )
    |> should (throwWithMessage "dest_eq: not an equation") typeof<System.Exception>

[<Fact>]
let ``rhs should return the LHS term of the equation.``() = 
    rhs !!"f(x) = y"
    |> sprint_term
    |> shouldEqual "``y``"

[<Fact>]
let ``rhs should fail with 'dest_eq: not an equation' when the input formula is not an equation.``() = 
    (fun () -> 
        rhs !!"P(x) <=> Q(y)"
        |> ignore
    )
    |> should (throwWithMessage "dest_eq: not an equation") typeof<System.Exception>

[<Fact>]
let ``predicates should return the list of pairs name, arity of the predicates in the input formula.``() = 
    predicates !!"x + 1 > 0 /\ f(z) > g(z,i)"
    |> shouldEqual [(">", 2)]

[<Fact>]
let ``function_congruence should return the list with the congruence axiom for the given function as the only element.``() = 
    function_congruence ("f",2)
    |> List.map sprint_fol_formula
    |> shouldEqual ["`forall x1 x2 y1 y2. x1 = y1 /\ x2 = y2 ==> f(x1,x2) = f(y1,y2)`"]

[<Fact>]
let ``predicate_congruence should return the list with the congruence axiom for the given predicate as the only element.``() = 
    predicate_congruence ("P",3)
    |> List.map sprint_fol_formula
    |> shouldEqual ["`forall x1 x2 x3 y1 y2 y3. x1 = y1 /\ x2 = y2 /\ x3 = y3 ==> P(x1,x2,x3) ==> P(y1,y2,y3)`"]

[<Fact>]
let ``equalitize should return the implication of the input formula from its equality axioms, if the input formula involves the equality symbol.``() = 
    
    !! @"(forall x. f(x) ==> g(x)) /\
         (exists x. f(x)) /\
         (forall x y. g(x) /\ g(y) ==> x = y)
         ==> forall y. g(y) ==> f(y)"
    |> equalitize
    |> sprint_fol_formula
    |> shouldEqual "`(forall x. x = x) /\ (forall x y z. x = y /\ x = z ==> y = z) /\ (forall x1 y1. x1 = y1 ==> f(x1) ==> f(y1)) /\ (forall x1 y1. x1 = y1 ==> g(x1) ==> g(y1)) ==> (forall x. f(x) ==> g(x)) /\ (exists x. f(x)) /\ (forall x y. g(x) /\ g(y) ==> x = y) ==> (forall y. g(y) ==> f(y))`"

[<Fact>]
let ``equalitize should return the input formula itself if it doesn't involve the equality symbol.``() = 
    
    equalitize !!"P(x) <=> Q(y)"
    |> equalitize
    |> sprint_fol_formula
    |> shouldEqual "`P(x) <=> Q(y)`"