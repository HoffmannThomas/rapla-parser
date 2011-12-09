using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaplaObjects;
using System.Xml;
using RaplaObjects.Rooms;
using RaplaObjects.Persons;
using ConfigurationManager;

namespace RaplaParser
{
    class ResourceParser
    {
        public Dictionary<String, Person> personDictionary { get; private set;} 
        public Dictionary<String, Room> roomDictionary { get; private set;} 

        public ResourceParser(XmlDocument xmlDocument)
        {
            this.personDictionary = new Dictionary<String, Person>();
            this.roomDictionary = new Dictionary<String, Room>();
            this.parseRooms(xmlDocument);
            this.parsePersons(xmlDocument);
        }

        private void parseRooms(XmlDocument xmlDocument)
        {
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_resource_type_name"));

            foreach (XmlNode node in nodeList)
            {

                XmlElement element = (XmlElement)node;
                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_course_room_type_name"))
                {
                    CourseRoom courseRoom = new CourseRoom(
                        element.GetAttributeNode("id").Value,
                        int.Parse(element.GetElementsByTagName(ConfigManager.getConfigString("rapla_room_number")).Item(0).ChildNodes.Item(0).Value)
                    );

                    this.roomDictionary.Add(courseRoom.getID(), courseRoom);
                }
            }
        }

        private void parsePersons(XmlDocument xmlDocument)
        {

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(ConfigManager.getConfigString("rapla_person_type_name"));

            foreach (XmlNode node in nodeList)
            {
                XmlElement element = (XmlElement)node;

                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_lecturer_type_name"))
                {
                    Lecturer lecturer = new Lecturer(
                        element.GetAttributeNode("id").Value,
                        element.GetElementsByTagName(ConfigManager.getConfigString("rapla_lecturer_forename")).Item(0).ChildNodes.Item(0).Value,
                        element.GetElementsByTagName(ConfigManager.getConfigString("rapla_lecturer_surname")).Item(0).ChildNodes.Item(0).Value
                     );

                    personDictionary.Add(lecturer.getID(), lecturer);
                }

                if (element.ChildNodes.Item(0).Name == ConfigManager.getConfigString("rapla_students_class_type_name"))
                {

                    StudentsClass studentsClass = new StudentsClass(
                        element.GetAttributeNode("id").Value,
                        element.GetElementsByTagName(ConfigManager.getConfigString("rapla_students_class_name")).Item(0).ChildNodes.Item(0).Value
                    );

                    personDictionary.Add(studentsClass.getID(), studentsClass);
                }
            }
        }

    }
}
