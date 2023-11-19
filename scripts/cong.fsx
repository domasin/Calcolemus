#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Partition
open Fol
open Cong

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

congruent 
    (equate (!!!"2",!!!"4")unequal) 
    (!!!"f(4)", !!!"f(2)")

congruent 
    (equate (!!!"2",!!!"4")unequal) 
    (!!!"f(4)", !!!"f(3)")

(unequal,undefined)
|> emerge (!!!"0",!!!"1") 
|> fun (Partition f,pfn) -> graph f, graph pfn