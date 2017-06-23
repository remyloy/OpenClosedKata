namespace BewerberAuswahl

module Logic =
    open System
    open System.IO

    let createAge : int -> Option<Age> =
        fun age ->
            if age >= 0 || age < 130 then Age age |> Some
            else None
     
    let createEmail : string -> Option<Email> =
        fun email ->
            if email.Contains("@") then Email email |> Some
            else None

    let tryCreateCandidate : TryCreateCandidate =
        let (<!>) = Option.map
        let (<*>) = Option.apply
        let create a n e =
            { Age = a
            ; Name = n
            ; Email = e
            }
        fun age name email ->
            let age =
                createAge age
            let name =
                Some name
            let email = 
                email
                |> Option.bind createEmail 
                |>  Some
            create <!> age <*> name <*> email

    let parseCSV : char -> ParseCSV =
        let tryParseInt s =
            let result, name = Int32.TryParse s
            match result, name with 
            | true, name -> Some name 
            | false, _ -> None

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
        let tryCreateCandidate (a,n,e) =
            tryCreateCandidate <!> a <*> n <*> Some e
            |> Option.flatten

        fun seperator ->
            let split (line : string) =
                line.Split(seperator)

            fun filename ->
                File.ReadAllLines filename
                |> Seq.map split
                |> Seq.map tokenize
                |> Seq.choose tryCreateCandidate
                |> Seq.toList

    let filterHasUmlaut : FilterCriteria =
        let umlaute = 
            ['ä';'ö';'ü'] 
            |> Set.ofList
        fun candidate ->
            candidate.Name
            |> Set.ofSeq
            |> Set.intersect umlaute
            |> fun set ->
            if Set.isEmpty set then Valid 
            else candidate.Name |> HasUmlaut |> Error

    let filterTooOld : Age -> FilterCriteria =
        fun (Age maximumAge) ->
            fun candidate ->
                let (Age age) = candidate.Age
                if age <= maximumAge then Valid
                else age |> Age |> TooOld |> Error

    let filterByNames : Set<string> -> FilterCriteria =
        fun blacklist ->
            fun candidate ->
                match Set.contains candidate.Name blacklist with
                | true ->
                    candidate.Name |> Blacklisted |> Error
                | false ->
                    Valid
