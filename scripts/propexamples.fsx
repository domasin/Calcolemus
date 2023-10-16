#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Propexamples
open FolAutomReas.Lib.List

// fsi.AddPrinter sprint_prop_formula

ramsey 3 3 4

allpairs (fun x y -> (x,y)) [(False:prop formula);True] [False;True]
|> List.map (fun (x,y) -> (x,y), eval (halfsum x y) (fun _ -> false))

let to01 fm = 
    match eval fm (fun _ -> false) with
    | false  -> 0
    | true  -> 1

printfn "-------------"
printfn "| x | y | s |"
printfn "-------------"
for x in [False;True] do 
    for y in [False;True] do 
        printfn "| %i | %i | %i |" 
            (x |> to01) 
            (y |> to01) 
            (halfsum x y |> to01)
printfn "-------------"

printfn "-------------"
printfn "| x | y | s |"
printfn "-------------"
for x in [False;True] do 
    for y in [False;True] do 
        printfn "| %i | %i | %i |" 
            (x |> to01) 
            (y |> to01) 
            (halfcarry x y |> to01)
printfn "-------------"

let fm = ha (True:prop formula) True False True
// `(false <=> true <=> ~true) /\ (true <=> true /\ true)`
tautology(fm)

printfn "-----------------"
printfn "| x | y | c | s |"
printfn "-----------------"
for x in [False;True] do 
    for y in [False;True] do 
        for c in [False;True] do 
            for s in [False;True] do 
                if tautology(ha x y s c) then 
                    printfn "| %i | %i | %i | %i |" 
                        (x |> to01) (y |> to01) (c |> to01) (s |> to01)
printfn "-----------------"

[for x in [(False:prop formula);True] do 
    for y in [False;True] do 
        for c in [False;True] do 
            for s in [False;True] do 
                if tautology(ha x y s c) then 
                    (x,y,c,s)]