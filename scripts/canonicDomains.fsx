#r "nuget: FolAutomReas,1.0.3"

open FolAutomReas.Lib.FPF
open FolAutomReas.Formulas
open FolAutomReas.Fol
open FolAutomReas.Herbrand

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

/// Propositional valuation of atomic formulas
let d = function 
    | Atom (R ("P", [Fn("0",[])])) -> true 
    | Atom (R ("P", [Fn("2",[])])) -> false
    | Atom (R ("Q", [Var "y"])) -> true
    | Atom (R ("Q", [Var "x"])) -> false
    | _ -> false

/// Canonical Interpretation based on d
let M_d = 
    [Fn("0",[]); Fn("2",[]); Var "x"; Var "y"]  // Domain
    , fun s xs -> Fn(s,xs)                      // Functions interpretation
    , fun s xs -> d(Atom (R(s,xs)))             // Predicates interpretation

/// Identity variable valuation
let VarId = 
    ("y" |=> Var "y")
    |> ("x" |-> Var "x")

// Lemma 3:14
["0";"2";"x";"y"]
|> List.map (fun x -> 
    let t = x |> parset
    let tInterp = termval M_d VarId t
    t, tInterp)
|> List.forall (fun (t, tInterp) -> t = tInterp)

// Theorem 3.15
["P(0)"; "P(2)"; "P(x)"; "P(y)"; "Q(x)"; "Q(y)";"P(0) ==> P(2)"]
|> List.map (fun s -> 
    let fm = s |> parse
    let propInterp = fm |> pholds d
    let folCanoniInterp = fm |> holds M_d VarId
    s, propInterp, folCanoniInterp
    )
|> List.forall (fun (s,p,f) -> p = f)

