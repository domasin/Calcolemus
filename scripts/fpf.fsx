#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib

valmod 1 100 id 1

valmod 1 100 id 2

valmod 1 100 (undef) 1

((undef 1):int)


"x" |=> 1 |> graph

fpf [1;2;3] [1;4;9] |> graph

let h = hash "x"
let k = hash "y"

let t1 = Leaf (h, [("x", 1)])
let t2 = Leaf (k, [("y", 2)])

let zp = h ^^^ k

let newbranch p1 t1 p2 t2 =
    let zp = p1 ^^^ p2
    let b = zp &&& -zp
    let p = p1 &&& (b - 1)
    if p1 &&& b = 0 then Branch (p, b, t1, t2)
    else Branch (p, b, t2, t1)

newbranch h t1 k t2

("x" |-> 1)undefined

("x" |-> 1)undefined |> graph
|> (=) ["x",1]

("y" |-> 2)(("x" |-> 1)undefined)
|> undefine "x"

("y" |-> 2)(("x" |-> 1)undefined)
|> undefine "z"

let input = ("y" |-> 2)(("x" |-> 1)undefined)

input
|> undefine "z"
|> (=) input

("y" |-> 2)(("x" |-> 1)undefined)
|> fun fpf -> tryapplyd fpf "x" 9

("y" |-> 2)(("x" |-> 1)undefined)
|> fun fpf -> tryapplyd fpf "z" 9

tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "x"

tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "z"

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

apply_listd ["x",1] (function "y" -> 2) "y"

compare "a" "x"

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

("w" |-> 4)(("z" |-> 3)(("y" |-> 2)(("x" |-> 1)undefined)))
("z" |-> 4)(("x" |-> 3)(("y" |-> 2)(("w" |-> 1)undefined)))


// if (k ^^^ p) &&& (b - 1) = 0 then exists
// if k &&& b = 0 then l else r

// &&& = 1, if both 1
// ^^^ = 1, if operands are unequal


111 &&& 000
111 ^^^ 110

7 ^^^ 6

System.Convert.ToString(3,2)
System.Convert.ToString(7,2)
System.Convert.ToString(4,2)

hash "x" // 644577120

644577120

// "y" |> exists 13 16 // true
// "x" |> exists 13 16 // false

// Branch (p, b, l, r) when (k ^^^ p) &&& (b - 1) = 0 ->




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




