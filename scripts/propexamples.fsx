#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Propexamples

// fsi.AddPrinter sprint_prop_formula

// eval the atom at the input valuations and convert to int
let toInt (v: prop -> bool) x =
    v (P x) |> System.Convert.ToInt32

// rippleshift

// let u,v,c,w = 
//     mk_index "u",
//     mk_index "v",
//     mk_index "c",
//     mk_index "w"

// // let rippleshift u v c z w n =
// //         ripplecarry0 u v (fun i -> if i = n then w (n - 1) else c (i + 1))
// //                         (fun i -> if i = 0 then z else w (i - 1)) n

// let rs = rippleshift u v c !>"z" w 2

// allsatvaluations (eval rs) (fun _ -> false) (atoms rs)
//  |> List.iteri (fun i v -> 
//     printfn "carry |   %A %A |" (toInt v "c_1") (toInt v "c_0")
//     printfn "---------------"
//     printfn "u     |   %A %A |" (toInt v "u_1") (toInt v "u_0")
//     printfn "v     |   %A %A |" (toInt v "v_1") (toInt v "v_0")
//     printfn "==============="
//     printfn "w     | %A %A   |" 
//         (toInt v "w_1")
//         (toInt v "w_0")
//     printfn "z     |     %A |" 
//         (toInt v "z")
//     printfn ""
//  )

// multiplier

let x,y,out = 
    mk_index "x",
    mk_index "y",
    mk_index "out"

let m i j = And(x i,y j)

let u,v = 
    mk_index2 "u",
    mk_index2 "v"

let ml = multiplier m u v out 3

ml 
|> atoms
|> List.map pname
|> List.sortDescending

/// checks if the variable x in the valuation v represent the binary number n.
let isIn v n x =  
    let max = (n |> String.length) - 1
    [0..max]
    |> List.forall (fun i -> 
        let bit = n |> seq |> Seq.item(max-i) |> string
        toInt v (sprintf "%s_%i" x i) = System.Int32.Parse(bit)
    )

// let myVal x = 
//     match x with
//     | P "x_0" -> false
//     | P "x_1" -> true
//     | P "x_2" -> true

// "x" |> isIn myVal "110"

let printIn v n x = 
    [0..n-1]
    |> List.sortDescending
    |> List.iter (fun i -> 
        try printf "%s" ((toInt v (sprintf "%s_%i" x i)) |> string)
        with _ -> printf " "
    )
    printfn ""

let decode x n v = 
    [0..n-1]
    |> Seq.sortDescending
    |> Seq.map (fun i -> 
        try sprintf "%s" ((toInt v (sprintf "%s_%i" x i)) |> string)
        with _ -> sprintf " "
    )
    |> System.String.Concat

let printEqSign n = 
    printf "%s" ("=" |> String.replicate n)
    printfn ""

let multiply (i1:int) (i2:int) = 
    let m1,m2 = 
        i1 |> string,
        i2 |> string
    let n = max (m1 |> String.length) (m2 |> String.length)
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
        "x" |> isIn v "110"
        && "y" |> isIn v "111"
    )
    |> List.head
    |> decode "out" 6

multiply "110" "111"

let mult2 (m1:int) (m2:int) = 
    let a = System.Convert.ToInt32(m1 |> string,2)
    let b = System.Convert.ToInt32(m2 |> string,2)
    System.Convert.ToString(a * b,2)

mult2 "110" "111"

allsatvaluations (eval ml) (fun _ -> false) (atoms ml)
|> List.filter (fun v -> 
    "x" |> isIn v "110"
    && "y" |> isIn v "111"
)
|> List.iteri (fun i v -> 
    "x" |> printIn v 6
    "y" |> printIn v 6
    printEqSign 6
    "out" |> printIn v 6
)

#r "nuget:FsCheck"

open FsCheck


let multiplyIsMult2 x y = (multiply x y) = mult2 x y

multiply "" ""

mult2 "" ""

Check.Quick multiplyIsMult2

// expected
//    110
//    111
//    ---
//    110
//   110
//   ----
//  10010
//  110
//  -----
// 101010