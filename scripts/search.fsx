#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Lib.Search

let f1 = fun n -> if n % 2 = 0 then string n else failwith "f"
let f2 = fun n -> if n > 3 then string n else failwith "f"
let f3 = fun n -> if n > 3 then string n else raise (System.ArgumentException(""))

[1;2;3] |> tryfind f1
[1;2;3] |> tryfind f2
[] |> tryfind f1
[1;2;3;4] |> mapfilter f1
[1;2;3] |> mapfilter f2
[1;2;3] |> mapfilter f3

let opt f = fun x -> try let r = f x in Some r with _ -> None

[1;2;3] |> List.pick (opt f1)
[] |> List.pick (opt f1)
[1;2;3] |> List.pick (opt f2)

optimize (>) (( * ) -1) [-1;2;3]
optimize (<) (( * ) -1) [-1;2;3]