// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace FolAutomReas.Lib

/// Explosion and implosion of strings.
module String = 

    /// Explosion of strings.
    val explode: s: string -> string list

    /// Implosion of strings.
    val implode: l: string list -> string