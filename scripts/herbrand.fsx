#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib
open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!!"0 + x = x"
!|"0"
|> fun x -> x.ToString()


groundterms [!|"0";!|"1"] ["f",1;"g",1] 1

groundterms [!|"0";!|"1"] ["f",1;"g",1] 0

groundterms [!|"0";] ["f",1] 1

groundtuples [!|"0";] ["f",1] 1 1

groundtuples [!|"0";] ["f",1] 1 2
