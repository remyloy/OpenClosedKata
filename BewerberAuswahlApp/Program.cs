using System;
using System.Linq;
using PersonLibrary;

namespace Kunde1.BewerberAuswahl.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var umlaute = new char[] { 'ö', 'ü', 'ä' };

            var bewerberDateiPfad = System.Configuration.ConfigurationManager.AppSettings["BewerberDatei"];
            var listeDerBewerber = BewerberProvider.HoleListeDerBewerber(bewerberDateiPfad);
            foreach (var person in listeDerBewerber)
            {
                if (person.BewerberVorname.ToLower().ToCharArray().Intersect(umlaute).Any())
                {
                    Console.WriteLine("{0} kann hier nicht arbeiten!", person.BewerberVorname);
                }
                else if (person.Alter > 38)
                {
                    Console.WriteLine("{0} kann hier nicht arbeiten!", person.BewerberVorname);
                }
                else
                {
                    Console.WriteLine("{0} kann hier arbeiten!", person.BewerberVorname);
                }
            }

            Console.WriteLine("Bitte drücken Sie eine belibige Taste um die Anwendung zu beenden.");
            Console.ReadLine();
        }
    }
}
