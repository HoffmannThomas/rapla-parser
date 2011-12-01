using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RaplaParser.Elements;
using RaplaParser.Elements.Resources;
using System.Configuration;
using ConfigurationManager;

namespace RaplaParser
{
    public class Parser
    {
        private static Dictionary<String, Resource> resourceDictionary = new Dictionary<String, Resource>();
        private static Dictionary<String, Reservation> reservationDictionary = new Dictionary<String, Reservation>();        

        private XmlDocument xmlDocument = new XmlDocument();

        public Parser(String sourceURL)
        {
            xmlDocument.Load(new XmlTextReader(sourceURL));
            this.readDocument();
        }

        public void printAppointments()
        {

            Console.WriteLine("Root element :" + this.xmlDocument.DocumentElement.Name);
            Console.WriteLine("-----------------------");

            foreach (Reservation appointment in reservationDictionary.Values)
            {
                appointment.print();
                Console.WriteLine("-----------------------");
            }

        }

        private void readDocument()
        {
            this.prepareResources();
            this.readAppointments();
        }

        private void readAppointments()
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_reservation_type_name"));

            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;
                reservationDictionary.Add(element.GetAttributeNode("id").Value, new Reservation(element));
            }
        }

        private void prepareResources()
        {
            this.readRooms();
            this.readPersons();
        }

        private void readRooms()
        {

            XmlNodeList nodeList = this.xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_resource_type_name"));

            foreach (XmlNode node in nodeList)
            {

                XmlElement element = (XmlElement)node;

                Room room = new Room(element);

                resourceDictionary.Add(element.GetAttributeNode("id").Value, room);
            }
        }

        private void readPersons()
        {

            XmlNodeList nodeList = this.xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_person_type_name"));

            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;

                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_lecturer_type_name"))
                {
                    Lecturer lecturer = new Lecturer(element);
                    resourceDictionary.Add(element.GetAttributeNode("id").Value, lecturer);
                }

                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_students_class_type_name"))
                {
                    StudentsClass studentsClass = new StudentsClass(element);
                    resourceDictionary.Add(element.GetAttributeNode("id").Value, studentsClass);
                }
            }
        }

        public static Dictionary<String, Resource> getResourceDictionary()
        {
            return resourceDictionary;
        }

        public static Dictionary<String, Reservation> getReservationDictionary()
        {
            return reservationDictionary;
        }
    }
}