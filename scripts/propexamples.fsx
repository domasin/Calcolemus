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

// carry

printfn "-----------------"
printfn "| x | y | z | c |"
printfn "-----------------"
for x in [False;True] do 
    for y in [False;True] do 
        for z in [False;True] do 
            printfn "| %i | %i | %i | %i |" 
                (x |> to01) 
                (y |> to01) 
                (z |> to01) 
                (carry x y z |> to01)
printfn "-----------------"

[for x in [(False:prop formula);True] do 
    for y in [False;True] do 
        for z in [False;True] do 
            (x,y,z,eval (carry x y z) (fun _ -> false))]

// sum

printfn "-----------------"
printfn "| x | y | z | s |"
printfn "-----------------"
for x in [False;True] do 
    for y in [False;True] do 
        for z in [False;True] do 
            printfn "| %i | %i | %i | %i |" 
                (x |> to01) 
                (y |> to01) 
                (z |> to01) 
                (sum x y z |> to01)
printfn "-----------------"

printfn "---------------------"
printfn "| x | y | z | c | s |"
printfn "---------------------"
for x in [False;True] do 
    for y in [False;True] do 
        for z in [False;True] do 
            for c in [False;True] do 
                for s in [False;True] do 
                    if tautology(fa x y z s c) then 
                        printfn "| %i | %i | %i | %i | %i |" 
                            (x |> to01) (y |> to01) (z |> to01) 
                            (c |> to01) (s |> to01)
printfn "---------------------"

[for x in [(False:prop formula);True] do 
    for y in [False;True] do 
        for z in [False;True] do 
            for c in [False;True] do 
                for s in [False;True] do 
                    if tautology(fa x y z s c) then 
                        (x,y,z,c,s)]

let conjoin2 f l = 
    l
    |> List.map f
    |> list_conj

// Constructs a conjunction of the formulas obtained by applying a function f to the elements of a list l
conjoin Atom [1;2;3]


let ripplecarry x y c out n =
    [0..(n - 1)]
    |> List.map (fun i -> fa (x i) (y i) (c i) (out i) (c (i + 1)))
    |> list_conj
                

mk_index "x" 3

let fa0 = fa (!>"x_0") (!>"y_0") (!>"c_0") (!>"s_0") (!>"c_1")
let atms = atoms fa0

let satvals = allsatvaluations (eval fa0) (fun _ -> false) atms
// graphs of all valuations satisfying fm
// |> List.map (fun v -> 
//     atms
//     |> List.map (fun a -> (a, v a))
// )

// let fm = ha (True:prop formula) True False True
// eval 

ha (True:prop formula) True False True
|> fun fm -> 
    let atms = atoms fm
    (allsatvaluations (eval fm) (fun _ -> false) atms), atms
|> fun (vv,atms) -> 
    vv
    |> List.map (fun v -> 
        atms
        |> List.map (fun a -> (a, v a))
    )

ha (True:prop formula) True (!>"s") True
|> fun fm -> 
    let atms = atoms fm
    (allsatvaluations (eval fm) (fun _ -> false) atms), atms
|> fun (vv,atms) -> 
    vv
    |> List.map (fun v -> 
        atms
        |> List.map (fun a -> (a, v a))
    )



printfn "-----------------"
printfn "| x | y | c | s |"
printfn "-----------------"

ha (!>"x") (!>"y") (!>"s") (!>"c")
|> fun fm -> 
    let atms = atoms fm
    (allsatvaluations (eval fm) (fun _ -> false) atms)
|> List.iter (fun v -> 
    printfn "| %A | %A | %A | %A |" 
        (to01 v (P "x"))
        (to01 v (P "y"))
        (to01 v (P "c"))
        (to01 v (P "s"))
)
printfn "-----------------"

let to01 v x = 
    match v x with
    | false -> 0
    | true -> 1

let fm = ha (!>"x") (!>"y") (!>"s") (!>"c")

(allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
|> List.map (fun v -> 
        (to01 v (P "x")),
        (to01 v (P "y")),
        (to01 v (P "c")),
        (to01 v (P "s"))
)

printfn "-----------------"
printfn "| x | y | c | s |"
printfn "-----------------"

(allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
|> List.iter (fun v -> 
    printfn "| %A | %A | %A | %A |" 
        (to01 v (P "x"))
        (to01 v (P "y"))
        (to01 v (P "c"))
        (to01 v (P "s"))
)
printfn "-----------------"