using System;
using ConfigurationManager;
using RaplaParser;
using ExchangeConnector;
using Microsoft.Exchange.WebServices.Data;
using System.Xml;

namespace Connector
{
    public class RaplaConnector
    {
        EWSConnector connector;
        ExchangeService service;
        String user;
        String password;

        // Dictionary<string, Appointment> appointments = RaplaConnectorTools.getEWSAppointments(service, raplaFolderId);

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
