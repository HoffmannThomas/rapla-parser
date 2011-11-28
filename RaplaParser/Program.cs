using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace RaplaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string selection = "";

            while (selection.ToLower() != "q")
            {
                Parser parser = new Parser();
                parser.printAppointments();

                Console.WriteLine();
                Console.WriteLine("Enter 'Q' to exit or press enter to continue.");
                Console.Write("> ");

                selection = Console.ReadLine();
            }
        }
    }
}