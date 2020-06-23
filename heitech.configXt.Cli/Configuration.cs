using System;
using System.IO;
using Newtonsoft.Json;

namespace heitech.configXt.Cli
{
    public class Configuration
    {
        public const string TCP_MQ = "tcp://localhost:5557";
        public const string INTERACT_DEFAULT = "Memory";
        public const string STORAGE_DEFAULT = "Memory";

        public string AdminPassword { get; set; }
        public string AdminName { get; set; }
        public string InitialAppName { get; set; }

        private Configuration() { }
        public static Configuration Parse()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "config.json");
            try
            {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
            }
            catch (System.Exception)
            {
                
                return new Configuration
                {
                    ZeroMQTcp = TCP_MQ,
                    InteractType = INTERACT_DEFAULT,
                    StorageModel = STORAGE_DEFAULT
                };
            }
        }

        public string ZeroMQTcp { get; set; }

        public string InteractType { get; set; }
        public string StorageModel { get; set; }
    }
}