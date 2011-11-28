using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RaplaParser.Elements.Resources
{
    class Room : Resource
    {
        String name;

        public Room(XmlElement element)
            : base(ResourceType.Room)
        {
            this.name = element.GetElementsByTagName("dynatt:name").Item(0).ChildNodes.Item(0).Value;
        }

        public override void print()
        {
            Console.WriteLine("    " + this.type.ToString() + ": " + this.name);
        }
    }
}
