(**
---
title: Resolution
category: Automated Proving
categoryindex: 2
index: 1
---
*)

(*** hide ***)
#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

(**
# Resolution
*)

open Calcolemus
open Lib.Time
open Fol
open Resolution

(*** hide ***)
let time f fm = 
    printfn "%s" (fm |> sprint_fol_formula)
    printfn ""
    fm |> time f

(**
##Propositional Logic
*)

Pelletier.p1
|> time presolution
(*** include-fsi-merged-output ***)

