using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ExchangeConnector
{
    public class EWSConnector
    {
        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

        String fqdn;

        public EWSConnector(String fqdn, String user, String password)
        {
            this.fqdn = fqdn;

            ServicePointManager.ServerCertificateValidationCallback =
                delegate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                    return true;
                };

            Console.WriteLine("Connecting...");
            service.Credentials = new WebCredentials(user + "@" + fqdn, password);

            service.AutodiscoverUrl(user + "@" + fqdn);

            Console.WriteLine("Connection established");


        }

        public void SendEmail(String subject, String body, String recipient)
        {
            Console.WriteLine("Sending...");

            // Create the e-mail message, set its properties, and send it to user2@contoso.com, saving a copy to the Sent Items folder. 
            EmailMessage message = new EmailMessage(service);
            message.Subject = subject;
            message.Body = body;
            message.ToRecipients.Add(recipient + "@" + fqdn);
            message.SendAndSaveCopy();

            // Write confirmation message to console window.
            Console.WriteLine("Message sent!");
        }
    }
}
