#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Formulas

mk_and (Atom "a") (Atom "b")

mk_forall "x" (Atom "a")

Iff (Atom "p", Atom "q") |> dest_iff

And (Atom "a", Atom "b")

Imp (Atom "a", Atom "b")
|> antecedent

And (And (Atom "p", Atom "q"), Atom "r")
|> dest_and

And (And (Atom "p", Atom "q"), Atom "r")
|> conjuncts

Imp (Atom "a", Atom "b")
|> conjuncts

And (Atom "a", Atom "b")
|> antecedent

Forall ("y", Forall ("x", Atom "p"))
|> strip_quant

Atom "p"
|> strip_quant

Atom "p"
|> dest_and

And (Atom 1, Atom 2)
|> onatoms (fun x -> Atom (x * 5))

And (Atom 1, Atom 2)
|> atom_union (fun x -> [x])

And (Atom "p", Atom "q")
|> fun fm -> overatoms (fun acc x -> acc + ";" + x) fm ""

And (Atom 1, Atom 2)
|> fun fm -> overatoms (fun acc x -> acc + x) fm 0

And (Atom "1", Atom "2")
|> fun fm -> overatoms (fun acc x -> x + acc) fm ""

And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
|> atom_union (fun x -> [1..x])

And (Atom 1, And (Atom 2, And (Atom 3, Atom 4)))
|> atom_union (fun x -> [x])