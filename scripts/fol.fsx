#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Fol
open Calcolemus.Formulas
open Calcolemus.Lib.Fpf

// fsi.AddPrinter sprint_term
// fsi.AddPrinter sprint_fol_formula

!! "P(x,f(z)) ==> Q(x)"
|> onformula (function 
    | Var x -> Var (x + "_1") 
    | tm -> tm
)

is_const_name "nil" // evaluates to true
is_const_name "123" // evaluates to true
is_const_name "x"   // evaluates to false

Fn("sqrt",[Fn("-",[Fn("1",[]);
                        Fn("power",[Fn("cos",[Fn("+",[Var "x"; Var "y"]);
                                             Fn("2",[])])])])])
|> sprint_term

!!! "sqrt(1 - power(cos(x + y,2"

parse "x + y < z"

parse "x + y"

parse "x"

!! "forall x y. exists z. x < z /\ y < z"
|> sprint_fol_formula

!!! "13" 
|> termval bool_interp undefined

!!! "0" |> termval bool_interp undefined    // evaluates to false
!!! "0" |> termval (mod_interp 3) undefined // evaluates to 0

!!! "0" |> termval (mod_interp 3) undefined                 // evaluates to 0
!!! "1" |> termval (mod_interp 3) undefined                 // evaluates to 1
!!! "1 + 1" |> termval (mod_interp 3) undefined             // evaluates to 2
!!! "1 + 1 + 1" |> termval (mod_interp 3) undefined         // evaluates to 0
!!! "1 + 1 + 1 + 1" |> termval (mod_interp 3) undefined     // evaluates to 1
!!! "1 + 1 + 1 + 1 + 1" |> termval (mod_interp 3) undefined // evaluates to 2
// ...

let fm = !! @"forall x. (x = 0) \/ (x = 1)" 

holds bool_interp undefined fm    // evaluates to true
holds (mod_interp 2) undefined fm // evaluates to true
holds (mod_interp 3) undefined fm // evaluates to false