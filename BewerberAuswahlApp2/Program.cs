using System.Linq;
using PersonLibrary;

namespace Kunde2.BewerberAuswahl.UI
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
                if (person.Alter > 25)
                {
                    continue;
                }
                else if (person.BewerberVorname.Equals("Carsten"))
                {
                    continue;
                }
                else if (person.BewerberVorname.ToLower().ToCharArray().Intersect(umlaute).Any())
                {
                    continue;
                }

                person.SendMail();
            }
        }
    }
}
