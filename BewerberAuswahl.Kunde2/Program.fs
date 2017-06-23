open BewerberAuswahl

module Logic =
    let skipValidation : string -> FilterCriteria<_> =
        fun name ->
            fun candidate ->
                if candidate.Name = name then Skip
                else Valid

module Environment =
    let sendEmail candidates =
        candidates
        |> List.filter (fun c -> c.Validation |> function | Valid -> true | Error _ -> false | Skip -> true)
        |> List.map (fun c -> c.Value)
        |> List.choose (fun c -> c.Email |> Option.map (fun email -> c.Name, email))
        |> List.iter (fun (name, (Email email)) ->
            printfn "send.exe to=%s body='Sehr geehrter %s, wir möchten Sie einladen.'" email name)

[<EntryPoint>]
let main argv = 
    let filters =
        Logic.combine 
            [ Logic.filterHasUmlaut ignore
            ; Logic.filterTooOld ignore (Age 25)
            ; Logic.filterByNames ignore (["Carsten"] |> Set.ofList)
            ; Logic.skipValidation "Michael"
            ]

    Logic.parseCSV ';' "BewerberListe.csv"
    |> Logic.validate filters
    |> Environment.sendEmail
    
    0 // return an integer exit code
