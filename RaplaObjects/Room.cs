using System;
using System.Xml;
using ConfigurationManager;

namespace RaplaObjects
{
    public abstract class Room : Resource
    {
        public Room(String id) : base(id) { }
    }
}
