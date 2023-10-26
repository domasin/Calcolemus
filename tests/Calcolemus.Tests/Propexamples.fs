module Calcolemus.Tests.Propexamples

open Xunit
open FsUnit.Xunit
open FsUnitTyped
open FsCheck

open Calcolemus.Formulas
open Calcolemus.Prop
open Calcolemus.Propexamples
open Calcolemus.Lib.List

[<Fact>]
let ``ramsey should return a prop equivalent of R(s,t)<=n.``() = 
    ramsey 3 3 4
    |> sprint_prop_formula
    |> should equal "`(p_1_2 /\ p_1_3 /\ p_2_3 \/ p_1_2 /\ p_1_4 /\ p_2_4 \/ p_1_3 /\ p_1_4 /\ p_3_4 \/ p_2_3 /\ p_2_4 /\ p_3_4) \/ ~p_1_2 /\ ~p_1_3 /\ ~p_2_3 \/ ~p_1_2 /\ ~p_1_4 /\ ~p_2_4 \/ ~p_1_3 /\ ~p_1_4 /\ ~p_3_4 \/ ~p_2_3 /\ ~p_2_4 /\ ~p_3_4`"

[<Fact>]
let ``ramsey 3 3 5 should return false.``() = 
    tautology(ramsey 3 3 5)
    |> should equal false

[<Fact>]
let ``ramsey 3 3 6 should return true.``() = 
    tautology(ramsey 3 3 6)
    |> should equal true

[<Fact>]
let ``halfsum should return x <=> ~ y.``() = 
    halfsum (True:prop formula) False
    |> sprint_prop_formula
    |> should equal "`true <=> ~false`"

[<Fact>]
let ``halfsum should return the sum of an half adder.``() = 
    allpairs (fun x y -> (x,y)) [(False:prop formula);True] [False;True]
    |> List.map (fun (x,y) -> x,y, eval (halfsum x y) (fun _ -> false))
    |> shouldEqual 
        [
            (False, False, false); 
            (False, True, true); 
            (True, False, true);
            (True, True, false)
        ]

[<Fact>]
let ``halfcarry should return x /\ ~ y.``() = 
    halfcarry (True:prop formula) False
    |> sprint_prop_formula
    |> should equal "`true /\ false`"

[<Fact>]
let ``halfcarry should return the carry of an half adder.``() = 
    allpairs (fun x y -> (x,y)) [(False:prop formula);True] [False;True]
    |> List.map (fun (x,y) -> x,y, eval (halfcarry x y) (fun _ -> false))
    |> shouldEqual
        [
            (False, False, false); 
            (False, True, false); 
            (True, False, false);
            (True, True, true)
        ]

[<Fact>]
let ``ha should return (s <=> x <=> ~y) /\ (c <=> x /\ y).``() = 
    ha (True:prop formula) True False True
    |> sprint_prop_formula
    |> should equal "`(false <=> true <=> ~true) /\ (true <=> true /\ true)`"

[<Fact>]
let ``ha should return a prop formula whose satisfying valuations represent the relations between input and output of an half adder.``() = 
    let fm = ha (!>"x") (!>"y") (!>"s") (!>"c")

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
            v (P "x") |> System.Convert.ToInt32,
            v (P "y") |> System.Convert.ToInt32,
            v (P "c") |> System.Convert.ToInt32,
            v (P "s") |> System.Convert.ToInt32
    )
    |> should equal 
        [
            (0, 0, 0, 0); 
            (0, 1, 0, 1); 
            (1, 0, 0, 1); 
            (1, 1, 1, 0)
        ]

[<Fact>]
let ``carry should return (x /\ y) \/ ((x \/ y) /\ z).``() = 
    carry (True:prop formula) False True
    |> sprint_prop_formula
    |> should equal @"`true /\ false \/ (true \/ false) /\ true`"

[<Fact>]
let ``carry should return the carry of a full adder.``() = 
    [for x in [(False:prop formula);True] do 
        for y in [False;True] do 
            for z in [False;True] do 
                (x,y,z,eval (carry x y z) (fun _ -> false))]
    |> shouldEqual 
        [
            (False, False, False, false); 
            (False, False, True, false);
            (False, True, False, false); 
            (False, True, True, true);
            (True, False, False, false); 
            (True, False, True, true);
            (True, True, False, true); 
            (True, True, True, true)
        ]

[<Fact>]
let ``sum should return (x <=> ~ y) <=> ~ z.``() = 
    sum (True:prop formula) False True
    |> sprint_prop_formula
    |> should equal @"`(true <=> ~false) <=> ~true`"

[<Fact>]
let ``sum should return the sum of a full adder.``() = 
    [for x in [(False:prop formula);True] do 
        for y in [False;True] do 
            for z in [False;True] do 
                (x,y,z,eval (sum x y z) (fun _ -> false))]
    |> shouldEqual 
        [
            (False, False, False, false); 
            (False, False, True, true);
            (False, True, False, true); 
            (False, True, True, false);
            (True, False, False, true); 
            (True, False, True, false);
            (True, True, False, false); 
            (True, True, True, true)
        ]

[<Fact>]
let ``fa should return (s <=> (x <=> ~y) <=> ~z) /\ (c <=> x /\ y \/ (x \/ y) /\ z).``() = 
    fa (True:prop formula) True True True True
    |> sprint_prop_formula
    |> should equal @"`(true <=> (true <=> ~true) <=> ~true) /\ (true <=> true /\ true \/ (true \/ true) /\ true)`"

[<Fact>]
let ``fa should return a prop formula whose satisfying valuations represent the relations between input and output of a full adder.``() = 
    let fm = fa (!>"x") (!>"y") (!>"z") (!>"s") (!>"c")

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
            v (P "x") |> System.Convert.ToInt32,
            v (P "y") |> System.Convert.ToInt32,
            v (P "z") |> System.Convert.ToInt32,
            v (P "c") |> System.Convert.ToInt32,
            v (P "s") |> System.Convert.ToInt32
    )
    |> shouldEqual 
        [
            (0, 0, 0, 0, 0); 
            (0, 0, 1, 0, 1); 
            (0, 1, 0, 0, 1); 
            (1, 0, 0, 0, 1);
            (0, 1, 1, 1, 0); 
            (1, 0, 1, 1, 0); 
            (1, 1, 0, 1, 0); 
            (1, 1, 1, 1, 1)
        ]

[<Fact>]
let ``conjoin should return the conjunctions of all formulas obtained applying the input function to the elements of the input list``() = 
    conjoin Atom [1;2;3]
    |> should equal (And (Atom 1, And (Atom 2, Atom 3)))

[<Fact>]
let ``ripplecarry should return a prop formula whose satisfying valuations represent the relations between input and output of a ripplecarry adder with c(0) propagated in.``() = 
    let x, y, s, c = 
        mk_index "x",
        mk_index "y",
        mk_index "s",
        mk_index "c"
 
    let fm = ripplecarry x y c s 2

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
        atoms fm
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 0; 0; 0; 0; 0; 0; 0]; [0; 0; 0; 0; 1; 0; 0; 0; 1];
         [0; 0; 0; 0; 1; 0; 1; 0; 0]; [0; 0; 0; 1; 0; 0; 0; 1; 0];
         [0; 0; 0; 1; 0; 1; 0; 0; 0]; [0; 0; 0; 1; 1; 0; 0; 1; 1];
         [0; 0; 0; 1; 1; 0; 1; 1; 0]; [0; 0; 0; 1; 1; 1; 0; 0; 1];
         [0; 0; 0; 1; 1; 1; 1; 0; 0]; [0; 0; 1; 0; 0; 0; 1; 0; 1];
         [0; 0; 1; 1; 0; 0; 1; 1; 1]; [0; 0; 1; 1; 0; 1; 1; 0; 1];
         [0; 1; 0; 0; 1; 1; 0; 1; 0]; [0; 1; 1; 0; 0; 1; 0; 1; 1];
         [0; 1; 1; 0; 0; 1; 1; 1; 0]; [0; 1; 1; 0; 1; 1; 1; 1; 1];
         [1; 0; 0; 1; 0; 0; 0; 0; 0]; [1; 0; 0; 1; 1; 0; 0; 0; 1];
         [1; 0; 0; 1; 1; 0; 1; 0; 0]; [1; 0; 1; 1; 0; 0; 1; 0; 1];
         [1; 1; 0; 0; 1; 0; 0; 1; 0]; [1; 1; 0; 0; 1; 1; 0; 0; 0];
         [1; 1; 0; 1; 1; 1; 0; 1; 0]; [1; 1; 1; 0; 0; 0; 0; 1; 1];
         [1; 1; 1; 0; 0; 0; 1; 1; 0]; [1; 1; 1; 0; 0; 1; 0; 0; 1];
         [1; 1; 1; 0; 0; 1; 1; 0; 0]; [1; 1; 1; 0; 1; 0; 1; 1; 1];
         [1; 1; 1; 0; 1; 1; 1; 0; 1]; [1; 1; 1; 1; 0; 1; 0; 1; 1];
         [1; 1; 1; 1; 0; 1; 1; 1; 0]; [1; 1; 1; 1; 1; 1; 1; 1; 1]]

[<Fact>]
let ``mk_index should return the appropriate indexed prop variable.``() = 
    mk_index "x" 0
    |> should equal (Atom (P "x_0"))

[<Fact>]
let ``mk_index2 should return the appropriate double indexed prop variable.``() = 
    mk_index2 "x" 0 1
    |> should equal (Atom (P "x_0_1"))

[<Fact>]
let ``ripplecarry0 should return a prop formula whose satisfying valuations represent the relations between input and output of a ripplecarry adder with c(0) forced to 0.``() = 
    let x, y, s, c = 
        mk_index "x",
        mk_index "y",
        mk_index "s",
        mk_index "c"
 
    let fm = ripplecarry0 x y c s 2

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
        atoms fm
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 0; 0; 0; 0; 0; 0]; [0; 0; 0; 1; 0; 0; 0; 1];
         [0; 0; 0; 1; 0; 1; 0; 0]; [0; 0; 1; 0; 0; 0; 1; 0];
         [0; 0; 1; 0; 1; 0; 0; 0]; [0; 0; 1; 1; 0; 0; 1; 1];
         [0; 0; 1; 1; 0; 1; 1; 0]; [0; 0; 1; 1; 1; 0; 0; 1];
         [0; 0; 1; 1; 1; 1; 0; 0]; [0; 1; 0; 0; 0; 1; 0; 1];
         [0; 1; 1; 0; 0; 1; 1; 1]; [0; 1; 1; 0; 1; 1; 0; 1];
         [1; 0; 0; 1; 1; 0; 1; 0]; [1; 1; 0; 0; 1; 0; 1; 1];
         [1; 1; 0; 0; 1; 1; 1; 0]; [1; 1; 0; 1; 1; 1; 1; 1]]

[<Fact>]
let ``ripplecarry1 should return a prop formula whose satisfying valuations represent the relations between input and output of a ripplecarry adder with c(0) forced to 1.``() = 
    let x, y, s, c = 
        mk_index "x",
        mk_index "y",
        mk_index "s",
        mk_index "c"
 
    let fm = ripplecarry1 x y c s 2

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
        atoms fm
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 1; 0; 0; 0; 0; 0]; [0; 0; 1; 1; 0; 0; 0; 1];
         [0; 0; 1; 1; 0; 1; 0; 0]; [0; 1; 1; 0; 0; 1; 0; 1];
         [1; 0; 0; 1; 0; 0; 1; 0]; [1; 0; 0; 1; 1; 0; 0; 0];
         [1; 0; 1; 1; 1; 0; 1; 0]; [1; 1; 0; 0; 0; 0; 1; 1];
         [1; 1; 0; 0; 0; 1; 1; 0]; [1; 1; 0; 0; 1; 0; 0; 1];
         [1; 1; 0; 0; 1; 1; 0; 0]; [1; 1; 0; 1; 0; 1; 1; 1];
         [1; 1; 0; 1; 1; 1; 0; 1]; [1; 1; 1; 0; 1; 0; 1; 1];
         [1; 1; 1; 0; 1; 1; 1; 0]; [1; 1; 1; 1; 1; 1; 1; 1]]

[<Fact>]
let ``mux should select in1 if sel is true.``() = 
    let fm = mux !>"true" !>"in0" !>"in1"

    allvaluations fm
    |> List.map (fun v -> 
        atoms fm
        |> List.map v, 
        eval fm v
    )
    |> shouldEqual 
        [
            ([false; false], false); 
            ([false; true], true); 
            ([true; false], false);
            ([true; true], true)
        ]

[<Fact>]
let ``mux should select in0 if sel is false.``() = 
    let fm = mux !>"false" !>"in0" !>"in1"

    allvaluations fm
    |> List.map (fun v -> 
        atoms fm
        |> List.map v, 
        eval fm v
    )
    |> shouldEqual 
        [
            ([false; false], false); 
            ([false; true], false); 
            ([true; false], true);
            ([true; true], true)
        ]

[<Fact>]
let ``offset should offsets variable indexes.``() = 
    offset 1 (mk_index "x") 2
    |> should equal (Atom (P "x_3"))

[<Fact>]
let ``carryselect should return a prop formula whose satisfying valuations represent the relations between input and output of a carryselect.``() = 
    let x, y, c0, c1, s0, s1, s, c = 
        mk_index "x",
        mk_index "y",
        mk_index "c0",
        mk_index "c1",
        mk_index "s0",
        mk_index "s1",
        mk_index "s",
        mk_index "c"
 
    let cs = carryselect x y c0 c1 s0 s1 c s 2 2

    (allsatvaluations (eval cs) (fun _ -> false) (atoms cs))
    |> List.map (fun v -> 
        atoms cs
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 1; 0; 0; 0; 1];
         [0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 1; 0; 1; 0; 0];
         [0; 0; 0; 0; 1; 0; 0; 0; 1; 0; 1; 0; 0; 0; 0; 0];
         [0; 0; 0; 0; 1; 0; 0; 1; 1; 1; 1; 1; 0; 0; 0; 1];
         [0; 0; 0; 0; 1; 0; 0; 1; 1; 1; 1; 1; 0; 1; 0; 0];
         [0; 0; 1; 0; 0; 0; 1; 0; 0; 1; 1; 0; 0; 0; 1; 0];
         [0; 0; 1; 0; 0; 0; 1; 0; 0; 1; 1; 0; 1; 0; 0; 0];
         [0; 0; 1; 0; 1; 0; 1; 0; 0; 1; 0; 1; 0; 0; 1; 0];
         [0; 0; 1; 0; 1; 0; 1; 0; 0; 1; 0; 1; 1; 0; 0; 0];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1; 0; 1; 1; 0];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1; 1; 0; 0; 1];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1; 1; 1; 0; 0];
         [0; 0; 1; 1; 1; 1; 1; 1; 0; 0; 0; 0; 0; 0; 1; 1];
         [0; 0; 1; 1; 1; 1; 1; 1; 0; 0; 0; 0; 0; 1; 1; 0];
         [0; 0; 1; 1; 1; 1; 1; 1; 0; 0; 0; 0; 1; 0; 0; 1];
         [0; 0; 1; 1; 1; 1; 1; 1; 0; 0; 0; 0; 1; 1; 0; 0];
         [0; 1; 0; 1; 0; 1; 0; 0; 1; 0; 0; 0; 0; 1; 0; 1];
         [0; 1; 0; 1; 1; 1; 0; 0; 1; 0; 1; 0; 0; 1; 0; 1];
         [0; 1; 1; 1; 0; 1; 1; 0; 0; 1; 1; 0; 0; 1; 1; 1];
         [0; 1; 1; 1; 0; 1; 1; 0; 0; 1; 1; 0; 1; 1; 0; 1];
         [0; 1; 1; 1; 1; 1; 1; 0; 0; 1; 0; 1; 0; 1; 1; 1];
         [0; 1; 1; 1; 1; 1; 1; 0; 0; 1; 0; 1; 1; 1; 0; 1];
         [1; 0; 1; 0; 0; 0; 0; 1; 1; 1; 0; 1; 1; 0; 1; 0];
         [1; 0; 1; 0; 1; 0; 0; 1; 1; 1; 1; 1; 1; 0; 1; 0];
         [1; 1; 1; 1; 0; 1; 0; 0; 1; 0; 0; 0; 1; 0; 1; 1];
         [1; 1; 1; 1; 0; 1; 0; 0; 1; 0; 0; 0; 1; 1; 1; 0];
         [1; 1; 1; 1; 0; 1; 0; 1; 1; 1; 0; 1; 1; 1; 1; 1];
         [1; 1; 1; 1; 1; 1; 0; 0; 1; 0; 1; 0; 1; 0; 1; 1];
         [1; 1; 1; 1; 1; 1; 0; 0; 1; 0; 1; 0; 1; 1; 1; 0];
         [1; 1; 1; 1; 1; 1; 0; 1; 1; 1; 1; 1; 1; 1; 1; 1]]

[<Fact>]
let ``carryselect of 2 bit number with 1 block should equal ripplecarry0.``() = 
    mk_adder_test 2 1
    |> tautology
    |> should equal true

[<Fact>]
let ``rippleshift should return a prop formula whose satisfying valuations represent the relations between input and output of a ripplecarry0 with lsb separated from other bits.``() = 
    let x, y, s, c = 
        mk_index "x",
        mk_index "y",
        mk_index "s",
        mk_index "c"
 
    let fm = rippleshift x y c !>"z" s 2

    (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
    |> List.map (fun v -> 
        atoms fm
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 0; 0; 0; 0; 0; 0]; [0; 0; 0; 0; 0; 1; 0; 1];
         [0; 0; 0; 1; 0; 0; 0; 1]; [0; 0; 1; 0; 1; 0; 1; 0];
         [0; 0; 1; 0; 1; 1; 1; 1]; [0; 0; 1; 1; 1; 0; 1; 1];
         [0; 1; 0; 0; 0; 0; 1; 0]; [0; 1; 0; 0; 0; 1; 1; 1];
         [0; 1; 0; 0; 1; 0; 0; 0]; [0; 1; 0; 0; 1; 1; 0; 1];
         [0; 1; 0; 1; 0; 0; 1; 1]; [0; 1; 0; 1; 1; 0; 0; 1];
         [1; 0; 1; 1; 0; 1; 1; 0]; [1; 0; 1; 1; 1; 1; 0; 0];
         [1; 1; 0; 1; 0; 1; 0; 0]; [1; 1; 1; 1; 1; 1; 1; 0]]

[<Fact>]
let ``multiplier should return a prop formula whose satisfying valuations represent the relations between input and output of a multiplier.``() = 
    let x,y,out = 
        mk_index "x",
        mk_index "y",
        mk_index "out"
    
    let m i j = And(x i,y j)
    
    let u,v = 
        mk_index2 "u",
        mk_index2 "v"
    
    let ml = multiplier m u v out 3

    (allsatvaluations (eval ml) (fun _ -> false) (atoms ml))
    |> List.map (fun v -> 
        atoms ml
        |> List.map (fun atm -> 
            (v atm |> System.Convert.ToInt32)
        )
    )
    |> shouldEqual
        [[0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 0; 0; 0];
         [0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 0; 0];
         [0; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1];
         [0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0];
         [0; 0; 0; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 1];
         [0; 0; 0; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 1];
         [0; 0; 0; 1; 1; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 1; 0; 0; 1];
         [0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 0];
         [0; 0; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 0];
         [0; 0; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 0; 1];
         [0; 0; 1; 0; 0; 1; 1; 1; 0; 0; 0; 0; 1; 0; 1; 1; 0; 1; 1];
         [0; 0; 1; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 1];
         [0; 0; 1; 0; 1; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 0; 1];
         [0; 0; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0];
         [0; 0; 1; 1; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 1; 0];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 1];
         [0; 0; 1; 1; 0; 0; 1; 1; 0; 0; 0; 0; 0; 1; 1; 0; 0; 0; 1];
         [0; 0; 1; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 1];
         [0; 0; 1; 1; 1; 0; 1; 1; 0; 0; 0; 0; 0; 1; 1; 1; 0; 0; 1];
         [0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 0];
         [0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 1; 0];
         [0; 1; 0; 0; 1; 0; 0; 0; 1; 0; 1; 0; 0; 1; 1; 0; 0; 1; 1];
         [0; 1; 0; 0; 1; 0; 1; 0; 0; 0; 0; 1; 1; 0; 1; 1; 1; 1; 0];
         [0; 1; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 1; 0];
         [0; 1; 0; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 1];
         [0; 1; 0; 1; 0; 1; 0; 0; 1; 0; 1; 0; 0; 1; 1; 1; 0; 1; 1];
         [0; 1; 0; 1; 0; 1; 1; 1; 0; 0; 0; 1; 1; 0; 1; 1; 1; 1; 1];
         [0; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 0];
         [0; 1; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 1; 0];
         [0; 1; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 0; 1; 1];
         [0; 1; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 1; 0; 0; 1; 0];
         [0; 1; 1; 1; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 1; 0];
         [0; 1; 1; 1; 0; 0; 1; 1; 0; 0; 0; 0; 0; 0; 1; 0; 1; 1; 1];
         [0; 1; 1; 1; 1; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 1; 1; 0; 1];
         [0; 1; 1; 1; 1; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 1; 0; 1; 1];
         [1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 0; 0];
         [1; 0; 0; 0; 1; 1; 1; 0; 1; 1; 1; 1; 1; 1; 1; 1; 1; 1; 1];
         [1; 0; 0; 1; 0; 0; 0; 1; 0; 1; 1; 0; 0; 1; 1; 0; 1; 1; 0];
         [1; 0; 0; 1; 1; 0; 1; 0; 0; 0; 0; 1; 0; 1; 0; 1; 1; 0; 1];
         [1; 0; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 1; 0; 0];
         [1; 0; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 0; 1];
         [1; 0; 1; 0; 1; 0; 0; 1; 0; 1; 1; 0; 1; 1; 1; 1; 1; 1; 0];
         [1; 0; 1; 0; 1; 0; 1; 0; 1; 1; 1; 0; 0; 1; 1; 0; 1; 1; 1];
         [1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 1; 0];
         [1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 0; 1; 0; 0];
         [1; 1; 0; 0; 0; 1; 1; 0; 0; 0; 0; 1; 1; 1; 0; 1; 1; 1; 1];
         [1; 1; 0; 0; 0; 1; 1; 1; 0; 0; 0; 1; 1; 1; 1; 1; 1; 0; 1];
         [1; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 1; 1; 1; 0; 0];
         [1; 1; 1; 0; 0; 0; 1; 0; 0; 0; 0; 0; 0; 1; 0; 0; 1; 1; 1];
         [1; 1; 1; 1; 0; 0; 0; 0; 0; 0; 0; 0; 0; 1; 0; 1; 1; 1; 0];
         [1; 1; 1; 1; 0; 0; 1; 1; 0; 0; 0; 0; 0; 1; 1; 0; 1; 0; 1]]

// [<Fact>]
let ``bitlength should return the number of bits needed to represent x in binary notation.``() = 
    let bitlengthIsCorrect x = 
        if x <= 0 then true else
        bitlength x = System.Convert.ToString(x,2).Length

    Check.Quick bitlengthIsCorrect

[<Fact>]
let ``bit 2 5 should return true.``() = 
    bit 2 5
    |> should equal true

[<Fact>]
let ``congruent_to should return the prop formula that encodes m in binary with n bits.``() = 
    [0..10]
    |> List.map (fun i -> 
        let fm = congruent_to (mk_index "x") i (bitlength i)
        i,
        (allsatvaluations (eval fm) (fun _ -> false) (atoms fm))
        |> List.map (fun v -> 
            atoms fm
            |> Seq.map (v >> System.Convert.ToInt32 >> string)
            |> Seq.rev
            |> System.String.Concat
        )
        |> System.String.Concat
    )
    |> should equal 
        [(0, ""); (1, "1"); (2, "10"); (3, "11"); (4, "100"); (5, "101"); (6, "110"); (7, "111"); (8, "1000"); (9, "1001"); (10, "1010")]

[<Fact>]
let ``prime should return a prop formula that is a tautology if p is prime.``() = 
    prime 2
    |> tautology
    |> should equal true