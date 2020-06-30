using System;
using System.Linq;
using System.Threading.Tasks;
using NetMQ;
using heitech.configXt.Models;
using System.Text;

namespace heitech.configXt.TraceBullet
{
    public static class TestUsers
    {
        ///<summary>
        /// Add a User, update a user, get a user, remove a user 
        /// all with the cli interaction 
        ///</summary>
        public static Task Run(Action<object> printCallback)
        {
            var log = printCallback;
            var comm = SetupComm.GetCommunication("tcp://localhost:5557");

            var model = new ContextModel
            {
                User = new AuthModel("timoheiten@googlemail.com", "admin1234!"),
                AppClaims = new AppClaimModel[] 
                { 
                    new AppClaimModel 
                    { 
                        ApplicationName = "App-1",
                        ConfigEntitiyKey = "RabbitMQ",
                        CanRead = true,
                        CanWrite = true
                    } 
                },
                Key = null,
                Value = null,
                AppName = "App-1",
                Type = ContextType.AddUser
            };

            using (comm.Socket)
            {
                UiOperationResult uiResult = InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("AddUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.GetUser;
                uiResult = InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("GetUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.UpdateUser;
                var list = model.AppClaims.ToList();
                list.Add(new AppClaimModel { ApplicationName = "App-2", ConfigEntitiyKey = "Simple", CanRead = true, CanWrite = true });
                model.AppClaims = list.ToArray();
                uiResult = InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("UpdateUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.DeleteUser;
                uiResult = InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("DeleteUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.GetUser;
                uiResult = InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("GetUser after Delete was: " + (uiResult.IsSuccess ? "Success" : "No Success"));
            }

            return Task.CompletedTask;
        }
    }
}