#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus.Formulas
open Calculemus.Fol
open Calculemus.Herbrand
open Calculemus.Tableaux
open Calculemus.Pelletier

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

let compare fm =
    prawitz fm, davisputnam fm

compare p19

compare p20

compare p24

compare p39

compare p42

compare p43

compare p44

compare p59

compare p60