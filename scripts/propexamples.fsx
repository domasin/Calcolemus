#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Propexamples

// fsi.AddPrinter sprint_prop_formula

ramsey 2 2 3

ramsey 3 3 4

let fm = sum (!>"x") (!>"y") (!>"z")
// evaluates to `(x <=> ~y) <=> ~z`

printfn "-----------------"
printfn "| x | y | z | c |"
printfn "-----------------"

// for each valuation:
allvaluations fm
|> List.iter (fun v -> 
    // for each atom:
    atoms fm
    |> List.iter (fun atm -> 
        // print the truth-value of the atom in the valuation;
        printf "| %A " (v atm |> System.Convert.ToInt32)
    )
    // and print the truth-value of the formula in the valuation.
    printfn "| %A |" 
        (eval fm v |> System.Convert.ToInt32)
)
printfn "-----------------"

let fa = fa (!>"x") (!>"y") (!>"z") (!>"s") (!>"c")
// `(s <=> (x <=> ~y) <=> ~z) /\ (c <=> x /\ y \/ (x \/ y) /\ z)`

printfn "---------------------"
printfn "| x | y | z | c | s |"
printfn "---------------------"
 
(allsatvaluations (eval fa) (fun _ -> false) (atoms fa))
|> List.iter (fun v -> 
    printfn "| %A | %A | %A | %A | %A |" 
        (v (P "x") |> System.Convert.ToInt32)
        (v (P "y") |> System.Convert.ToInt32)
        (v (P "z") |> System.Convert.ToInt32)
        (v (P "c") |> System.Convert.ToInt32)
        (v (P "s") |> System.Convert.ToInt32)
)
printfn "---------------------"
                
let x, y, s, c = 
    mk_index "x",
    mk_index "y",
    mk_index "s",
    mk_index "c"

// 2-bit ripple carry adder
let rc = ripplecarry x y c s 2
// ((s_0 <=> (x_0 <=> ~y_0) <=> ~c_0) /\ (c_1 <=> x_0 /\ y_0 \/ 
// (x_0 \/ y_0) /\ c_0)) /\ 
// (s_1 <=> (x_1 <=> ~y_1) <=> ~c_1) /\ (c_2 <=> x_1 /\ y_1 \/ 
// (x_1 \/ y_1) /\ c_1)

// eval the atom at the input valuations and convert to int
let toInt (v: prop -> bool) x =
    v (P x) |> System.Convert.ToInt32

allsatvaluations (eval rc) (fun _ -> false) (atoms rc)
|> List.iteri (fun i v -> 
    printfn "carry    |   %A %A |" (toInt v "c_1") (toInt v "c_0")
    printfn "------------------"
    printfn "addend 1 |   %A %A |" (toInt v "x_1") (toInt v "x_0")
    printfn "addend 2 |   %A %A |" (toInt v "y_1") (toInt v "y_0")
    printfn "=================="
    printfn "sum      | %A %A %A |" 
        (toInt v "c_2")
        (toInt v "s_1")
        (toInt v "s_0")
    printfn ""
)

let rc0 = ripplecarry0 x y c s 2
// ((s_0 <=> x_0 <=> ~y_0) /\ 
//      (c_1 <=> x_0 /\ y_0)) /\ 
// (s_1 <=> (x_1 <=> ~y_1) <=> ~c_1) /\ 
//      (c_2 <=> x_1 /\ y_1 \/ (x_1 \/ y_1) /\ c_1)

allsatvaluations (eval rc0) (fun _ -> false) (atoms rc0)
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

let rc1 = ripplecarry1 x y c s 2
// ((s_0 <=> ~(x_0 <=> ~y_0)) /\ 
//    (c_1 <=> x_0 /\ y_0 \/ x_0 \/ y_0)) /\ 
// (s_1 <=> (x_1 <=> ~y_1) <=> ~c_1) /\ 
//    (c_2 <=> x_1 /\ y_1 \/ (x_1 \/ y_1) /\ c_1)

allsatvaluations (eval rc1) (fun _ -> false) (atoms rc1)
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

mux !>"true" !>"in0" !>"in1"
|> print_truthtable

mux !>"false" !>"in0" !>"in1"
|> print_truthtable

let fmMux = mux !>"true" !>"in0" !>"in1"

allvaluations fmMux
|> List.map (fun v -> 
    atoms fmMux
    |> List.map v
)

offset 1 (mk_index "x") 2

let u, v, c, w = 
    mk_index "u", // fst addend indexed variables
    mk_index "v", // snd addend indexed variables
    mk_index "c", // carry indexed variables
                  // z=lowest bit result variable
    mk_index "w"  // other bits indexed variables

let rs = rippleshift u v c !>"z" w 2
// ((z <=> u_0 <=> ~v_0) /\ (c_2 <=> u_0 /\ v_0)) /\ (w_0 <=> (u_1 <=> ~v_1) <=> ~c_2) /\ (w_1 <=> u_1 /\ v_1 \/ (u_1 \/ v_1) /\ c_2)

allsatvaluations (eval rs) (fun _ -> false) (atoms rs)
|> List.filter (fun v -> 
    (toInt v "u_1") = 0 && (toInt v "u_0") = 1      // x = 01
    && (toInt v "v_1") = 1 && (toInt v "v_0") = 1   // y = 11
)
|> List.iteri (fun i v -> 
    printfn "carry |   %A   |" (toInt v "c_2")
    printfn "-----------------"
    printfn "u     |   %A %A |" (toInt v "u_1") (toInt v "u_0")
    printfn "v     |   %A %A |" (toInt v "v_1") (toInt v "v_0")
    printfn "==============="
    printfn "sum   | %A %A %A |" 
        (toInt v "w_1")
        (toInt v "w_0")
        (toInt v "z")
    printfn ""
)

let x, u, v, out = 
    mk_index2 "x", 
    mk_index2 "u", 
    mk_index2 "v", 
    mk_index "out"  

let ml = multiplier x u v out 2

allsatvaluations (eval ml) (fun _ -> false) (atoms ml)
|> List.filter (fun v -> 
    (toInt v "x_0_1") = 0 && (toInt v "x_0_0") = 1      // x_0 = 01
    && (toInt v "x_1_1") = 1 && (toInt v "x_1_0") = 1   // x_1 = 11
)
|> List.iteri (fun i v -> 
    printfn "x0    |   %A %A |" (toInt v "x_0_1") (toInt v "x_0_0")
    printfn "x1    |   %A %A |" (toInt v "x_1_1") (toInt v "x_1_0")
    printfn "==============="
    printfn "carry |   %A   |" (toInt v "v_0_1")
    printfn "-----------------"
    printfn "u_0   |   %A %A |" (toInt v "u_0_1") (toInt v "u_0_1")
    printfn "u_1   |   %A %A |" (toInt v "u_1_1") (toInt v "u_1_0")
    printfn "==============="
    printfn "res   | %A %A %A |" 
        (toInt v "out_2")
        (toInt v "out_1")
        (toInt v "out_0")
    printfn ""
)

// expected
// x0    |   0 1 |
// x1    |   1 1 |
// ---------------
//           0 1
// 		   0 1
// ===============
// 		   0 1 1