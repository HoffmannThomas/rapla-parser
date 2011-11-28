using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RaplaParser.Elements.Resources
{
    class Person : Resource
    {
        String forname;
        String lastname;

        public Person(XmlElement element)
            : base(ResourceType.Person)
        {
            this.forname = element.GetElementsByTagName("dynatt:forename").Item(0).ChildNodes.Item(0).Value;
            this.lastname = element.GetElementsByTagName("dynatt:surname").Item(0).ChildNodes.Item(0).Value;
        }

        public override void print()
        {
            Console.WriteLine("    " + this.type.ToString() + ": " + this.forname + " " + this.lastname);
        }
    }
}
