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

    /// <c>~(exists x. U(x) /\ Q(x)) /\ (forall x. P(x) ==> Q(x) \/ R(x)) /\ ~(exists x. P(x) ==> (exists x. Q(x))) /\ (forall x. Q(x) /\ R(x) ==> U(x)) ==> (exists x. P(x) /\ R(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p24: formula<fol>

    /// <c>(forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\ (forall x. Q(x) /\ H(x) ==> J(x)) /\ (forall x. R(x) ==> H(x)) ==> (forall x. P(x) /\ R(x) ==> J(x))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p32: formula<fol>

    /// <c>((exists x. forall y. P(x) &lt;=&gt; P(y)) &lt;=&gt; ((exists x. Q(x)) &lt;=&gt; (forall y. Q(y)))) &lt;=&gt; ((exists x. forall y. Q(x) &lt;=&gt; Q(y)) &lt;=&gt; ((exists x. P(x)) &lt;=&gt; (forall y. P(y))))</c>
    /// 
    /// <category index="2">Monadic Predicate Logic</category>
    val p34: formula<fol>

    (* ---------------------------------------------------------------------- *)
    (*  Full predicate logic (without Identity and Functions)                 *)
    (* ---------------------------------------------------------------------- *)

    /// <c>
    /// (forall x. exists y. J(x,y)) /\
    ///  (forall x. exists y. G(x,y)) /\
    ///  (forall x y. J(x,y) \/ G(x,y) 
    ///  ==> (forall z. J(y,z) \/ G(y,z) ==> H(x, z)))
    ///      ==> (forall x. exists y. H(x,y))`
    /// </c>
    /// 
    /// <category index="3">Full predicate logic (without Identity and Functions)</category>
    val p36: formula<fol>

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

    (* ---------------------------------------------------------------------- *)
    (* Example from Manthey and Bry, CADE-9.                                  *)
    (* ---------------------------------------------------------------------- *)

    // val p55

    (* ---------------------------------------------------------------------- *)
    (* See info-hol, circa 1500.                                              *)
    (* ---------------------------------------------------------------------- *)

    /// <c>
    /// (forall x. P(x) &lt;=&gt; ~P(f(x))) ==> (exists x. P(x) /\ ~P(f(x)))
    /// </c>
    /// 
    /// <category index="5">info hol</category>
    val p59: formula<fol>