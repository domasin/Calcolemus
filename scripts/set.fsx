#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Set

let rec canonical lis =
    match lis with
    | x :: (y :: _ as rest) ->
        compare x y < 0
        && canonical rest
    | _ -> true

#time
// [1..50000000] too much
[1..10000000]
|> List.sort
|> List.distinct
// Real: 00:00:05.771, CPU: 00:00:05.470, GC gen0: 167, gen1: 90, gen2: 14
#time

#time
[1..50000000]
|> canonical
// Real: 00:00:14.181, CPU: 00:00:13.130, GC gen0: 652, gen1: 143, gen2: 14
#time

#time
[1..10000000] |> Set.ofList
// Real: 00:00:07.894, CPU: 00:00:08.230, GC gen0: 1562, gen1: 80, gen2: 5
#time

let prefix = [10..100000]
let xs = [1;2;3]@prefix
let ys = [1;5;4;3]@prefix

#time
// union xs ys
// fails with stack overflow with original implementation
#time

#time
Set.union (xs |> Set.ofList) (ys |> Set.ofList)
|> Set.toList
// Real: 00:00:00.145, CPU: 00:00:00.149, GC gen0: 26, gen1: 1, gen2: 0
#time

union [1;2;3] [1;5;4;3]

union [1;1;1] [1;2;3;2] 