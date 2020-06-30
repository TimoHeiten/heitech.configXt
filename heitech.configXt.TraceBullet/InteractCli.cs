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
        public static Task Run(Action<object> log)
        {
            var comm = SetupComm.GetCommunication("tcp://localhost:5557");
            // without upload is error on get
            var readContext = new ContextModel
            {
                Value = "",
                Key = "RabbitMQ:Host",
                Type = ContextType.ReadEntry,
                User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!"),
                AppName = "test-app-1"
            };
            using (comm.Socket)
            {
                // todo prio2 check if socket is available for easier testing
                // send
                RequestReceiveLog(readContext, comm.Socket, log);
                // send wrong context 
                RequestReceiveLog(new FailureObject(), comm.Socket, log);
                // upload
                RequestReceiveLog
                (
                    new ContextModel() 
                    {
                         User = readContext.User,  
                         Type = ContextType.UploadAFile, 
                         Key ="Json", 
                         Value = comm.JsonString,
                         AppName = "test-app-1"
                    }, 
                    comm.Socket, 
                    log
                );
                // create new value
                // get
                var result = RequestReceiveLog(readContext, comm.Socket, log);

                // get all values including the new one
                // update the created value
                var updateContext = new ContextModel
                {
                    Value = "my-Host",
                    Key = "RabbitMQ:Host",
                    Type = ContextType.UpdateEntry,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!"),
                    AppName = "test-app-1"
                };
                RequestReceiveLog(updateContext, comm.Socket, log);
                result = RequestReceiveLog(readContext, comm.Socket, log);

                // auth not allowed
                readContext.AppName = "app-test-2";
                RequestReceiveLog(readContext, comm.Socket, log);

                // delete the created value
                var deleteContext = new ContextModel
                {
                    Value = "",
                    Key = "RabbitMQ:Host",
                    Type = ContextType.DeleteEntry,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!"),
                    AppName = "test-app-1"
                };
                RequestReceiveLog(deleteContext, comm.Socket, log);
 
                // get all values
                var allCtxt = new ContextModel
                {
                    Key = "",
                    Value = "",
                    Type = ContextType.ReadAllEntries,
                    User = new AuthModel("timoheiten@t-heiten.net", "configXt-Admin!"),
                    AppName = "test-app-1"
                };
                var uiResult = RequestReceiveLog(allCtxt, comm.Socket, log);
                System.Console.WriteLine(string.Join("\n", uiResult.ConfigurationModels.Select(x => x.Name)));

            }
            log("-".PadRight(50, '-'));
            log("finished");
            return Task.CompletedTask;
        }

        public class FailureObject { }

        public static UiOperationResult RequestReceiveLog<T>(T model, RequestSocket _socket, Action<object> _log)
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