using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;
using NetMQ;
using NetMQ.Sockets;
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
            using (var responseSocket = new ResponseSocket())
            {
                var config = Configuration.Parse();
                responseSocket.Bind(config.ZeroMQTcp);
                _interact = InteractFromConfig(config);
                while (true)
                {
                    Log("running loop");
                    UiOperationResult uiResult = await LoopAsync(responseSocket, config);
                    var json = JsonConvert.SerializeObject(uiResult);
                    Log("returning result - issuccess:" + uiResult.IsSuccess);
                    responseSocket.SendFrame(json);
                }
            }
        }

        private static async Task<UiOperationResult> LoopAsync(ResponseSocket socket, Configuration config)
        {
            try
            {
                // todo check if one can handle multiple concurrent requests on same socket? or do we need thread dispatching here
                var contextModel = socket.ReceiveFrameString();
                Log($"received: {contextModel}");

                ContextModel model = JsonConvert.DeserializeObject<ContextModel>(contextModel);
                Log($"model is null {model == null}");
                if (model.User == null)
                {
                    throw new InvalidOperationException($"input: {contextModel} ain`t no ContextModel");
                }

                OperationResult result = await _interact.Run(model);

                return FromOperationResult(result);
            }
            catch (System.Exception ex)
            {
                var oR = OperationResult.Failure
                (
                    ResultType.BadRequest,
                    $"could not parse contextModel: {ex.Message}"
                );
                return FromOperationResult(oR);
            }
        }

        private static UiOperationResult FromOperationResult(OperationResult result)
        {
            ConfigEntity entity = result.Result;
            var models = new List<ConfigurationModel>();
            if (entity != null)
            {
                if (entity is ConfigCollection collection)
                {
                    models = collection.WrappedConfigEntities
                                                    .Select(x => new ConfigurationModel { Name = x.Name, Value = x.Value })
                                                    .ToList();
                }
                else
                {
                    models.Add
                    (
                        new ConfigurationModel 
                        { 
                            Name = entity.Name, 
                            Value = entity.Value 
                        }
                    );
                }
            }
            return new UiOperationResult
            (
                success: result.IsSuccess,
                resultType: result.ResultType.ToString(),
                errorMessage: result.ErrorMessage,
                model: models
            );
        }

        private static IInteract InteractFromConfig(Configuration config)
        {
            return new MemoryInteract(StorageFromConfig(config));
        }

        private static IStorageModel StorageFromConfig(Configuration configuration)
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
