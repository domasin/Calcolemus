#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.String
open FolAutomReas.Lib.Lexer

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