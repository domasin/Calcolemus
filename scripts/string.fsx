#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.String

let r1 = 
    "The Quick fox."
    |> explode

let r2 = 
    "The Quick fox."
    |> Seq.map string
    |> Seq.toList

r1 = r2

r2
|> String.concat ""

""
|> Seq.map string
|> Seq.toList