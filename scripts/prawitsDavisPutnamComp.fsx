#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand
open FolAutomReas.Tableaux
open FolAutomReas.Pelletier

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