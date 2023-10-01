#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib

let f = ("w" |-> 4)(("z" |-> 3)(("y" |-> 2)(("x" |-> 1)undefined)))

("y" |-> 2)(("x" |-> 1)undefined) 
|> foldl (fun a x y -> a + y) 0 

mapf (fun x -> x * 10) (("x" |-> 1)undefined)

is_undefined undefined
// val it: bool = true

is_undefined (("x" |-> 1)undefined)
// val it: bool = false



// \(x \mapsto 1, y \mapsto 2, z \mapsto 3, w \mapsto 4\)

let rec foldl_list f a l =
    match l with
    | [] -> a
    | (x, y) :: t ->
        foldl_list f (f a x y) t

Branch (0, 1, Leaf (2131634918, [("x", 1)]), Leaf (-1524676365, [("y", 2)]))
// |> foldl (fun a x y -> (x, y) :: a) [] 
|> function Branch (_, _, l, r) ->  foldl (fun a x y -> (x, y) :: a) (foldl (fun a x y -> (x, y) :: a) [] l) r

let rec foldl f a t =
    match t with
    | Empty -> a
    | Leaf (_, l) -> foldl_list f a l
    | Branch (_, _, l, r) ->  foldl f (foldl f a l) r


let graph f =
    foldl (fun a x y -> (x, y) :: a) [] f
    |> setify




