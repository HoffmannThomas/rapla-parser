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
        static Dictionary<String, RaplaConnector> connectors = new Dictionary<String, RaplaConnector>();
        static UserManager userManager = new UserManager();

        public RaplaService()
        {
            foreach (User user in userManager.dictionary.Values)
            {
                if (connectors.ContainsKey(user.rapla_user_id) == false)
                {
                    connectors.Add(user.rapla_user_id, new RaplaConnector(user.getExchangeUserName(), user.getPassword()));
                }
            }
        }

        [WebMethod(Description = "")]
        public String showConnections()
        {
            String connections = "";

            foreach (String user in connectors.Keys)
            {
                connections += user + "\r\n";
            }

            return connections;
        }

        [WebMethod(Description = "")]
        public String SaveAppointment(String rapla_user_id, XmlDocument raplaXML)
        {
            try
            {
                connectors[rapla_user_id].saveReservation(raplaXML);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public String DeleteAppointment(String rapla_user_id, String exchange_id)
        {
            try
            {
                connectors[rapla_user_id].deleteReservation(exchange_id);
                return "Data computed";
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(Description = "")]
        public int registerUser(String rapla_user_id, String exchange_user_name, String password)
        {
            User user = new User(rapla_user_id, exchange_user_name, password, true);

            connectors.Add(user.rapla_user_id, new RaplaConnector(user.getExchangeUserName(), user.getPassword()));
            userManager.addUser(user);

            return 0;
        }
        [WebMethod(Description = "")]
        public int unregisterUser(String rapla_user_id)
        {
            userManager.removeUser(rapla_user_id);
            connectors.Remove(rapla_user_id);
            return 0;
        }

        [WebMethod(Description = "")]
        public XmlDocument getLastModified(String rapla_user_id, DateTime last_modified)
        {
            return connectors[rapla_user_id].getLastModified(last_modified);
        }
    }
}