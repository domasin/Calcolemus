// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //


/// Some propositional formulas to test, and functions to generate classes.
module FolAutomReas.Propexamples

open FolAutomReas.Lib

open Formulas
open Prop

// ========================================================================= //
// Ramsey Therorem                                                           //
// ========================================================================= //

/// Generate assertion equivalent to R(s,t) <= n for the Ramsey number R(s,t) 
let ramsey s t n =
    let vertices = 1 -- n
    let yesgrps = List.map (allsets 2) (allsets s vertices)
    let nogrps = List.map (allsets 2) (allsets t vertices)
    let e [m;n] = Atom(P("p_" + (string m) + "_" + (string n)))
    Or (list_disj (List.map (list_conj << List.map e) yesgrps),
        list_disj (List.map (list_conj << List.map (fun p -> Not (e p))) nogrps))

// ========================================================================= //
// Addition of n-bit numbers as circuits or propositional formulas           //
// ========================================================================= //

//     1 0 1 1 0 0 1 1
// +   0 1 1 0 0 1 0 1
// = 1 0 0 0 1 1 0 0 0

// ------------------------------------------------------------------------- //
// Half adder.                                                          
//
// An half adder (or 1-bit adder) calculates the sum and carry for just two 
// digits x and y to be added. 
// 
// x|y||c|s
// ---||---
// 0|0||0|0
// 0|1||0|1
// 1|0||0|1
// 1|1||1|0
// ------------------------------------------------------------------------- //

/// Generates the propositional formula whose truth value correponds to the 
/// carry of an half adder, given the `x` and `y` digits to be summed also 
/// represented as prop formulas: false for 0 true for 1.
/// 
/// x /\ y
let halfsum x y = 
    Iff (x, Not y)

/// Generates the propositional formulas whose truth value correponds to the 
/// sum of an half adder, given the `x` and `y` digits to be summed also 
/// represented as prop formulas: false for 0 true for 1. 
/// 
/// x <=> ~ y.
let halfcarry x y = 
    And (x, y)

/// Half adder function.
/// 
/// Generates a propositional formula that is true if the input formulas 
/// represent respectively two digits `x` and `y` to be summed, the resulting 
/// sum `s` and the carry `c`.
let ha x y s c = 
    And (Iff (s, halfsum x y), Iff (c, halfcarry x y))

// ------------------------------------------------------------------------- //
// Full adder.
//
// A full adder (or n-bit adder) calculates the sum and carry for three digits 
// x, y and z where x and y are the digits to be summed and z is the carry 
// from a previous sum.
// 
// x|y|z||c|s
// -----||---
// 0|0|0||0|0
// 0|0|1||0|1
// 0|1|0||0|1
// 0|1|1||1|0
// 1|0|0||0|1
// 1|0|1||1|0
// 1|1|0||1|0
// 1|1|1||1|1
// ------------------------------------------------------------------------- //

/// Generates the propositional formula whose truth value correponds to the 
/// carry of a full adder, given the `x`, `y` and `z` digits to be summed also 
/// represented as formulas: false for 0 true for 1. 
/// 
/// (x /\ y) \/ ((x \/ y) /\ z)
let carry x y z = Or (And (x, y), And (Or (x, y), z))

/// Generates the propositional formula whose truth value correponds to the sum 
/// of a full adder, given the `x`, `y` and `z` digits to be summed also 
/// represented as formulas: false for 0 true for 1. 
/// 
/// (x <=> ~ y) <=> ~ z
let sum x y z = halfsum (halfsum x y) z

/// Full adder function.
/// 
/// Generates a propositional formula that is true if the input terms represent 
/// respectively two digits `x` and `y` to be summed, the `z` carry from a 
/// previous sum, the resulting sum `s` and the carry `c`.
let fa x y z s c = 
    And (Iff (s, sum x y z), Iff (c, carry x y z))

// pg. 67
// ------------------------------------------------------------------------- //
// Useful idiom.                                                             //
// ------------------------------------------------------------------------- //

/// An auxiliary function to define ripplecarry.
/// 
/// Given a function that creates a prop formula from an index and a list of 
/// indexes, it puts multiple full-adders together into an n-bit adder.
let conjoin f l = list_conj (List.map f l)
    
// pg. 67
// ------------------------------------------------------------------------- //
// n-bit ripple carry adder with carry c(0) propagated in and c(n) out.      //
// ------------------------------------------------------------------------- //

/// n-bit ripple carry adder with carry c(0) propagated in and c(n) out.  
/// 
/// Generates a propsitional formula that represent a riple-carry adder circuit.
/// Filtering the true rows of its truth table gives the sum and carry values 
/// for each digits.
/// 
/// It expects the user to supply functions `x`, `y`, `out` and `c` that, when 
/// given an index, generates an appropriate new variable. Use `mk_index` 
/// to generate such functions.
/// 
/// For example, 
/// 
/// `let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]`
/// 
/// `ripplecarry x y c out 2`
let ripplecarry x y c out n =
    conjoin (fun i -> fa (x i) (y i) (c i) (out i) (c (i + 1)))
            (0 -- (n - 1))

// pg. 67
// ------------------------------------------------------------------------- //
// Example.                                                                  //
// ------------------------------------------------------------------------- //

/// An auxiliary function to generate input for ripplecarry.
/// 
/// Given a prpo formula `x` and an index `i`, it generates a propositional 
/// variable `P "x_i"`.
/// 
/// `let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]` 
/// generates the x, y, out and c functions that can be given 
/// as input to ripplecarry
let mk_index x (i : int) = Atom (P (x + "_" + (string i)))

/// Similar to `mk_index`. 
/// 
/// Given a prop formula `x` and an indexes `i` and `j`, it generates a 
/// propositional variable `P "x_i_j"`.
let mk_index2 x i j =
    Atom (P (x + "_" + (string i) + "_" + (string j)))

// pg. 67
// ------------------------------------------------------------------------- //
// Special case with 0 instead of c(0).                                      //
// ------------------------------------------------------------------------- //

/// n-bit ripple carry adder with carry c(0) forced to 0.
/// 
/// It can be used when we are not interested in a carry in at the low end.
let ripplecarry0 x y c out n =
    psimplify (ripplecarry x y (fun i -> if i = 0 then False else c i) out n)

// pg. 68
// ------------------------------------------------------------------------- //
// Carry-select adder                                                        //
// ------------------------------------------------------------------------- //

/// n-bit ripple carry adder with carry c(0) forced at 1.
/// 
/// It is used to define the carry-select adder. In a carry-select adder the 
/// n-bit inputs are split into several blocks of k, and corresponding k-bit 
/// blocks are added twice, once assuming a carry-in of 0 and once assuming a 
/// carry-in of 1.
let ripplecarry1 x y c out n =
    psimplify (ripplecarry x y (fun i -> if i = 0 then True else c i) out n)

/// Multiplexer used to define the carry-select adder. We will use it to 
/// select between the two alternatives (carry-in of 0 or 1) when we do 
/// carry propagation.
let mux sel in0 in1 = Or (And (Not sel, in0), And (sel, in1))

/// An auxiliary function to oﬀset the indices in an array of bits. 
/// It is used to define the carry-select adder.
let offset n x i = x (n + i)

let rec carryselect x y c0 c1 s0 s1 c s n k =
    let k' = min n k
    let fm =
        And (And (ripplecarry0 x y c0 s0 k', ripplecarry1 x y c1 s1 k'),
            And (Iff (c k', mux (c 0) (c0 k') (c1 k')),
                conjoin (fun i -> Iff (s i, mux (c 0) (s0 i) (s1 i)))
                    (0 -- (k' - 1))))
    if k' < k then fm else
        And (fm, carryselect
            (offset k x) (offset k y) (offset k c0) (offset k c1)
            (offset k s0) (offset k s1) (offset k c) (offset k s)
            (n - k) k)

// pg. 69
// ------------------------------------------------------------------------- //
// Equivalence problems for carry-select vs ripple carry adders.             //
// ------------------------------------------------------------------------- //

/// Generates propositions that state the equivalence of various ripplecarry 
/// and carryselect circuits based on the input `n` (number of bit to be added) 
/// and `k` (number of blocks in the carryselect circuit).
/// 
/// If the proposition generated is a tautology, the equivalence between the 
/// two circuit is proved.
let mk_adder_test n k =
    let l = List.map mk_index ["x"; "y"; "c"; "s"; "c0"; "s0"; "c1"; "s1"; "c2"; "s2"]
    match l with
    | [x; y; c; s; c0; s0; c1; s1; c2; s2] -> 
        Imp (And (And (carryselect x y c0 c1 s0 s1 c s n k, Not(c 0)), ripplecarry0 x y c2 s2 n), And (Iff (c n,c2 n), conjoin (fun i -> Iff (s i, s2 i)) (0 -- (n - 1))))
    | _ -> failwith "mk_adder_test"

// ========================================================================= //
// Multiplication of n-bit numbers as circuits or propositional terms        //
// ========================================================================= //

//      2222 (A)
// x    2222 (B)
// ---------
//      4444
// +   4444
// +  4444
// + 4444
// ---------
// = 4937284
// 
// 
//   |    |      |      |      | A0B3 | A0B2 | A0B1 | A0B0 |
// + |    |      |      | A1B3 | A1B2 | A1B1 | A1B0 |      |
// + |    |      | A2B3 | A2B2 | A2B1 | A2B0 |      |      |
// + |	  | A3B3 | A3B2 | A3B1 | A3B0 |      |      |      |
// ---------------------------------------------------------
// = | P7 |  P6  |  P5  |  P4  |  P3  |  P2  |  P1  |  P0  |
        
// pg. 70
// ------------------------------------------------------------------------- //
// Ripple carry stage that separates off the final result.                   //
//                                                                           //
//       UUUUUUUUUUUUUUUUUUUU  (u)                                           //
//    +  VVVVVVVVVVVVVVVVVVVV  (v)                                           //
//                                                                           //
//    = WWWWWWWWWWWWWWWWWWWW   (w)                                           //
//    +                     Z  (z)                                           //
// ------------------------------------------------------------------------- //

/// Ripple carry stage that separates off the final result of a multiplication.
let rippleshift u v c z w n =
    ripplecarry0 u v (fun i -> if i = n then w (n - 1) else c (i + 1))
                    (fun i -> if i = 0 then z else w (i - 1)) n

/// Naive multiplier based on repeated ripple carry. 
let multiplier x u v out n =
    if n = 1 then And (Iff (out 0, x 0 0), Not (out 1)) else
    psimplify (
        And (Iff (out 0, x 0 0),
            And (rippleshift (fun i -> 
                    if i = n - 1 then False
                    else x 0 (i + 1)) (x 1) (v 2) (out 1) (u 2) n, 
                    if n = 2 then And (Iff (out 2, u 2 0), Iff(out 3, u 2 1)) 
                    else conjoin (fun k ->
                        rippleshift (u k) (x k) (v(k + 1)) (out k) (
                            if k = n - 1 then fun i -> out (n + i) 
                            else u (k + 1)) n) (2 -- (n - 1)))))

// pg. 71
// ------------------------------------------------------------------------- //
// Primality examples.                                                       //
// For large examples, should use "num" instead of "int" in these functions. //
// ------------------------------------------------------------------------- //

/// Returns the nuber of bit needed to represent x in binary notation.
let rec bitlength x = if x = 0 then 0 else 1 + bitlength (x / 2)

/// Extract the `n`th bit (as a boolean value) of a nonnegative integer `x`.
let rec bit n x = if n = 0 then x % 2 = 1 else bit (n - 1) (x / 2)

/// Produces a propositional formula asserting that the atoms `x`(i) encode 
/// the bits of a value `m`, at least modulo 2^`n`.
let congruent_to x m n =
    conjoin (fun i -> if bit i m then x i else Not (x i))
            (0 -- (n - 1))

/// Applied to a positive integer `p` generates a propositional formula 
/// that is a tautology precisely if `p` is prime.
let prime p =
    let l1 = List.map mk_index ["x"; "y"; "out"]
    match l1 with
    | [x; y; out] ->
        let m i j = And (x i,y j)
        let l2 = List.map mk_index2 ["u"; "v"]
        match l2 with
        | [u; v] ->
            let n = bitlength p
            Not (And (multiplier m u v out (n - 1), congruent_to out p (max n (2 * n - 2))))
        | _ -> failwith "prime"
    | _ -> failwith "prime"
