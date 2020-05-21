using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace EventCatcherSelfbot
{
    public static class ConfigurationManager
    {
        public const string ConfigurationFile = "Configuration.json";

        public static Configuration GetConfiguration()
        {
            SetDefaults();

            using (var fileStream = File.OpenRead(ConfigurationFile))
            using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
            {
                return JsonConvert.DeserializeObject<Configuration>(streamReader.ReadToEnd());
            }
        }

        private static void SetDefaults()
        {
            if (File.Exists(ConfigurationFile))
            {
                return;
            }

            File.WriteAllText(ConfigurationFile, JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));

            Console.WriteLine("First, configure the configuration file.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}