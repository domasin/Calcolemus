#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus.Lib.Fpf
open Calculemus.Formulas
open Calculemus.Prop

let sprint_bigint (bi:bigint) = sprintf "%OI" bi

type def = func<prop formula, prop formula * prop formula>

let printDefinitions (def:def) = 
    def
    |> graph
    |> List.map (fun (a,(b1,b2)) -> 
        sprintf "%s |-> (%s, %s)" 
            (a |> sprint_prop_formula) 
            (b1 |> sprint_prop_formula)
            (b2 |> sprint_prop_formula)
    )
    |> String.concat "\n"

fsi.AddPrinter sprint_prop_formula
fsi.AddPrinter sprint_bigint
fsi.AddPrinter printDefinitions