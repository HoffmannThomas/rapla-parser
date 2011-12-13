using System;
using System.Xml;
using RaplaObjects;
using System.Collections.Generic;

namespace RaplaParser
{
    public class Parser
    {
        ResourceParser resourceParser;
        ReservationParser reservationParser;

        private XmlDocument xmlDocument = new XmlDocument();

        public Parser(XmlDocument raplaData)
        {
            this.xmlDocument = raplaData;
            this.resourceParser = new ResourceParser(this.xmlDocument);
            this.reservationParser = new ReservationParser(this.xmlDocument, resourceParser);
        }

        public Dictionary<String, Reservation> getReservations()
        {
            return this.reservationParser.getReservationDictionary();
        }
    }
}