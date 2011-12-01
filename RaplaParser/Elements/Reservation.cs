using System;
using System.Collections.Generic;
using System.Xml;

namespace RaplaParser.Elements
{
    public class Reservation
    {

        private String name;
        private String owner;
        private String start_date;
        private String end_date;
        private String start_time;
        private String end_time;
        private Repeating repeating;
        private List<Resource> resources = new List<Resource>();

        public Reservation(XmlElement element)
        {
            this.name = element.GetElementsByTagName("dynatt:name").Item(0).ChildNodes.Item(0).Value;
            this.owner = element.Attributes.GetNamedItem("owner").Value;
            this.start_date = element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("start-date").Value;
            this.end_date = element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("end-date").Value;
            this.start_time = element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("start-time").Value;
            this.end_time = element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("end-time").Value;

            this.repeating = new Repeating(element);

            this.resolveResources(element);
        }

        private void resolveResources(XmlElement element)
        {

            XmlNodeList nodeList = element.GetElementsByTagName("rapla:allocate");

            foreach (XmlNode node in nodeList)
            {

                String idString = node.Attributes.GetNamedItem("idref").Value;

                Resource resource;
                Parser.getResourceDictionary().TryGetValue(idString, out resource);
                this.resources.Add(resource);

            }
        }

        public void print()
        {
            Console.WriteLine("Name : " + this.name);
            Console.WriteLine("Owner : " + this.owner);

            Console.WriteLine("Start-Date : " + this.start_date);
            Console.WriteLine("Start-Time : " + this.start_time);
            Console.WriteLine("End-Date : " + this.end_date);
            Console.WriteLine("End-Time : " + this.end_time);

            this.repeating.print();

            Console.WriteLine("Resources:");
            foreach (Resource resource in this.resources)
            {
                resource.print();

            }

        }

        public void addResource(Resource resource)
        {
            this.resources.Add(resource);
        }
    }
}
