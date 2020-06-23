using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Models;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace heitech.configXt.TraceBullet
{
    public class InteractCli
    {
        static Action<object> _log;
        static RequestSocket _socket;
        public static Task Run(Action<object> log)
        {
            _log = log;
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "config.json");
            var json =  File.ReadAllText(path);
            log(string.Format("json is :\n{0}", json));
            // without upload is error on get
            var readContext = new ContextModel
            {
                Value = "",
                Key = "RabbitMQ:Host",
                Type = ContextType.ReadEntry,
                User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!")
            };
            using (var socket = new RequestSocket())
            {
                socket.Connect("tcp://localhost:5557");
                _socket = socket;
                // todo prio2 check if socket is available for easier testing
                // send
                RequestReceiveLog(readContext);
                // send wrong context 
                RequestReceiveLog(new FailureObject());
                // upload
                RequestReceiveLog(new ContextModel() { User = readContext.User, Type = ContextType.UploadAFile, Key ="Json", Value = json});
                // create new value
                // get
                var result = RequestReceiveLog(readContext);

                // get all values including the new one
                // update the created value
                var updateContext = new ContextModel
                {
                    Value = "my-Host",
                    Key = "RabbitMQ:Host",
                    Type = ContextType.UpdateEntry,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!")
                };
                RequestReceiveLog(updateContext);
                result = RequestReceiveLog(readContext);

                // delete the created value
                var deleteContext = new ContextModel
                {
                    Value = "",
                    Key = "RabbitMQ:Host",
                    Type = ContextType.DeleteEntry,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!")
                };
                RequestReceiveLog(deleteContext);
 
                // get all values
                var allCtxt = new ContextModel
                {
                    Key = "",
                    Value = "",
                    Type = ContextType.ReadAllEntries,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!")
                };
                var uiResult = RequestReceiveLog(allCtxt);
                System.Console.WriteLine(string.Join("\n", uiResult.ConfigurationModels.Select(x => x.Name)));
            }
            log("-".PadRight(50, '-'));
            log("finished");
            return Task.CompletedTask;
        }

        public class FailureObject { }

        private static UiOperationResult RequestReceiveLog<T>(T model)
        {
            System.Console.WriteLine("send model - Type: [" + typeof(T).Name + "]");
            _socket.SendFrame(JsonConvert.SerializeObject(model));
            // receive
            string receive = _socket.ReceiveFrameString();
            var uiResult = JsonConvert.DeserializeObject<UiOperationResult>(receive);
            _log($"uiResult: [{uiResult.IsSuccess}] - ResultName: [{uiResult.ResultName}] - Amount: [{uiResult.ConfigurationModels?.Count}]\nError:[{uiResult.ErrorMessage}]");

            System.Console.WriteLine("-".PadRight(50,'-'));
            return uiResult;
        }
    }
}