namespace EdwinRotgans.Graph

open System.Collections.Generic

module ColoredGraph = 

    /// Generic Node with color option
    type Node<'T,'C> when 'T : comparison = {
        Label: 'T
        Color:  'C option
        Neighbours: Set<'T>     
    } with 
        static member Create label = 
            {Label=label; Color=None; Neighbours = set<'T>[] }

        member node.UpdateColor color = {node with Color = Some color}

        member node.LinkNode label = 
            {node with Neighbours = node.Neighbours.Add label}    

    
    
    /// Function to generate a dictionary of linked nodes form a list of vertices
    /// Any loops between a node and itself are ignored
    let nodesFromVertices vertices = 

        let nodeLabels = 
            vertices
            |> List.collect (fun (node1,node2) -> [node1;node2])
            |> Set |> Set.toList        

        // Store all connections as a dict of nodes and neighbours   
        let graph = Dictionary<'T,Node<'T,'C>>()                         
        for label in nodeLabels do 
            graph.Add (label, Node<'T,'C>.Create label)   
           
        // Store all connections 
        for label1,label2 in vertices do
            // prevent loop   
            if label1 = label2 then 
                printf "\nIGNORED LOOP %A <=> %A\n" label1 label2; ()                         
            graph.[label1] <- graph.[label1].LinkNode label2
            graph.[label2] <- graph.[label2].LinkNode label1
        
        // return graphDict
        graph


    /// Function to assign a color from the to each of the graph nodes 
    let colorGraph paletSeq (graph: Dictionary<'T,Node<'T,'C>>) = 

        let nodes = graph.Values |> Seq.toList
        // Colors       
        let maxNumberOfNeighbours = nodes |> List.map (fun n -> n.Neighbours.Count)  |> List.max
        let palet = paletSeq |> Seq.take (maxNumberOfNeighbours+1) |> Seq.toList
                
        // Set Graph colors        
        for node in nodes do
            // Filter colors
            let forbiddenColors =                 
                node.Neighbours 
                |> Set.filter (fun label -> graph.[label].Color.IsSome )                  
                |> Set.map (fun label -> graph.[label].Color.Value)
            // Pick color
            let color = palet |> List.find (forbiddenColors.Contains >> not) 
            // Assign color
            graph.[node.Label] <- node.UpdateColor color                 
        
        // return node list
        graph.Values |> Seq.toList  


    /// This dummy palet creates a sequence of strings indicating the color ID
    let dummyPalet = 
        let rec generateColors n = seq{
            yield sprintf "Color#%d" n
            yield! generateColors (n+1) }
        generateColors 1 // First color = "Color#1"

    /// Assigns dummy color labels to each node
    let colorGraphDummyPalet graph = colorGraph dummyPalet graph 