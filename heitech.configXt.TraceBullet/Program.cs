using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.TraceBullet
{
    class Program
    {

        private static string ToStringOpResult(OperationResult r)
        {
            if (r == null)
                return "Result is null!";
            return $"ResultType: [{r.ResultType}] - ConfigEntity is Null [{r.Result == null}] - Error: [{r.ErrorMessage} + {r.Error}]";
        }
        static void Main(string[] args)
        {
            var memory = new InMemoryStore();
            OperationResult Aresult = CreateInMemory(memory).Result;

            System.Console.WriteLine(ToStringOpResult(Aresult));
            var exists = ReadConfigEntityInMemory("ConnectionString", memory).Result;
            System.Console.WriteLine(ToStringOpResult(exists));

            StepIn("now update --> press any key");
            Aresult = UpdateValueInMemory(memory).Result;
            System.Console.WriteLine(ToStringOpResult(Aresult));

            StepIn("Read works--> press any key");
            var read1 = ReadConfigEntityInMemory("ConnectionString", memory).Result;
            System.Console.WriteLine(ToStringOpResult(read1));

            StepIn("now query fail in try catch! --> press any key");
            try
            {
                OperationResult read = ReadConfigEntityInMemory("Not found", memory).Result;
                System.Console.WriteLine(ToStringOpResult(read));
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("failed with:\n" + ex);
            }

            StepIn("Create another one and query all");
            var r2 = CreateInMemory(memory).Result;
            System.Console.WriteLine(ToStringOpResult(r2));

            var readAll = ReadAllAsync(memory).Result;
            System.Console.WriteLine(ToStringOpResult(readAll));

            StepIn("now Delete ConnectionString Value and catch exception! --> press any key");
            Aresult = DeleteAndQueryAfter(memory).Result;
            System.Console.WriteLine(ToStringOpResult(Aresult));
        }

        private static void StepIn(string nextText)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(nextText);
            Console.ForegroundColor = temp;
            Console.ReadKey();
        }

        private static async Task<OperationResult> ReadConfigEntityInMemory(string queryName, InMemoryStore store)
        {
            var context = new QueryContext(queryName, QueryTypes.ValueRequest, store);
            var result = await Factory.RunOperationAsync(context);

            System.Console.WriteLine("in memory count: " + store._store.Count);
            System.Console.WriteLine("ConfigEntityResult Name: " + result.Result?.Name);
            System.Console.WriteLine("ConfigEntityResult Value: " + result.Result?.Value);

            return result;
        }

        private static async Task<OperationResult> ReadAllAsync(InMemoryStore store)
        {
            var context = new QueryContext(QueryContext.QUERY_ALL, QueryTypes.AllValues, store);
            OperationResult result = await Factory.RunOperationAsync(context);
            
            System.Console.WriteLine("in memory count: " + store._store.Count);
            var coll = result.Result as ConfigCollection;
            System.Console.WriteLine("ConfigEntityResult items from all: " + coll.WrappedConfigEntities.Count());

            return result;
        }

        private static async Task<OperationResult> UpdateValueInMemory(InMemoryStore store)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "/Users/timoheiten/my-db-2.db"
            };

            var context = new CommandContext(CommandTypes.UpdateValue, changeRequest, store);

            var result = await Factory.RunOperationAsync(context);
            return result;
        }

        private static async Task<OperationResult> CreateInMemory(InMemoryStore store, string other=null)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = other == null ? "ConnectionString" : other,
                Value = "/Users/timoheiten/my-db.db"
            };
            var context = new CommandContext(CommandTypes.Create, changeRequest, store);

            OperationResult result = await Factory.RunOperationAsync(context);
            return result;
        }

        private static async Task<OperationResult> DeleteAndQueryAfter(InMemoryStore store)
        {
            var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "null"
            };
            var context = new CommandContext(CommandTypes.Delete, changeRequest, store);

            OperationResult result  = await Factory.RunOperationAsync(context);

            var queryResult = await ReadConfigEntityInMemory("ConnectionString", store);
            if (queryResult.IsSuccess == false)
            {
                System.Console.WriteLine("now after delete it was not found!");
            }
            else
            {
                System.Console.WriteLine("could still find --> error in test");
            }
            return result;
        }
    }
}
