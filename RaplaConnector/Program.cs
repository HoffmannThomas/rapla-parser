using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExchangeConnector;
using ConfigurationManager;
using RaplaParser;
using System.Collections;

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

                Console.WriteLine("Please enter subject, body and recipient:");
                String subject = Console.ReadLine();
                String body = Console.ReadLine();
                String recipient = Console.ReadLine();

                ewsConnector.SendEmail(subject, body, recipient);
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
