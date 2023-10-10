#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Parser

let rec parseIntList i = 
    match parseInt i with
    | e1, "," :: i1 ->
        let e2, i2 = parseIntList i1
        e1@e2, i2
    | x -> x
and parseInt i = 
    match i with
    | [] -> failwith "eof"
    | tok :: i1 -> [int tok], i1

parseIntList ["11";",";"12"] // ([11; 12], [])

make_parser parseIntList "11,12"

