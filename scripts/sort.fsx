#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Sort

merge (<) [1;3;7] [4;6;5;2;]

let compareEntries (n1: int, s1: string) (n2: int, s2: string) =
    let c = compare s1.Length s2.Length
    if c <> 0 then c else
    compare n1 n2

compare 1 2

let boolCompareEntries (n1: int, s1: string) (n2: int, s2: string) =
    let c = compare s1.Length s2.Length

    if c < 0 then 
        true
    else if compare n1 n2 < 0 then
        true
    else
        false

let input = [ (0,"aa"); (1,"bbb"); (2,"cc"); (3,"dd") ]

input |> List.sortWith compareEntries
input |> sort boolCompareEntries

sort (<) [3;1;4;1;5;9;2;6;5;3;5] 
List.sortWith compare [3;1;4;1;5;9;2;6;5;3;5] 

let comparer ord = fun x y ->  
    match ord x y with
    | true -> -1
    | false -> 1

[3;1;4;1;5;9;2;6;5;3;5] 
|> List.sortWith (comparer (<)) 

increasing List.length [1] [1;2]

increasing List.length [1;2] [1]