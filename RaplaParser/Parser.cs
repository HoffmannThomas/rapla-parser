using System;
using System.Xml;
using RaplaObjects;

namespace RaplaParser
{
    public class Parser
    {
        ResourceParser resourceParser;
        ReservationParser reservationParser;

        private XmlDocument xmlDocument = new XmlDocument();

        public Parser(String sourceURL)
        {
            xmlDocument.Load(new XmlTextReader(sourceURL));
            this.resourceParser = new ResourceParser(this.xmlDocument);
            this.reservationParser = new ReservationParser(this.xmlDocument, resourceParser);
        }

        public void printAppointments()
        {

            Console.WriteLine("Root element :" + this.xmlDocument.DocumentElement.Name);
            Console.WriteLine("-----------------------");

            foreach (Reservation appointment in this.reservationParser.getReservationDictionary().Values)
            {
                appointment.print();
                Console.WriteLine("-----------------------");
            }
        }
    }
}