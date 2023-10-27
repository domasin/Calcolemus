#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Fol
open Calcolemus.Formulas

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

!!! "sqrt(1 - power(cos(x + y,2"

parse "x + y < z"

parse "x + y"

parse "x"