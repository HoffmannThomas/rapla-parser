using System;
using ConfigurationManager;
using RaplaParser;
using ExchangeConnector;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.Generic;
using System.Threading;

namespace Connector
{
    public class RaplaConnector
    {
        Parser parser;
        FolderId raplaFolderId;
        EWSConnector connector;
        ExchangeService service;
        String user;
        String password;

        private volatile bool _shouldStop;

        public void stopConnector()
        {
            Logger.Log.message("Terminating thread...");
            _shouldStop = true;
        }

        public void startConnector()
        {
            Logger.Log.message("thread for user " + user + " working...");

            connector = null;

            try
            {
                connector = new ExchangeConnector.EWSConnector(ConfigManager.getConfigString("fqdn"), user, password);
            }
            catch (Exception ex)
            {
                Logger.Log.message("Error: " + ex.Message);
            }

            service = connector.getEWSService();

            parser = new Parser(ConfigManager.getConfigString("rapla_data_path"));

            raplaFolderId = RaplaConnectorTools.getRaplaFolder(service);

            while (!_shouldStop)
            {
                Dictionary<string, Appointment> appointments = RaplaConnectorTools.getEWSAppointments(service, raplaFolderId);

                foreach (String id in parser.getReservations().Keys)
                {
                    if (appointments.ContainsKey(id))
                    {
                        Logger.Log.message("Reservation ID " + id + " is already present.");
                    }
                    else
                    {
                        Logger.Log.message("New Reservation found:");
                        parser.getReservations()[id].print();
                        RaplaConnectorTools.saveReservation(service, raplaFolderId, parser.getReservations()[id]);
                    }
                }
                Thread.Sleep(10000);
            }
            Logger.Log.message("Thread for user " + user + " terminated.");
        }

        public RaplaConnector(String user, String password)
        {
            this.user = user;
            this.password = password;
        }
    }
}
