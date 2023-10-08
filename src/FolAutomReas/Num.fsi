namespace FolAutomReas.Lib

module Num = 

    /// Infinite-precision rational number.
    type ratio = MathNet.Numerics.BigRational

    /// arbitrary-precision rational numbers derived from OCaml equivalent
    [<CustomEquality; CustomComparison>]
    type Num =

        /// 32-bit signed integer.
        | Int of int

        /// Arbitrary-precision integer.
        | Big_int of bigint
        | Ratio of ratio
        interface System.IComparable<Num>
        interface System.IComparable
        interface System.IEquatable<Num>

        static member (%) : x: Num * y: Num -> Num

        static member ( * ) : x: Num * y: Num -> Num

        static member (+) : x: Num * y: Num -> Num

        static member (-) : x: Num * y: Num -> Num

        static member (/) : x: Num * y: Num -> Num

        static member (~-) : x: Num -> Num

        static member Abs: x: Num -> Num

        static member private AreEqual: x: Num * y: Num -> bool

        // static member Ceiling: x: Num -> Num

        static member private Compare: x: Num * y: Num -> int

        static member Floor: x: Num -> Num

        static member private FromBigInt: value: bigint -> Num

        static member
          private FromBigRational: value: MathNet.Numerics.BigRational -> Num

        static member private FromInt64: value: int64 -> Num

        static member Max: x: Num * y: Num -> Num

        static member Min: x: Num * y: Num -> Num

        static member Parse: str: string -> Num

        static member Pow: x: Num * y: int -> Num

        static member Pow: x: Num * y: Num -> Num

        static member Quotient: x: Num * y: Num -> Num

        static member Round: x: Num -> Num

        static member Sign: x: Num -> int

        static member Truncate: x: Num -> Num

        override Equals: other: obj -> bool

        override GetHashCode: unit -> int

        override ToString: unit -> string

        member IsZero: bool

        static member One: Num

        static member Zero: Num

    type num = Num

    val inline num_of_int: r: int -> num

    /// Convert a string to a number.
    /// Raise Failure "num_of_string" if the given string is not a valid    representation of an integer
    val num_of_string: str: string -> num

    val inline (=/) : x: num -> y: num -> bool

    /// Computes greatest common divisor of two unlimited-precision integers.
    /// 
    /// The call gcd_num m n for two unlimited-precision (type num) integers m and  n 
    /// returns the (positive) greatest common divisor of m and n. If both m and n  are zero, 
    /// it returns zero.
    /// 
    /// Fails if either number is not an integer (the type num supports arbitrary   rationals).
    val gcd_num: n1: num -> n2: num -> num

    /// Computes lowest common multiple of two unlimited-precision integers.
    /// 
    /// The call lcm_num m n for two unlimited-precision (type num) integers m and  n 
    /// returns the(positive) lowest common multiple of m and n. If either m or n   (or both) 
    /// are both zero, it returns zero.
    /// 
    /// Fails if either number is not an integer (the type num supports arbitrary   rationals).
    val lcm_num: n1: num -> n2: num -> Num

    /// First number starting at n for which p succeeds.
    val first:
      n: Num ->
        p: (Num -> bool) -> Num