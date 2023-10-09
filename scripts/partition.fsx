#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Fpf
open FolAutomReas.Lib.Partition

let (Partition f as ptn) = 
    unequal
    |> equate (2,1) 
    |> equate (3,1)
    |> equate (4,1) 
    |> equate (6,5) 
    |> equate (7,5)

unequal
|> equate (2,1)
|> fun (Partition f) -> graph f

terminus ptn 4
tryterminus ptn 8

canonize ptn 4
canonize ptn 8

equated ptn

f |> graph

canonize ptn 4
terminus ptn 1
terminus ptn 4

terminus ptn 7
tryterminus ptn 7

canonize ptn 7

equate (1,5) ptn
|> fun (Partition f) -> graph f

equivalent ptn 3 4 // true
equivalent ptn 3 5 // false
equivalent ptn 5 6 // true

canonize ptn 3 // 1
canonize ptn 6 // 5

equated ptn
|> List.iter (
    fun x -> 
    let tAndS = tryterminus ptn x
    printfn "%A = %A" x tAndS
)

equated ptn
|> List.iter (
    fun x -> 
    let tAndS = terminus ptn x
    printfn "%A = %A" x tAndS
)

tryterminus ptn 5 // (5, 2)
tryterminus ptn 6 // (5, 2)

equated ptn


undefined
|> fun x -> (0 |-> Terminal (0,1))x
|> fun x -> (1 |-> Nonterminal (1))x
|> fun x -> (2 |-> Nonterminal (2))x
|> Partition
|> canonize
|> fun f -> f 1

