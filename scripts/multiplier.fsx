#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Propexamples

// fsi.AddPrinter sprint_prop_formula

/// eval the atom at the input valuations and convert to int
let toInt (v: prop -> bool) x =
    v (P x) |> System.Convert.ToInt32

/// checks if the variable x in the valuation v represent the binary number n.
let isIn v n x =  
    let max = (n |> String.length) - 1
    [0..max]
    |> List.forall (fun i -> 
        let bit = n |> seq |> Seq.item(max-i) |> string
        toInt v (sprintf "%s_%i" x i) = System.Int32.Parse(bit)
    )

let decode x n v = 
    [0..n-1]
    |> Seq.sortDescending
    |> Seq.map (fun i -> 
        try sprintf "%s" ((toInt v (sprintf "%s_%i" x i)) |> string)
        with _ -> sprintf " "
    )
    |> System.String.Concat

let multiply (i1:int) (i2:int) = 
    if i1 <= 0 || i2 <= 0 then "0" else
    let m1 = i1 |> string
    let m2 = i2 |> string
    let m1Len = m1 |> String.length
    let m2Len = m2 |> String.length
    let n = max m1Len m2Len
    let x,y,out = 
        mk_index "x",
        mk_index "y",
        mk_index "out"

    let m i j = And(x i,y j)

    let u,v = 
        mk_index2 "u",
        mk_index2 "v"
    let ml = multiplier m u v out n

    allsatvaluations (eval ml) (fun _ -> false) (atoms ml)
    |> List.filter (fun v -> 
        "x" |> isIn v m1
        && "y" |> isIn v m2
    )
    |> List.head
    |> decode "out" (m1Len + m2Len)

let mult2 m1 m2 = 
    if m1 <= 0 || m2 <= 0 then "0" else
    let a = System.Convert.ToInt32(m1 |> string,2)
    let b = System.Convert.ToInt32(m2 |> string,2)
    System.Convert.ToString(a * b,2)

#r "nuget:FsCheck"

open FsCheck

let multiplyIsMult2 x y = (multiply x y) = mult2 x y

multiply 1 1

mult2 1 1

Check.Quick multiplyIsMult2