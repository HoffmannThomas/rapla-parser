using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ExchangeConnector;

namespace RaplaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string selection = "";

            Parser parser = new Parser();
            parser.printAppointments();

            ExchangeConnector.EWSConnector connector = new ExchangeConnector.EWSConnector();

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