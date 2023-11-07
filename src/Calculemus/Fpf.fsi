// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus.Lib

/// <summary>
/// Polymorphic Finite Partial functions via Patricia Trees.
/// </summary>
/// 
/// <category index="5">Finite partial functions and equivalence classes</category>
module Fpf =

    /// <summary>
    /// Type of polymorphic finite partial functions represented as a patricia
    /// tree, <c>'a</c> being the type of the arguments and <c>'b</c> that of
    /// the values.
    /// </summary>
    /// 
    /// <note>
    /// For a description of patricia trees see the article "Fast mergeable 
    /// integer maps" by Chris Okasaki, Andy Gill - Workshop on ML, 1998.
    /// </note>
    /// 
    /// <example>
    /// The following represents the function \(x \mapsto 1, y \mapsto 2, z
    /// \mapsto 3, w \mapsto 4\):
    /// <code lang="fsharp">
    /// Branch
    ///   (0, 1, Leaf (2131634918, [("x", 1)]),
    ///    Branch
    ///      (1, 2, Leaf (147666021, [("w", 4)]),
    ///       Branch
    ///         (3, 4, Leaf (-1524676365, [("y", 2)]),
    ///          Leaf (-1991064537, [("z", 3)]))))
    /// </code>
    /// </example>
    /// <typeparam name="'a">The type of the domain.</typeparam>
    /// <typeparam name="'b">The type of the codomain.</typeparam>
    type func<'a,'b> =
        /// Represents the empty, or everywhere undefined, FPF.
        | Empty
        /// <summary>
        /// Represents a mapping from an element of the domain to an element of
        /// the codomain.
        /// </summary>
        /// <example>
        /// <code lang="fsharp">Leaf (2131634918, [("x", 'a)]</code>
        /// </example>
        /// <param name="Item1">A generic hash used for comparison.</param>
        /// <param name="Item2">The pair of domain-codomain elements.</param>
        | Leaf of int * list<'a * 'b>
        /// <summary>Used to store more then one mapping.</summary>
        /// <example>
        /// <code lang="fsharp">Branch
        /// (3, 4,
        /// Leaf (-1524676365, [("y", 'b')]),
        /// Leaf (-1991064537, [("z", 'c')])
        /// )</code>
        /// </example>
        /// <param name="Item1">The prefix.</param>
        /// <param name="Item2">The branching bit.</param>
        /// <param name="Item3">The left branch.</param>
        /// <param name="Item4">The right branch.</param>
        | Branch of int * int * func<'a,'b> * func<'a,'b>

    /// <summary>The empty, or everywhere undefined, fpf.</summary>
    /// <returns><c>Empty</c>.</returns>
    val undefined: func<'a,'b>

    /// <summary>
    /// Returns true if the function is completely undefined, false
    /// otherwise.
    /// </summary>
    ///
    /// <remarks>
    /// In case of equality comparison worries, better use this.
    /// </remarks>
    ///
    /// <param name="_arg1">The function to be checked.</param>
    ///
    /// <returns>True if the function is undefined.</returns>
    ///
    /// <example id="is_undefined-1">
    /// <code lang="fsharp">is_undefined undefined</code>
    /// Evaluates to <c>true</c>.
    /// </example>
    ///
    /// <example id="is_undefined-2">
    /// <code lang="fsharp">is_undefined (("x" |-> 1)undefined)</code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val is_undefined: _arg1: func<'a,'b> -> bool

    /// <summary>
    /// Builds a new fpf whose values are the results of applying the given
    /// function to the values of the input fpf.
    /// </summary>
    ///
     /// <remarks>
    /// It is, for finite partial functions, the same operation that <see cref='M:Microsoft.FSharp.Collections.ListModule.Map``2'/> is for <see cref='T:Microsoft.FSharp.Collections.list`1'/>.
    /// </remarks>
    ///
    /// <param name="mapping">The function to transform values of the input fpf.</param>
    /// <param name="fpf">The input fpf.</param>
    ///
    /// <returns>
    /// The fpf with transformed values.
    /// </returns>
    ///
    /// <example id="mapf-2">
    /// <code lang="fsharp">
    /// ("x" |-> 1)undefined
    /// |> mapf (fun x -> x * 10)
    /// </code>
    /// Evaluates to <c>Leaf (..., [("x", 10)])</c>.
    /// </example>
    val mapf: mapping: ('a -> 'b) -> fpf: func<'c,'a> -> func<'c,'b>

    /// <summary>
    /// Applies a function to the argument and value of an fpf, threading
    /// an accumulator argument through the computation. Take the second
    /// argument, and apply the function to it and the first argument and value
    /// of the fpf. Then feed this result into the function along with the
    /// second argument and value and so on. Return the final result. If the
    /// input function is <c>f</c> and the fpf's arguments and values are <c>
    /// (i0,j0)...(iN,jN)</c> then
    /// computes <c>f (... (f s i0 j0) i1 j1 ...) iN jN</c>.
    /// </summary>
    ///
    /// <remarks>
    /// It is, for finite partial functions, the same operation that <see cref='M:Microsoft.FSharp.Collections.ListModule.Fold``2'/> is for <see cref='T:Microsoft.FSharp.Collections.list`1'/>.
    /// </remarks>
    ///
    /// <param name="folder">The normal F# function to update the state given the input fpf.</param>
    /// <param name="state">The initial state.</param>
    /// <param name="fpf">The input fpf</param>
    ///
    /// <returns>The final state value.</returns>
    ///
    /// <example id="foldl-1">
    /// <code lang="fsharp">
    /// ("y" |-> 2)(("x" |-> 1)undefined)
    /// |> foldl (fun acc i j -> acc + j) 0
    /// </code>
    /// Evaluates to <c>3</c>.
    /// </example>
    val foldl:
        folder: ('State -> 'a -> 'b -> 'State) ->
        state: 'State ->
        fpf: func<'a,'b>
        -> 'State

    /// <summary>Returns the graph of the input <c>fpf</c>.</summary>
    ///
    /// <param name="fpf">The input fpf</param>
    ///
    /// <returns>
    /// The graph (the set of pairs argument-value) of the input <c>fpf</c>.
    /// </returns>
    ///
    /// <example id="graph-1">
    /// <code lang="fsharp">
    /// ("y" |-> 2)(("x" |-> 1)undefined)
    /// |> graph
    /// </code>
    /// Evaluates to <c>[("x", 1); ("y", 2)]</c>.
    /// </example>
    val graph:
      fpf: func<'a,'b> -> ('a * 'b) list when 'a: comparison and 'b: comparison

    /// <summary>Returns the domain of the input <c>fpf</c>.</summary>
    ///
    /// <param name="fpf">The input fpf</param>
    ///
    /// <returns>
    /// The domain (the set of arguments) of the input <c>fpf</c>.
    /// </returns>
    ///
    /// <example id="dom-1">
    /// <code lang="fsharp">
    /// ("y" |-> 2)(("x" |-> 1)undefined)
    /// |> dom
    /// </code>
    /// Evaluates to <c>["x"; "y"]</c>.
    /// </example>
    val dom: fpf: func<'a,'b> -> 'a list when 'a: comparison

    /// <summary>Returns the range of the input <c>fpf</c>.</summary>
    ///
    /// <param name="fpf">The input fpf</param>
    ///
    /// <returns>
    /// The range (the set of values) of the input <c>fpf</c>.
    /// </returns>
    ///
    /// <example id="ran-1">
    /// <code lang="fsharp">
    /// ("y" |-> 2)(("x" |-> 1)undefined)
    /// |> ran
    /// </code>
    /// Evaluates to <c>[1; 2]</c>.
    /// </example>
    val ran: fpf: func<'a,'b> -> 'b list when 'b: comparison

    /// <summary>
    /// Applies <c>fpf</c> to <c>a</c> and returns the corresponding value, if
    /// <c>fpf</c> is actually defined for it, otherwise applies <c>d</c> to
    /// <c>a</c>.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to be applied.</param>
    /// <param name="d">The function to apply to <c>x</c>, if it's not an argument of the <c>fpf</c>.</param>
    /// <param name="a">The supposed fpf's argument.</param>
    ///
    /// <returns>
    /// The <c>fpf</c>'s value for <c>a</c>, if <c>fpf</c> is
    /// actually defined for it, otherwise applies <c>d</c> to <c>x</c>.
    /// </returns>
    val applyd: fpf: func<'a,'b> -> d: ('a -> 'b) -> a: 'a -> 'b when 'a: comparison

    /// <summary>
    /// Applies <c>fpf</c> to <c>a</c>.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to be applied.</param>
    /// <param name="a">The supposed fpf's argument.</param>
    ///
    /// <returns>
    /// The <c>fpf</c>'s value for <c>a</c>, if <c>fpf</c> is
    /// actually defined for it.
    /// </returns>
    ///
    /// <exception cref="T:System.Exception">Thrown with message <c>apply</c>, when <c>fpf</c> is not defined for <c>a</c>.</exception>
    ///
    /// <example id="apply-1">
    /// <code lang="fsharp">
    /// apply (("y" |-> 2)(("x" |-> 1)undefined)) "y"
    /// </code>
    /// Evaluates to <c>2</c>.
    /// </example>
    ///
    /// <example id="apply-2">
    /// <code lang="fsharp">
    /// apply (("y" |-> 2)(("x" |-> 1)undefined)) "z"
    /// </code>
    /// Throws <c>System.Exception: apply</c>.
    /// </example>
    val apply: fpf: func<'a,'b> -> a: 'a -> 'b when 'a: comparison

    /// <summary>
    /// Tries to apply <c>fpf</c> to an argument <c>a</c>, returning <c>d</c>
    /// as a default value if it fails.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to be applied.</param>
    /// <param name="a">The supposed fpf's argument.</param>
    /// <param name="d">The default value to return in case of failure.</param>
    ///
    /// <returns>
    /// The fpf's value for <c>a</c>, if fpf is actually defined for it.
    /// Otherwise, the default value <c>d</c>.
    /// </returns>
    ///
    /// <example id="tryapplyd-1">
    /// <code lang="fsharp">
    /// tryapplyd (("y" |-> 2)(("x" |-> 1)undefined)) "x" 9
    /// </code>
    /// Evaluates to <c>1</c>.
    /// </example>
    ///
    /// <example id="tryapplyd-2">
    /// <code lang="fsharp">
    /// tryapplyd (("y" |-> 2)(("x" |-> 1)undefined)) "x" 9
    /// </code>
    /// Evaluates to <c>9</c>.
    /// </example>
    val tryapplyd: fpf: func<'a,'b> -> a: 'a -> d: 'b -> 'b when 'a: comparison

    /// <summary>
    /// Tries to apply an <c>fpf</c> whose values are lists to an argument 
    /// <c>a</c>,returning <c>[]</c> as a default value if it fails.
    /// </summary>
    ///
    /// <param name="fpf">The input fpf to be applied.</param>
    /// <param name="a">The supposed fpf's argument.</param>
    ///
    /// <returns>
    /// The fpf's value for <c>a</c>, if fpf is actually defined for it.
    /// Otherwise, the default value <c>[]</c>.
    /// </returns>
    ///
    /// <example id="tryapplyl-1">
    /// <code lang="fsharp">
    /// tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "x"
    /// </code>
    /// Evaluates to <c>[1;2;3]</c>.
    /// </example>
    ///
    /// <example id="tryapplyl-2">
    /// <code lang="fsharp">
    /// tryapplyl (("y" |-> [4;5;6])(("x" |-> [1;2;3])undefined)) "z"
    /// </code>
    /// Evaluates to <c>[]</c>.
    /// </example>
    val tryapplyl: fpf: func<'a,'b list> -> a: 'a -> 'b list when 'a: comparison

    /// <summary>
    /// Checks if <c>fpf</c> is defined for the argument <c>a</c>.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to check.</param>
    /// <param name="a">The argument to check.</param>
    ///
    /// <returns>
    /// true if <c>fpf</c> is defined for <c>a</c>.
    /// </returns>
    ///
    /// <example id="defined-1">
    /// <code lang="fsharp">
    /// defined (("y" |-> 2)(("x" |-> 1)undefined)) "x"
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    ///
    /// <example id="defined-2">
    /// <code lang="fsharp">
    /// defined (("y" |-> 2)(("x" |-> 1)undefined)) "z"
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val defined: fpf: func<'a,'b> -> a: 'a -> bool when 'a: comparison

    /// <summary>
    /// Undefines the <c>fpf</c> for the given argument <c>a</c>.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to undefine.</param>
    /// <param name="a">The argument to undefine.</param>
    ///
    /// <returns>
    /// The new fpf with the argument undefined, or the input fpf unchanged if
    /// it was already undefined for that argument.
    /// </returns>
    ///
    /// <example id="undefine-1">
    /// <code lang="fsharp">
    /// ("y" |-> 2)(("x" |-> 1)undefined)
    /// |> undefine "x"
    /// </code>
    /// Evaluates to <c>Leaf (..., [("y", 2)])</c>.
    /// </example>
    ///
    /// <example id="undefine-2">
    /// <code lang="fsharp">
    /// let input = ("y" |-> 2)(("x" |-> 1)undefined)
    ///
    /// input
    /// |> undefine "z"
    /// |> (=) input
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    val undefine:
      a: 'a -> fpf: func<'a,'b> -> func<'a,'b> when 'a: comparison and 'b: equality

    /// <summary>
    /// Updates the <c>fpf</c> with a new mapping <c>argument</c>-<c>value</c>.
    /// </summary>
    ///
    /// <param name="fpf">The fpf to update.</param>
    /// <param name="argument">The argument of the new mapping.</param>
    /// <param name="value">The value of the new mapping.</param>
    ///
    /// <returns>
    /// The fpf with the new defined mapping.
    /// </returns>
    ///
    /// <example id="pipeline-minus-greater-example">
    /// <code lang="fsharp">
    /// ("x" |-> 1)undefined |> graph
    /// </code>
    /// Evaluates to <c>[("x", 1)]</c>.
    /// </example>
    val (|->) : argument: 'a -> value: 'b -> fpf: func<'a,'b> -> func<'a,'b> when 'a: comparison

    // it seems unused
    // val combine:
    //   (('c -> 'c -> 'c) -> ('c -> bool) -> func<'d,'c> -> func<'d,'c> -> func<'d,   'c>)
    //     when 'c: equality and 'd: comparison

    /// <summary>
    /// Creates a point function: a special case of fpf defined only for a
    /// single argument <c>a</c> mapped to a value <c>b</c>.
    /// </summary>
    ///
    /// <param name="a">The argument of the only mapping.</param>
    /// <param name="b">The value of the only mapping.</param>
    ///
    /// <returns>
    /// The new point function with the defined mapping.
    /// </returns>
    ///
    /// <example id="pipeline-equals-greater-example">
    /// <code lang="fsharp">
    /// "x" |=> 1 |> graph
    /// </code>
    /// Evaluates to <c>[("x", 1)]</c>.
    /// </example>
    val (|=>) : a: 'a -> b: 'b -> func<'a,'b> when 'a: comparison

    /// <summary>
    /// Creates a new fpf from two lists <c>xs</c> and <c>ys</c> representing
    /// its domain and range. It associates argument to value based on the
    /// order of items in the two lists.
    /// </summary>
    ///
    /// <param name="xs">The list of arguments of the new fpf.</param>
    /// <param name="ys">The list of values of the new fpf.</param>
    ///
    /// <returns>
    /// The new fpf with the defined mappings.
    /// </returns>
    ///
    /// <example id="fpf-1">
    /// <code lang="fsharp">
    /// fpf [1;2;3] [1;4;9] |> graph
    /// </code>
    /// Evaluates to <c>[(1, 1); (2, 4); (3, 9)]</c>.
    /// </example>
    val fpf: xs: 'a list -> ys: 'b list -> func<'a,'b> when 'a: comparison

    // it seems unused.
    // val choose: t: func<'a,'b> -> 'a * 'b

    /// <summary>
    /// Updates to <c>b</c> the value of a normal F# function <c>f</c> for 
    /// the argument <c>a</c>, creating, instead, a new mapping 
    /// <c>a</c>-<c>b</c> if <c>f</c> it's not already defined for <c>a</c>. 
    /// Then it applies this modified function to the argument <c>x</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Corresponds to the mathematical notation \((a \mapsto b)f\) and it is 
    /// the same thing of what <c>(x |-> y) f</c> is in the context of the 
    /// finite partial functions.
    /// </remarks>
    /// 
    /// <param name="a">The argument to update.</param>
    /// <param name="b">The value to assign to the argument.</param>
    /// <param name="f">The F# function to update.</param>
    /// <param name="x">The argument to apply the modified function to.</param>
    /// 
    /// <returns>
    /// The new value, if applied to the updated argument. Otherwise, the value 
    /// of the original function.
    /// </returns>
    /// 
    /// <example id="valmod-1">
    /// <code lang="fsharp">
    /// valmod 1 100 id 1
    /// </code>
    /// Evaluates to <c>100</c>.
    /// </example>
    /// 
    /// <example id="valmod-2">
    /// <code lang="fsharp">
    /// valmod 1 100 id 2
    /// </code>
    /// Evaluates to <c>2</c>.
    /// </example>
    val valmod: a: 'a -> b: 'b -> f: ('a -> 'b) -> x: 'a -> 'b when 'a: equality

    /// <summary>
    /// A function undefined for any argument <c>x</c> and that always fails.
    /// </summary>
    /// 
    /// <remarks>
    /// It is to be the same thing of what 
    /// <see cref='M:Calculemus.Lib.Fpf.undefined``2'/> is in the context of 
    /// the finite partial functions.
    /// </remarks>
    /// 
    /// <note>
    /// <p>In a non-functional world you can create a list of values and
    /// initialize the list signifying nothing: e.g. <c>[]</c>.
    /// Then when you process the list it could return without exception
    /// or if you wanted the processing of the list to return with
    /// exception when there is nothing in the list, you would check
    /// the list for nothing and return an exception.</p>
    ///
    /// <p>In a functional world you can create a list of functions and
    /// initialize the list with a function causing an exception given that
    /// the items is the list are evaluated as functions.</p>
    ///
    /// <p>undef is that function which is used to initialize a list to
    /// cause an exception if the list is empty when evaluated.</p>
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with the message 'undefined function' when applied to any argument.</exception>
    /// 
    /// <example id="undef-1">
    /// <code lang="fsharp">
    /// ((undef 1):int)
    /// </code>
    ///  Throws <c>System.Exception: undefined function</c>.
    /// </example>
    /// 
    /// <example id="undef-2">
    /// <code lang="fsharp">
    /// valmod 1 100 (undef) 1
    /// </code>
    /// Evaluates to <c>100</c>.
    /// </example>
    val undef: x: 'a -> 'b