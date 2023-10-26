#r "../src/Calcolemus/bin/Debug/net7.0/Calcolemus.dll"

open Calcolemus.Lib.String
open Calcolemus.Lib.Lexer

matches "abc" "a"
matches "abc" "d"
matches "abc" "ab"

punctuation ","

"((1 + 2) * x_1)"
|> explode
|> lexwhile punctuation 

"((1 + 2) * x_1)"
|> explode
|> lex