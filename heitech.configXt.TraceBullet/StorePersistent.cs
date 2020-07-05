using System;
using System.Diagnostics;
using System.Threading.Tasks;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Models;

namespace heitech.configXt.TraceBullet
{
    public class StorePersistent
    {
        public static async Task Run(Action<object> log)
        {
            // run migrate async
            // use efstore
            // run different tests  by using in memory service
            var persistent = new EfStore("Data Source=./config.db");
            var memoryInteract = new MemoryInteract(persistent, persistent);

            var authModel = new AuthModel { Name = "timoh", Password = "admin123" };
            var appClaims = new AppClaimModel[]
            { 
                new AppClaimModel() 
                { 
                    ApplicationName = "test-app-1", 
                    ConfigEntitiyKey = "ConnectionString", 
                    CanRead = true, 
                    CanWrite = true
                }
            };
            // add user
            var createUser = persistent.CreateUserContext(authModel, persistent, appClaims);
            OperationResult result = await memoryInteract.Run(new ContextModel
            {
                Type = ContextType.AddUser,
                AppClaims = appClaims,
                User = authModel,
                Value = "",
                Key = "",
            });

            Debug.Assert(result.IsSuccess);

            // create
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "/Users/timoheiten/my-db.db"
            };
            var context = new CommandContext(CommandTypes.Create, changeRequest, persistent);
            result = await Factory.RunOperationAsync(context);
            Debug.Assert(result.IsSuccess);
            // read
            result = await Factory.RunOperationAsync(persistent.QueryOne(changeRequest.Name));
            Debug.Assert(result.IsSuccess);
            Debug.Assert(result.Result.Value == changeRequest.Value);

            // delete 
            result = await Factory.RunOperationAsync(persistent.DeleteContext(changeRequest.Name));
            Debug.Assert(result.IsSuccess);

            // read after delete
            result = await Factory.RunOperationAsync(persistent.QueryOne(changeRequest.Name));
            Debug.Assert(result.IsSuccess == false);
            Debug.Assert(result.ResultType  == ResultType.NotFound);

            string padding = "-".PadRight(50, '-');
            System.Console.WriteLine(padding);
            System.Console.WriteLine($"if no assertion error occured, everything is working fine with EF store and Sqlite");
            System.Console.WriteLine(padding);
        }
    }
}