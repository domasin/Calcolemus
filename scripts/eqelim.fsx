#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Eqelim
open Fol
open Lib.Search
open Lib.Fpf
open Clause
open Equal
open Meson

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

!! @"(forall x. f(x) ==> g(x)) /\
(exists x. f(x)) /\
(forall x y. g(x) /\ g(y) ==> x = y)
==> forall y. g(y) ==> f(y)"
|> emeson


!! @"(forall x y z. x * (y * z) = (x * y) * z) /\
(forall x. e * x = x) /\
(forall x. i(x) * x = e)
==> forall x. x * i(x) = e"
|> bmeson

!!"~ x = x"
|> bpuremeson

!!"~ x = x"
|> equalitize
|> meson


!!>>[["P(1,1)"]]
|> brand

!!>["f(0,x) = y"]
|> emodify ["x";"y";"z"]

!!>["(x * y) * z = x * (y * z)"]
|> emodify ["x";"y";"z"]

!!>["(x * y) * z = x * (y * z)"]
|> modify_E

!!!"f(0,1)"
|> replacet ((!!!"0" |-> !!!"1")undefined)

!!"f(0,1) = 0"
|> replace ((!!!"0" |-> !!!"1")undefined)

!!"R(0,1)"
|> find_nvsubterm

!!"~x = f(0)"
|> find_nvsubterm

!!"~x = f(0) /\ P(x)"
|> find_nvsubterm

!!"~x = f(y)"
|> find_nvsubterm

[3;4]
|> tryfind (fun x -> if x % 2 = 0 then x |> string else failwith "")

[!!!"x";!!!"0"]
|> tryfind find_nestnonvar

!!"R(0,1) /\ x = 1"
|> find_nvsubterm

!!!"f(0,1)"
|> find_nestnonvar

!!!"f(x,y)"
|> find_nestnonvar

!!!"x"
|> find_nestnonvar

!!!"f(x)"
|> is_nonvar

!!!"x"
|> is_nonvar

!!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
|> modify_S
|> List.map (List.map sprint_fol_formula)

!!>["s1 = t1"; "s2 = t2"; "s3 = t3"; "~s1 = t1"; "~s2 = t2"]
|> modify_T
|> List.map sprint_fol_formula