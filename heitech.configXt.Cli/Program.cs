using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core;
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
            System.Console.WriteLine("Starting to connect");
            _log = args.Contains("log") || args.Contains("l");
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
            var (store, authStore) = StorageFromConfig(config);
            return new MemoryInteract(store, authStore);
        }

        private static (IStorageModel, IAuthStorageModel) StorageFromConfig(Configuration configuration)
        {
            if (configuration.StorageModel.Equals("persistent", StringComparison.InvariantCultureIgnoreCase))
            {
                Log("using ef store with sqlite and location:  " + configuration.StorageLocation);
                var persist = new EfStore(configuration.StorageLocation);
                return (persist, persist);
            }

            Log("using in memory store!");
            // default
            var store = new InMemoryStore
            (
                configuration.AdminName, 
                configuration.AdminPassword, 
                configuration.InitialAppName
            );

            return (store, store);
        }

        private static void Log(object o)
        {
            if (_log) System.Console.WriteLine(o);
        }

        private ContextModel Parse(string json) =>  JsonConvert.DeserializeObject<ContextModel>(json);
    }
}
