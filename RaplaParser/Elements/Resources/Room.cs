using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaParser.Elements.Resources
{
    class Room : Resource
    {
        String name;

        public Room(XmlElement element)
            : base(ResourceType.Room)
        {
            this.name = element.GetElementsByTagName(ConfigManager.getConfigString("rapla_room_number")).Item(0).ChildNodes.Item(0).Value;
        }

        public override void print()
        {
            Console.WriteLine("    " + this.type.ToString() + ": " + this.name);
        }
    }
}
