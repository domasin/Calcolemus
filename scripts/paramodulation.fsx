#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Eqelim
open Fol
open Lib.Search
open Lib.Fpf
open Clause
open Equal
open Meson
open Paramodulation

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

paramodulate !!>["f(x) = f(x)"] !!>["P(x)";"0 = 1"]

paramodulate !!>["C";"S(0) = 1"] !!>["P(S(x))";"D"]

([],!!>>[
    ["x = x"]; 
    // ["f(f(x)) = f(x)"]; 
    ["f(f_y(x)) = x"];
    ["~f(c_x) = c_x"]
])
|> paraloop 
   