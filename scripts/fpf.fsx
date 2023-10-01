#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib



apply (("y" |-> 2)(("x" |-> 1)undefined)) 
<| "y"

apply (("y" |-> 2)(("x" |-> 1)undefined)) 
<| "z"

let rec apply_listd l d x =
    match l with
    | [] -> d x
    | (a, b) :: tl ->
        let c = compare x a
        if c = 0 then b
        elif c > 0 then apply_listd tl d x
        else d x

apply_listd ["x",1] (function "y" -> 2) "x"

let rec look d x fpf =
    let k = hash x
    match fpf with
    | Leaf (h, l) when h = k ->
        apply_listd l d x
    | Branch (p, b, l, r) when (k ^^^ p) &&& (b - 1) = 0 ->
        if k &&& b = 0 then l else r
        |> look d x
    | _ -> d x

("w" |-> 4)(("z" |-> 3)(("y" |-> 2)(("x" |-> 1)undefined)))
|> look (fun x -> 2) "w"

let f = ("w" |-> 4)(("z" |-> 3)(("y" |-> 2)(("x" |-> 1)undefined)))




("y" |-> 2)(("x" |-> 1)undefined) 
|> foldl (fun a x y -> a + y) 0 

("y" |-> 2)(("x" |-> 1)undefined) 
|> graph 

("y" |-> 2)(("x" |-> 1)undefined) 
|> dom 




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




