using System;
using ConfigurationManager;

namespace Logger
{
    public static class Log
    {
        public static void message(String msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(ConfigManager.getConfigString("log_file"));
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
