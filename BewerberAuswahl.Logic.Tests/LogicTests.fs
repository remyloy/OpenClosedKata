namespace BewerberAuswahl

open NUnit.Framework
open System
open System.Reflection
open System.IO

[<TestFixture>]
type LogicTests() =
    static member candidate (age, name, ?email) =
        { Age = Age age; Name = name; Email = email |> Option.map Email }

    [<Test>]
    static member parseCSV () =
        let baseDir = 
            Path.GetDirectoryName( Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath)
        let csvPath = 
            Path.Combine(baseDir, "BewerberListe.csv")
        let persons = Logic.parseCSV ';' csvPath
        let expected =  
            [LogicTests.candidate (20, "Hans")
            ;LogicTests.candidate (30, "Carsten")
            ;LogicTests.candidate (25, "Özgür")
            ;LogicTests.candidate (26, "Ursula")
            ;LogicTests.candidate (49, "Maurice")
            ;LogicTests.candidate (20, "Hans", "HansBVB@google.de")
            ;LogicTests.candidate (30, "Carsten", "CarstenLucky@alter.com")
            ;LogicTests.candidate (25, "Özgür", "fuzuli@fuzuli.de")
            ;LogicTests.candidate (26, "Ursula", "ursula@yahoo.com")
            ;LogicTests.candidate (49, "Maurice", "maurice@cloudworld.us")
            ]
        let condition = persons = expected
        Assert.IsTrue(condition)

    [<Test>]
    static member ``filterHasUmlaut fails with Özgür`` () =
        let candidate =
            LogicTests.candidate (25, "Özgür")
        let result =
            Logic.filterHasUmlaut ignore candidate
        match result with
        | Valid -> Assert.Fail()
        | Error e -> Assert.Pass()
        | Skip -> Assert.Fail()

    [<Test>]
    static member ``filterHasUmlaut passes with Hans`` () =
        let candidate =
            LogicTests.candidate (20, "Hans")
        let result =
            Logic.filterHasUmlaut ignore candidate
        match result with
        | Valid -> Assert.Pass()
        | Error e -> Assert.Fail(e |> string)
        | Skip -> Assert.Fail()

    [<Test>]
    static member ``filterTooOld passes with Hans`` () =
        let candidate =
            LogicTests.candidate (20, "Hans")
        let result =
            Logic.filterTooOld ignore (Age 38) candidate
        match result with
        | Valid -> Assert.Pass()
        | Error e -> Assert.Fail(e |> string)
        | Skip -> Assert.Fail()

    [<Test>]
    static member ``filterTooOld fails with Maurice`` () =
        let candidate =
            LogicTests.candidate (49, "Maurice")
        let result =
            Logic.filterTooOld ignore (Age 38) candidate
        match result with
        | Valid -> Assert.Fail()
        | Error e -> Assert.Pass()
        | Skip -> Assert.Fail()

    [<Test>]
    static member ``filterByNames passes with Hans`` () =
        let candidate =
            LogicTests.candidate (20, "Hans")
        let result =
            Logic.filterByNames ignore (["Carsten"] |> Set.ofList) candidate
        match result with
        | Valid -> Assert.Pass()
        | Error e -> Assert.Fail(e |> string)
        | Skip -> Assert.Fail()

    [<Test>]
    static member ``filterByNames fails with Carsten`` () =
        let candidate =
            LogicTests.candidate (30, "Carsten")
        let result =
            Logic.filterByNames ignore (["Carsten"] |> Set.ofList) candidate
        match result with
        | Valid -> Assert.Fail()
        | Error e -> Assert.Pass()
        | Skip -> Assert.Fail()
