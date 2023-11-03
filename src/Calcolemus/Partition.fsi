// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calcolemus.Lib

open Calcolemus.Lib.Fpf

/// <summary>
/// Equivalence relations (or partitions, equivalence classes) on finite sets.
/// </summary>
/// 
/// <remarks>
/// See also <a target="_blank" href="https://en.wikipedia.org/wiki/Disjoint-set_data_structure">Disjoint set/Union Find data structure</a> 
/// on Wikipedia.
/// </remarks>
/// 
/// <category index="5">Finite partial functions and equivalence classes</category>
module Partition =

    /// <summary>
    /// Type of polymorphic 
    /// <see cref='T:Calcolemus.Lib.Partition.partition`1'/>
    /// node.
    /// </summary>
    type pnode<'a> =
      /// <summary>
      /// Nonterminal node: associated to non canonical elements.
      /// </summary>
      /// <param name="Item">The representative of the equivalence class.</param>
      /// <remarks>
      /// <c>Nonterminal 1</c> is the node that would be associated to an 
      /// element that is **not** the representative, or the canonical 
      /// element, of its equivalence class and in fact in this case the 
      /// representative would be <c>1</c>.
      /// </remarks>
      | Nonterminal of 'a
      /// <summary>
      /// Terminal node: associated to canonical elements.
      /// </summary>
      /// <param name="Item1">The representative of the equivalence class.</param>
      /// <param name="Item2">The size of the equivalence class.</param>
      /// <remarks>
      /// <c>Terminal (1, 4)</c> is the node that would be associated to
      /// <c>1</c> when <c>1</c> is the representative, or the canonical 
      /// element, of its equivalence class and the equivalence class has 4 
      /// elements.
      /// </remarks>
      | Terminal of 'a * int

    /// <summary>
    /// Union-Find datatype to represent equivalence relations (partitions) on 
    /// finite sets.
    /// </summary>
    /// 
    /// <example id="partition-1">
    /// The following creates two disjoint sets (or partitions, or equivalence 
    /// classes): 
    /// <ul>
    /// <li>\((1,2,3,4)\) with \(1\) as its representative;</li>
    /// <li>\((5,6)\) with \(5\) as its representative.</li>
    /// </ul>
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   
    /// f |> graph
    /// </code>
    /// Evaluates to 
    /// <code lang="fsharp">
    /// val it: (int * pnode&lt;int&gt;) list =
    ///   [(1, Terminal (1, 4)); (2, Nonterminal 1); (3, Nonterminal 1);
    ///    (4, Nonterminal 1); (5, Terminal (5, 2)); (6, Nonterminal 5)]
    /// </code>
    /// </example>
    type partition<'a> = 
      /// <summary>
      /// Represents the partitions (or equivalence classes) of the domain 
      /// through an fpf (see <see cref='T:Calcolemus.Lib.Fpf.func`2'/>) that 
      /// maps an element of the domain to a 
      /// <see cref='T:Calcolemus.Lib.Partition.pnode`1'/>.
      /// </summary>
      /// <param name="Item">The fpf that defines the equivalence classes in the domain of the partition. It maps each element of the domain to the representative of the equivalence class the element belongs to.</param>
      | Partition of func<'a,pnode<'a>>

    /// <summary>The empty partition: used to define new partitions.</summary>
    /// <returns>The empty partition.</returns>
    val unequal: partition<'a>

    /// <summary>
    /// Returns the domain of the partition <c>ptn</c>.
    /// </summary>
    /// 
    /// <param name="ptn">The input partition.</param>
    /// 
    /// <returns>The domain of the partition.</returns>
    /// 
    /// <example id="equated-1">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// equated ptn
    /// </code>
    /// Evaluates to <c>[1; 2; 3; 4; 5; 6; 7]</c>.
    /// </example>
    val equated: ptn: partition<'a> -> 'a list when 'a: comparison

    /// <summary>
    /// Searches for the canonical representative of the <c>ptn</c>-equivalence 
    /// class containing <c>a</c>, failing if <c>a</c> does not belong to the 
    /// domain of the partition.
    /// </summary>
    /// 
    /// <param name="ptn">The input partition.</param>
    /// <param name="a">The element of the partition to search.</param>
    /// 
    /// <returns>
    /// The pair of the canonical element of <c>a</c>'s equivalence class plus 
    /// its size, if <c>a</c> is an element in the domain of the partition.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>apply</c>, when
    /// <c>a</c> is not an element in the domain of the partition.</exception>
    /// 
    /// <example id="terminus-1">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// terminus ptn 3
    /// </code>
    /// Evaluates to <c>(1, 4)</c>.
    /// </example>
    /// 
    /// <example id="terminus-2">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// terminus ptn 8
    /// </code>
    /// Throws <c>System.Exception: apply</c>.
    /// </example>
    val terminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

    /// <summary>
    /// Searches for the canonical representative of the <c>ptn</c>-equivalence 
    /// class containing <c>a</c>, returning the input element <c>a</c> itself 
    /// if it does not belong to the domain of the partition.
    /// </summary>
    /// 
    /// <param name="ptn">The input partition.</param>
    /// <param name="a">The element of the partition to search.</param>
    /// 
    /// <returns>
    /// The pair of the canonical element of <c>a</c>'s equivalence class plus 
    /// its size, if <c>a</c> is an element in the domain of the partition; 
    /// otherwise <c>(a, 1)</c>.
    /// </returns>
    /// 
    /// <example id="tryterminus-1">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// tryterminus ptn 3
    /// </code>
    /// Evaluates to <c>(1, 4)</c>.
    /// </example>
    /// 
    /// <example id="tryterminus-2">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// tryterminus ptn 8
    /// </code>
    /// Evaluates to <c>(8, 1)</c>.
    /// </example>
    val tryterminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

    /// <summary>
    /// Returns the canonical representative of the <c>ptn</c>-equivalence 
    /// class containing <c>a</c>, if <c>a</c> is in the domain of the 
    /// partition. Otherwise it returns just <c>a</c> itself.
    /// </summary>
    /// 
    /// <remarks>
    /// Corresponds to the find method in the union-find algorithm.
    /// </remarks>
    /// 
    /// <param name="ptn">The input partition.</param>
    /// <param name="a">The element of the partition to search.</param>
    /// 
    /// <returns>
    /// The canonical element of <c>a</c>'s equivalence class, if <c>a</c> is 
    /// an element in the domain of the partition; otherwise <c>a</c> itself.
    /// </returns>
    /// 
    /// <example id="canonize-1">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// canonize ptn 3
    /// </code>
    /// Evaluates to <c>1</c>.
    /// </example>
    /// 
    /// <example id="canonize-2">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// canonize ptn 8
    /// </code>
    /// Evaluates to <c>8</c>.
    /// </example>
    val canonize: ptn: partition<'a> -> a: 'a -> 'a when 'a: comparison

    /// <summary>
    /// Tests if <c>a</c> and <c>b</c> are equivalent w.r.t. <c>ptn</c>.
    /// </summary>
    /// 
    /// <param name="ptn">The input partition.</param>
    /// <param name="a">The first element to compare.</param>
    /// <param name="b">The second element to compare.</param>
    /// 
    /// <returns>
    /// true if <c>a</c> and <c>b</c> belong to the same equivalence class 
    /// in <c>ptn</c>; otherwise false.
    /// </returns>
    /// 
    /// <example id="equivalent-1">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// equivalent ptn 3 2
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="equivalent-2">
    /// <code lang="fsharp">
    /// let (Partition f as ptn) = 
    ///   unequal
    ///   |> equate (2,1) 
    ///   |> equate (3,1)
    ///   |> equate (4,1) 
    ///   |> equate (6,5) 
    ///   |> equate (7,5) 
    /// 
    /// equivalent ptn 6 1
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    val equivalent: ptn: partition<'a> -> a: 'a -> b: 'a -> bool when 'a: comparison

    /// <summary>
    /// Creates a new partition that results from merging the <c>a</c> and 
    /// <c>b</c> classes in <c>ptn</c>, i.e. the smallest equivalence relation 
    /// containing <c>ptn</c> such that <c>a</c> and <c>b</c> are equivalent.
    /// </summary>
    /// 
    /// <remarks>
    /// Corresponds to the union method in the union-find algorithm.
    /// </remarks>
    /// 
    /// <param name="a">The first element to equate.</param>
    /// <param name="b">The second element to equate.</param>
    /// <param name="ptn">The input partition.</param>
    /// 
    /// <returns>
    /// The new partition with the updated equivalence classes.
    /// </returns>
    /// 
    /// <example id="equivalent-1">
    /// <code lang="fsharp">
    /// unequal
    /// |> equate (2,1)
    /// |> fun (Partition f) -> graph f
    /// </code>
    /// Evaluates to <c>[(1, Terminal (1, 2)); (2, Nonterminal 1)]</c>.
    /// </example>
    val equate:
      a: 'a * b: 'a -> ptn: partition<'a> -> partition<'a> when 'a: comparison

    

    