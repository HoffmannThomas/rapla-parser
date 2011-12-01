using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaObjects.Persons
{
    public class Lecturer : Person
    {
        String forname;
        String surname;

        public Lecturer(String id, String forname, String surname)
            : base(id)
        {
            this.forname = forname;
            this.surname = surname;
        }

        public override void print()
        {
            Console.WriteLine("    " + "Lecturer: " + this.forname + " " + this.surname);
        }
    }
}
