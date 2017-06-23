namespace BewerberAuswahl

module Option =
    let apply f o =
        match f, o with
        | Some f, Some o ->
            f o 
            |> Some
        | _ -> 
            None

module Logic =
    open System
    open System.IO

    let createCandidate : CreateCandidate =
        fun age name email -> 
            { Age = age
            ; Name = name
            ; Email = email 
            }

    let tryParseInt s =
        let result, name = Int32.TryParse s
        match result, name with 
        | true, name -> Some name 
        | false, _ -> None

    let parseCSV : char -> ParseCSV =
        let tokenize tokens =
            let age =
                tokens 
                |> Array.tryItem 0
                |> Option.bind tryParseInt
            let name =
                tokens 
                |> Array.tryItem 1
            let email =
                tokens 
                |> Array.tryItem 2
            age, name, email

        let (<!>) = Option.map
        let (<*>) = Option.apply

        let tryCreateCandidate (age, name, email) = 
            createCandidate <!> age <*> name <*> Some email

        fun seperator ->
            let split (line : string) =
                line.Split(seperator)

            fun filename ->
                File.ReadAllLines filename
                |> Seq.map split
                |> Seq.map tokenize
                |> Seq.choose tryCreateCandidate
                |> Seq.toList

module Program =
    [<EntryPoint>]
    let main argv = 
        printfn "%A" argv
        0 // return an integer exit code
