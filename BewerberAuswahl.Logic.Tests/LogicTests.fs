namespace BewerberAuswahl

open NUnit.Framework
open System
open System.Reflection
open System.IO

[<TestFixture>]
type LogicTests() = 
    [<Test>]
    static member parseCSV () =
        let baseDir = 
            Path.GetDirectoryName( Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath)
        let csvPath = 
            Path.Combine(baseDir, "BewerberListe.csv")
        let persons = Logic.parseCSV ';' csvPath
        let expected =  
            [{Age=20;Name="Hans";Email=None}
            ;{Age=30;Name="Carsten";Email=None}
            ;{Age=25;Name="Özgür";Email=None}
            ;{Age=26;Name="Ursula";Email=None}
            ;{Age=49;Name="Maurice";Email=None}
            ;{Age=20;Name="Hans";Email=Some "HansBVB@google.de"}
            ;{Age=30;Name="Carsten";Email=Some "CarstenLucky@alter.com"}
            ;{Age=25;Name="Özgür";Email=Some "fuzuli@fuzuli.de"}
            ;{Age=26;Name="Ursula";Email=Some "ursula@yahoo.com"}
            ;{Age=49;Name="Maurice";Email=Some "maurice@cloudworld.us"}
            ]
        let condition = persons = expected
        Assert.IsTrue(condition)
