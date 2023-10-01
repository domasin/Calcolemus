namespace FolAutomReas.Lib

/// <summary>Polymorphic finite partial functions via Patricia trees.</summary>
/// <remarks>
/// The point of this strange representation is that it is canonical (equal 
/// functions have the same encoding) yet reasonably efficient on average. 
/// </remarks>
/// <note>
/// Idea due to Diego Olivier Fernandez Pons (OCaml list, 2003/11/10).
/// </note>
[<AutoOpen>]
module FPF = 

    /// <summary>
    /// Type of polymorphic finite partial functions represented as a patricia 
    /// tree, <c>'a</c> being the type of the domain and <c>'b</c> of the 
    /// codomain.
    /// </summary>
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
        /// <summary>Used to store mode then one mapping.</summary>
        /// <example>
        /// <code lang="fsharp">Branch
        /// (3, 4, 
        /// Leaf (-1524676365, [("y", 'b')]),
        /// Leaf (-1991064537, [("z", 'c')])
        /// )</code>
        /// </example>
        /// <param name="Item1">The label of the first branch.</param>
        /// <param name="Item2">The label of the second branch.</param>
        /// <param name="Item3">The first branch.</param>
        /// <param name="Item4">The second branch.</param>
        | Branch of int * int * func<'a,'b> * func<'a,'b>
    
    /// <summary>The empty, or everywhere undefined, function.</summary>
    /// <returns><c>Empty</c>.</returns>
    val undefined: func<'a,'b>
    
    /// <summary>
    /// Returns true if the function is completely undefined, false 
    /// otherwise.
    /// </summary>
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
    /// 
    /// <remarks>
    /// In case of equality comparison worries, better use this.
    /// </remarks>
    val is_undefined: _arg1: func<'a,'b> -> bool
    
    /// <summary>Composes a normal F# function with an FPF.</summary>
    /// 
    /// <param name="f">The normal F# function.</param>
    /// <param name="t">The FPF function.</param>
    /// 
    /// <returns>
    /// The input FPF <c>t</c> with all \(x \mapsto y\) replaced with \(x 
    /// \mapsto f(y)\)
    /// </returns>
    /// 
    /// <example id="is_undefined-2">
    /// <code lang="fsharp">
    /// mapf (fun x -> x * 10) (("x" |-> 1)undefined)
    /// </code>
    /// Evaluates to <code lang="fsharp">Leaf (1907997954, [("x", 10)])</code>.
    /// </example>
    val mapf: f: ('a -> 'b) -> t: func<'c,'a> -> func<'c,'b>
    
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
    /// It is, for finite partial functions, the same operation that <c>fold</c> is for lists.
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
    
    /// Operation for `func` analogous to `foldr` for lists.
    val foldr: (('a -> 'b -> 'c -> 'c) -> func<'a,'b> -> 'c -> 'c)
    
    /// Graph of function `f`.
    val graph:
      f: func<'a,'b> -> ('a * 'b) list when 'a: comparison and 'b: comparison
    
    /// Domain of function `f`.
    val dom: f: func<'a,'b> -> 'a list when 'a: comparison
    
    /// Range of function `f`.
    val ran: f: func<'a,'b> -> 'b list when 'b: comparison
    
    val applyd: (func<'a,'b> -> ('a -> 'b) -> 'a -> 'b) when 'a: comparison
    
    val apply: f: func<'a,'b> -> ('a -> 'b) when 'a: comparison
    
    val tryapplyd: f: func<'a,'b> -> a: 'a -> d: 'b -> 'b when 'a: comparison
    
    val tryapplyl: f: func<'a,'b list> -> x: 'a -> 'b list when 'a: comparison
    
    /// Checks if the function `f` is defined for the argument `x`.
    val defined: f: func<'a,'b> -> x: 'a -> bool when 'a: comparison
    
    /// Undefines the function for the given argument.
    val undefine:
      ('a -> func<'a,'b> -> func<'a,'b>) when 'a: comparison and 'b: equality
    
    /// Updates the function with a new mapping.
    val (|->) : ('a -> 'b -> func<'a,'b> -> func<'a,'b>) when 'a: comparison
    
    /// Updates the function with a new mapping.
    val combine:
      (('c -> 'c -> 'c) -> ('c -> bool) -> func<'d,'c> -> func<'d,'c> -> func<'d,   'c>)
        when 'c: equality and 'd: comparison
    
    /// Creates a new FPF defined only for the value `x` and maps it to `y`.
    val (|=>) : x: 'a -> y: 'b -> func<'a,'b> when 'a: comparison
    
    /// Creates a new FPF from lists `xs` and `ys` representing its domain 
    /// and range. It associates argument to value based on the order of items 
    /// in the two lists.
    val fpf: xs: 'a list -> ys: 'b list -> func<'a,'b> when 'a: comparison
    
    val choose: t: func<'a,'b> -> 'a * 'b
    
    val valmod: a: 'a -> y: 'b -> f: ('a -> 'b) -> x: 'a -> 'b when 'a: equality
    
    /// In a non-functional world you can create a list of values and
    /// initialize the list signifying nothing. e.g. []
    /// Then when you process the list it could return without exception
    /// or if you wanted the processing of the list to return with
    /// exception when there is nothing in the list, you would check
    /// the list for nothing and return an exception.
    ///
    /// In a functional world you can create a list of functions and
    /// initialize the list with a function causing an exception given that
    /// the items is the list are evaluated as functions.
    /// 
    /// undef is that function which is used to initialize a list to
    /// cause an exception if the list is empty when evaluated.
    val undef: x: 'a -> 'b