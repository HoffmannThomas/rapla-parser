using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaObjects.Persons
{
    public class Lecturer : Person
    {
        public String forname { get; private set; }
        public String surname { get; private set; }

        public Lecturer(String id, String forname, String surname)
            : base(id)
        {
            this.forname = forname;
            this.surname = surname;
        }

        public override string ToString()
        {
            return "Lecturer: " + this.forname + " " + this.surname;
        }
    }
}
