using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConfigurationManager
{
    public static class ConfigManager
    {

        private static AppSettingsReader config;

        static ConfigManager()
        {
            config = new AppSettingsReader();
        }

        public static String getConfigString(String key)
        {
            return (String)config.GetValue(key, typeof(String));
        }
    }
}
