#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Meson
open Formulas
open Fol

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

contrapositives !!>["P";"Q";"~R"]
contrapositives !!>["~P";"~Q";"~R"]
