using System;
using ConfigurationManager;
using RaplaParser;
using ExchangeConnector;
using Microsoft.Exchange.WebServices.Data;
using System.Xml;
using System.Collections.Generic;

namespace Connector
{
    public class RaplaConnector
    {
        EWSConnector connector;
        ExchangeService service;
        String user;
        String password;

        public void saveReservation(XmlDocument raplaData)
        {
            Parser parser = new Parser(raplaData);

            foreach (String id in parser.getReservations().Keys)
            {
                if (id.Equals(0))
                {
                    Logger.Log.message("New Reservation found:");
                    parser.getReservations()[id].print();
                    RaplaConnectorTools.SaveReservation(service, parser.getReservations()[id]);
                }
                else
                {
                    RaplaConnectorTools.UpdateReservation(this.service, parser.getReservations()[id]);
                }
            }
        }

        public XmlDocument getLastModified(DateTime time)
        {

            XmlDocument results = new XmlDocument();
            XmlDeclaration dec = results.CreateXmlDeclaration("1.0", null, null);
            results.AppendChild(dec);
            XmlElement root = results.CreateElement("results");
            results.AppendChild(root);

            SearchFilter filter = new SearchFilter.IsGreaterThan(AppointmentSchema.LastModifiedTime, time);

            List<Appointment> appointments = RaplaConnectorTools.getEWSAppointments(service, filter);

            foreach (Appointment appointment in appointments)
            {
                XmlElement reservation = results.CreateElement("reservation");
                reservation.SetAttribute("id", appointment.Id.ToString());
                root.AppendChild(reservation);
            }

            return results;

        }

        public void deleteReservation(String id)
        {
            RaplaConnectorTools.DeleteReservation(this.service, id);
        }

        public RaplaConnector(String user, String password)
        {
            this.user = user;
            this.password = password;

            this.connector = null;

            this.connector = new ExchangeConnector.EWSConnector(ConfigManager.getConfigString("fqdn"), user, password);

            this.service = connector.getEWSService();
        }
    }
}
