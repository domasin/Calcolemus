#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Partition
open Fol
open Order

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

lexord (>) [1;1;1;2] [1;1;1;1]  // true
lexord (>) [1;1;2;1] [1;1;1;1]  // true
lexord (>) [1;0;2;1] [1;1;1;1]  // false
lexord (>) [1;1;1;1] [1;1;1;1]  // false
lexord (>) [2;2;2;2] [1;1;1]    // false


lexord (>) [2;1;1;2] [2;1;1;1]  // true
lexord (>) [2;2;2;2] [1]

lexord (>) [2;1;1] [1;9;9]      // true
lexord (>) [2;1;1;1] [1;9;9]    // false
lexord (>) [2;2;1] [2;1;1]      // true
lexord (>) [2;2;1] [2;3;1]      // false

