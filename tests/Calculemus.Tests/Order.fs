// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Order

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus
open Fol
open Order

[<Fact>]
let ``termsize should return the sum of the number of variables and function symbols in a term.``() = 
    termsize !!!"f(f(f(x)))"
    |> should equal 4

[<Fact>]
let ``lexord should return true, if the input list are equally long and reading from left to right the items in the first list are not less then the second's and there is at leas one greater;otherwise false.``() = 

    Assert.Equal(true, lexord (>) [1;1;1;2] [1;1;1;1]) 
    Assert.Equal(true, lexord (>) [1;1;2;1] [1;1;1;1]) 
    Assert.Equal(false,lexord (>) [1;0;2;1] [1;1;1;1])
    Assert.Equal(false,lexord (>) [1;1;1;1] [1;1;1;1])
    Assert.Equal(false,lexord (>) [2;2;2;2] [1;1;1])  

[<Fact>]
let ``weight should return true, if f comes after g in lis, or if f and g are the same symbol but n > m; otherwise, false.``() = 

    Assert.Equal(true, weight ["g";"f"] ("f",1) ("g",2))
    Assert.Equal(true, weight ["f";"f"] ("f",2) ("f",1))
    Assert.Equal(false, weight ["f";"g"] ("f",1) ("g",2))
    Assert.Equal(false, weight ["f";"f"] ("f",1) ("f",2))

[<Fact>]
let ``lpo_gt should return true if the inputs are function terms with the same function symbol but the arguments sequence of the first is greater than that of the second.``() = 
    lpo_gt (weight ["0"; "1"]) !!!"f(0,1)" !!!"f(0,0)"
    |> shouldEqual true

[<Fact>]
let ``lpo_gt should return true if the first term is a function term and its arguments are all greater or equal than the second term.``() = 
    lpo_gt (weight ["0"; "1"]) !!!"h(0,1)" !!!"0"
    |> shouldEqual true

[<Fact>]
let ``lpo_gt should return true if the inputs are function terms and the function symbol of the first is greater than the second based on the precedence defined by the weighting function (without further analysis of their arguments).``() = 
    lpo_gt (weight ["g";"f"]) !!!"f(1)" !!!"g(1)" 
    |> shouldEqual true

[<Fact>]
let ``lpo_gt should return false if the term are equal.``() = 
    lpo_gt (weight []) !!!"f(1)" !!!"f(1)"
    |> shouldEqual false

[<Fact>]
let ``lpo_ge should return true if the inputs are function terms with the same function symbol but the arguments sequence of the first is greater than that of the second.``() = 
    lpo_ge (weight ["0"; "1"]) !!!"f(0,1)" !!!"f(0,0)"
    |> shouldEqual true

[<Fact>]
let ``lpo_ge should return true if the first term is a function term and its arguments are all greater or equal than the second term.``() = 
    lpo_ge (weight ["0"; "1"]) !!!"h(0,1)" !!!"0"
    |> shouldEqual true

[<Fact>]
let ``lpo_ge should return true if the inputs are function terms and the function symbol of the first is greater than the second based on the precedence defined by the weighting function (without further analysis of their arguments).``() = 
    lpo_ge (weight ["g";"f"]) !!!"f(1)" !!!"g(1)" 
    |> shouldEqual true

[<Fact>]
let ``lpo_ge should return true if the term are equal.``() = 
    lpo_ge (weight []) !!!"f(1)" !!!"f(1)"
    |> shouldEqual true