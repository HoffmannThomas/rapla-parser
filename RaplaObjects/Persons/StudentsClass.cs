using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaObjects.Persons
{
    public class StudentsClass : Person
    {
        String course_name;

        public StudentsClass(String id, String course_name)
            : base(id)
        {
            this.course_name = course_name;
        }

        public override void print()
        {
            Console.WriteLine("    " + "StudentsClass: " + this.course_name);
        }
    }
}
