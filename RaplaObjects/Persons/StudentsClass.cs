using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaObjects.Persons
{
    public class StudentsClass : Person
    {
        public String course_name { get; private set; }

        public StudentsClass(String id, String course_name)
            : base(id)
        {
            this.course_name = course_name;
        }

        public override string ToString()
        {
            return "StudentsClass: " + this.course_name;
        }
    }
}
