module Tests.Pelletier

open FolAutomReas.Fol

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

let p45 = 
    !! @"(forall x. P(x) /\ (forall y. G(y) /\ H(x,y) ==> J(x,y))
    ==> (forall y. G(y) /\ H(x,y) ==> R(y))) /\
    ~(exists y. L(y) /\ R(y)) /\
    (exists x. P(x) /\ (forall y. H(x,y) ==> L(y)) /\
    (forall y. G(y) /\ H(x,y) ==> J(x,y)))
    ==> (exists x. P(x) /\ ~(exists y. G(y) /\ H(x,y)))"