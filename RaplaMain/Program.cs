using System;
using System.Collections.Generic;
using System.Threading;
using Connector;

namespace Main
{
    class Program
    {
        static Dictionary<String, RaplaConnector> connectorThreads = new Dictionary<string, RaplaConnector>();

        static void Main()
        {
            while (true)
            {
                string[] mainMenuItems = { "1: Create new worker", "2: Show worker", "3: Exit" };

                switch (arrowMenu(mainMenuItems))
                {
                    case 0: startNewConnector(); break;
                    case 1: showWorker(); break;
                    case 2: System.Environment.Exit(-1); break;
                }
            }

            //connector.stopConnector();
            //workerThread.Join();
        }

        static int arrowMenu(string[] menuItems)
        {
            // A variable to keep track of the current Item, and a simple counter.
            short curItem = 0, c;

            // The object to read in a key
            ConsoleKeyInfo key;

            // Our array of Items for the menu (in order)
            do
            {
                // Clear the screen.  One could easily change the cursor position,
                // but that won't work out well with tabbing out menu items.
                Console.Clear();

                // Replace this with whatever you want.
                Console.WriteLine("Pick an option . . .");

                // The loop that goes through all of the menu items.
                for (c = 0; c < menuItems.Length; c++)
                {
                    // If the current item number is our variable c, tab out this option.
                    // You could easily change it to simply change the color of the text.
                    if (curItem == c)
                    {
                        Console.Write(">>");
                        Console.WriteLine(menuItems[c]);
                    }
                    // Just write the current option out if the current item is not our variable c.
                    else
                    {
                        Console.WriteLine(menuItems[c]);
                    }
                }

                // Waits until the user presses a key, and puts it into our object key.
                Console.Write("Select your choice with the arrow keys.");
                key = Console.ReadKey(true);

                // Simply put, if the key the user pressed is a "DownArrow", the current item will deacrease.
                // Likewise for "UpArrow", except in the opposite direction.
                // If curItem goes below 0 or above the maximum menu item, it just loops around to the other end.
                if (key.Key.ToString() == "DownArrow")
                {
                    curItem++;
                    if (curItem > menuItems.Length - 1) curItem = 0;
                }
                else if (key.Key.ToString() == "UpArrow")
                {
                    curItem--;
                    if (curItem < 0) curItem = Convert.ToInt16(menuItems.Length - 1);
                }
                // Loop around until the user presses the enter go.
            } while (key.KeyChar != 13);
            return curItem;
        }

        static void showWorker()
        {
            Console.Clear();

            foreach (String user in connectorThreads.Keys)
            {
                Console.WriteLine("Worker for user " + user + " running.");
            }

            Console.WriteLine("Press any key too continue...");
            Console.ReadLine();
        }

        static void startNewConnector()
        {
            Console.Clear();

            Console.WriteLine("Please enter your username:");
            String user = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            String password = PasswordReader.ReadPassword();

            Console.WriteLine();

            RaplaConnector connector = new RaplaConnector(user, password);

            connectorThreads.Add(user, connector);

            Thread workerThread = new Thread(connector.startConnector);

            workerThread.Start();
            while (!workerThread.IsAlive) ;
            Thread.Sleep(1);
        }
    }
}
