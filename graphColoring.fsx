#load "./GraphColor.fs"


open System
open EdwinRotgans.ColorGraph


let vertices = [("a","b");("a","e");("b","c");("b","f");("c","a");("c","f");("d","e");("e","a")]  

let coloredNodes = colorGraph dummyPalet vertices 

// Output
Console.WriteLine "\n(Node, color)"
coloredNodes |> List.iter (sprintf "%A" >> Console.WriteLine)
Console.WriteLine "\nPress Enter to quit\n"
Console.ReadLine() |> ignore

let test = true