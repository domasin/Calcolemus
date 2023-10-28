// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus

open Formulas
open Fol

/// <summary>
/// Relation between first order and propositional logic; Herbrand theorem.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module Herbrand = 

    /// <summary>
    /// Evaluates the truth-value of a fol formula, in the sense of 
    /// propositional logic, given a valuation of its atoms.
    /// </summary>
    /// 
    /// <remarks>
    /// It is a variant of the notion of propositional evaluation 
    /// <see cref='M:Calcolemus.Prop.eval``1'/> where the input propositional 
    /// valuation <c>d</c> maps atomic formulas themselves to truth values.
    /// </remarks>
    /// 
    /// <example id="pholds-1">
    /// <code lang="fsharp">
    /// !!"P(x)"
    /// |> pholds (function 
    ///     x when x = !!"P(x)" -> true 
    ///     | _ -> false
    /// )
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="1">Propositional valuation</category>
    val pholds:
      d: (formula<'a> -> bool) -> fm: formula<'a> -> bool

    /// <summary>
    /// Gets the constants for Herbrand base, adding nullary one if necessary. 
    /// </summary>
    /// 
    /// <category index="2">Herbrand models</category>
    val herbfuns:
      fm: formula<fol> -> (string * int) list * (string * int) list

    /// <summary>
    /// Enumerates all ground terms involving <c>n</c> functions.
    /// </summary>
    /// 
    /// <remarks>
    /// If <c>n</c> = 0, it returns the constant terms, otherwise tries all 
    /// possible functions.
    /// </remarks>
    /// 
    /// <example id="groundterms-1">
    /// <code lang="fsharp">
    /// groundterms [!!!"0";!!!"1"] [("f",1);("g",2)] 0
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [``0``; ``1``]
    /// </code>
    /// </example>
    /// 
    /// <example id="groundterms-2">
    /// <code lang="fsharp">
    /// groundterms [!!!"0";!!!"1"] [("f",1);("g",2)] 1
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// [``f(0)``; ``f(1)``; ``g(0,0)``; ``g(0,1)``; ``g(1,0)``; ``g(1,1)``]
    /// </code>
    /// </example>
    /// 
    /// <example id="groundterms-3">
    /// <code lang="fsharp">
    /// groundterms [!!!"0";!!!"1"] [("f",1);("g",2)]
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [``f(f(0))``; ``f(f(1))``; ``f(g(0,0))``; ``f(g(0,1))``; ``f(g(1,0))``;
    ///  ``f(g(1,1))``; ``g(0,f(0))``; ``g(0,f(1))``; ``g(0,g(0,0))``;
    ///  ``g(0,g(0,1))``; ``g(0,g(1,0))``; ``g(0,g(1,1))``; ``g(1,f(0))``;
    ///  ``g(1,f(1))``; ``g(1,g(0,0))``; ``g(1,g(0,1))``; ``g(1,g(1,0))``;
    ///  ``g(1,g(1,1))``; ``g(f(0),0)``; ``g(f(0),1)``; ``g(f(1),0)``; ``g(f(1),1)``;
    ///  ``g(g(0,0),0)``; ``g(g(0,0),1)``; ``g(g(0,1),0)``; ``g(g(0,1),1)``;
    ///  ``g(g(1,0),0)``; ``g(g(1,0),1)``; ``g(g(1,1),0)``; ``g(g(1,1),1)``]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Herbrand models</category>
    val groundterms:
      cntms: term list -> funcs: (string * int) list -> n: int -> term list

    /// <summary>
    /// generates all <c>m</c>-tuples of ground terms involving (in total) 
    /// <c>n</c> functions.
    /// </summary>
    /// 
    /// <example id="groundtuples-1">
    /// <code lang="fsharp">
    /// groundtuples [!!!"0"] [("f",1)] 1 1 // evaluates to [[``f(0)``]]
    /// groundtuples [!!!"0"] [("f",1)] 1 2 // evaluates to [[``0``; ``f(0)``]; [``f(0)``; ``0``]]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Herbrand models</category>
    val groundtuples:
      cntms: term list ->
        funcs: (string * int) list -> n: int -> m: int -> term list list

    /// <summary>
    /// A generic function to be used with different 'herbrand procedures'.
    /// </summary>
    /// 
    /// <remarks>
    /// It tests larger and larger conjunctions of ground instances for 
    /// unsatisfiability, iterating modifier <c>mfn</c> over ground terms 
    /// till <c>tfn</c> fails. 
    /// </remarks>
    /// 
    /// <param name="mfn">The modification function that augments the ground 
    /// instances with a new instance.</param>
    /// <param name="tfn">The satisfiability test to be done.</param>
    /// <param name="fl0">The initial formula in some transformed list 
    /// representation.</param>
    /// <param name="cntms">The constant terms of the formula.</param>
    /// <param name="funcs">The functions (name, arity) of the formula.</param>
    /// <param name="fvs">The free variables of the formula.</param>
    /// <param name="n">The next level of the enumeration to generate.</param>
    /// <param name="fl">The set of ground instances so far.</param>
    /// <param name="tried">The instances tried.</param>
    /// <param name="tuples">The remaining ground instances in the current level.
    /// </param>
    /// 
    /// <category index="2">Herbrand models</category>
    val herbloop:
      mfn: ('a ->
              (formula<fol> -> formula<fol>) ->
              'b list -> 'b list) ->
        tfn: ('b list -> bool) ->
        fl0: 'a ->
        cntms: term list ->
        funcs: (string * int) list ->
        fvs: string list ->
        n: int ->
        fl: 'b list ->
        tried: term list list ->
        tuples: term list list -> term list list

    /// <summary>
    /// <see cref='M:Calcolemus.Herbrand.herbloop``2'/> specific for Gilmore 
    /// procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// In the specific case of the gilmore procedure, the generic herbrand 
    /// loop <see cref='M:Calcolemus.Herbrand.herbloop``2'/> is called with the 
    /// initial formula <c>fl0</c> and the ground instances so far <c>fl</c> 
    /// are maintained in a DNF list representation and the modification 
    /// function applies the instantiation to the starting formula and combines 
    /// the DNFs by distribution.
    /// </remarks>
    /// 
    /// <category index="3">A gilmore-like procedure</category>
    val gilmore_loop:
      (formula<fol> list list ->
         term list ->
         (string * int) list ->
         string list ->
         int ->
         formula<fol> list list ->
         term list list -> term list list -> term list list)

    /// <summary>
    /// Tests the input fol formula <c>fm</c> for validity based on a 
    /// gilmore-like procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The initial formula is generalized, negated and Skolemized, then the 
    /// specific herbrand loop for the gilmore procedure is called to test for 
    /// the unsatisfiability of the transformed formula.
    /// <p></p>
    /// If the test terminates, it reports how many ground instances where 
    /// tried.
    /// </remarks>
    /// 
    /// <category index="3">A gilmore-like procedure</category>
    val gilmore: fm: formula<fol> -> int

    /// <summary>
    /// The modification function (specific to the Davis-Putnam procedure), 
    /// that augments the ground instances with a new one.
    /// </summary>
    /// 
    /// <example id="dp_mfn-1">
    /// This example shows the first generation of ground instance when the set 
    /// is initially empty.
    /// <code lang="fsharp">
    /// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"c"])) []
    /// </code>
    /// Evaluates to <c>[[`P(c)`]; [`~P(f_y(c))`]]</c>.
    /// </example>
    /// 
    /// <example id="dp_mfn-2">
    /// This example shows the second generation of ground instance when the 
    /// nonempty set is augmented.
    /// <code lang="fsharp">
    /// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"f_y(c)"]))     [[!!"P(c)"]; [!!"~P(f_y(c))"]]
    /// </code>
    /// Evaluates to <c>[[`P(c)`]; [`P(f_y(c))`]; [`~P(f_y(c))`]; [`~P(f_y(f_y(c)))`]]</c>.
    /// </example>
    /// 
    /// <param name="cjs0">The initial formula in a list of list representation of  conjunctive normal.</param>
    /// <param name="ifn">The instantiation to be applied to the formula to     generate ground instances.</param>
    /// <param name="cjs">The set of ground instances so far.</param>
    /// 
    /// <returns>
    /// The set of ground instances incremented.
    /// </returns>
    /// 
    /// <category index="4">The Davis-Putnam procedure for first order logic</category>
    val dp_mfn:
      cjs0: 'a list list -> ifn: ('a -> 'b) -> cjs: 'b list list -> 'b list list
        when 'b: comparison

    /// <summary>
    /// <see cref='M:Calcolemus.Herbrand.herbloop``2'/> specific for the 
    /// Davis-Putnam procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// In the specific case of the davis-putnam procedure, the generic 
    /// herbrand loop <see cref='M:Calcolemus.Herbrand.herbloop``2'/> is called 
    /// with the initial formula <c>fl0</c> and the ground instances so far 
    /// <c>fl</c> are maintained in a CNF list representation and each time we 
    /// incorporate a new instance, we check for unsatisfiability using 
    /// <c>dpll</c>.
    /// </remarks>
    /// 
    /// <category index="4">The Davis-Putnam procedure for first order logic</category>
    val dp_loop:
      (formula<fol> list list ->
         term list ->
         (string * int) list ->
         string list ->
         int ->
         formula<fol> list list ->
         term list list -> term list list -> term list list)

    /// <summary>
    /// Tests an input fol formula <c>fm</c> for validity based on the 
    /// Davis-Putnam procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The initial formula is generalized, negated and Skolemized, then the 
    /// specific herbrand loop for the davis-putnam procedure is called to test 
    /// for the unsatisfiability of the transformed formula.
    /// <p></p>
    /// If the test terminates, it reports how many ground instances where 
    /// tried.
    /// </remarks>
    /// 
    /// <category index="4">The Davis-Putnam procedure for first order logic</category>
    val davisputnam: fm: formula<fol> -> int

    /// <summary>
    /// Auxiliary function to redefine the Davis-Putnam procedure to run 
    /// through the list of possibly-needed instances <c>dunno</c>, putting 
    /// them onto the list of needed ones <c>need</c> only if the other 
    /// instances are satisfiable.
    /// </summary>
    /// 
    /// <category index="5">The Davis-Putnam procedure refined</category>
    val dp_refine:
      cjs0: formula<fol> list list ->
        fvs: string list ->
        dunno: term list list -> need: term list list -> term list list

    /// <summary>
    /// <see cref='M:Calcolemus.Herbrand.herbloop``2'/> specific for the 
    /// Davis-Putnam procedure refined.
    /// </summary>
    /// 
    /// <category index="5">The Davis-Putnam procedure refined</category>
    val dp_refine_loop:
      cjs0: formula<fol> list list ->
        cntms: term list ->
        funcs: (string * int) list ->
        fvs: string list ->
        n: int ->
        cjs: formula<fol> list list ->
        tried: term list list ->
        tuples: term list list -> term list list

    /// <summary>
    /// Tests an input fol formula <c>fm</c> for validity based on the 
    /// Davis-Putnam procedure refined. 
    /// </summary>
    /// 
    /// <remarks>
    /// The refined procedure runs through the list of possibly-needed 
    /// instances, putting them onto the list of needed ones only if 
    /// the other instances are satisfiable.
    /// </remarks>
    /// 
    /// <category index="5">The Davis-Putnam procedure refined</category>
    val davisputnam002: fm: formula<fol> -> int