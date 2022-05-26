using System;
using System.IO;
using System.Threading.Tasks;
using heitech.configXt;
using heitech.configXt.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ///<summary>
            /// Test all CRUD Operations and the correct ConfigResults
            ///</summary>
            var services = new ServiceCollection();
            services.AddServices("testdb");
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IService>();
            var store = scope.ServiceProvider.GetRequiredService<IStore>();
            if (store is SqliteStore sqlite)
                await sqlite.Database.MigrateAsync();

            try 
            {
                // create literal, complex object and collection
                var one = await service.CreateAsync(ConfigModel.From("key", 42));
                var two = await service.CreateAsync(ConfigModel.From("key-2", new[] { 42, 43, 44, }));
                var three = await service.CreateAsync(ConfigModel.From("key-3", new Item { _42 = 42, Value = "abc affee schnee" }));
                System.Console.WriteLine("created all " + string.Join(" - ", new [] { one.Result?.Key, two.Result?.Key, three.Result?.Key }));

                // read all
                async Task get<T>(string key)
                {
                    var result = await service.RetrieveAsync(key);
                    bool isType = result.TryGetAs<T>(out T value); 
                    System.Console.WriteLine($"{result.IsSuccess} - {isType} - {value}");
                }

                await get<int>("key");
                await get<int[]>("key-2");
                await get<Item>("key-3");

                // update all
                one = await service.UpdateAsync(ConfigModel.From("key", 84));
                two = await service.UpdateAsync(ConfigModel.From("key-2", new[] { 100 }));
                three = await service.UpdateAsync(ConfigModel.From("key-3", new Item { _42 = 21, Value = "not affee schnee" }));
                System.Console.WriteLine("updated all " + string.Join(" - ", new [] { one.Result?.Key, two.Result?.Key, three.Result?.Key }));

                await get<int>("key");
                await get<int[]>("key-2");
                await get<Item>("key-3");

                // delete all
                one = await service.DeleteAsync("key");
                two = await service.DeleteAsync("key-2");
                three = await service.DeleteAsync("key-3");
                System.Console.WriteLine("deleted all " + string.Join(" - ", new [] { one.Result?.Key, two.Result?.Key, three.Result?.Key }));

                // try read all after delete is not found
                await get<int>("key");
                await get<int[]>("key-2");
                await get<Item>("key-3");

                // create same twice is idempotent
                var createdResult = await service.CreateAsync(ConfigModel.From("twice", new { O = "O" }));
                createdResult = await service.CreateAsync(ConfigModel.From("twice", new { O = "O" }));
                System.Console.WriteLine($"idempotent created {createdResult.IsSuccess} - {createdResult?.Result?.Key}");

                // update, read, delete non existing
                void writeNonExisting(ConfigResult opResult, string op)
                {
                    System.Console.WriteLine($"{op} - {opResult?.IsSuccess} - {opResult?.Exception?.ToString()}");
                }
                var nonExisting = await service.UpdateAsync(ConfigModel.From("none", 42));
                writeNonExisting(nonExisting, "update");
                nonExisting = await service.RetrieveAsync("none");
                writeNonExisting(nonExisting, "get");
                nonExisting = await service.DeleteAsync("none");
                writeNonExisting(nonExisting, "delete");
            }
            finally
            {
                if (store is SqliteStore sqliteStore)
                    await sqliteStore.Database.EnsureDeletedAsync();
            }
        }

        public class Item
        {
            public int _42 { get; set; }
            public string Value { get; set; }
        }
    }
}
