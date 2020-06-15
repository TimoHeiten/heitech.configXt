using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.TraceBullet
{
    public class UsingApplication
    {
        ///<summary>
        /// load simple json file
        /// save to inmemory store 
        /// and load all and print to console
        ///</summary>
        public static async Task Run(Action<object> printCallback)
        {
            var store = new InMemoryStore();
            // upload file from the current directory
            string json = JsonFromConfigFile();
            ITransform transform = new JsonTransform();

            OperationResult result = transform.Transform(json);
            if (result.Result is ConfigCollection collection)
            {
                foreach (var item in collection.WrappedConfigEntities)
                {
                    var request = new ConfigChangeRequest{ Name =item.Name, Value = item.Value };
                    var context = new CommandContext(CommandTypes.Create, request, store);

                    await Factory.RunOperationAsync(context);
                }
            }
            else
            {
                throw new InvalidCastException("should have been ConfigCollection yet is " + result.Result.GetType().Name);
            }
            foreach (var item in await store.AllEntitesAsync())
            {
                printCallback($"next one is [{item.Name}] - [{item.Value}]");
            }

            printCallback("also find RabbitMQ:Port and change it to 5674");
            ConfigEntity port = await QueryRabbitPortAsync(store);
            printCallback(port.Name + " - " + port.Value );

            var updateRq = new ConfigChangeRequest { Name = port.Name, Value = "5674" };
            var update = new CommandContext(CommandTypes.UpdateValue, updateRq, store);

            OperationResult rs = await Factory.RunOperationAsync(update);

            printCallback("after the update:");
            port = await QueryRabbitPortAsync(store);
            printCallback(port.Name + " - " + port.Value );
        }

        private static async Task<ConfigEntity> QueryRabbitPortAsync(IStorageModel store)
        {
            var find = new QueryContext("RabbitMQ:Port", QueryTypes.ValueRequest, store);
            OperationResult rabbitPort = await Factory.RunOperationAsync(find);
            return rabbitPort.Result;
        }

        private static string JsonFromConfigFile()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "config.json");

            return File.ReadAllText(path);
        }
    }
}