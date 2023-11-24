#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Partition
open Fol
open Order

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

weight ["g";"f"] ("f",1) ("g",2) // true
weight ["f";"f"] ("f",2) ("f",1) // true
weight ["f";"g"] ("f",1) ("g",2) // false
weight ["f";"f"] ("f",1) ("f",2) // false

let w : string * int -> string * int -> bool = 
    weight ["0"; "1"; "g";"f"]

w ("1",0) ("0",0)
w ("f",1) ("1",0)

lpo_gt (weight ["0"; "1"]) !!!"f(0,1)" !!!"f(0,0)"

lpo_gt (weight ["0"; "1"]) !!!"h(0,1)" !!!"0"

lpo_gt (weight ["g";"f"]) !!!"f(1)" !!!"g(1)"

lpo_gt (weight []) !!!"f(1,1)" !!!"f(1)" // true
lpo_gt (weight []) !!!"f(1)" !!!"f(1,1)" // false

lpo_gt (weight ["0";"1";"f"]) !!!"f(1)" !!!"f(f(0))"

lpo_gt (weight []) !!!"x*(y+z)" !!!"x*(z+y)+z"
lpo_gt (weight [])  !!!"x*(z+y)+z" !!!"x*(y+z)"
lpo_gt (weight ["*";"+"]) !!!"+(x,+(y,z))" !!!"+(*(x,(z+y)),z)"

lpo_gt (weight ["0";"1"]) !!!"f(1)" !!!"f(0)"

lpo_gt (weight []) !!!"f(1)" !!!"f(1)"

(!!!"f(1)", !!!"f(1)")
||> lpo_gt (weight [])
    

lpo_gt w !!!"1" !!!"0"

lpo_gt w !!!"h(0,1)" !!!"0" // true
lpo_gt w !!!"f(1,1,1,1,1)" !!!"g(1,1,1,1,1)" // ok, true

lpo_gt w !!!"f(1,1,1,1,1)" !!!"f(1,1,1,1,1)" 

lpo_gt w !!!"f(1,1,1,1,1)" !!!"f(1,1,1,1)" // true
lpo_gt w !!!"f(1,1,1,1)" !!!"f(1,1,1,1,1)" // false

lpo_ge w !!!"f(1,1,1,1,1)" !!!"f(1,1,1,1)" // true
lpo_ge w !!!"f(1,1,1,1)" !!!"f(1,1,1,1,1)" // false

