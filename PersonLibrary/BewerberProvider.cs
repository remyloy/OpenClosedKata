using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonLibrary
{
    public class BewerberProvider
    {
        public static List<Person> HoleListeDerBewerber(string dateiPfad)
        {
            var result = new List<Person>();
            var lines = File.ReadAllLines(dateiPfad).Select(a => a.Split(';'));
            foreach (var personDatenLine in lines)
            {
                var alter = int.Parse(personDatenLine[0]);
                var vorname = personDatenLine[1];
                var email = string.Empty;

                try
                {
                    email = personDatenLine[2];
                }
                catch (Exception) { }

                var person = new Person(alter, vorname, email);
                result.Add(person);
            }


            return result;
        }
    }
}