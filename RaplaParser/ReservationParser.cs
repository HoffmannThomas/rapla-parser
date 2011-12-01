using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaplaObjects;
using RaplaObjects.Reservations;
using RaplaObjects.Utils;
using System.Xml;
using ConfigurationManager;

namespace RaplaParser
{
    class ReservationParser
    {
        private Dictionary<String, Reservation> reservationDictionary = new Dictionary<String, Reservation>();
        private ResourceParser resourceParser;

        public ReservationParser(XmlDocument xmlDocument, ResourceParser resourceParser)
        {
            this.resourceParser = resourceParser;

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_reservation_type_name"));

            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;

                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_course_type_name"))
                {

                    DateTime start = DateTime.Parse(
                        element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("start-date").Value
                        + " " +
                        element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("start-time").Value
                        );

                    DateTime end = DateTime.Parse(
                        element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("end-date").Value
                        + " " +
                        element.GetElementsByTagName("rapla:appointment").Item(0).Attributes.GetNamedItem("end-time").Value
                        );

                    Repeating repeating = parseRepeating(element);

                    Course course = new Course(
                        element.GetAttributeNode("id").Value,
                        element.Attributes.GetNamedItem("owner").Value,
                        element.GetElementsByTagName(ConfigManager.getConfigString("rapla_course_title")).Item(0).ChildNodes.Item(0).Value,
                        start,
                        end,
                        repeating
                    );

                    this.resolveResources(element, course);

                    reservationDictionary.Add(course.getID(), course);
                }
            }
        }

        private Repeating parseRepeating(XmlElement element)
        {
            RepeatingType repeatingType = RepeatingType.forever;
            int count = int.MinValue;
            DateTime until = DateTime.MinValue;

            try
            {
                switch (element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("type").Value)
                {
                    case "daily": repeatingType = RepeatingType.daily; break;
                    case "weekly": repeatingType = RepeatingType.weekly; break;
                    case "monthly": repeatingType = RepeatingType.monthly; break;
                    case "yearly": repeatingType = RepeatingType.yearly; break;
                }

                try
                {
                    count = int.Parse(element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("number").Value);
                    repeatingType = RepeatingType.forever;
                }
                catch (Exception e) { }

                try
                {
                    until = DateTime.Parse(element.GetElementsByTagName("rapla:date").Item(0).Attributes.GetNamedItem("date").Value);
                    repeatingType = RepeatingType.forever;
                }
                catch (Exception e) { }

            }
            catch (Exception e)
            {
                repeatingType = RepeatingType.once;
            }

            return new Repeating(repeatingType, until, count);
        }

        public Dictionary<String, Reservation> getReservationDictionary()
        {
            return this.reservationDictionary;
        }

        private void resolveResources(XmlElement element, Reservation reservation)
        {
            XmlNodeList nodeList = element.GetElementsByTagName("rapla:allocate");

            foreach (XmlNode node in nodeList)
            {
                String idString = node.Attributes.GetNamedItem("idref").Value;

                Resource resource;
                this.resourceParser.getResourceDictionary().TryGetValue(idString, out resource);
                reservation.addResource(resource);
            }
        }
    }
}
