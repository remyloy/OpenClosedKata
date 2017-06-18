using System;
using System.IO;
using System.Text;
using Kunde2.BewerberAuswahl.UI;
using NUnit.Framework;

namespace Kunde2.BewerberAuswahl.Test.UI
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
                    expectedAusgabe.AppendFormat("Mail mit dem Betreff: \"Einladung zum Bewerbungsgespräch\" wurde an HansBVB@google.de geschickt.\r\n");

                    Assert.That(ausgabeDerKonsolenAnwendung.ToString(), Is.EqualTo(expectedAusgabe.ToString()));
                }
            }
        }
    }
}
