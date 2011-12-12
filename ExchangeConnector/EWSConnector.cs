using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeConnector
{
    public class EWSConnector
    {
        ExchangeService service;
        String fqdn;

        public EWSConnector(String fqdn, String user, String password)
        {

            this.service = new ExchangeService();
            this.fqdn = fqdn;

            // allow all certificates (unsecure, may need improvement)
            ServicePointManager.ServerCertificateValidationCallback =
                delegate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                    return true;
                };

            Logger.Log.message("Connecting...");

            service.Credentials = new WebCredentials(user, password);
            service.AutodiscoverUrl(user + "@" + fqdn);

            Logger.Log.message("Connection established");
        }

        public ExchangeService getEWSService()
        {
            return this.service;
        }

        public String getFQDN()
        {
            return this.fqdn;
        }
    }
}