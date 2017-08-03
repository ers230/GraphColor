
namespace EdwinRotgans

open System
open System.Collections.Generic

 /// Generic Node with color option
module ColoredGraph = 

    /// Generic Node with color option
    type Node<'T,'C> when 'T : comparison and 'C :> IComparable = {
        Label: 'T
        Color:  'C option
        Neighbours: Set<'T>     
    } with 
        static member Create label = 
            {Label=label; Color=None; Neighbours = set<'T>[] }

        member node.UpdateColor color = {node with Color = Some color}

        member node.LinkNode label = 
            {node with Neighbours = node.Neighbours.Add label}    


          

/// Functions to assign colors to nodes of the type ColoredGraph
module ColorGraph =   

    open ColoredGraph

    let private getIndexedNode nodes label = 
        nodes 
        |> Array.indexed
        |> Array.find (fun (i,node) -> node.Label = label ) 
    
    /// Function to generate a dictionary of linked nodes form a list of vertices
    /// Any loops between a node and itself are ignored
    let private nodesFromVertices vertices =        
        vertices
        |> List.collect (fun (label1,label2) -> [label1;label2])
        |> Set |> Seq.toArray
        |> Array.map Node<'T,IComparable>.Create    


    /// Creates a list of nodes with neighbours as provided by the list of vertices
    let connectNodes vertices =          
        
        let nodes = nodesFromVertices vertices         
        let findByLabel = getIndexedNode nodes
        // Store all connections in g
        for label1,label2 in vertices do 
            let idx1, node1  = findByLabel label1
            let idx2, node2  = findByLabel label2
            nodes.[idx1] <- node1.LinkNode label2
            nodes.[idx2] <- node2.LinkNode label1        
        // return graphDict
        nodes


    /// Assigns a color to each of the nodes
    let colorGraph paletSeq nodes = 
       
        let findByLabel = getIndexedNode nodes

        // Colors       
        let maxNumberOfNeighbours = nodes |> Array.map (fun node -> node.Neighbours.Count)  |> Array.max
        let palet = paletSeq |> Seq.take (maxNumberOfNeighbours+1) |> Seq.toList

       
        // Set Graph colors        
        for i,node in nodes |> Array.indexed do
            // Filter colors
            let forbiddenColors =                 
                node.Neighbours 
                |> Set.map (findByLabel >> snd)
                |> Set.filter (fun neighbour -> neighbour.Color.IsSome )                  
                |> Set.map (fun neighbour -> neighbour.Color.Value)
            // Pick color
            let color = palet |> List.tryFind (forbiddenColors.Contains >> not) 
            // Assign color
            match color with 
            | Some c -> nodes.[i] <- node.UpdateColor c
            | None -> () 
                                  
        // return list of colored nodes
        nodes |> Array.toList

    

    