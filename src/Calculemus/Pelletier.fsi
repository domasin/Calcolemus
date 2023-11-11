// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Formulas
open Fol

/// <summary>
/// Some Pelletier problems to compare proof procedures.
/// </summary>
/// 
/// <category index="4">First order logic</category>
[<RequireQualifiedAccess>]
module Pelletier = 

    (* ---------------------------------------------------------------------- *)
    (* Propositional Logic.                                                   *)
    (* ---------------------------------------------------------------------- *)

    /// <summary>
    /// <c>p ==> q &lt;=&gt; ~q ==> ~p</c>
    /// </summary>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p1: formula<fol>

    /// <c>~ ~p &lt;=&gt; p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p2: formula<fol>

    /// <c>~(p ==> q) ==> q ==> p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p3: formula<fol>

    /// <c>~p ==> q &lt;=&gt; ~q ==> p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p4: formula<fol>

    /// <c>(p \/ q ==> p \/ r) ==> p \/ (q ==> r)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p5: formula<fol>

    /// <c>p \/ ~p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p6: formula<fol>

    /// <c>p \/ ~ ~ ~p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p7: formula<fol>

    /// <c>((p ==> q) ==> p) ==> p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p8: formula<fol>

    /// <c>(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p9: formula<fol>

    /// <c>(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p &lt;=&gt; q)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p10: formula<fol>

    /// <c>p &lt;=&gt; p</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p11: formula<fol>

    /// <c>((p &lt;=&gt; q) &lt;=&gt; r) &lt;=&gt; (p &lt;=&gt; (q &lt;=&gt; r))</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p12: formula<fol>

    /// <c>p \/ q /\ r &lt;=&gt; (p \/ q) /\ (p \/ r)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p13: formula<fol>

    /// <c>(p &lt;=&gt; q) &lt;=&gt; (q \/ ~p) /\ (~q \/ p)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p14: formula<fol>

    /// <c>p ==> q &lt;=&gt; ~p \/ q</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p15: formula<fol>

    /// <c>(p ==> q) \/ (q ==> p)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p16: formula<fol>

    /// <c>p /\ (q ==> r) ==> s &lt;=&gt; (~p \/ q \/ s) /\ (~p \/ ~r \/ s)</c>
    /// 
    /// <category index="1">Propositional Logic</category>
    val p17: formula<fol>

    (* ---------------------------------------------------------------------- *)
    (* Monadic Predicate Logic.                                               *)
    (* ---------------------------------------------------------------------- *)

    /// <c>exists y. forall x. P(y) ==> P(x)</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p18: formula<fol>

    /// <c>exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p19: formula<fol>

    /// <c>(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w)) ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p20: formula<fol>

    /// <c>(exists x. P ==> Q(x)) /\ (exists x. Q(x) ==> P) ==> (exists x. P &lt;=&gt; Q(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p21: formula<fol>

    /// <c>(forall x. P &lt;=&gt; Q(x)) ==> (P &lt;=&gt; (forall x. Q(x)))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p22: formula<fol>

    /// <c>(forall x. P \/ Q(x)) &lt;=&gt; P \/ (forall x. Q(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p23: formula<fol>

    /// <c>~(exists x. U(x) /\ Q(x)) /\ (forall x. P(x) ==> Q(x) \/ R(x)) /\ ~(exists x. P(x) ==> (exists x. Q(x))) /\ (forall x. Q(x) /\ R(x) ==> U(x)) ==> (exists x. P(x) /\ R(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p24: formula<fol>

    /// <c>(exists x. P(x)) /\ (forall x. U(x) ==> ~G(x) /\ R(x)) /\ (forall x. P(x) ==> G(x) /\ U(x)) /\ ((forall x. P(x) ==> Q(x)) \/ (exists x. Q(x) /\ P(x))) ==> (exists x. Q(x) /\ P(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p25: formula<fol>

    /// <c>((exists x. P(x)) &lt;=&gt; (exists x. Q(x))) /\ (forall x y. P(x) /\ Q(y) ==> (R(x) &lt;=&gt; U(y))) ==> ((forall x. P(x) ==> R(x)) &lt;=&gt; (forall x. Q(x) ==> U(x)))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p26: formula<fol>

    /// <c>(exists x. P(x) /\ ~Q(x)) /\ (forall x. P(x) ==> R(x)) /\ (forall x. U(x) /\ V(x) ==> P(x)) /\ (exists x. R(x) /\ ~Q(x)) ==> (forall x. U(x) ==> ~R(x)) ==> (forall x. U(x) ==> ~V(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p27: formula<fol>

    /// <c>(forall x. P(x) ==> (forall x. Q(x))) /\ ((forall x. Q(x) \/ R(x)) ==> (exists x. Q(x) /\ R(x))) /\ ((exists x. R(x)) ==> (forall x. L(x) ==> M(x))) ==> (forall x. P(x) /\ L(x) ==> M(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p28: formula<fol>

    /// <c>(exists x. P(x)) /\ (exists x. G(x)) ==> ((forall x. P(x) ==> H(x)) /\ (forall x. G(x) ==> J(x)) &lt;=&gt; (forall x y. P(x) /\ G(y) ==> H(x) /\ J(y)))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p29: formula<fol>

    /// <c>(forall x. P(x) \/ G(x) ==> ~H(x)) /\ (forall x. (G(x) ==> ~U(x)) ==> P(x) /\ H(x)) ==> (forall x. U(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p30: formula<fol>

    /// <c>~(exists x. P(x) /\ (G(x) \/ H(x))) /\ (exists x. Q(x) /\ P(x)) /\ (forall x. ~H(x) ==> J(x)) ==> (exists x. Q(x) /\ J(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p31: formula<fol>

    /// <c>(forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\ (forall x. Q(x) /\ H(x) ==> J(x)) /\ (forall x. R(x) ==> H(x)) ==> (forall x. P(x) /\ R(x) ==> J(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p32: formula<fol>

    /// <c>(forall x. P(a) /\ (P(x) ==> P(b)) ==> P(c)) &lt;=&gt; (forall x. P(a) ==> P(x) \/ P(c)) /\ (P(a) ==> P(b) ==> P(c))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p33: formula<fol>

    /// <c>((exists x. forall y. P(x) &lt;=&gt; P(y)) &lt;=&gt; ((exists x. Q(x)) &lt;=&gt; (forall y. Q(y)))) &lt;=&gt; ((exists x. forall y. Q(x) &lt;=&gt; Q(y)) &lt;=&gt; ((exists x. P(x)) &lt;=&gt; (forall y. P(y))))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p34: formula<fol>

    /// <c>exists x y. P(x,y) ==> (forall x y. P(x,y))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p35: formula<fol>

    (* ---------------------------------------------------------------------- *)
    (*  Full predicate logic (without Identity and Functions)                 *)
    (* ---------------------------------------------------------------------- *)

    /// <c>
    /// (forall x. exists y. J(x,y)) /\
    ///  (forall x. exists y. G(x,y)) /\
    ///  (forall x y. J(x,y) \/ G(x,y) 
    ///  ==> (forall z. J(y,z) \/ G(y,z) ==> H(x, z)))
    ///      ==> (forall x. exists y. H(x,y))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p36: formula<fol>

    /// <c>
    /// (forall z.
    ///     exists w. forall x. exists y. (P(x,z) ==> P(y,w)) /\ P(y,z) /\
    ///     (P(y,w) ==> (exists u. Q(u,w)))) /\
    /// (forall x z. ~P(x,z) ==> (exists y. Q(y,z))) /\
    /// ((exists x y. Q(x,y)) ==> (forall x. R(x,x))) ==>
    /// (forall x. exists y. R(x,y))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p37: formula<fol>

    /// <c>
    /// (forall x.
    ///   P(a) /\ (P(x) ==> (exists y. P(y) /\ R(x,y))) ==>
    ///   (exists z w. P(z) /\ R(x,w) /\ R(w,z))) &lt;=&gt;
    /// (forall x.
    ///   (~P(a) \/ P(x) \/ (exists z w. P(z) /\ R(x,w) /\ R(w,z))) /\
    ///   (~P(a) \/ ~(exists y. P(y) /\ R(x,y)) \/
    ///   (exists z w. P(z) /\ R(x,w) /\ R(w,z))))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p38: formula<fol>

    /// <c>
    /// ~(exists x. forall y. P(y,x) <=> ~P(y,y))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p39: formula<fol>

    /// <c>
    /// (exists y. forall x. P(x,y) <=> P(x,x))
    ///    ==> ~(forall x. exists y. forall z. P(z,y) <=> ~P(z,x))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p40: formula<fol>

    /// <c>
    /// (forall z. exists y. forall x. P(x,y) <=> P(x,z) /\ ~P(x,x))
    ///    ==> ~(exists z. forall x. P(x,z))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p41: formula<fol>

    /// <c>
    /// ~(exists y. forall x. P(x,y) <=> ~(exists z. P(x,z) /\ P(z,x)))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p42: formula<fol>

    /// <c>
    /// (forall x y. Q(x,y) <=> forall z. P(z,x) <=> P(z,y))
    ///    ==> forall x y. Q(x,y) <=> Q(y,x)
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p43: formula<fol>

    /// <c>
    /// (forall x. P(x) ==> (exists y. G(y) /\ H(x,y)) /\
    ///    (exists y. G(y) /\ ~H(x,y))) /\
    ///    (exists x. J(x) /\ (forall y. G(y) ==> H(x,y))) ==>
    ///    (exists x. J(x) /\ ~P(x))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p44: formula<fol>

    /// <c>
    /// (forall x.
    ///   P(x) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y)) ==>
    ///     (forall y. G(y) /\ H(x,y) ==> R(y))) /\
    /// ~(exists y. L(y) /\ R(y)) /\
    /// (exists x. P(x) /\ (forall y. H(x,y) ==>
    ///   L(y)) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y))) ==>
    /// (exists x. P(x) /\ ~(exists y. G(y) /\ H(x,y)))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p45: formula<fol>

    /// <c>
    /// (forall x. P(x) /\ (forall y. P(y) /\ H(y,x) ==> G(y)) ==> G(x)) /\
    ///    ((exists x. P(x) /\ ~G(x)) ==>
    ///    (exists x. P(x) /\ ~G(x) /\
    ///                (forall y. P(y) /\ ~G(y) ==> J(x,y)))) /\
    ///    (forall x y. P(x) /\ P(y) /\ H(x,y) ==> ~J(y,x)) ==>
    ///    (forall x. P(x) ==> G(x))
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p46: formula<fol>

    (* ---------------------------------------------------------------------- *)
    (* Example from Manthey and Bry, CADE-9.                                  *)
    (* ---------------------------------------------------------------------- *)

    /// <c>
    /// lives(agatha) /\ lives(butler) /\ lives(charles) /\
    ///    (killed(agatha,agatha) \/ killed(butler,agatha) \/
    ///    killed(charles,agatha)) /\
    ///    (forall x y. killed(x,y) ==> hates(x,y) /\ ~richer(x,y)) /\
    ///    (forall x. hates(agatha,x) ==> ~hates(charles,x)) /\
    ///    (hates(agatha,agatha) /\ hates(agatha,charles)) /\
    ///    (forall x. lives(x) /\ ~richer(x,agatha) ==> hates(butler,x)) /\
    ///    (forall x. hates(agatha,x) ==> hates(butler,x)) /\
    ///    (forall x. ~hates(x,agatha) \/ ~hates(x,butler) \/ ~hates(x,charles))
    ///    ==> killed(agatha,agatha) /\
    ///        ~killed(butler,agatha) /\
    ///        ~killed(charles,agatha)
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p55: formula<fol>

    /// <c>
    /// P(f((a),b),f(b,c)) /\
    ///    P(f(b,c),f(a,c)) /\
    ///    (forall (x) y z. P(x,y) /\ P(y,z) ==> P(x,z))
    ///    ==> P(f(a,b),f(a,c))
    /// </c>
    /// 
    /// <category index="5">info hol</category>
    val p57: formula<fol>

    (* ---------------------------------------------------------------------- *)
    (* See info-hol, circa 1500.                                              *)
    (* ---------------------------------------------------------------------- *)

    /// <c>
    /// forall P Q R. forall x. exists v. exists w. forall y. forall z.
    ///    ((P(x) /\ Q(y)) ==> ((P(v) \/ R(w))  /\ (R(z) ==> Q(v))))
    /// </c>
    /// 
    /// <category index="5">info hol</category>
    val p58: formula<fol>

    /// <c>
    /// (forall x. P(x) &lt;=&gt; ~P(f(x))) ==> (exists x. P(x) /\ ~P(f(x)))
    /// </c>
    /// 
    /// <category index="5">info hol</category>
    val p59: formula<fol>

    /// <c>
    /// forall x. P(x,f(x)) <=>
    ///            exists y. (forall z. P(z,y) ==> P(z,f(x))) /\ P(x,y)
    /// </c>
    /// 
    /// <category index="5">info hol</category>
    val p60: formula<fol>