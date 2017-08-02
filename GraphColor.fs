namespace EdwinRotgans

/// Function to assign a color from the to eacht of the graph nodes
module ColorGraph = 

    /// This function takes a list of vertices in a tuplepair (node1,node2) and assigns a color to each node in such a way that each node color is different from its neighbours
    let colorGraph paletSeq vertices =   

        let nodes = 
            vertices
            |> List.collect (fun (node1,node2) -> [node1;node2])
            |> Set |> Set.toList

        let labelToIndex labels label =
            labels |> List.findIndex (fun label' -> label' = label) 

        // Converts the vertices into sets of neighbours used for the graph color selection
        let neighbours = 
            let sets = Array.replicate nodes.Length (Set<'T>[])
            // for all vertices, update the sets of neighbours
            vertices |> List.iter (fun (label1,label2) -> 
                let idx1 = labelToIndex nodes label1
                let idx2 = labelToIndex nodes label2 
                // Add node1 to node2 and visa versa  
                sets.[idx1] <- Set.add label2 sets.[idx1]
                sets.[idx2] <- Set.add label1 sets.[idx2])
            // return list of sets of neighbours
            sets |> Array.toList

        let getIndicesNeighbours node = 
            neighbours
            |> List.indexed
            |> List.fold (fun acc (idx,s) -> 
                if s.Contains node 
                then idx::acc else acc) []

        // Colors
        let maxNumberOfNeighbours = neighbours |> List.map (fun s -> s.Count) |> List.max
        let palet = paletSeq |> Seq.take (maxNumberOfNeighbours+1) |> Set
        let remainingColorOptions = Array.replicate nodes.Length palet
        
        // Select Graph colors
        nodes |> List.map (fun node -> 
            let idx1 = labelToIndex nodes node
            // Pick color
            let color = remainingColorOptions.[idx1].MinimumElement
            // remove color form neighbours colorOptions
            getIndicesNeighbours node 
            |> List.iter (fun idx2 -> 
                remainingColorOptions.[idx2] <- remainingColorOptions.[idx2].Remove color)
            // Return labels and the color
            (node, color)
        )

    /// This dummy palet creates a sequence of strings indicating the color ID
    let dummyPalet = 
        let rec generateColors n = seq{
            yield sprintf "Color#%d" n
            yield! generateColors (n+1) }
        generateColors 1

    /// Assigns dummy color labels to each node
    let colorGraphDummyPalet vertices = colorGraph dummyPalet vertices 