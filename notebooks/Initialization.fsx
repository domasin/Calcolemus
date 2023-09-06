#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"
#r "nuget: MathNet.Numerics.FSharp"

open MathNet.Numerics

open FolAutomReas
open FolAutomReas.lib
open FolAutomReas.formulas
open FolAutomReas.prop

module FormulasAndTermsFormatter =
    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop>> ,"text/html")
    Formatter.Register<formula<prop>>((fun fm -> sprint_prop_formula fm), "text/html")

    // Formatter.SetPreferredMimeTypesFor(typeof<term> ,"text/html")
    // Formatter.Register<term>((fun tm -> print_term tm), "text/html")
    // Formatter.Register<term list>((fun xs -> 
    //                                 xs
    //                                 |> Seq.map (print_term)
    //                                 |> fun x -> sprintf "[%s]" (x |> String.concat ", ")),"text/html")
    // Formatter.Register<term * term>((fun (x,y) -> 
    //     sprintf "(%s, %s)" (x |> print_term) (y |> print_term)
    // ),"text/html")

    // Formatter.SetPreferredMimeTypesFor(typeof<thm> ,"text/html")
    // Formatter.Register<thm>((fun thm -> print_thm thm), "text/html")

    // Formatter.SetPreferredMimeTypesFor(typeof<Proof Location> ,"text/html")
    // Formatter.Register<Proof Location>((fun loc -> "$" + (latexStr loc) + "$"), "text/html")