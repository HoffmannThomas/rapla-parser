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
        private static Dictionary<String, Appointment> appointmentDictionary = new Dictionary<String, Appointment>();        

        private XmlDocument xmlDocument = new XmlDocument();

        public Parser()
        {
            xmlDocument.Load(ConfigManager.getConfigString("rapla_data_path"));
            this.readDocument();
        }

        public void printAppointments()
        {

            Console.WriteLine("Root element :" + this.xmlDocument.DocumentElement.Name);
            Console.WriteLine("-----------------------");

            foreach (Appointment appointment in appointmentDictionary.Values)
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
                appointmentDictionary.Add(element.GetAttributeNode("id").Value, new Appointment(element));
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

                Person person = new Person(element);

                resourceDictionary.Add(element.GetAttributeNode("id").Value, person);
            }
        }

        public static Dictionary<String, Resource> getResourceDictionary()
        {
            return resourceDictionary;
        }

        public static Dictionary<String, Appointment> getAppointmentDictionary()
        {
            return appointmentDictionary;
        }
    }
}