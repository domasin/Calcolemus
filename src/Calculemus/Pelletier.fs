// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus.Fol

module Pelletier = 

    (* ---------------------------------------------------------------------- *)
    (* Propositional Logic.                                                   *)
    (* ---------------------------------------------------------------------- *)
    
    let p1 = 
        !! @"p ==> q <=> ~q ==> ~p"
    
    let p2 = 
        !! @"~ ~p <=> p"
    
    let p3 = 
        !! @"~(p ==> q) ==> q ==> p"
    
    let p4 = 
        !! @"~p ==> q <=> ~q ==> p"
    
    let p5 = 
        !! @"(p \/ q ==> p \/ r) ==> p \/ (q ==> r)"
    
    let p6 = 
        !! @"p \/ ~p"
    
    let p7 = 
        !! @"p \/ ~ ~ ~p"
    
    let p8 = 
        !! @"((p ==> q) ==> p) ==> p"
    
    let p9 = 
        !! @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)"
    
    let p10 = 
        !! @"(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p <=> q)"
    
    let p11 = 
        !! @"p <=> p"
    
    let p12 = 
        !! @"((p <=> q) <=> r) <=> (p <=> (q <=> r))"
    
    let p13 = 
        !! @"p \/ q /\ r <=> (p \/ q) /\ (p \/ r)"
    
    let p14 = 
        !! @"(p <=> q) <=> (q \/ ~p) /\ (~q \/ p)"
    
    let p15 = 
        !! @"p ==> q <=> ~p \/ q"
    
    let p16 = 
        !! @"(p ==> q) \/ (q ==> p)"
    
    let p17 = 
        !! @"p /\ (q ==> r) ==> s <=> (~p \/ q \/ s) /\ (~p \/ ~r \/ s)"
    
    (* ---------------------------------------------------------------------- *)
    (* Monadic Predicate Logic.                                               *)
    (* ---------------------------------------------------------------------- *)
    
    let p18 = 
        !! @"exists y. forall x. P(y) ==> P(x)"
    
    let p19 = 
        !! @"exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)"
    
    let p20 = 
        !! @"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w))
        ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))"
    
    let p21 = 
        !! @"(exists x. P ==> Q(x)) /\ (exists x. Q(x) ==> P)
        ==> (exists x. P <=> Q(x))"
    
    let p22 = 
        !! @"(forall x. P <=> Q(x)) ==> (P <=> (forall x. Q(x)))"
    
    let p23 = 
        !! @"(forall x. P \/ Q(x)) <=> P \/ (forall x. Q(x))"
    
    let p24 = 
        !! @"~(exists x. U(x) /\ Q(x)) /\
        (forall x. P(x) ==> Q(x) \/ R(x)) /\
        ~(exists x. P(x) ==> (exists x. Q(x))) /\
        (forall x. Q(x) /\ R(x) ==> U(x)) ==>
        (exists x. P(x) /\ R(x))"
    
    let p25 = 
        !! @"(exists x. P(x)) /\
        (forall x. U(x) ==> ~G(x) /\ R(x)) /\
        (forall x. P(x) ==> G(x) /\ U(x)) /\
        ((forall x. P(x) ==> Q(x)) \/ (exists x. Q(x) /\ P(x))) ==>
        (exists x. Q(x) /\ P(x))"
    
    let p26 = 
        !! @"((exists x. P(x)) <=> (exists x. Q(x))) /\
        (forall x y. P(x) /\ Q(y) ==> (R(x) <=> U(y))) ==>
        ((forall x. P(x) ==> R(x)) <=> (forall x. Q(x) ==> U(x)))"
    
    let p27 = 
        !! @"(exists x. P(x) /\ ~Q(x)) /\
        (forall x. P(x) ==> R(x)) /\
        (forall x. U(x) /\ V(x) ==> P(x)) /\
        (exists x. R(x) /\ ~Q(x)) ==>
        (forall x. U(x) ==> ~R(x)) ==>
        (forall x. U(x) ==> ~V(x))"
    
    let p28 = 
        !! @"(forall x. P(x) ==> (forall x. Q(x))) /\
        ((forall x. Q(x) \/ R(x)) ==> (exists x. Q(x) /\ R(x))) /\
        ((exists x. R(x)) ==> (forall x. L(x) ==> M(x))) ==>
        (forall x. P(x) /\ L(x) ==> M(x))"
    
    let p29 = 
        !! @"(exists x. P(x)) /\ (exists x. G(x)) ==>
        ((forall x. P(x) ==> H(x)) /\ (forall x. G(x) ==> J(x)) <=>
        (forall x y. P(x) /\ G(y) ==> H(x) /\ J(y)))"
    
    let p30 = 
        !! @"(forall x. P(x) \/ G(x) ==> ~H(x)) /\
        (forall x. (G(x) ==> ~U(x)) ==> P(x) /\ H(x)) ==>
        (forall x. U(x))"
    
    let p31 = 
        !! @"~(exists x. P(x) /\ (G(x) \/ H(x))) /\ (exists x. Q(x) /\ P(x)) /\
        (forall x. ~H(x) ==> J(x)) ==>
        (exists x. Q(x) /\ J(x))"
    
    let p32 = 
        !! @"(forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\
        (forall x. Q(x) /\ H(x) ==> J(x)) /\
        (forall x. R(x) ==> H(x)) ==>
        (forall x. P(x) /\ R(x) ==> J(x))"
    
    let p33 = 
        !! @"(forall x. P(a) /\ (P(x) ==> P(b)) ==> P(c)) <=>
        (forall x. P(a) ==> P(x) \/ P(c)) /\ (P(a) ==> P(b) ==> P(c))"
    
    let p34 = 
        !! @"((exists x. forall y. P(x) <=> P(y)) <=>
        ((exists x. Q(x)) <=> (forall y. Q(y)))) <=>
        ((exists x. forall y. Q(x) <=> Q(y)) <=>
        ((exists x. P(x)) <=> (forall y. P(y))))"
    
    let p35 = 
        !! @"exists x y. P(x,y) ==> (forall x y. P(x,y))"
    
    (* ---------------------------------------------------------------------- *)
    (*  Full predicate logic (without Identity and Functions)                 *)
    (* ---------------------------------------------------------------------- *)
    
    let p36 = 
        !! @"(forall x. exists y. P(x,y)) /\
        (forall x. exists y. G(x,y)) /\
        (forall x y. P(x,y) \/ G(x,y)
        ==> (forall z. P(y,z) \/ G(y,z) ==> H(x,z)))
            ==> (forall x. exists y. H(x,y))"
    
    let p37 = 
        !! @"(forall z.
            exists w. forall x. exists y. (P(x,z) ==> P(y,w)) /\ P(y,z) /\
            (P(y,w) ==> (exists u. Q(u,w)))) /\
        (forall x z. ~P(x,z) ==> (exists y. Q(y,z))) /\
        ((exists x y. Q(x,y)) ==> (forall x. R(x,x))) ==>
        (forall x. exists y. R(x,y))"
    
    let p38 = 
        !! @"(forall x.
            P(a) /\ (P(x) ==> (exists y. P(y) /\ R(x,y))) ==>
            (exists z w. P(z) /\ R(x,w) /\ R(w,z))) <=>
        (forall x.
            (~P(a) \/ P(x) \/ (exists z w. P(z) /\ R(x,w) /\ R(w,z))) /\
            (~P(a) \/ ~(exists y. P(y) /\ R(x,y)) \/
            (exists z w. P(z) /\ R(x,w) /\ R(w,z))))"
    
    let p39 = 
        !! @"~(exists x. forall y. P(y,x) <=> ~P(y,y))"
    
    let p40 = 
        !! @"(exists y. forall x. P(x,y) <=> P(x,x))
        ==> ~(forall x. exists y. forall z. P(z,y) <=> ~P(z,x))"
    
    let p41 = 
        !! @"(forall z. exists y. forall x. P(x,y) <=> P(x,z) /\ ~P(x,x))
        ==> ~(exists z. forall x. P(x,z))"
    
    let p42 = 
        !! @"~(exists y. forall x. P(x,y) <=> ~(exists z. P(x,z) /\ P(z,x)))"
    
    let p43 = 
        !! @"(forall x y. Q(x,y) <=> forall z. P(z,x) <=> P(z,y))
        ==> forall x y. Q(x,y) <=> Q(y,x)"
    
    let p44 = 
        !! @"(forall x. P(x) ==> (exists y. G(y) /\ H(x,y)) /\
        (exists y. G(y) /\ ~H(x,y))) /\
        (exists x. J(x) /\ (forall y. G(y) ==> H(x,y))) ==>
        (exists x. J(x) /\ ~P(x))"
    
    let p45 = 
        !! @"(forall x.
            P(x) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y)) ==>
            (forall y. G(y) /\ H(x,y) ==> R(y))) /\
        ~(exists y. L(y) /\ R(y)) /\
        (exists x. P(x) /\ (forall y. H(x,y) ==>
            L(y)) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y))) ==>
        (exists x. P(x) /\ ~(exists y. G(y) /\ H(x,y)))"
    
    let p46 = 
        !! @"(forall x. P(x) /\ (forall y. P(y) /\ H(y,x) ==> G(y)) ==> G(x)) /\
        ((exists x. P(x) /\ ~G(x)) ==>
        (exists x. P(x) /\ ~G(x) /\
                    (forall y. P(y) /\ ~G(y) ==> J(x,y)))) /\
        (forall x y. P(x) /\ P(y) /\ H(x,y) ==> ~J(y,x)) ==>
        (forall x. P(x) ==> G(x))"
    
    (* ------------------------------------------------------------------------- *)
    (* Example from Manthey and Bry, CADE-9.                                     *)
    (* ------------------------------------------------------------------------- *)
    
    let p55 = 
        !! @"lives(agatha) /\ lives(butler) /\ lives(charles) /\
        (killed(agatha,agatha) \/ killed(butler,agatha) \/
        killed(charles,agatha)) /\
        (forall x y. killed(x,y) ==> hates(x,y) /\ ~richer(x,y)) /\
        (forall x. hates(agatha,x) ==> ~hates(charles,x)) /\
        (hates(agatha,agatha) /\ hates(agatha,charles)) /\
        (forall x. lives(x) /\ ~richer(x,agatha) ==> hates(butler,x)) /\
        (forall x. hates(agatha,x) ==> hates(butler,x)) /\
        (forall x. ~hates(x,agatha) \/ ~hates(x,butler) \/ ~hates(x,charles))
        ==> killed(agatha,agatha) /\
            ~killed(butler,agatha) /\
            ~killed(charles,agatha)"
    
    let p57 = 
        !! @"P(f((a),b),f(b,c)) /\
        P(f(b,c),f(a,c)) /\
        (forall (x) y z. P(x,y) /\ P(y,z) ==> P(x,z))
        ==> P(f(a,b),f(a,c))"
    
    (* ------------------------------------------------------------------------- *)
    (* See info-hol, circa 1500.                                                 *)
    (* ------------------------------------------------------------------------- *)
    
    let p58 = 
        !! @"forall P Q R. forall x. exists v. exists w. forall y. forall z.
        ((P(x) /\ Q(y)) ==> ((P(v) \/ R(w))  /\ (R(z) ==> Q(v))))"
    
    let p59 = 
        !! @"(forall x. P(x) <=> ~P(f(x))) ==> (exists x. P(x) /\ ~P(f(x)))"
    
    let p60 = 
        !! @"forall x. P(x,f(x)) <=>
                exists y. (forall z. P(z,y) ==> P(z,f(x))) /\ P(x,y)"
    