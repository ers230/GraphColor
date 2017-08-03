namespace EdwinRotgans

open System
open System.Drawing

/// Functions for the creation of a endless color palet in the form of a sequence
module EndlessPalet = 
    
    /// htmlPalet is a sequence of html color tags that provide strong contrast 
    let htmlPalet = 

        let scale factor = (float Int32.MaxValue / 50.) * factor  // Dirty: manual factor to avoid grey colors at ColorTranslator.FromWin32
        
        let listOfFactors n = 
            let rec next offset (ls:float list) = 
                if offset < n/2 then 
                    let high = float offset / float n
                    let low = float (n-offset) / float n 
                    next (offset+2) (low::high::ls)                            
                else ls                                
            next 1 []    
        
        let rec generateColors n = seq{ 
            yield listOfFactors n |> List.map (scale >> int >> ColorTranslator.FromWin32 >> ColorTranslator.ToHtml)
            yield! generateColors (n*2) }
        
        generateColors 4 |> Seq.collect (fun s -> s |> Seq.toList )


    /// This dummy palet creates a sequence of strings indicating the color ID
    let dummyPalet =         
        let rec generateColors n = seq{
            yield sprintf "Color#%d" n
            yield! generateColors (n+1) }
        generateColors 1 // First color = "Color#1" 