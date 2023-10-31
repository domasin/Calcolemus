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
    /// Evaluates the truth-value of a quantifier-free formula, in the 
    /// sense of propositional logic, given a valuation of its atoms.
    /// </summary>
    /// 
    /// <remarks>
    /// It is a variant of the notion of propositional evaluation 
    /// <see cref='M:Calcolemus.Prop.eval``1'/> where the input propositional 
    /// valuation <c>d</c> maps atomic formulas themselves to truth values.
    /// </remarks>
    /// 
    /// <param name="d">The propositional valuation that maps atomic formulas to truth values.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <return>
    /// true, if the formula is quantifier-free and true in the given 
    /// valuation; false if the formula is quantifier-free and true in the 
    /// given valuation.
    /// </return>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message 'Not part of propositional logic.' when the input formula contains quantifiers.</exception>
    /// 
    /// <example id="pholds-1">
    /// <code lang="fsharp">
    /// !!"P(x) /\ Q(x)"
    /// |> pholds (function 
    ///     | x when x = !!"P(x)" -> true 
    ///     | x when x = !!"Q(x)" -> true 
    ///     | _ -> false
    /// )
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="pholds-2">
    /// <code lang="fsharp">
    /// !!"P(x) /\ Q(x)"
    /// |> pholds (function 
    ///     | x when x = !!"P(x)" -> true 
    ///     | x when x = !!"Q(x)" -> false 
    ///     | _ -> false
    /// )
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="pholds-3">
    /// <code lang="fsharp">
    /// !!"forall x. P(x) /\ Q(x)"
    /// |> pholds (function 
    ///     | x when x = !!"P(x)" -> true
    ///     | x when x = !!"Q(x)" -> true
    ///     | _ -> false
    /// )
    /// </code>
    /// Throws <c>System.Exception: Not part of propositional logic.</c>.
    /// </example>
    /// 
    /// <category index="1">Propositional valuation</category>
    val pholds:
      d: (formula<'a> -> bool) -> fm: formula<'a> -> bool

    /// <summary>
    /// Returns the functions in the formula <c>fm</c> separated in nullary and 
    /// non, and adding nullary one if necessary. 
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The pair of lists of functions in the formula separated in nullary and 
    /// non.
    /// </returns>
    /// 
    /// <example id="herbfuns-1">
    /// <code lang="fsharp">
    /// !!"forall x. P(x) /\ (Q(f(y)) ==> R(g(x,y),z))"
    /// |> herbfuns
    /// </code>
    /// Evaluates to <c>([("c", 0)], [("f", 1); ("g", 2)])</c>.
    /// </example>
    /// 
    /// <category index="2">Herbrand models</category>
    val herbfuns:
      fm: formula<fol> -> (string * int) list * (string * int) list

    /// <summary>
    /// Returns all ground terms that can be created from constant 
    /// terms <c>cntms</c> and functions symbols <c>funcs</c> involving 
    /// <c>n</c> function symbols.
    /// </summary>
    /// 
    /// <remarks>
    /// If <c>n</c> = 0, it returns the constant terms, otherwise tries all 
    /// possible functions.
    /// </remarks>
    /// 
    /// <param name="cntms">The input list of constant terms.</param>
    /// <param name="funcs">The input list of function name-arity pairs.</param>
    /// <param name="n">The number of function symbols the resulting ground terms will contain.</param>
    /// 
    /// <returns>
    /// The input <c>cntms</c> itself, if <c>n</c> is <c>0</c>; otherwise, all 
    /// the ground terms that can be created with the give <c>funcs</c> and 
    /// that contain only <c>n</c> function symbols.
    /// </returns>
    /// 
    /// <example id="groundterms-1">
    /// <code lang="fsharp">
    /// groundterms !!!>["0";"1"] [("f",1);("g",2)] 0
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [``0``; ``1``]
    /// </code>
    /// </example>
    /// 
    /// <example id="groundterms-2">
    /// <code lang="fsharp">
    /// groundterms !!!>["0";"1"] [("f",1);("g",2)] 1
    /// </code>
    /// Evaluates to
    /// <code lang="fsharp">
    /// [``f(0)``; ``f(1)``; ``g(0,0)``; 
    ///  ``g(0,1)``; ``g(1,0)``; ``g(1,1)``]
    /// </code>
    /// </example>
    /// 
    /// <example id="groundterms-3">
    /// <code lang="fsharp">
    /// groundterms !!!>["0";"1"] [("f",1);("g",2)] 2
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [``f(f(0))``; ``f(f(1))``; ``f(g(0,0))``; 
    ///  ``f(g(0,1))``; ``f(g(1,0))``; ``f(g(1,1))``; 
    ///  ``g(0,f(0))``; ``g(0,f(1))``; ``g(0,g(0,0))``;
    ///  ``g(0,g(0,1))``; ``g(0,g(1,0))``; ``g(0,g(1,1))``; 
    ///  ``g(1,f(0))``; ``g(1,f(1))``; ``g(1,g(0,0))``; 
    ///  ``g(1,g(0,1))``; ``g(1,g(1,0))``;``g(1,g(1,1))``; 
    ///  ``g(f(0),0)``; ``g(f(0),1)``; ``g(f(1),0)``; 
    ///  ``g(f(1),1)``; ``g(g(0,0),0)``; ``g(g(0,0),1)``; 
    ///  ``g(g(0,1),0)``; ``g(g(0,1),1)``;``g(g(1,0),0)``; 
    ///  ``g(g(1,0),1)``; ``g(g(1,1),0)``; ``g(g(1,1),1)``]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Herbrand models</category>
    val groundterms:
      cntms: term list -> funcs: (string * int) list -> n: int -> term list

    /// <summary>
    /// Returns all the <c>m</c>-tuples of ground terms that can be created 
    /// from constant terms <c>cntms</c> and functions symbols <c>funcs</c> 
    /// involving <c>n</c> function symbols.
    /// </summary>
    /// 
    /// <param name="cntms">The input list of constant terms.</param>
    /// <param name="funcs">The input list of function name-arity pairs.</param>
    /// <param name="m">The number of element of the resulting ground terms tuples.</param>
    /// <param name="n">The number of function symbols the resulting ground terms will contain.</param>
    /// 
    /// <returns>
    /// All the <c>m</c>-tuples of ground terms that can be created from then 
    /// input <c>cntms</c> and <c>funcs</c> where each ground term contain  
    /// only <c>n</c> function symbols.
    /// </returns>
    /// 
    /// <example id="groundtuples-1">
    /// <code lang="fsharp">
    /// groundtuples !!!>["0";"1"] [("f",1);("g",2)] 0 2
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[``0``; ``0``]; [``0``; ``1``]; 
    ///  [``1``; ``0``]; [``1``; ``1``]]
    /// </code>
    /// </example>
    /// 
    /// <example id="groundtuples-2">
    /// <code lang="fsharp">
    /// groundtuples !!!>["0";"1"] [("f",1);("g",2)] 2 3
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[``0``; ``0``; ``f(f(0))``]; 
    /// [``0``; ``0``; ``f(f(1))``];
    /// [``0``; ``0``; ``f(g(0,0))``]; 
    /// [``0``; ``0``; ``f(g(0,1))``];
    /// [``0``; ``0``; ``f(g(1,0))``]; 
    /// ...
    /// [``1``; ``g(g(0,1),0)``; ``0``];
    /// ...
    /// [``g(g(1,1),1)``; ``1``; ``1``]]
    /// </code>
    /// </example>
    /// 
    /// <category index="2">Herbrand models</category>
    val groundtuples:
      cntms: term list ->
        funcs: (string * int) list -> n: int -> m: int -> term list list

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses.
    /// </summary>
    /// 
    /// <remarks>
    /// This is a generic function to be used with different 'herbrand 
    /// procedures'.
    /// <p></p>
    /// It tests larger and larger conjunctions of ground instances for 
    /// unsatisfiability, iterating the given modifier function <c>mfn</c> over 
    /// ground terms till the given test function <c>tfn</c> fails. 
    /// </remarks>
    /// 
    /// <param name="mfn">The modification function that augments the ground instances with a new instance.</param>
    /// <param name="tfn">The satisfiability test to be done.</param>
    /// <param name="fl0">The input set of clauses: i.e. a formula in some transformed list representation.</param>
    /// <param name="cntms">The constant terms to generate the ground terms.</param>
    /// <param name="funcs">The function symbols (name, arity) to generate the ground terms.</param>
    /// <param name="fvs">The free variables to instantiate to generate the ground terms.</param>
    /// <param name="n">The number of function symbols the new ground terms to be generated should contain.</param>
    /// <param name="fl">The set of ground instances so far.</param>
    /// <param name="tried">The instances tried.</param>
    /// <param name="tuples">The remaining ground instances in the current level.</param>
    /// 
    /// <returns>
    /// The list of ground tuples generated and prints to the <c>stdout</c> how 
    /// many ground instances were tried, if the procedure terminates (thus 
    /// indicating that the formula is valid); otherwise, loops generating and 
    /// testing larger and larger ground instances till the memory is full. 
    /// </returns>
    /// 
    /// <note>
    /// See the specific implementations 
    /// <see cref='M:Calcolemus.Herbrand.gilmore_loop'/>, 
    /// <see cref='M:Calcolemus.Herbrand.dp_loop'/>, 
    /// <see cref='M:Calcolemus.Herbrand.dp_refine_loop'/> for examples.
    /// </note>
    /// 
    /// <category index="2">Herbrand procedures</category>
    val herbloop:
      mfn: ('a ->
              (formula<fol> -> formula<fol>) ->
              list<list<formula<fol>>>-> list<list<formula<fol>>>) ->
        tfn: (list<list<formula<fol>>> -> bool) ->
        fl0: 'a ->
        cntms: term list ->
        funcs: (string * int) list ->
        fvs: string list ->
        n: int ->
        fl: list<list<formula<fol>>> ->
        tried: term list list ->
        tuples: term list list -> term list list

    /// <summary>
    /// Generates the ground instances for the Gilmore procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// Updates the ground instance <c>djs</c> of the initial formula in 
    /// DNF 'clausal' form <c>djs0</c> with the new ground terms generated from 
    /// the given instantiation <c>ifn</c>.
    /// <p></p>
    /// Since we are in a DNF context, a list of clauses here is an iterated 
    /// disjunction of conjunctions and so each clause is a conjunction.
    /// The ground instances generated for each of the conjunctions are added 
    /// as new conjuncts to the corresponding conjunction, deleting those  
    /// that become contradictory due to the presence of complementary 
    /// literals.
    /// </remarks>
    /// 
    /// <param name="djs0">The input formula in DNF 'clausal' form.</param>
    /// <param name="ifn">The instantiation function.</param>
    /// <param name="djs">The ground instance of the input formula so far.</param>
    /// <returns>
    /// The updated ground instance of the input formula.
    /// </returns>
    /// 
    /// <example id="gilmore_mfn-1">
    /// In this example each conjunct in the original formula 
    /// <c>P(f(x)) \/ ~P(y)</c> is just a literal.
    /// As new ground terms are generated, each conjunction becomes the 
    /// conjunction of all ground instances generated so far for the 
    /// corresponding original literal.
    /// <code lang="fsharp">
    /// gilmore_mfn !!>>[["P(f(x))"]; ["~P(y)"]] 
    ///   (subst (fpf ["x"; "y"] !!!>["c";"f(c)"])) 
    ///   !!>>[["P(f(c))"]; ["~P(c)"]] 
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// [[`P(f(c))`]; [`P(f(c))`; `~P(c)`]; [`~P(c)`; `~P(f(c))`]]
    /// </code>
    /// Corresponding to the formula <c>`P(f(c)) \/ P(f(c)) /\ ~P(c) \/ ~P(c) /\ ~P(f(c))`</c>.
    /// <p></p>
    /// Note that the input ground instance <c>[["P(f(c))"]; ["~P(c)"]]</c> 
    /// must have been generated by an instantiation that mapped both <c>x</c> 
    /// and <c>y</c> to <c>c</c> while the new given instantiation 
    /// maps <c>y</c> to <c>f(c)</c>.
    /// <p></p>
    /// Note also that the conjunct <c>[`P(f(c))`; `~P(f(c))`]</c> has been 
    /// removed from the output, since contradictory.
    /// </example>
    /// 
    /// <category index="4">A gilmore-like procedure</category>
    val gilmore_mfn:
      djs0: list<list<formula<fol>>> ->
      ifn : (formula<fol> -> formula<fol>) ->
      djs : list<list<formula<fol>>>
          -> list<list<formula<fol>>>

    /// <summary>
    /// Gilmore test function: tests if the input set of clauses is nonempty.
    /// </summary>
    /// 
    /// <remarks>
    /// It simply tests that the set of DNF 'clauses' is nonempty, because the 
    /// contradiction checking is done by the modification function.
    /// </remarks>
    /// 
    /// <param name="djs">The input formula (more precisely its ground instance so far) in a DNF 'clausal' form.</param>
    /// 
    /// <returns>
    /// true, if the input is nonempty; otherwise, false
    /// </returns>
    /// 
    /// <example id="gilmore_tfn-1">
    /// <code lang="fsharp">
    /// !!>>[["P(f(c))"]; ["P(f(c))"; "~P(c)"]; ["P(f(c))"; "~P(f(c))"];
    ///    ["~P(c)"; "~P(f(c))"]]
    /// |> gilmore_tfn
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="gilmore_tfn-2">
    /// <code lang="fsharp">
    /// !!>>[]
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="4">A gilmore-like procedure</category>
    val gilmore_tfn:
      djs: list<'a>
        -> bool

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses with a Gilmore-like 
    /// procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The input set of clauses <c>fl0</c> is intended to represent a DNF 
    /// formula and <c>cntms</c>, <c>funcs</c> and <c>fvs</c> are supposed to 
    /// be the constant and function symbols and the free variables of the 
    /// formula, respectively.
    /// 
    /// The function implements an 
    /// <see cref='M:Calcolemus.Herbrand.herbloop``2'/> specific for the 
    /// Gilmore procedure, calling it with the specific gilmore modification 
    /// and test functions.
    /// </remarks>
    /// 
    /// <param name="fl0">The input set of clauses.</param>
    /// <param name="cntms">The constant terms to generate the ground terms.</param>
    /// <param name="funcs">The function symbols (name, arity) to generate the ground terms.</param>
    /// <param name="fvs">The free variables to instantiate to generate the ground terms.</param>
    /// <param name="n">The number of function symbols the new ground terms to be generated should contain.</param>
    /// <param name="fl">The set of ground instances so far.</param>
    /// <param name="tried">The instances tried.</param>
    /// <param name="tuples">The remaining ground instances in the current level.</param>
    /// 
    /// <returns>
    /// The list of ground tuples generated and prints to the <c>stdout</c> how 
    /// many ground instances were tried, if the procedure terminates (thus 
    /// indicating that the formula is valid); otherwise, loops generating and 
    /// testing larger and larger ground instances till the memory is full.
    /// </returns>
    /// 
    /// <example id="gilmore_loop-2">
    /// <code lang="fsharp">
    /// gilmore_loop !!>>[["Q(f_z(x))"; "~Q(y)"]] 
    ///   !!!>["c"] [("f_z",1);("f_y",1)] ["x";"y"] 0 [[]] [] [] 
    /// </code>
    /// Evaluates to <c>[[``c``; ``f_z(c)``]; [``c``; ``c``]]</c> and print to 
    /// the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 1 items in list.
    /// 0 ground instances tried; 1 items in list.
    /// 1 ground instances tried; 1 items in list.
    /// 1 ground instances tried; 1 items in list.
    /// </code>
    /// </example>
    /// 
    /// <category index="4">A gilmore-like procedure</category>
    val gilmore_loop:
      fl0: formula<fol> list list ->
        cntms: term list ->
        funcs: (string * int) list ->
        fvs: string list ->
        n: int ->
        fl: formula<fol> list list ->
        tried: term list list -> 
        tuples: term list list -> term list list

    /// <summary>
    /// Tests the validity of a formula with a Gilmore-like procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The number of ground tuples generated and prints to the <c>stdout</c> 
    /// how many ground instances were tried, if the procedure terminates (thus 
    /// indicating that the formula is valid); otherwise, loops indefinitely 
    /// till the memory is full.
    /// </returns>
    /// 
    /// <example id="gilmore-2">
    /// <code lang="fsharp">
    /// gilmore !! "exists x. forall y. P(x) ==> P(y)"
    /// </code>
    /// Evaluates to <c>2</c> and print to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 1 items in list.
    /// 0 ground instances tried; 1 items in list.
    /// 1 ground instances tried; 1 items in list.
    /// 1 ground instances tried; 1 items in list.
    /// </code>
    /// </example>
    /// 
    /// <note>
    /// The procedure loops infinitely if the initial formula is not valid and 
    /// even if it is, it is not guaranteed to end.
    /// </note>
    /// 
    /// <category index="4">A gilmore-like procedure</category>
    val gilmore: fm: formula<fol> -> int

    /// <summary>
    /// Generates the ground instances for the Davis-Putnam procedure.
    /// </summary>
    /// 
    /// <example id="dp_mfn-1">
    /// This example shows the first generation of ground instance when the set 
    /// is initially empty.
    /// <code lang="fsharp">
    /// dp_mfn !!>>[["P(x)"]; ["~P(f_y(x))"]] 
    ///   (subst (fpf ["x"] [!!!"c"])) []
    /// </code>
    /// Evaluates to <c>[[`P(c)`]; [`~P(f_y(c))`]]</c>.
    /// </example>
    /// 
    /// <example id="dp_mfn-2">
    /// This example shows the second generation of ground instance when the 
    /// nonempty set is augmented.
    /// <code lang="fsharp">
    /// dp_mfn !!>>[["P(x)"]; ["~P(f_y(x))"]] 
    ///   (subst (fpf ["x"] [!!!"f_y(c)"])) 
    ///   !!>>[["P(c)"]; ["~P(f_y(c))"]]
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
    /// <category index="5">The Davis-Putnam procedure for first order logic</category>
    val dp_mfn:
      cjs0: 'a list list -> ifn: ('a -> 'b) -> cjs: 'b list list -> 'b list list
        when 'b: comparison

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses with the Davis-Putnam 
    /// procedure.
    /// </summary>
    /// 
    /// <remarks>
    /// The input set of clauses <c>fl0</c> is intended to represent a CNF 
    /// formula and <c>cntms</c>, <c>funcs</c> and <c>fvs</c> are supposed to 
    /// be the constant and function symbols and the free variables of the 
    /// formula, respectively.
    /// 
    /// The function implements an 
    /// <see cref='M:Calcolemus.Herbrand.herbloop``2'/> specific for the 
    /// Davis-Putnam procedure, calling it with the specific Davis-Putnam 
    /// modification function <see cref='M:Calcolemus.Herbrand.dp_mfn``2'/> and 
    /// using <see cref='M:Calcolemus.DP.dpll``1'/> as test function.
    /// </remarks>
    /// 
    /// <param name="fl0">The input set of clauses.</param>
    /// <param name="cntms">The constant terms to generate the ground terms.</param>
    /// <param name="funcs">The function symbols (name, arity) to generate the ground terms.</param>
    /// <param name="fvs">The free variables to instantiate to generate the ground terms.</param>
    /// <param name="n">The number of function symbols the new ground terms to be generated should contain.</param>
    /// <param name="fl">The set of ground instances so far.</param>
    /// <param name="tried">The instances tried.</param>
    /// <param name="tuples">The remaining ground instances in the current level.</param>
    /// 
    /// <returns>
    /// The list of ground tuples generated and prints to the <c>stdout</c> how 
    /// many ground instances were tried, if the procedure terminates (thus 
    /// indicating that the formula is valid); otherwise, loops generating and 
    /// testing larger and larger ground instances till the memory is full.
    /// </returns>
    /// 
    /// <example id="dp_loop-2">
    /// <code lang="fsharp">
    /// dp_loop !!>>[["P(x)"]; ["~P(f_y(y))"]]
    ///     !!!>["c"] [("f_y",1);] ["x";"y"] 0 [] [] []
    /// </code>
    /// Evaluates to <c>[[``f_y(c)``; ``c``]; [``c``; ``f_y(c)``]; [``c``; ``c``]]</c> and print to 
    /// the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 0 items in list.
    /// 0 ground instances tried; 0 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// 2 ground instances tried; 3 items in list.
    /// </code>
    /// </example>
    /// 
    /// <category index="5">The Davis-Putnam procedure for first order logic</category>
    val dp_loop:
      fl0: formula<fol> list list ->
         cntms: term list ->
         funcs: (string * int) list ->
         fvs: string list ->
         n: int ->
         fl: formula<fol> list list ->
         tried: term list list -> 
         tuples: term list list -> term list list

    /// <summary>
    /// Tests the validity of a formula with the Davis-Putnam procedure.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The number of ground tuples generated and prints to the <c>stdout</c> 
    /// how many ground instances were tried, if the procedure terminates (thus 
    /// indicating that the formula is valid); otherwise, loops indefinitely 
    /// till the memory is full.
    /// </returns>
    /// 
    /// <example id="davisputnam-1">
    /// <code lang="fsharp">
    /// davisputnam !! "exists x. forall y. P(x) ==> P(y)"
    /// </code>
    /// Evaluates to <c>2</c> and print to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 0 items in list.
    /// 0 ground instances tried; 0 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// </code>
    /// </example>
    /// 
    /// <note>
    /// The procedure loops infinitely if the initial formula is not valid and 
    /// even if it is, it is not guaranteed to end.
    /// </note>
    /// 
    /// <category index="5">The Davis-Putnam procedure for first order logic</category>
    val davisputnam: fm: formula<fol> -> int

    /// <summary>
    /// Returns the list of ground tuples that are really needed to refute 
    /// <c>cjs0</c>.
    /// </summary>
    /// 
    /// <param name="cjs0">The input set of clauses.</param>
    /// <param name="fvs">The free variables to instantiate to generate the ground terms.</param>
    /// <param name="dunno">The list of possibly-needed instances.</param>
    /// <param name="need">The list of really needed instances.</param>
    /// 
    /// <returns>
    /// The list of really needed instances to refute the input set of clauses, 
    /// if there is one; otherwise, the input <c>dunno</c> itself in reverse 
    /// order.
    /// </returns>
    /// 
    /// <example id="dp_refine-2">
    /// <code lang="fsharp">
    /// dp_refine !!>>[["P(x)"]; ["~P(f_y(y))"]] ["x";"y"] 
    ///   !!!>>[["f_y(c)"; "c"]; ["c";"c"]; ["d";"d"]] []
    /// </code>
    /// Evaluates to <c>[[``f_y(c)``]; [``c``]]</c>.
    /// </example>
    /// 
    /// <category index="6">The Davis-Putnam procedure refined</category>
    val dp_refine:
      cjs0: formula<fol> list list ->
        fvs: string list ->
        dunno: term list list -> need: term list list -> term list list

    /// <summary>
    /// Tests the unsatisfiability of a set of clauses with the Davis-Putnam 
    /// procedure refined.
    /// </summary>
    /// 
    /// <remarks>
    /// The difference with <see cref='M:Calcolemus.Herbrand.dp_loop'/> is just 
    /// that the list returned is not the number of ground tuples generated 
    /// but of those that are really needed to refute the input.
    /// </remarks>
    /// 
    /// <param name="fl0">The input set of clauses.</param>
    /// <param name="cntms">The constant terms to generate the ground terms.</param>
    /// <param name="funcs">The function symbols (name, arity) to generate the ground terms.</param>
    /// <param name="fvs">The free variables to instantiate to generate the ground terms.</param>
    /// <param name="n">The number of function symbols the new ground terms to be generated should contain.</param>
    /// <param name="fl">The set of ground instances so far.</param>
    /// <param name="tried">The instances tried.</param>
    /// <param name="tuples">The remaining ground instances in the current level.</param>
    /// 
    /// <returns>
    /// The list of ground tuples needed to refute the input and prints to the 
    /// <c>stdout</c> how many ground instances were tried, if the procedure 
    /// terminates (thus indicating that the formula is valid); otherwise, 
    /// loops generating and testing larger and larger ground instances till 
    /// the memory is full.
    /// </returns>
    /// 
    /// <example id="dp_refine_loop-2">
    /// <code lang="fsharp">
    /// dp_refine_loop !!>>[["P(x)"]; ["~P(f_y(y))"]]
    ///     !!!>["c"] [("f_y",1);] ["x";"y"] 0 [] [] []
    /// </code>
    /// Evaluates to <c>[[``f_y(c)``; ``c``]]</c> and print to 
    /// the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 0 items in list.
    /// 0 ground instances tried; 0 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// 1 ground instances tried; 2 items in list.
    /// 2 ground instances tried; 3 items in list.
    /// </code>
    /// </example>
    /// 
    /// <category index="6">The Davis-Putnam procedure refined</category>
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
    /// Tests the validity of a formula with the Davis-Putnam procedure refined.
    /// </summary>
    /// 
    /// <remarks>
    /// The difference with <see cref='M:Calcolemus.Herbrand.davisputnam'/> is 
    /// just that the number returned is not the number of ground instance 
    /// generated but that of those that are really needed.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The number of ground tuples needed to prove the validity and prints to 
    /// the <c>stdout</c> how many ground instances were tried, if the 
    /// procedure terminates (thus indicating that the formula is valid); 
    /// otherwise, loops indefinitely till the memory is full.
    /// </returns>
    /// 
    /// <example id="davisputnam002-1">
    /// <code lang="fsharp">
    /// davisputnam002 p36
    /// </code>
    /// Evaluates to <c>3</c> and print to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 ground instances tried; 0 items in list.
    /// 0 ground instances tried; 0 items in list.
    /// 1 ground instances tried; 6 items in list.
    /// ...
    /// 37 ground instances tried; 168 items in list.
    /// 38 ground instances tried; 172 items in list.
    /// 39 ground instances tried; 176 items in list.
    /// </code>
    /// </example>
    /// 
    /// <note>
    /// The procedure loops infinitely if the initial formula is not valid and 
    /// even if it is, it is not guaranteed to end.
    /// </note>
    /// 
    /// <category index="6">The Davis-Putnam procedure refined</category>
    val davisputnam002: fm: formula<fol> -> int