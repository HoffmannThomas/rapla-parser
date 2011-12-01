using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaParser.Elements.Resources
{
    class StudentsClass : Resource
    {
        String name;

        public StudentsClass(XmlElement element)
            : base(ResourceType.Class)
        {
            this.name = element.GetElementsByTagName(ConfigManager.getConfigString("rapla_students_class_name")).Item(0).ChildNodes.Item(0).Value;
        }

        public override void print()
        {
            Console.WriteLine("    " + this.type.ToString() + ": " + this.name);
        }
    }
}
