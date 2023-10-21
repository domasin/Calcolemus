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

// let rs = rippleshift u v c !>"z" w 2

// allsatvaluations (eval rs) (fun _ -> false) (atoms rs)
// |> List.iteri (fun i v -> 
//     printfn "u     |   %A %A |" (toInt v "u_1") (toInt v "u_0")
//     printfn "v     |   %A %A |" (toInt v "v_1") (toInt v "v_0")
//     printfn "==============="
//     printfn "w     | %A %A   |" 
//         (toInt v "w_1")
//         (toInt v "w_0")
//     printfn "z     |     %A |" 
//         (toInt v "z")
//     printfn ""
// )

// multiplier

// let x,y,out = 
//     mk_index "x",
//     mk_index "y",
//     mk_index "out"

// let m i j = And(x i,y j)

// let u,v = 
//     mk_index2 "u",
//     mk_index2 "v"

// let ml = multiplier m u v out 3


// // Checks if the variable x in the valuation v represent the binary number n.
// let isIn v n x =  
//     let max = (n |> String.length) - 1
//     [0..max]
//     |> List.forall (fun i -> 
//         let bit = n |> seq |> Seq.item(max-i) |> string
//         toInt v (sprintf "%s_%i" x i) = System.Int32.Parse(bit)
//     )

// let printIn v n x = 
//     [0..n-1]
//     |> List.sortDescending
//     |> List.iter (fun i -> 
//         try printf "%s" ((toInt v (sprintf "%s_%i" x i)) |> string)
//         with _ -> printf " "
//     )
//     printfn ""

// allsatvaluations (eval ml) (fun _ -> false) (atoms ml)
// |> List.filter (fun v -> 
//     "x" |> isIn v "110"
//     && "y" |> isIn v "111"
// )
// |> List.iteri (fun i v -> 
//     "x" |> printIn v 6
//     "y" |> printIn v 6
//     printfn "======"
//     "out" |> printIn v 6
// )

// // expected
// //    110
// //    111
// //    ---
// //    110
// //   110
// //   ----
// //  10010
// //  110
// //  -----
// // 101010

#r "nuget:FsCheck"

bitlength 0

System.Convert.ToString(0,2).Length

open FsCheck

let bitlengthIsCorrect x = 
    if x <= 0 then true else
    bitlength x = System.Convert.ToString(x,2).Length

Check.Quick bitlengthIsCorrect

// let bitIsCorrect n x = 
//     let checkFunction (n:int) (x:int) = 
//         let s = System.Convert.ToString(x,2)
//         // printfn "%s" s
//         match s[n] with
//         | '0' -> false
//         | '1' -> true
//         | _ -> failwith ""
//     if 0 > n || n > s.Length || x < 0 then true else
//     bit n x = checkFunction n x

// let n,x = 2,5
// let s = System.Convert.ToString(x,2)

// bit 2 5

// Check.Quick bitIsCorrect

congruent_to (mk_index "x") 10 4
// `~x_0 /\ x_1 /\ ~x_2 /\ x_3`
//   0      1       0      1
// to be read in reverse order 1010

[0..10]
|> List.map (fun i -> 
    let fm = congruent_to (mk_index "x") i (bitlength i)
    i,
    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
        atoms fm
        |> Seq.map (v >> System.Convert.ToInt32 >> string)
        |> Seq.rev
        |> System.String.Concat
    )
    |> System.String.Concat
)

let cng = congruent_to (mk_index "x") 4 3

prime 2
// `~(((out_0 <=> x_0 /\ y_0) /\ ~out_1) /\ ~out_0 /\ out_1)`
|> tautology

prime 2
|> allvaluations
|> List.map (fun v -> 
    atoms (prime 2)
    |> List.map v
)

mk_adder_test 2 1
|> tautology

let x, y, c0, c1, s0, s1, s, c = 
    mk_index "x",
    mk_index "y",
    mk_index "c0",
    mk_index "c1",
    mk_index "s0",
    mk_index "s1",
    mk_index "s",
    mk_index "c"

let cs = carryselect x y c0 c1 s0 s1 c s 2 2
 
//  // eval the atom at the input valuations and convert to int
// let toInt (v: prop -> bool) x =
//     v (P x) |> System.Convert.ToInt32

allsatvaluations (eval cs) (fun _ -> false) (atoms cs)
|> List.filter (fun v -> 
    (toInt v "x_1") = 0 && (toInt v "x_0") = 1      // x = 01
    && (toInt v "y_1") = 1 && (toInt v "y_0") = 1   // y = 11
)
|> List.iteri (fun i v -> 
    printfn "carry |   %A %A |" (toInt v "c_1") (toInt v "c_0")
    printfn "---------------"
    printfn "x     |   %A %A |" (toInt v "x_1") (toInt v "x_0")
    printfn "y     |   %A %A |" (toInt v "y_1") (toInt v "y_0")
    printfn "==============="
    printfn "sum   | %A %A %A |" 
        (toInt v "c_2")
        (toInt v "s_1")
        (toInt v "s_0")
    printfn ""
)