#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Function

4 |> non ((=) 2)

4 |> check ((=) 2)

funpow 2 (( ** ) 2.) 2.

4 |> funpow 2 ((/) 0)