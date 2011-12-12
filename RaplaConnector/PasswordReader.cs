using System;
using System.Collections;

namespace Connector
{
    public static class PasswordReader
    {
        public static string ReadPassword()
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
