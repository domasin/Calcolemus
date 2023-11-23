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
let ``lexord should return true, if the input list are equally long and reading from left to right the items in the first list are not less then the second's and there is at leas one greater;otherwise false..``() = 

    Assert.Equal(true, lexord (>) [1;1;1;2] [1;1;1;1]) 
    Assert.Equal(true, lexord (>) [1;1;2;1] [1;1;1;1]) 
    Assert.Equal(false,lexord (>) [1;0;2;1] [1;1;1;1])
    Assert.Equal(false,lexord (>) [1;1;1;1] [1;1;1;1])
    Assert.Equal(false,lexord (>) [2;2;2;2] [1;1;1])  