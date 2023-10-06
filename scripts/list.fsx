#r "../src/FolAutomReas/bin/Debug/net7.0/FolAutomReas.dll"

open FolAutomReas.Lib.List

last ([]:int list)

chop_list 2 [1;2;3;4;5;6]
List.splitAt 2 [1;2;3;4;5;6]

['a';'b';'c';'d'] |> chop_list -1
['a';'b';'c';'d'] |> List.splitAt -1

['a';'b';'c';'d'] |> chop_list 5
['a';'b';'c';'d'] |> List.splitAt 5

[ 0; 1; 2 ] |> insertat 1 9
[ 0; 1; 2 ] |> List.insertAt 1 9

[ 0; 1; 2 ] |> insertat -1 9
[ 0; 1; 2 ] |> List.insertAt -1 9

[ 0; 1; 2 ] |> insertat 4 9
[ 0; 1; 2 ] |> List.insertAt 4 9

index 2 [0;1;3;3;2;3]
index 5 [0;1;3;3;2;3]

earlier [0;1;2;3] 2 3
earlier [0;1;2;3] 3 4

earlier [0;1;2;3] 3 2
earlier [0;1;3] 2 3
earlier [0;1;2;3] 4 5



