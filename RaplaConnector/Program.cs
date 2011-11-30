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

            Parser parser = new Parser();
            parser.printAppointments();


            Console.WriteLine("Please enter your username:");
            String user = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            String password = ReadPassword();

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

        private static string ReadPassword()
        {
            Stack pass = new Stack();

            for (ConsoleKeyInfo consKeyInfo = Console.ReadKey(true);
              consKeyInfo.Key != ConsoleKey.Enter; consKeyInfo = Console.ReadKey(true))
            {
                if (consKeyInfo.Key == ConsoleKey.Backspace)
                {
                    try
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        pass.Pop();
                    }
                    catch (InvalidOperationException)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else
                {
                    Console.Write("*");
                    pass.Push(consKeyInfo.KeyChar.ToString());
                }
            }

            string[] password = Transform(pass.ToArray());
            Array.Reverse(password);
            return string.Join(string.Empty, password);
        }

        private static string[] Transform(object[] array)
        {
            string[] final = new string[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                final[i] = (string)array[i];
            }
            return final;
        }
    }
}
