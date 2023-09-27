#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Fol
open FolAutomReas.Lib
open FolAutomReas.Unif

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

unify undefined [!!!"f(x,y)",!!!"f(y,x)"]

("x" |-> !!!"y")undefined