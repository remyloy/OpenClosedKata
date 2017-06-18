using System;
using System.IO;
using System.Text;
using Kunde1.BewerberAuswahl.UI;
using NUnit.Framework;

namespace Kunde1.BewerberAuswahl.Test.UI
{
    [TestFixture]
    public class ProgramTest
    {
        [SetUp]
        public void InitializeTest()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };


            Console.SetOut(standardOut);
        }

        [Test]
        public void TesteAnwendungMitKundenDaten()
        {
            using (var ausgabeDerKonsolenAnwendung = new StringWriter())
            {
                Console.SetOut(ausgabeDerKonsolenAnwendung);
                using (var reader = new StringReader(string.Format("Mark{0}Ploeh{0}", Environment.NewLine)))
                {
                    Console.SetIn(reader);
                    Program.Main(new string[] { });

                    var expectedAusgabe = new StringBuilder();
                    expectedAusgabe.AppendFormat("Hans kann hier arbeiten!\r\n");
                    expectedAusgabe.AppendFormat("Carsten kann hier arbeiten!\r\n");
                    expectedAusgabe.AppendFormat("Özgür kann hier nicht arbeiten!\r\n");
                    expectedAusgabe.AppendFormat("Ursula kann hier arbeiten!\r\n");
                    expectedAusgabe.AppendFormat("Maurice kann hier nicht arbeiten!\r\n");
                    expectedAusgabe.AppendFormat("Bitte drücken Sie eine belibige Taste um die Anwendung zu beenden.\r\n");



                    Assert.That(ausgabeDerKonsolenAnwendung.ToString(), Is.EqualTo(expectedAusgabe.ToString()));
                }
            }
        }
    }
}