using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DankMemerEventCatcher
{
    public static class ConfigurationManager
    {
        public const string ConfigurationFile = "Configuration.json";

        public static Configuration GetConfiguration()
        {
            if (!File.Exists(ConfigurationFile))
            {
                SetDefaults();
            }

            using (var fileStream = File.OpenRead(ConfigurationFile))
            using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
            {
                return JsonConvert.DeserializeObject<Configuration>(streamReader.ReadToEnd());
            }
        }

        private static void SetDefaults()
        {
            File.WriteAllText(ConfigurationFile, JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));

            Console.WriteLine("First, configure the configuration file.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}