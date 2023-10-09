#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.Fpf
open FolAutomReas.Lib.Set

[1;4;5]
|> List.fold (fun acc x -> (x|->x)acc) undefined

[-3..3]
|> List.fold (fun acc x -> (x|->x)acc) undefined


[-10..10]
|> List.fold (fun acc x -> (x|->x)acc) undefined

(5|->5)((4|->4)((3|->3)((2|-> 2)((1 |-> 1)undefined))))

printfn "old|new|  b|  p"
for o in [-10..10] do 
    for n in [-10..10] do 
        let h = hash o
        let k = hash n

        let zp = h ^^^ k
        let b = zp &&& -zp
        let p = k &&& (b - 1)
        let position = 
            if o &&& b = 0 then 
                "old,new"
            else 
                "new,old"
        printfn "%3i|%3i|%3i|%3i|%s" o n p b position



// let t1 = Leaf (h, [(1, 1)])
// let t2 = Leaf (k, [(2, 2)])


System.Convert.ToString(-3,2)

let newbranch oldHsh oldMap newHsh newMap =
    let zp = oldHsh ^^^ newHsh  
    let b = zp &&& -zp         
    let p = oldHsh &&& (b - 1)
    // if old hash not equals b
    if oldHsh &&& b = 0 then 
        Branch (p, b, oldMap, newMap)
    // if old hash equals b
    else 
        Branch (p, b, newMap, oldMap)

// newbranch h t1 k t2

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




