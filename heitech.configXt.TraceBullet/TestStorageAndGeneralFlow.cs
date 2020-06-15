using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.TraceBullet
{
    public class TestStorageAndGeneralFlow
    {
        static Action<object> printCallback;
        public static async Task Run(Action<object> _printCallback)
        {
            printCallback = _printCallback;
            var memory = new InMemoryStore();
            OperationResult Aresult = await CreateInMemory(memory);

            printCallback(ToStringOpResult(Aresult));
            var exists =await  ReadConfigEntityInMemory("ConnectionString", memory);
            printCallback(ToStringOpResult(exists));

            StepIn("now update --> press any key");
            Aresult = await UpdateValueInMemory(memory);
            printCallback(ToStringOpResult(Aresult));

            StepIn("Read works--> press any key");
            var read1 = await ReadConfigEntityInMemory("ConnectionString", memory);
            printCallback(ToStringOpResult(read1));

            StepIn("now query fail in try catch! --> press any key");
            try
            {
                OperationResult read = await ReadConfigEntityInMemory("Not found", memory);
                printCallback(ToStringOpResult(read));
            }
            catch (System.Exception ex)
            {
                printCallback("failed with:\n" + ex);
            }

            StepIn("Create another one and query all");
            var r2 = CreateInMemory(memory).Result;
            printCallback(ToStringOpResult(r2));

            var readAll = await ReadAllAsync(memory);
            printCallback(ToStringOpResult(readAll));

            StepIn("now Delete ConnectionString Value and catch exception! --> press any key");
            Aresult = await DeleteAndQueryAfter(memory);
            printCallback(ToStringOpResult(Aresult));
        }

        private static string ToStringOpResult(OperationResult r)
        {
            if (r == null)
                return "Result is null!";
            return $"ResultType: [{r.ResultType}] - ConfigEntity is Null [{r.Result == null}] - Error: [{r.ErrorMessage} + {r.Error}]";
        }

        private static void StepIn(string nextText)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            printCallback(nextText);
            Console.ForegroundColor = temp;
            Console.ReadKey();
        }

        private static async Task<OperationResult> ReadConfigEntityInMemory(string queryName, InMemoryStore store)
        {
            var context = new QueryContext(queryName, QueryTypes.ValueRequest, store);
            var result = await Factory.RunOperationAsync(context);

            printCallback("in memory count: " + store._store.Count);
            printCallback("ConfigEntityResult Name: " + result.Result?.Name);
            printCallback("ConfigEntityResult Value: " + result.Result?.Value);

            return result;
        }

        private static async Task<OperationResult> ReadAllAsync(InMemoryStore store)
        {
            var context = new QueryContext(QueryContext.QUERY_ALL, QueryTypes.AllValues, store);
            OperationResult result = await Factory.RunOperationAsync(context);
            
            printCallback("in memory count: " + store._store.Count);
            var coll = result.Result as ConfigCollection;
            printCallback("ConfigEntityResult items from all: " + coll.WrappedConfigEntities.Count());

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
                printCallback("now after delete it was not found!");
            }
            else
            {
                printCallback("could still find --> error in test");
            }
            return result;
        }
    }
}