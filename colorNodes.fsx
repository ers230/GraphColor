#load "./GraphColor.fs"
#load "./EndlessPalet.fs"



open System
open EdwinRotgans.Graph.ColorGraph
open EdwinRotgans.EndlessPalet
// open EdwinRotgans.Graph.Palet


// Test data
// let vertices = [("c","c");("a","b");("a","e");("b","c");("b","f");("c","a");("c","f");("d","e");("e","a")]  
let vertices = [(3,3);(1,2);(1,5);(2,3);(2,6);(3,1);(3,6);(4,5);(5,1)]  

let nodes = connectNodes vertices

// Assign colors to nodes
let coloredNodes = colorGraph htmlPalet nodes 

// Output to screen
Console.WriteLine "\n(Node, color)"
coloredNodes |> List.iter (sprintf "%A" >> Console.WriteLine)
Console.WriteLine "\nPress Enter to quit\n"
Console.ReadLine() |> ignore