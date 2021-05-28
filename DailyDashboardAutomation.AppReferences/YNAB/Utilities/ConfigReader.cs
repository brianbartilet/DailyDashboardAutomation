using System;
using System.Configuration;
using System.Xml;

namespace AppReferences.Utilities
{
    /// <summary>
    /// ReadConfigFile is just going to read the config file 
    /// </summary>
    public class ReadConfigFile : ConfigurationElement
    {
        /// <summary>
        /// This is only going to read the config file and return the XmlDocument
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument ReadFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            // read the file
            doc.Load(@fileName);
            // return the XmlDocument
            return doc;
        }

        /// <summary>
        /// This method reads the value of a setting based on the key passed in as a parameter.
        /// </summary>
        /// <param name="key">string: The key of interested setting</param>
        /// <returns>string: The value of the key</returns>
        public static string GetSettingAsString(string key, string section=null)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// This method reads the value of a setting based on the key passed in as a parameter.
        /// </summary>
        /// <param name="key">string: The key of interested setting</param>
        /// <returns>int: The value of the key</returns>
        public static int GetSettingAsInt(string key, string section = null)
        {
            int setting;

            return int.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : -1;
        }


        /// <summary>
        /// This method reads the value of a setting based on the key passed in as a parameter.
        /// </summary>
        /// <param name="key">string: The key of interested setting</param>
        /// <returns>int: The value of the key</returns>
        public static int GetSetting(string key, string section = null)
        {
            int setting;

            return int.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : -1;
        }

        public static void AddValue(string key, string value)
        {
            // Open App.Config of executable
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Add an Application Setting.
            config.AppSettings.Settings.Add(key, value);
            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified, true);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
            

        }

    }
}
