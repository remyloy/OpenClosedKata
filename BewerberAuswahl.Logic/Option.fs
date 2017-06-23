namespace BewerberAuswahl

module Option =
    let apply f o =
        match f, o with
        | Some f, Some o ->
            f o 
            |> Some
        | _ -> 
            None
