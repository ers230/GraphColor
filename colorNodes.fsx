#load "./GraphColor.fs"

open System
open EdwinRotgans.ColorGraph

// Test data
let vertices = [("a","b");("a","e");("b","c");("b","f");("c","a");("c","f");("d","e");("e","a")]  

// Assign colors to nodes
let coloredNodes = colorGraphDummyPalet vertices 

// Output to screen
Console.WriteLine "\n(Node, color)"
coloredNodes |> List.iter (sprintf "%A" >> Console.WriteLine)
Console.WriteLine "\nPress Enter to quit\n"
Console.ReadLine() |> ignore