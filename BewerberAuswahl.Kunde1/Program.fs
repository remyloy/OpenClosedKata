open BewerberAuswahl
open System

module Logic =
    let filterNamesStartingWith : List<string> -> FilterCriteria<string> =
        fun prefixes ->
            let filters =
                prefixes
                |> List.map (fun prefix -> 
                    fun candidate -> 
                        if candidate.Name.StartsWith prefix then sprintf "Name beginnt mit %s" prefix |> Error
                        else Valid)

            Logic.combine filters

module CLI =
    let formatResult v =
        match v with
        | Valid ->
            "-/-"
        | Skip -> 
            "-/-"
        | Error e ->
            e

    let formatValidation v =
        sprintf "%s: %s" v.Value.Name (formatResult v.Validation)

    let writeToConsole : GenerateOutput<string> =
        fun candidates ->
            candidates
            |> List.map formatValidation
            |> List.iter Console.WriteLine

[<EntryPoint>]
let main argv = 
    let filters =
        Logic.combine 
            [ Logic.filterHasUmlaut (fun _ -> "hat Umlaute")
            ; Logic.filterTooOld (fun (Age a) -> sprintf "ist mit %d zu alt" a) (Age 38)
            ; Logic.filterNamesStartingWith ["Do"; "Re"]
            ]

    Logic.parseCSV ';' "BewerberListe.csv"
    |> Logic.validate filters
    |> CLI.writeToConsole
    
    0
