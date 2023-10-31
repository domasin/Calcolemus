#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Formulas
open Calcolemus.Fol
open Calcolemus.Herbrand
open Calcolemus.Tableaux
open Calcolemus.Pelletier

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