module FolAutomReas.Tests.Propexamples

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open FolAutomReas.Formulas
open FolAutomReas.Prop
open FolAutomReas.Propexamples
open FolAutomReas.Lib.List

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
let ``ha returns a tautology if inputs correspond to x y s and c of an half adder.``() = 
    let fm = ha (True:prop formula) True False True
    Assert.Equal(
        "`(false <=> true <=> ~true) /\ (true <=> true /\ true)`", 
        fm |> sprint_prop_formula)
    Assert.Equal(
        true, 
        tautology(fm))

[<Fact>]
let ``ha tautological results represent correct sum s and carry c of an half adder that adds two digits x and y.``() = 
    [for x in [False;True] do 
        for y in [False;True] do 
            for c in [False;True] do 
                for s in [False;True] do 
                    if tautology(ha x y s c) then 
                        (x,y,c,s)]
    |> shouldEqual
        [
            (False, False, False, False); 
            (False, True, False, True);
            (True, False, False, True); 
            (True, True, True, False)
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
let ``fa returns a tautology if inputs correspond to x y z s and c of a full adder.``() = 
    let fm = fa (True:prop formula) True True True True
    Assert.Equal(
        @"`(true <=> (true <=> ~true) <=> ~true) /\ (true <=> true /\ true \/ (true \/ true) /\ true)`", 
        fm |> sprint_prop_formula)
    Assert.Equal(
        true, 
        tautology(fm))

[<Fact>]
let ``fa tautological results represent corrects sum s and carry c of an half adder that adds two digits x and y.``() = 
    [for x in [(False:prop formula);True] do 
        for y in [False;True] do 
            for z in [False;True] do 
                for c in [False;True] do 
                    for s in [False;True] do 
                        if tautology(fa x y z s c) then 
                            (x,y,z,c,s)]
    |> shouldEqual
        [
            (False, False, False, False, False); 
            (False, False, True, False, True);
            (False, True, False, False, True); 
            (False, True, True, True, False);
            (True, False, False, False, True); 
            (True, False, True, True, False);
            (True, True, False, True, False); 
            (True, True, True, True, True)
        ]