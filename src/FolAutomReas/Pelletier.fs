/// Some Pelletier problems to compare proof procedures.
module FolAutomReas.Pelletier

open FolAutomReas.Fol

/// ¬¬(∃x. ∀y z. (P(y) ⟶ Q(z)) ⟶ (P(x) ⟶ Q(x)))
let p19 = 
    !!"exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)"

/// (∀x y. ∃z. ∀w. (P(x) ∧ Q(y) ⟶ R(z) ∧ S(w))) ⟶ (∃x y. P(x) ∧ Q(y)) ⟶ (∃z. R(z))
let p20 = 
    !!"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w))
    ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))"

let p24 = 
    !! @"~(exists x. U(x) /\ Q(x)) /\
    (forall x. P(x) ==> Q(x) \/ R(x)) /\
    ~(exists x. P(x) ==> (exists x. Q(x))) /\
    (forall x. Q(x) /\ R(x) ==> U(x))
    ==> (exists x. P(x) /\ R(x))"

let p36 = 
    !! @"(forall x. exists y. J(x,y)) /\
        (forall x. exists y. G(x,y)) /\
        (forall x y. J(x,y) \/ G(x,y) ==> (forall z. J(y,z) \/ G(y,z) ==> H(x, z)))
    ==> (forall x. exists y. H(x,y))"

let p38 = 
    !! @"(forall x.
        P(a) /\ (P(x) ==> (exists y. P(y) /\ R(x,y))) ==>
        (exists z w. P(z) /\ R(x,w) /\ R(w,z))) <=>
        (forall x.
        (~P(a) \/ P(x) \/ (exists z w. P(z) /\ R(x,w) /\ R(w,z))) /\
        (~P(a) \/ ~(exists y. P(y) /\ R(x,y)) \/
        (exists z w. P(z) /\ R(x,w) /\ R(w,z))))"

/// ¬ (∃x. ∀y. P(y,x) ⟷ (¬P(y,y))
let p39 = 
    !! @"~(exists x. forall y. P(y,x) <=> ~P(y,y))"

/// ¬ (∃y. ∀x. P(x,y) ⟷ (∃z. P(x,z) ∧ P(z,y))
let p42 = 
    !! @"~(exists y. forall x. P(x,y) <=> ~(exists z. P(x,z) /\ P(z,x)))"

/// (∀x y. Q(x,y) ⟷ ∀z. P(z,x) ⟷ P(z,y)) ⟶ ∀x y. Q(x,y) ⟷ Q(y,x)
let p43 = 
    !! @"(forall x y. Q(x,y) <=> forall z. P(z,x) <=> P(z,y))
   ==> forall x y. Q(x,y) <=> Q(y,x)"

/// (∀x. P(x) ⟶ (∃y. G(y) ∧ H(x,y) ∧ 
/// (∃y. G(y) ∧ ~ H(x,y)))) ∧ 
/// (∃x. J(x) ∧ (∀y. G(y) ⟶ H(x,y))) 
/// ⟶ (∃x. j(x) ∧ ¬f(x))
let p44 = 
    !! @"(forall x. P(x) ==> (exists y. G(y) /\ H(x,y)) /\
        (exists y. G(y) /\ ~H(x,y))) /\
        (exists x. J(x) /\ (forall y. G(y) ==> H(x,y)))
        ==> (exists x. J(x) /\ ~P(x))"

let p45 = 
    !! @"(forall x. P(x) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y))
    ==> (forall y. G(y) /\ H(x,y) ==> R(y))) /\
    ~(exists y. L(y) /\ R(y)) /\
    (exists x. P(x) /\ (forall y. H(x,y) ==> L(y)) /\
    (forall y. G(y) /\ H(x,y) ==> J(x,y)))
    ==> (exists x. P(x) /\ ~(exists y. G(y) /\ H(x,y)))"

/// (∀ x. P(x) ⟷ ~P(f(x))) ⟶ (∃ x. P(x) ∧ ~P(f(x)))
let p59 = 
    !! @"(forall x. P(x) <=> ~P(f(x))) ==> (exists x. P(x) /\ ~P(f(x)))"

/// ∀x. P(x,f(x)) ⟷ (∃y. (∀z. P(z,y) ⟶ P (z,f(x))) ∧ P(x,y))
let p60 = 
    !! @"forall x. P(x,f(x)) <=>
             exists y. (forall z. P(z,y) ==> P(z,f(x))) /\ P(x,y)"