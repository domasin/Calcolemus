#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"
#r "nuget: MathNet.Numerics.FSharp"

open MathNet.Numerics

open Calcolemus
open Calcolemus.Lib
open Calcolemus.Intro
open Calcolemus.Formulas
open Calcolemus.Prop
open Calcolemus.Fol

module FormulasAndTermsFormatter =

    // string
    Formatter.SetPreferredMimeTypesFor(typeof<string list> ,"text/plain")
    Formatter.Register<string list>((fun xs -> 
                                    xs
                                    |> Seq.map (id)
                                    |> fun x -> sprintf "[\"%s\"]" (x |> String.concat "; ")),"text/plain")

    // expression
    Formatter.SetPreferredMimeTypesFor(typeof<expression> ,"text/plain")
    Formatter.Register<expression>((fun expr -> expr |> sprint_exp), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<expression list> ,"text/plain")
    Formatter.Register<expression list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_exp)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")
    
    Formatter.SetPreferredMimeTypesFor(typeof<expression * expression> ,"text/plain")
    Formatter.Register<expression * expression>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_exp) (y |> sprint_exp)),"text/plain")
    
    // prop formula
    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop>> ,"text/plain")
    Formatter.Register<formula<prop>>((fun fm -> sprint_prop_formula fm), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop> list> ,"text/plain")
    Formatter.Register<formula<prop> list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_prop_formula)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop> * formula<prop>> ,"text/plain")
    Formatter.Register<formula<prop> * formula<prop>>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_prop_formula) (y |> sprint_prop_formula)),"text/plain")

    // fol terms
    Formatter.SetPreferredMimeTypesFor(typeof<term> ,"text/plain")
    Formatter.Register<term>((fun tm -> tm |> sprint_term), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<term list> ,"text/plain")
    Formatter.Register<term list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_term)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<term * term> ,"text/plain")
    Formatter.Register<term * term>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_term) (y |> sprint_term)),"text/plain")

    // fol formula
    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol>> ,"text/plain")
    Formatter.Register<formula<fol>>((fun fm -> sprint_fol_formula fm), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol> list> ,"text/plain")
    Formatter.Register<formula<fol> list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_fol_formula)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<(formula<fol> list) list> ,"text/plain")

    let printFun = 
        (fun ys -> 
                ys 
                |> Seq.map (fun xs -> 
                    xs 
                    |> Seq.map (sprint_fol_formula) 
                    |> Seq.fold (fun acc x -> 
                        if acc = "" then "[" + x else acc + ";" + x) ""
                    |> fun x -> x + "]"
                )
                |> Seq.fold (fun acc x -> 
                    if acc = "" then "[" + x else acc + ";" + x) ""
                    |> fun x -> x + "]"
    )

    Formatter.Register<(formula<fol> list) list>((printFun),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol> * formula<fol>> ,"text/plain")
    Formatter.Register<formula<fol> * formula<fol>>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_fol_formula) (y |> sprint_fol_formula)),"text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<term> ,"text/plain")
    // Formatter.Register<term>((fun tm -> print_term tm), "text/plain")
    
    // Formatter.Register<term * term>((fun (x,y) -> 
    //     sprintf "(%s, %s)" (x |> print_term) (y |> print_term)
    // ),"text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<thm> ,"text/plain")
    // Formatter.Register<thm>((fun thm -> print_thm thm), "text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<Proof Location> ,"text/plain")
    // Formatter.Register<Proof Location>((fun loc -> "$" + (latexStr loc) + "$"), "text/plain")