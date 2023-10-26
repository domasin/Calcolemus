#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Lib.Function

4 |> non ((=) 2)

4 |> check ((=) 2)

funpow 2 (( ** ) 2.) 2.

4 |> funpow 2 ((/) 0)