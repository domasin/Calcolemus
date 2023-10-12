namespace FolAutomReas

open FolAutomReas.Lib.Fpf
open FolAutomReas.Formulas

/// <summary>
/// Binary decision diagrams (BDDs) using complement edges.
/// </summary>
/// 
/// <remarks>
/// In practice one would use hash tables, but we use abstract finite
/// partial functions here. They might also look nicer imperatively.
/// </remarks>
/// 
/// <category index="3">Propositional logic</category>
module Bdd = 

    /// <summary>The type of binary decision diagram 
    /// (<see cref='T:FolAutomReas.Bdd.bdd'/>) node.</summary>
    /// 
    /// <remarks>It is composed by a propositional variable, a left node and a 
    /// right node.</remarks>
    type bddnode = Prop.prop * int * int

    /// <summary>The type of Binary Decision Diagram.</summary>
    /// <remarks>It is composed by 
    /// <ul>
    /// <li>a unique table <c>func&lt;bddnode, int&gt;</c>: a finite partial function mapping a bddnode to an integer index;</li>
    /// <li>an expansion table <c>func&lt;int, bddnode&gt;</c>: a finite partial function mapping an integer index to a bddnode;</li>
    /// <li>an integer to store the smallest unused positive node index;</li>
    /// <li>a prop variable order <c>(prop -&gt; prop -&gt; bool)</c>;</li>
    /// </ul>
    /// </remarks>
    type bdd =
        | Bdd of
          (func<bddnode,int> * func<int,bddnode> * int) *
          (Prop.prop -> Prop.prop -> bool)

    val print_bdd: bdd -> unit

    /// Returns the bddnode corresponding to the index `n` of the bdd
    /// If a negative index is used the complement of the node is returned.
    val expand_node: bdd -> n: int -> bddnode

    /// Lookup or insertion if not there in unique table. 
    val lookup_unique: bdd: bdd -> Prop.prop * int * int -> bdd * int

    /// Produce a BDD node (old or new).
    val mk_node: bdd: bdd -> s: Prop.prop * l: int * r: int -> bdd * int

    /// Create a new BDD with a given ordering. 
    val mk_bdd: ord: (Prop.prop -> Prop.prop -> bool) -> bdd

    /// Extract the ordering field of a BDD. 
    val order: bdd -> p1: Prop.prop -> p2: Prop.prop -> bool

    /// Threading state through.  
    val thread:
      s: 'a ->
        g: ('b -> 'c * 'd -> 'e) ->
        f1: ('a -> 'f -> 'g * 'c) * x1: 'f ->
          f2: ('g -> 'h -> 'b * 'd) * x2: 'h -> 'e

    /// Perform an AND operation on BDDs, maintaining canonicity.
    val bdd_and:
      bdd * func<(int * int),int> ->
        m1: int * m2: int -> (bdd * func<(int * int),int>) * int

    /// Perform an OR operation on BDDs, maintaining canonicity.
    val bdd_or:
      bdd * func<(int * int),int> ->
        m1: int * m2: int -> (bdd * func<(int * int),int>) * int

    /// Perform an IMP operation on BDDs, maintaining canonicity.
    val bdd_imp:
      bdd * func<(int * int),int> ->
        m1: int * m2: int -> (bdd * func<(int * int),int>) * int

    /// Perform an IFF operation on BDDs, maintaining canonicity.
    val bdd_iff:
      bdd * func<(int * int),int> ->
        m1: int * m2: int -> (bdd * func<(int * int),int>) * int

    /// Formula to BDD conversion.
    val mkbdd:
      bdd * func<(int * int),int> ->
        fm: formula<Prop.prop> ->
        (bdd * func<(int * int),int>) * int

    /// Tautology checking using BDDs.  
    val bddtaut: fm: formula<Prop.prop> -> bool

    val dest_nimp:
      fm: formula<'a> -> formula<'a> * formula<'a>

    val dest_iffdef: fm: formula<'a> -> 'a * formula<'a>

    val restore_iffdef:
      x: 'a * e: formula<'a> ->
        fm: formula<'a> -> formula<'a>

    val suitable_iffdef:
      defs: ('a * 'b) list -> x: 'c * q: formula<'a> -> bool
        when 'a: comparison

    val sort_defs:
      acc: ('a * formula<'a>) list ->
        defs: ('a * formula<'a>) list ->
        fm: formula<'a> ->
        ('a * formula<'a>) list * formula<'a> when 'a: comparison

    /// Formula to BDD conversion with improved setup
    val mkbdde:
      sfn: func<Prop.prop,int> ->
        bdd * func<(int * int),int> ->
          fm: formula<Prop.prop> ->
          (bdd * func<(int * int),int>) * int

    val mkbdds:
      sfn: func<Prop.prop,int> ->
        bdd * func<(int * int),int> ->
          defs: (Prop.prop * formula<Prop.prop>) list ->
          fm: formula<Prop.prop> ->
          (bdd * func<(int * int),int>) * int

    /// Tautology checking using BDDs with an improved setup
    val ebddtaut: fm: formula<Prop.prop> -> bool