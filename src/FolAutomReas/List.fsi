namespace FolAutomReas.Lib

module List = 

    /// Gives a finite list of integers between the given bounds.
    /// 
    /// The call m--n returns the list of consecutive numbers from m to n.
    val inline (--) : m: int -> n: int -> int list

    /// Gives a finite list of nums between the given bounds.
    /// 
    /// The call m--n returns the list of consecutive numbers from m to n.
    val inline (---) :
      m: FolAutomReas.Lib.Num.num ->
        n: FolAutomReas.Lib.Num.num -> FolAutomReas.Lib.Num.num list

    /// Computes the last element of a list.
    /// 
    /// last [x1;...;xn] returns xn.
    /// 
    /// Fails with last if the list is empty.
    val last: l: 'a list -> 'a

    /// Computes the sub-list of a list consisting of all but the last element.
    /// 
    /// butlast [x1;...;xn] returns [x1;...;x(n-1)].
    /// 
    /// Fails if the list is empty.
    val butlast: l: 'a list -> 'a list

    /// Compute list of all results from applying function to pairs from two lists.
    /// 
    /// The call allpairs f [x1;...;xm] [y1;...;yn] returns the list of results 
    /// [f x1 y1; f x1 y2; ... ; f x1 yn; fx2 y1 ; f x2 y2 ...; f xm y1; 
    /// f xm y2 ...; f xm yn ]
    /// 
    /// Never fails.
    val allpairs: f: ('a -> 'b -> 'c) -> l1: 'a list -> l2: 'b list -> 'c list

    /// produces all pairs of distinct elements from a single list
    /// 
    /// distinctpairs [1;2;3;4] returns [(1, 2); (1, 3); (1, 4); (2, 3); (2, 4);    (3, 4)]
    val distinctpairs: l: 'a list -> ('a * 'a) list

    /// Chops a list into two parts at a specified point.
    /// 
    /// chop_list i [x1;...;xn] returns ([x0;...;xi-1],[xi;...;xn]).
    /// 
    /// Fails with chop_list if n is negative or greater than the length of the     list.
    val chop_list: n: int -> l: 'a list -> 'a list * 'a list

    /// Adds an element in a list at the specified index
    /// 
    /// insertat 2 999 [0;1;2;3] returns [0; 1; 999; 2; 3]
    /// 
    /// Fails if index is negative or exceeds the list length-1
    val insertat: i: int -> x: 'a -> l: 'a list -> 'a list

    /// Returns position of given element in list.
    /// 
    /// The call index x l where l is a list returns the position number of the 
    /// first instance of x in the list, failing if there is none. The indices 
    /// start at zero, corresponding to el.
    val inline index: x: 'a -> xs: 'a list -> int when 'a: equality

    /// Checks if x comes earlier than y in list l
    /// 
    /// earlier [0;1;2;3] 2 3 return true, earlier [0;1;2;3] 3 2 false.
    /// earlier returns false also if x or y is not in the list.
    val earlier: l: 'a list -> x: 'a -> y: 'a -> bool when 'a: comparison