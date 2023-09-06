module Tests.Repetitions

open Xunit
open FsUnit.Xunit

open FolAutomReas.lib.repetitions

[<Fact>]
let ``uniq [1;1;3;2;2] returns [1;3;2].``() = 
    uniq [1;1;3;2;2]
    |> should equal [1;3;2]

[<Fact>]
let ``repetitions [1;1;3;2;2] returns [(1,2);(3,1);(2,2)].``() = 
    repetitions [1;1;3;2;2] 
    |> should equal [(1,2);(3,1);(2,2)]