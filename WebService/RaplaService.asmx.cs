using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Threading;
using Connector;
using System.Xml;
using ConfigurationManager;

namespace WebService
{
    /// <summary>
    /// Webservice for xml-parsing
    /// </summary>
    [WebService(Namespace = "http://win2k3.21sheeps.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class RaplaService : System.Web.Services.WebService
    {
        static Dictionary<String, RaplaConnector> connectorThreads = new Dictionary<string, RaplaConnector>();

        [WebMethod(Description = "")]
        public String showWorker()
        {
            String workers = "";

            foreach (String user in connectorThreads.Keys)
            {
                workers += user + "\r\n";
            }

            return workers;
        }

        [WebMethod(Description = "")]
        public String CreateAppointment(XmlDocument raplaXML, String user)
        {
            try
            {
                connectorThreads[user].saveReservation(raplaXML);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public String UpdateAppointment(XmlDocument raplaXML, String user)
        {
            try
            {
                connectorThreads[user].updateReservation(raplaXML);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public String DeleteAppointment(String id, String user)
        {
            try
            {
                connectorThreads[user].deleteReservation(id);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public String ParseRapla(String user)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigManager.getConfigString("rapla_data_path"));

                connectorThreads[user].saveReservation(doc);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }



        [WebMethod(Description = "")]
        public String stopWorker(String user)
        {
            try
            {
                connectorThreads[user].stopConnector();
                connectorThreads.Remove(user);
                return "Worker for user " + user + " stopped.";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public String startNewWorker(String user, String password)
        {
            try
            {
                RaplaConnector connector = new RaplaConnector(user, password);

                connectorThreads.Add(user, connector);

                Thread workerThread = new Thread(connector.startConnector);

                workerThread.Start();
                while (!workerThread.IsAlive) ;
                Thread.Sleep(1);

                return "Worker for user " + user + " created.";
            }
            catch (Exception e) { return e.Message; }
        }
    }
}