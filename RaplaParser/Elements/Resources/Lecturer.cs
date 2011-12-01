using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaParser.Elements.Resources
{
    class Lecturer : Resource
    {
        String forname;
        String surname;

        public Lecturer(XmlElement element)
            : base(ResourceType.Class)
        {
            this.forname = element.GetElementsByTagName(ConfigManager.getConfigString("rapla_lecturer_forename")).Item(0).ChildNodes.Item(0).Value;
            this.surname = element.GetElementsByTagName(ConfigManager.getConfigString("rapla_lecturer_surname")).Item(0).ChildNodes.Item(0).Value;
        }

        public override void print()
        {
            Console.WriteLine("    " + this.type.ToString() + ": " + this.forname + " " + this.surname);
        }
    }
}
