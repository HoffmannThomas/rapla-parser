using System;
using ConfigurationManager;
using RaplaParser;

namespace RaplaConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            string selection = "";

            Parser parser = new Parser(ConfigManager.getConfigString("rapla_data_path"));
            parser.printAppointments();

            Console.WriteLine("Please enter your username:");
            String user = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            String password = PasswordReader.ReadPassword();

            try
            {
                ExchangeConnector.EWSConnector ewsConnector = new ExchangeConnector.EWSConnector(ConfigManager.getConfigString("fqdn"), user, password);

                // some testing variables
                String subject = "TestSubject";
                String body = "TestBody";
                String recipient = "TestRecipient";
                String location = "TestLocation";
                DateTime start = new DateTime(2011, 12, 12, 12,12,12);
                DateTime end = start.AddHours(2);

                // test calendar
                ExchangeConnector.AccountObjectFactory.CreateCalendarObject(ewsConnector, subject, body,start, end, location);

                // test email
                ExchangeConnector.AccountObjectFactory.SendEmail(ewsConnector, subject, body, recipient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            while (selection.ToLower() != "q")
            {
                Console.WriteLine();
                Console.WriteLine("Enter 'Q' to exit.");
                Console.Write("> ");
                selection = Console.ReadKey().KeyChar.ToString();
            }
        }
    }
}
