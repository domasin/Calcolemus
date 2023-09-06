module Tests.Num

open Xunit
open FsUnit.Xunit

open FolAutomReas.lib.num

[<Fact>]
let ``gcd of 35 and -77 is -7``() = 
    gcd_num (Int 35) (Int(-77))
    |> should equal (Int -7)