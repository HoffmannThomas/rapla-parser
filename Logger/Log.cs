using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public static class Log
    {
        public static void message(String msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText("log.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
