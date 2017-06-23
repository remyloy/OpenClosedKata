﻿namespace BewerberAuswahl

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
    static member ``validateUmlaut fails with Özgür`` () =
        let candidate =
            LogicTests.candidate (25, "Özgür")
        let result =
            Logic.validateUmlaut candidate
        match result with
        | Valid -> Assert.Fail()
        | Error e -> Assert.Pass()

    [<Test>]
    static member ``validateUmlaut passes with Hans`` () =
        let candidate =
            LogicTests.candidate (20, "Hans")
        let result =
            Logic.validateUmlaut candidate
        match result with
        | Valid -> Assert.Pass()
        | Error e -> Assert.Fail(e |> string)

    [<Test>]
    static member ``validateAge passes with Hans`` () =
        let candidate =
            LogicTests.candidate (20, "Hans")
        let result =
            Logic.validateAge (Age 38) candidate
        match result with
        | Valid -> Assert.Pass()
        | Error e -> Assert.Fail(e |> string)

    [<Test>]
    static member ``validateAge fails with Maurice`` () =
        let candidate =
            LogicTests.candidate (49, "Maurice")
        let result =
            Logic.validateAge (Age 38) candidate
        match result with
        | Valid -> Assert.Fail()
        | Error e -> Assert.Pass()
    