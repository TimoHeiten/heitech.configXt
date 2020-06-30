using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Models;
using Newtonsoft.Json;

namespace heitech.configXt.Cli
{
    class Program
    {
        static bool _log;
        static IInteract _interact;
        static async Task Main(string[] args)
        {
            // yes the irony of a config file inside said config service
            // is apparent to the author...
            // maybe use in memory config service here.
            System.Console.WriteLine("Starting to connect");
            _log = args.Contains("log") || args.Contains("l");
            // todo run in memory config for this service too
            var config = Configuration.Parse();
            var bus = new ResponseBus(config.ZeroMQTcp);
            using (bus)
            {
                bus.Connect();
                _interact = InteractFromConfig(config);
                while (true)
                {
                    Log("running loop");
                    await bus.RespondAsync(async (model) => await _interact.Run(model));
                }
            }
        }

        private static IInteract InteractFromConfig(Configuration config)
        {
            InMemoryStore store = StorageFromConfig(config);
            return new MemoryInteract(store, store);
        }
        private static InMemoryStore StorageFromConfig(Configuration configuration)
        {
            return new InMemoryStore
            (
                configuration.AdminName, 
                configuration.AdminPassword, 
                configuration.InitialAppName)
            ;
        }

        private static void Log(object o)
        {
            if (_log)
                System.Console.WriteLine(o);
        }

        private ContextModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<ContextModel>(json);
        }
    }
}
