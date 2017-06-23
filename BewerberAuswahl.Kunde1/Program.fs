open BewerberAuswahl
open System

let formatResult v =
    match v with
    | Valid ->
        "-/-"
    | Error e ->
        match e with
        | HasUmlaut n ->
            "hat Umlaute"
        | TooOld (Age a) ->
            sprintf "ist mit %d zu alt" a
        | Blacklisted n ->
            "ist auf der schwarzen Liste"

let formatValidation v =
    sprintf "%s: %s" v.Value.Name (formatResult v.Validation)

let writeToConsole : GenerateOutput =
    fun candidates ->
        candidates
        |> List.map formatValidation
        |> List.iter Console.WriteLine

[<EntryPoint>]
let main argv = 
    let filters =
        Logic.combine [Logic.filterHasUmlaut; Logic.filterTooOld (Age 38)]

    Logic.parseCSV ';' "BewerberListe.csv"
    |> Logic.validate filters
    |> writeToConsole
    
    0
