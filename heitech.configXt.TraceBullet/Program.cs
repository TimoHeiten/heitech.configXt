using System;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.TraceBullet
{
    class Program
    {
        static void Main(string[] args)
        {
            var memory = new InMemoryStore();
            Result Aresult = CreateInMemory("AdminName", memory).Result;

            System.Console.WriteLine("Result from call is null: " + Aresult == null);
            ReadConfigEntityInMemory("ConnectionString", memory).Wait();

            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("now update --> press any key");
            Console.ForegroundColor = temp;
            Console.ReadKey();

            Aresult = UpdateValueInMemory("AdminName", memory).Result;
            System.Console.WriteLine("Result from call is null: " + Aresult == null);
            ReadConfigEntityInMemory("ConnectionString", memory).Wait();

            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("now query fail in try catch! --> press any key");
            Console.ForegroundColor = temp;
            Console.ReadKey();

            try
            {
                ReadConfigEntityInMemory("Not found", memory).Wait();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("failed with:\n" + ex);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("now Delete ConnectionString Value and catch exception! --> press any key");
            Console.ForegroundColor = temp;
            Console.ReadKey();
            Aresult = DeleteAndQueryAfter("AdminName", memory).Result;
            System.Console.WriteLine("Result from call is null: " + Aresult == null);
        }

        private static async Task ReadConfigEntityInMemory(string queryName, InMemoryStore store)
        {
            var context = new QueryContext("AdminName", queryName, QueryTypes.ValueRequest, store);
            IRunConfigOperation operation = Factory.CreateOperation(context);

            var result = await operation.ExecuteAsync();
            
            System.Console.WriteLine("in memory count: " + store._store.Count);
            System.Console.WriteLine("ConfigEntityResult Name: " + result?.Current?.Name);
            System.Console.WriteLine("ConfigEntityResult Value: " + result?.Current?.Value);
        }

        private static async Task<Result> UpdateValueInMemory(string adminName, InMemoryStore store)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "/Users/timoheiten/my-db-2.db"
            };

            var context = new CommandContext(adminName, CommandTypes.UpdateValue, changeRequest, store);

            IRunConfigOperation operation = Factory.CreateOperation(context);
            Result result = await operation.ExecuteAsync();
            
            return result;
        }

        private static async Task<Result> CreateInMemory(string adminName, InMemoryStore store)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "/Users/timoheiten/my-db.db"
            };
            var context = new CommandContext(adminName, CommandTypes.Create, changeRequest, store);

            IRunConfigOperation operation = Factory.CreateOperation(context);
            Result result = await operation.ExecuteAsync();
            
            return result;
        }

        private static async Task<Result> DeleteAndQueryAfter(string adminName, InMemoryStore store)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = null
            };
            var context = new CommandContext(adminName, CommandTypes.Delete, changeRequest, store);

            IRunConfigOperation operation = Factory.CreateOperation(context);
            Result result = await operation.ExecuteAsync();

            try
            {
                await ReadConfigEntityInMemory("ConnectionString", store);
            } 
            catch
            {
                System.Console.WriteLine("now after delete it was not found!");
            }

            return result;
        }
    }
}
