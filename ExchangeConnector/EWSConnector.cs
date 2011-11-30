using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeConnector
{
    public class EWSConnector
    {
        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

        public EWSConnector()
        {
            Console.WriteLine("Connecting...");
            service.Credentials = new WebCredentials("thomas@win2k3.21sheeps.com", "Start123 ");
            service.AutodiscoverUrl("thomas@win2k3.21sheeps.com");
            Console.WriteLine("Connection established");
        }

        public void exampleEmailRequest()
        {
            // Validate the server certificate.
            // For a certificate validation code example, see the Validating X509 Certificates topic in the Core Tasks section.

            try
            {
                // Create the e-mail message, set its properties, and send it to user2@contoso.com, saving a copy to the Sent Items folder. 
                EmailMessage message = new EmailMessage(service);
                message.Subject = "Interesting";
                message.Body = "The proposition has been considered.";
                message.ToRecipients.Add("user2@contoso.com");
                message.SendAndSaveCopy();

                // Write confirmation message to console window.
                Console.WriteLine("Message sent!");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
