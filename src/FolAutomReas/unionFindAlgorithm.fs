// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini (derived from lib.fs)
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// Union-find algorithm.
[<AutoOpen>]
module FolAutomReas.Lib.UnionFindAlgorithm

// ------------------------------------------------------------------------- //
// Union-find algorithm.                                                     //
// ------------------------------------------------------------------------- //

// Not in book
// Type for use with union-find algorithm  
type pnode<'a> =
    | Nonterminal of 'a 
    | Terminal of 'a * int
    
// Not in book
// Type for use with union-find algorithm 
// HOL Light: termequivalence
type partition<'a> = Partition of func<'a, pnode<'a>>
    
// Not in book
// Support function for use with union-find algorithm 
let rec terminus (Partition f as ptn) a =
    match apply f a with
    | Terminal (p, q) ->
        p, q
    | Nonterminal b ->
        terminus ptn b
        
// Not in book
// Support function for use with union-find algorithm 
let tryterminus ptn a =
    try terminus ptn a
    with _ -> (a, 1)
        
// pg. 622
let canonize ptn a =
    fst <| tryterminus ptn a

// pg. 622
let equivalent eqv a b =
    canonize eqv a = canonize eqv b
    
// pg. 622
let equate (a, b) (Partition f as ptn) =
    let a', na = tryterminus ptn a
    let b', nb = tryterminus ptn b
    if a' = b' then f
    elif na <= nb then
        List.foldBack id [a' |-> Nonterminal b'; b' |-> Terminal (b', na + nb)] f
    else
        List.foldBack id [b' |-> Nonterminal a'; a' |-> Terminal (a', na + nb)] f
    |> Partition

// pg. 622
let unequal = Partition undefined
    
// pg. 622
let equated (Partition f) = dom f

// ------------------------------------------------------------------------- //
// First number starting at n for which p succeeds.                          //
// ------------------------------------------------------------------------- //

// pg. 618
let rec first n p =
    if p n then n
    else first (n + Int 1) p

/// Write from a StringWriter to a string
let writeToString fn = 
    use sw = new System.IO.StringWriter()
    fn sw
    sw.ToString()