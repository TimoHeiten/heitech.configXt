using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Models;

namespace heitech.configXt.TraceBullet
{
    public static class TestUsers
    {
        ///<summary>
        /// Add a User, update a user, get a user, remove a user 
        /// all with the cli interaction 
        ///</summary>
        public static async Task Run(Action<object> printCallback)
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

            string[] expected = Enumerable.Repeat("Success", 3).Append("No Success").ToArray();
            System.Console.WriteLine("-".PadRight(50, '-'));
            System.Console.WriteLine("expected:\n" + string.Join("\n", expected));
            System.Console.WriteLine("-".PadRight(50, '-'));

            using (comm.Socket)
            {
                UiOperationResult uiResult = await InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("AddUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.GetUser;
                uiResult = await InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("GetUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.UpdateUser;
                var list = model.AppClaims.ToList();
                list.Add(new AppClaimModel { ApplicationName = "App-2", ConfigEntitiyKey = "Simple", CanRead = true, CanWrite = true });
                model.AppClaims = list.ToArray();
                uiResult = await InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("UpdateUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.DeleteUser;
                uiResult = await InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("DeleteUser was: " + (uiResult.IsSuccess ? "Success" : "No Success"));

                model.Type = ContextType.GetUser;
                uiResult = await InteractCli.RequestReceiveLog(model, comm.Socket, log);
                System.Console.WriteLine("GetUser after Delete was: " + (uiResult.IsSuccess ? "Success" : "No Success"));
            }
        }
    }
}