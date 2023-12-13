namespace FSharp

module Calculemus.Qelim

val qelim:
  bfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val lift_qelim:
  afn: (string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    nfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    qfn: (string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val cnnf:
  lfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val lfn_dlo: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val dlobasic: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val afn_dlo:
  vars: 'a -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val quelim_dlo: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module Calculemus.Cooper

val zero: Fol.term

val mk_numeral: n: Lib.Num.num -> Fol.term

val dest_numeral: t: Fol.term -> Lib.Num.num

val is_numeral: (Fol.term -> bool)

val numeral1: fn: (Lib.Num.num -> Lib.Num.num) -> n: Fol.term -> Fol.term

val numeral2:
  fn: (Lib.Num.num -> Lib.Num.num -> Lib.Num.num) ->
    m: Fol.term -> n: Fol.term -> Fol.term

val linear_cmul: n: Lib.Num.num -> tm: Fol.term -> Fol.term

val linear_add: vars: string list -> tm1: Fol.term -> tm2: Fol.term -> Fol.term

val linear_neg: tm: Fol.term -> Fol.term

val linear_sub: vars: string list -> tm1: Fol.term -> tm2: Fol.term -> Fol.term

val linear_mul: tm1: Fol.term -> tm2: Fol.term -> Fol.term

val lint: vars: string list -> tm: Fol.term -> Fol.term

val mkatom:
  vars: string list -> p: string -> t: Fol.term -> Formulas.formula<Fol.fol>

val linform:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val posineq: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val formlcm: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lib.Num.num

val adjustcoeff:
  x: Fol.term ->
    l: Lib.Num.Num -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val unitycoeff:
  x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val minusinf:
  x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val divlcm: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lib.Num.num

val bset: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Fol.term list

val linrep:
  vars: string list ->
    x: Fol.term ->
    t: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val cooper:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val operations: (string * (Lib.Num.num -> Lib.Num.num -> bool)) list

val evalc: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val integer_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val relativize:
  r: (string -> Formulas.formula<'a>) ->
    fm: Formulas.formula<'a> -> Formulas.formula<'a>

val natural_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module Calculemus.Complex

val poly_add: vars: string list -> pol1: Fol.term -> pol2: Fol.term -> Fol.term

val poly_ladd: vars: string list -> pol1: Fol.term -> Fol.term -> Fol.term

val poly_neg: _arg1: Fol.term -> Fol.term

val poly_sub: vars: string list -> p: Fol.term -> q: Fol.term -> Fol.term

val poly_mul: vars: string list -> pol1: Fol.term -> pol2: Fol.term -> Fol.term

val poly_lmul: vars: string list -> pol1: Fol.term -> Fol.term -> Fol.term

val poly_pow: vars: string list -> p: Fol.term -> n: int -> Fol.term

val poly_div: vars: string list -> p: Fol.term -> q: Fol.term -> Fol.term

val poly_var: x: string -> Fol.term

val polynate: vars: string list -> tm: Fol.term -> Fol.term

val polyatom:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val coefficients: vars: string list -> _arg1: Fol.term -> Fol.term list

val degree: vars: string list -> p: Fol.term -> int

val is_constant: vars: string list -> p: Fol.term -> bool

val head: vars: string list -> p: Fol.term -> Fol.term

val behead: vars: string list -> _arg1: Fol.term -> Fol.term

val poly_cmul: k: Lib.Num.Num -> p: Fol.term -> Fol.term

val headconst: p: Fol.term -> Lib.Num.num

val monic: p: Fol.term -> Fol.term * bool

val pdivide: (string list -> Fol.term -> Fol.term -> int * Fol.term)

type sign =
    | Zero
    | Nonzero
    | Positive
    | Negative

val swap: swf: bool -> s: sign -> sign

val findsign: sgns: (Fol.term * sign) list -> p: Fol.term -> sign

val assertsign:
  sgns: (Fol.term * sign) list ->
    p: Fol.term * s: sign -> (Fol.term * sign) list

val split_zero:
  sgns: (Fol.term * sign) list ->
    pol: Fol.term ->
    cont_z: ((Fol.term * sign) list -> Formulas.formula<Fol.fol>) ->
    cont_n: ((Fol.term * sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val poly_nonzero:
  vars: string list ->
    sgns: (Fol.term * sign) list -> pol: Fol.term -> Formulas.formula<Fol.fol>

val poly_nondiv:
  vars: string list ->
    sgns: (Fol.term * sign) list ->
    p: Fol.term -> s: Fol.term -> Formulas.formula<Fol.fol>

val cqelim:
  vars: string list ->
    eqs: Fol.term list * neqs: Fol.term list ->
      sgns: (Fol.term * sign) list -> Formulas.formula<Fol.fol>

val init_sgns: (Fol.term * sign) list

val basic_complex_qelim:
  vars: string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val complex_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module Calculemus.Real

val poly_diffn: x: Fol.term -> n: int -> p: Fol.term -> Fol.term

val poly_diff: vars: string list -> p: Fol.term -> Fol.term

val rel_signs: (string * Complex.sign list) list

val testform:
  pmat: (Fol.term * Complex.sign) list -> fm: Formulas.formula<Fol.fol> -> bool

val inferpsign:
  pd: Complex.sign list * qd: Complex.sign list -> Complex.sign list

val condense: ps: Complex.sign list list -> Complex.sign list list

val inferisign: ps: Complex.sign list list -> Complex.sign list list

val dedmatrix:
  cont: (Complex.sign list list -> 'a) -> mat: Complex.sign list list -> 'a

val pdivide_pos:
  vars: string list ->
    sgns: (Fol.term * Complex.sign) list ->
    s: Fol.term -> p: Fol.term -> Fol.term

val split_sign:
  sgns: (Fol.term * Complex.sign) list ->
    pol: Fol.term ->
    cont: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val split_trichotomy:
  sgns: (Fol.term * Complex.sign) list ->
    pol: Fol.term ->
    cont_z: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    cont_pn: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val casesplit:
  vars: string list ->
    dun: Fol.term list ->
    pols: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val delconst:
  vars: string list ->
    dun: Fol.term list ->
    p: Fol.term ->
    ops: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val matrix:
  vars: string list ->
    pols: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val basic_real_qelim:
  vars: string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val real_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val grpterm: tm: Fol.term -> Fol.term

val grpform: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val real_qelim': (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module Calculemus.Grobner

val mmul:
  c1: Lib.Num.Num * m1: int list ->
    c2: Lib.Num.Num * m2: int list -> Lib.Num.Num * int list

val mdiv:
  (Lib.Num.Num * int list -> Lib.Num.Num * int list -> Lib.Num.Num * int list)

val mlcm:
  c1: 'a * m1: 'b list -> c2: 'c * m2: 'b list -> Lib.Num.Num * 'b list
    when 'b: comparison

val morder_lt: m1: int list -> m2: int list -> bool

val mpoly_mmul:
  Lib.Num.Num * int list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_neg: ((Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list)

val mpoly_const:
  vars: 'a list -> c: Lib.Num.Num -> (Lib.Num.Num * int list) list

val mpoly_var:
  vars: 'a list -> x: 'a -> (Lib.Num.Num * int list) list when 'a: equality

val mpoly_add:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_sub:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_mul:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_pow:
  vars: 'a list ->
    l: (Lib.Num.Num * int list) list -> n: int -> (Lib.Num.Num * int list) list

val mpoly_inv: p: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_div:
  p: (Lib.Num.Num * int list) list ->
    q: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpolynate:
  vars: string list -> tm: Fol.term -> (Lib.Num.Num * int list) list

val mpolyatom:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> (Lib.Num.Num * int list) list

val reduce1:
  Lib.Num.Num * int list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val reduceb:
  Lib.Num.Num * int list ->
    pols: (Lib.Num.Num * int list) list list -> (Lib.Num.Num * int list) list

val reduce:
  pols: (Lib.Num.Num * int list) list list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val spoly:
  pol1: (Lib.Num.Num * int list) list ->
    pol2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val grobner:
  basis: (Lib.Num.Num * int list) list list ->
    pairs: ((Lib.Num.Num * int list) list * (Lib.Num.Num * int list) list) list ->
    (Lib.Num.Num * int list) list list

val groebner:
  basis: (Lib.Num.Num * int list) list list ->
    (Lib.Num.Num * int list) list list

val rabinowitsch:
  vars: 'a list ->
    v: 'a -> p: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list
    when 'a: equality

val grobner_trivial: fms: Formulas.formula<Fol.fol> list -> bool

val grobner_decide: fm: Formulas.formula<Fol.fol> -> bool

val term_of_varpow: vars: 'a -> x: string * k: int -> Fol.term

val term_of_varpows: vars: string list -> lis: int list -> Fol.term

val term_of_monomial:
  vars: string list -> c: Lib.Num.num * m: int list -> Fol.term

val term_of_poly:
  vars: string list -> pol: (Lib.Num.num * int list) list -> Fol.term

val grobner_basis:
  vars: string list -> pols: Formulas.formula<Fol.fol> list -> Fol.term list


module Calculemus.Geom

val coordinations: (string * Formulas.formula<Fol.fol>) list

val coordinate: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val invariant:
  x': Fol.term * y': Fol.term ->
    s: string * z: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_translation:
  (string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val invariant_under_rotation:
  string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val originate: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_scaling:
  string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_shearing:
  (string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val pprove:
  vars: string list ->
    triang: Fol.term list ->
    p: Fol.term ->
    degens: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val triangulate:
  vars: string list ->
    consts: Fol.term list -> pols: Fol.term list -> Fol.term list

val wu:
  fm: Formulas.formula<Fol.fol> ->
    vars: string list -> zeros: string list -> Formulas.formula<Fol.fol> list


module Calculemus.Interpolation

val pinterpolate:
  p: Formulas.formula<'a> -> q: Formulas.formula<'a> -> Formulas.formula<'a>
    when 'a: comparison

val urinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val toptermt: fns: (string * int) list -> tm: Fol.term -> Fol.term list

val topterms:
  fns: (string * int) list -> (Formulas.formula<Fol.fol> -> Fol.term list)

val uinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val cinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val interpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val einterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>


module Calculemus.Combining

val real_lang:
  (string * int -> bool) * (string * int -> bool) *
  (Formulas.formula<Fol.fol> -> bool)

val int_lang:
  (string * int -> bool) * (string * int -> bool) *
  (Formulas.formula<Fol.fol> -> bool)

val add_default:
  langs: (('a -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    (('a -> bool) * (string * int -> bool) * (Formulas.formula<Fol.fol> -> bool)) list

val chooselang:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fm: Formulas.formula<Fol.fol> ->
    (string * int -> bool) * (string * int -> bool) * 'a

val listify:
  f: ('a -> ('b -> 'c) -> 'c) -> l: 'a list -> cont: ('b list -> 'c) -> 'c

val homot:
  fn: (string * int -> bool) * pr: 'a * dp: 'b ->
    tm: Fol.term ->
    cont: (Fol.term -> Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'c) ->
    n: Lib.Num.num -> defs: Formulas.formula<Fol.fol> list -> 'c

val homol:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fm: Formulas.formula<Fol.fol> ->
    cont: (Formulas.formula<Fol.fol> ->
             Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b) ->
    n: Lib.Num.num -> defs: Formulas.formula<Fol.fol> list -> 'b

val homo:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list ->
    cont: (Formulas.formula<Fol.fol> list ->
             Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b) ->
    (Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b)

val homogenize:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val belongs:
  fn: (string * int -> bool) * pr: (string * int -> bool) * dp: 'a ->
    fm: Formulas.formula<Fol.fol> -> bool

val langpartition:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val arreq: l: string list -> Formulas.formula<Fol.fol> list

val arrangement: part: string list list -> Formulas.formula<Fol.fol> list

val dest_def: fm: Formulas.formula<Fol.fol> -> string * Fol.term

val redeqs:
  eqs: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val trydps:
  ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
           Formulas.formula<Fol.fol> list) list ->
    fms: Formulas.formula<Fol.fol> list -> bool

val allpartitions: ('a list -> 'a list list list) when 'a: comparison

val nelop_refute001:
  vars: string list ->
    ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
             Formulas.formula<Fol.fol> list) list -> bool

val nelop1001:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fms0: Formulas.formula<Fol.fol> list -> bool

val nelop001:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fm: Formulas.formula<Fol.fol> -> bool

val findasubset: p: ('a list -> 'b) -> m: int -> l: 'a list -> 'b

val findsubset: p: ('a list -> bool) -> l: 'a list -> 'a list

val nelop_refute:
  eqs: Formulas.formula<Fol.fol> list ->
    ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
             Formulas.formula<Fol.fol> list) list -> bool

val nelop1:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fms0: Formulas.formula<Fol.fol> list -> bool

val nelop:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fm: Formulas.formula<Fol.fol> -> bool


module Calculemus.Lcf

/// checks whether a term s occurs as a sub-term of another term t
val occurs_in: s: Fol.term -> t: Fol.term -> bool

/// checks whether a term t occurs free in a formula fm
val free_in: t: Fol.term -> fm: Formulas.formula<Fol.fol> -> bool

/// The Core LCF proof system
/// 
/// The core proof system is the minimum set of inference rules and/or axioms 
/// sound and complete with respect to the defined semantics.
[<AutoOpen>]
module ProofSystem =
    
    type thm = private | Theorem of Formulas.formula<Fol.fol>
    
    /// modusponens (proper inference rule)
    /// 
    /// |- p -> q |- p ==> |- q
    val modusponens: pq: thm -> thm -> thm
    
    /// generalization (proper inference rule)
    /// 
    /// |- p ==> !x. p
    val gen: x: string -> thm -> thm
    
    /// |- p -> (q -> p)
    val axiom_addimp:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p -> q -> r) -> (p -> q) -> (p -> r)
    val axiom_distribimp:
      p: Formulas.formula<Fol.fol> ->
        q: Formulas.formula<Fol.fol> -> r: Formulas.formula<Fol.fol> -> thm
    
    /// |- ((p -> ⊥) -> ⊥) -> p
    val axiom_doubleneg: p: Formulas.formula<Fol.fol> -> thm
    
    /// |- (!x. p -> q) -> (!x. p) -> (!x. q)
    val axiom_allimp:
      x: string ->
        p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- p -> !x. p [provided x not in FV(p)]
    val axiom_impall: x: string -> p: Formulas.formula<Fol.fol> -> thm
    
    /// |- (?x. x = t) [provided x not in FVT(t)]
    val axiom_existseq: x: string -> t: Fol.term -> thm
    
    /// |- t = t
    val axiom_eqrefl: t: Fol.term -> thm
    
    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    val axiom_funcong:
      f: string -> lefts: Fol.term list -> rights: Fol.term list -> thm
    
    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    val axiom_predcong:
      p: string -> lefts: Fol.term list -> rights: Fol.term list -> thm
    
    /// |- (p <-> q) -> p -> q
    val axiom_iffimp1:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p <-> q) -> q -> p
    val axiom_iffimp2:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p -> q) -> (q -> p) -> (p <-> q)
    val axiom_impiff:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- ⊤ <-> (⊥ -> ⊥)
    val axiom_true: thm
    
    /// |- ~p <-> (p -> ⊥)
    val axiom_not: p: Formulas.formula<Fol.fol> -> thm
    
    /// |- p /\ q <-> (p -> q -> ⊥) -> ⊥
    val axiom_and:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- p \/ q <-> ~(~p /\ ~q)
    val axiom_or:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// (?x. p) <-> ~(!x. ~p)
    val axiom_exists: x: string -> p: Formulas.formula<Fol.fol> -> thm
    
    /// maps a theorem back to the formula that it proves
    val concl: thm -> Formulas.formula<Fol.fol>

/// Prints a theorem using a TextWriter.
val fprint_thm: sw: System.IO.TextWriter -> th: ProofSystem.thm -> unit

/// A printer for theorems
val inline print_thm: th: ProofSystem.thm -> unit

/// Theorem to string
val inline sprint_thm: th: ProofSystem.thm -> string


module Calculemus.Lcfprop

val imp_refl: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_unduplicate: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val negatef: fm: Formulas.formula<'a> -> Formulas.formula<'a>

val negativef: fm: Formulas.formula<'a> -> bool

val add_assum:
  p: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_add_assum:
  p: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_insert:
  q: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_swap: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans_th:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    r: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_add_concl:
  r: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_swap_th:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    r: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_swap2: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_mp:
  ith: Lcf.ProofSystem.thm -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_imp1: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_imp2: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_antisym:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_doubleneg: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val ex_falso: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_trans2:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans_chain:
  ths: Lcf.ProofSystem.thm list ->
    th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_truefalse:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_mono_th:
  p: Formulas.formula<Fol.fol> ->
    p': Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    q': Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val truth: Lcf.ProofSystem.thm

val contrapos: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val and_left:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val and_right:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val conjths: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm list

val and_pair:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val shunt: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val unshunt: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_def:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val expand_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val eliminate_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_false_conseqs:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm list

val imp_false_rule: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_true_rule:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_contr:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_front_th: n: int -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_front: n: int -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val lcfptab:
  fms: Formulas.formula<Fol.fol> list ->
    lits: Formulas.formula<Fol.fol> list -> Lcf.ProofSystem.thm

val lcftaut: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm


module Calculemus.Folderived

val eq_sym: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val eq_trans: s: Fol.term -> t: Fol.term -> u: Fol.term -> Lcf.ProofSystem.thm

val icongruence:
  s: Fol.term ->
    t: Fol.term -> stm: Fol.term -> ttm: Fol.term -> Lcf.ProofSystem.thm

val gen_right_th:
  x: string ->
    p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val genimp: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val gen_right: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val exists_left_th:
  x: string ->
    p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val exists_left: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val subspec: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val subalpha: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val isubst:
  s: Fol.term ->
    t: Fol.term ->
    sfm: Formulas.formula<Fol.fol> ->
    tfm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val alpha: z: string -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val ispec: t: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val spec: t: Fol.term -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm


module Calculemus.Lcffol

val unify_complementsf:
  env: Lib.FPF.func<string,Fol.term> ->
    Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
      Lib.FPF.func<string,Fol.term>

val use_laterimp:
  i: Formulas.formula<Fol.fol> ->
    fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_false_rule':
  th: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val imp_true_rule':
  th1: ('a -> Lcf.ProofSystem.thm) ->
    th2: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val imp_front':
  n: int -> thp: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val add_assum':
  fm: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    (Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm

val eliminate_connective':
  fm: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    (Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm

val spec':
  y: Fol.term ->
    fm: Formulas.formula<Fol.fol> ->
    n: int ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    e: (Fol.term -> Fol.term) * s: 'a -> Lcf.ProofSystem.thm

val ex_falso':
  fms: Formulas.formula<Fol.fol> list ->
    e: (Fol.term -> Fol.term) * s: Formulas.formula<Fol.fol> ->
      Lcf.ProofSystem.thm

val complits':
  Formulas.formula<Fol.fol> list * lits: Formulas.formula<Fol.fol> list ->
    i: int ->
    e: (Fol.term -> Fol.term) * s: Formulas.formula<Fol.fol> ->
      Lcf.ProofSystem.thm

val deskol':
  skh: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    e: (Fol.term -> Fol.term) * s: 'a -> Lcf.ProofSystem.thm

val lcftab:
  skofun: (Formulas.formula<Fol.fol> -> Fol.term) ->
    fms: Formulas.formula<Fol.fol> list * lits: Formulas.formula<Fol.fol> list *
    n: int ->
      cont: (((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
                Lcf.ProofSystem.thm) ->
               Lib.FPF.func<string,Fol.term> *
               (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a) ->
      Lib.FPF.func<string,Fol.term> *
      (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a

val quantforms:
  e: bool -> fm: Formulas.formula<'a> -> Formulas.formula<'a> list
    when 'a: comparison

val skolemfuns:
  fm: Formulas.formula<Fol.fol> -> (Formulas.formula<Fol.fol> * Fol.term) list

val form_match:
  Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
    env: Lib.FPF.func<string,Fol.term> -> Lib.FPF.func<string,Fol.term>

val lcfrefute:
  fm: Formulas.formula<Fol.fol> ->
    n: int ->
    cont: (((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
              Lcf.ProofSystem.thm) ->
             Lib.FPF.func<string,Fol.term> *
             (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a) -> 'a

val mk_skol:
  Formulas.formula<Fol.fol> * fx: Fol.term ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val simpcont:
  thp: ((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> -> 'a) ->
    env: Lib.FPF.func<string,Fol.term> *
    sks: (Formulas.formula<Fol.fol> * Fol.term) list * k: 'b -> 'a

val elim_skolemvar: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val deskolcont:
  thp: ((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
          Lcf.ProofSystem.thm) ->
    env: Lib.FPF.func<string,Fol.term> *
    sks: (Formulas.formula<Fol.fol> * Fol.term) list * k: 'a ->
      Lcf.ProofSystem.thm

val lcffol: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm


module Calculemus.Tactics

type goals =
    | Goals of
      ((string * Formulas.formula<Fol.fol>) list * Formulas.formula<Fol.fol>) list *
      (Lcf.ProofSystem.thm list -> Lcf.ProofSystem.thm)

val fprint_goal: sw: System.IO.TextWriter -> (goals -> unit)

val inline print_goal: g: goals -> unit

val inline sprint_goal: g: goals -> string

val set_goal: p: Formulas.formula<Fol.fol> -> goals

val extract_thm: gls: goals -> Lcf.ProofSystem.thm

val tac_proof: g: goals -> prf: (goals -> goals) list -> Lcf.ProofSystem.thm

val prove:
  p: Formulas.formula<Fol.fol> ->
    prf: (goals -> goals) list -> Lcf.ProofSystem.thm

val conj_intro_tac: goals -> goals

val jmodify: jfn: ('a list -> 'b) -> tfn: ('a -> 'a) -> 'a list -> 'b

val gen_right_alpha:
  y: string -> x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val forall_intro_tac: y: string -> goals -> goals

val right_exists:
  x: string ->
    t: Fol.term -> p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val exists_intro_tac: t: Fol.term -> goals -> goals

val imp_intro_tac: s: string -> goals -> goals

val assumptate: goals -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val firstassum:
  asl: ('a * Formulas.formula<Fol.fol>) list -> Lcf.ProofSystem.thm
    when 'a: equality

val using:
  ths: Lcf.ProofSystem.thm list -> p: 'a -> g: goals -> Lcf.ProofSystem.thm list

val assumps:
  asl: ('a * Formulas.formula<Fol.fol>) list -> ('a * Lcf.ProofSystem.thm) list

val by: hyps: string list -> p: 'a -> goals -> Lcf.ProofSystem.thm list

val justify:
  byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> p: Formulas.formula<Fol.fol> -> g: goals -> Lcf.ProofSystem.thm

val proof:
  tacs: (goals -> goals) list ->
    p: Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list

val at: once: 'a -> p: 'b -> gl: 'c -> 'd list

val once: 'a list

val auto_tac:
  byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val lemma_tac:
  s: string ->
    p: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val exists_elim_tac:
  l: string ->
    fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val ante_disj:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val disj_elim_tac:
  l: string ->
    fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val multishunt: i: int -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val assume: lps: (string * Formulas.formula<Fol.fol>) list -> goals -> goals

val note:
  l: string * p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val have:
  p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val so:
  tac: ('a -> ('b -> 'c -> goals -> Lcf.ProofSystem.thm list) -> 'd) ->
    arg: 'a -> byfn: ('b -> 'c -> goals -> Lcf.ProofSystem.thm list) -> 'd

val fix: (string -> goals -> goals)

val consider:
  x: string * p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val take: (Fol.term -> goals -> goals)

val cases:
  fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val conclude:
  p: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> gl: goals -> goals

val our:
  thesis: 'a ->
    byfn: ('b -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'b -> gl: goals -> goals

val thesis: string

val qed: gl: goals -> goals

val test001: n: 'a -> (Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm)

val double_th: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val testcase: n: int -> Lcf.ProofSystem.thm

val test002: n: int -> Lcf.ProofSystem.thm * Formulas.formula<Fol.fol>


module Calculemus.Limitations

val numeral: n: Lib.Num.Num -> Fol.term

val number: s: string -> Lib.Num.Num

val pair: x: Lib.Num.Num -> y: Lib.Num.Num -> Lib.Num.Num

val gterm: tm: Fol.term -> Lib.Num.Num

val gform: fm: Formulas.formula<Fol.fol> -> Lib.Num.Num

val gnumeral: n: Lib.Num.Num -> Lib.Num.Num

val diag001: s: string -> string

val phi001: string

val qdiag001: s: 'a -> (string -> string -> string)

val phi002: (string -> string -> string)

val diag002:
  x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val qdiag002:
  x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val dtermval: v: Lib.FPF.func<string,Lib.Num.Num> -> tm: Fol.term -> Lib.Num.Num

val dholds:
  v: Lib.FPF.func<string,Lib.Num.Num> -> fm: Formulas.formula<Fol.fol> -> bool

val dhquant:
  pred: ((Lib.Num.Num -> bool) -> Lib.Num.Num list -> bool) ->
    v: Lib.FPF.func<string,Lib.Num.Num> ->
    x: string ->
    y: string ->
    a: string -> t: Fol.term -> p: Formulas.formula<Fol.fol> -> bool

type formulaclass =
    | Sigma
    | Pi
    | Delta

val opp: _arg1: formulaclass -> formulaclass

val classify: c: formulaclass -> n: int -> fm: Formulas.formula<Fol.fol> -> bool

val veref:
  sign: (bool -> bool) ->
    m: Lib.Num.num ->
    v: Lib.FPF.func<string,Lib.Num.Num> -> fm: Formulas.formula<Fol.fol> -> bool

val verefboundquant:
  m: Lib.Num.num ->
    v: Lib.FPF.func<string,Lib.Num.Num> ->
    x: string ->
    y: string ->
    a: string ->
    t: Fol.term -> sign: (bool -> bool) -> p: Formulas.formula<Fol.fol> -> bool

val sholds:
  (Lib.Num.num ->
     Lib.FPF.func<string,Lib.Num.Num> -> Formulas.formula<Fol.fol> -> bool)

val sigma_bound: fm: Formulas.formula<Fol.fol> -> Lib.Num.Num

type symbol =
    | Blank
    | One

type direction =
    | Left
    | Right
    | Stay

type tape = | Tape of int * Lib.FPF.func<int,symbol>

val look: tape -> symbol

val write: s: symbol -> tape -> tape

val move: dir: direction -> tape -> tape

type config = | Config of int * tape

val run:
  prog: Lib.FPF.func<(int * symbol),(symbol * direction * int)> ->
    config: config -> config

val input_tape: (int list -> tape)

val output_tape: tape: tape -> int

val exec:
  prog: Lib.FPF.func<(int * symbol),(symbol * direction * int)> ->
    args: int list -> int

val robinson: Formulas.formula<Fol.fol>

val suc_inj: Lcf.ProofSystem.thm

val num_cases: Lcf.ProofSystem.thm

val mul_suc: Lcf.ProofSystem.thm

val mul_0: Lcf.ProofSystem.thm

val lt_def: Lcf.ProofSystem.thm

val le_def: Lcf.ProofSystem.thm

val add_suc: Lcf.ProofSystem.thm

val add_0: Lcf.ProofSystem.thm

val right_spec: t: Fol.term -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_mp:
  ith: Lcf.ProofSystem.thm -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_imp_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_sym: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val robop: tm: Fol.term -> Lcf.ProofSystem.thm

val robeval: tm: Fol.term -> Lcf.ProofSystem.thm

val robinson_consequences: Formulas.formula<Fol.fol>

val robinson_thm: Lcf.ProofSystem.thm

val suc_inj_false: Lcf.ProofSystem.thm

val suc_0_r: Lcf.ProofSystem.thm

val suc_0_l: Lcf.ProofSystem.thm

val num_lecases: Lcf.ProofSystem.thm

val lt_suc: Lcf.ProofSystem.thm

val lt_0: Lcf.ProofSystem.thm

val le_suc: Lcf.ProofSystem.thm

val le_0: Lcf.ProofSystem.thm

val expand_nlt: Lcf.ProofSystem.thm

val expand_nle: Lcf.ProofSystem.thm

val expand_lt: Lcf.ProofSystem.thm

val expand_le: Lcf.ProofSystem.thm

val rob_eq: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val rob_nen: s: Fol.term * t: Fol.term -> Lcf.ProofSystem.thm

val rob_ne: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val introduce_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val elim_bex: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val sigma_elim: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val boundquant_step:
  th0: Lcf.ProofSystem.thm -> th1: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val sigma_prove: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val bounded_prove:
  a: string * x: string * t: Fol.term * q: Formulas.formula<Fol.fol> ->
    Lcf.ProofSystem.thm

val boundednum_prove:
  a: string * x: string * t: Fol.term * q: Formulas.formula<Fol.fol> ->
    Lcf.ProofSystem.thm

