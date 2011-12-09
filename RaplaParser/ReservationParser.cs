using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaplaObjects;
using RaplaObjects.Reservations;
using RaplaObjects.Repeatings;
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
            Repeating repeating = null;
            int count = int.MaxValue;
            DateTime until = DateTime.MaxValue;
            List<DateTime> exceptions = new List<DateTime>();
            bool isForever = false;
            int interval = 1;

            try
            {
                try
                {
                    count = int.Parse(element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("number").Value);
                    isForever = true;
                }
                catch (Exception) { }

                try
                {
                    until = DateTime.Parse(element.GetElementsByTagName("rapla:date").Item(0).Attributes.GetNamedItem("date").Value);
                    isForever = true;
                }
                catch (Exception) { }

                try
                {
                    interval = int.Parse(element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("interval").Value);
                }
                catch (Exception) { }

                foreach (XmlNode node in element.GetElementsByTagName("rapla:exception"))
                {
                    exceptions.Add(DateTime.Parse(node.Attributes.GetNamedItem("date").Value));
                }

                switch (element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("type").Value)
                {
                    case "daily":
                        repeating = new RepeatDaily(
                            interval,
                            until,
                            count,
                            exceptions,
                            isForever
                            ); break;

                    case "weekly":
                        repeating = new RepeatWeekly(
                            interval,
                            until,
                            count,
                            exceptions,
                            isForever
                            ); break;
                    case "monthly":
                        repeating = new RepeatMonthly(
                            until,
                            count,
                            exceptions,
                            isForever
                            ); break;
                    case "yearly":
                        repeating = new RepeatYearly(
                            until,
                            count,
                            exceptions,
                            isForever
                            ); break;
                }
            }
            catch (Exception)
            {
            }

            return repeating;
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

                try
                {
                    reservation.addAttendant(this.resourceParser.personDictionary[idString]);
                }
                catch (KeyNotFoundException)
                {
                    try { reservation.addRoom(this.resourceParser.roomDictionary[idString]); }
                    catch (KeyNotFoundException)
                    {
                        throw new Exception("Die ID " + idString + " ist nich definiert.");
                    }
                }
            }
        }
    }
}
