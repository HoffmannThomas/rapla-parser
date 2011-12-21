using System;
using ConfigurationManager;
using RaplaParser;
using ExchangeConnector;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Connector
{
    public class RaplaConnector
    {
        FolderId raplaFolderId;
        EWSConnector connector;
        ExchangeService service;
        String user;
        String password;

        private volatile bool _shouldStop;

        public void stopConnector()
        {
            Logger.Log.message("Terminating thread..");
            _shouldStop = true;
        }

        public void startConnector()
        {
            Logger.Log.message("Thread for user " + user + " working..");

            while (!_shouldStop)
            {
                // Dictionary<string, Appointment> appointments = RaplaConnectorTools.getEWSAppointments(service, raplaFolderId);
                //TODO:  try to write back new data to rapla

                Thread.Sleep(10000);
            }
            Logger.Log.message("Thread for user " + user + " terminated");
        }

        public void saveReservation(XmlDocument raplaData)
        {
            Parser parser = new Parser(raplaData);

            Dictionary<string, Appointment> appointments = RaplaConnectorTools.getEWSAppointments(service, raplaFolderId);

            foreach (String id in parser.getReservations().Keys)
            {
                if (appointments.ContainsKey(id))
                {
                    Logger.Log.message("Reservation ID " + id + " is already present");
                }
                else
                {
                    Logger.Log.message("New Reservation found:");
                    parser.getReservations()[id].print();
                    RaplaConnectorTools.SaveReservation(service, raplaFolderId, parser.getReservations()[id]);
                }
            }
        }

        public void updateReservation(XmlDocument raplaData)
        {
            Parser parser = new Parser(raplaData);

            foreach (String id in parser.getReservations().Keys)
            {
                RaplaConnectorTools.UpdateReservation(this.service, this.raplaFolderId, parser.getReservations()[id]);
            }
        }

        public void deleteReservation(String id)
        {
            RaplaConnectorTools.DeleteReservation(this.service, this.raplaFolderId, id);
        }

        public RaplaConnector(String user, String password)
        {
            this.user = user;
            this.password = password;

            this.connector = null;

            this.connector = new ExchangeConnector.EWSConnector(ConfigManager.getConfigString("fqdn"), user, password);

            this.service = connector.getEWSService();

            raplaFolderId = RaplaConnectorTools.getRaplaFolder(service);
        }
    }
}
