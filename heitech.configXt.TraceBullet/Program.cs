using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.TraceBullet
{
    class Program
    {
        static void Main(string[] args)
        {
            var memory = new InMemoryStore();
            Result Aresult = CreateInMemory("AdminName", memory).Result;

            System.Console.WriteLine("Result from call is null: " + Aresult != null);

            var result = memory.GetEntityByNameAsync<ConfigEntity>("ConnectionString").Result;
            System.Console.WriteLine("in memory count: " + memory._store.Count);
            System.Console.WriteLine("ConfigEntityResult Name: " + result?.Name);
            System.Console.WriteLine("ConfigEntityResult Value: " + result?.Value);
            Console.ReadLine();
        }

        private static async Task<Result> CreateInMemory(string adminName, InMemoryStore store)
        {
             var changeRequest = new ConfigChangeRequest
            {
                Name = "ConnectionString",
                Value = "/Users/timoheiten/my-db.db",
                Claims = new Dictionary<string, string>()
                {
                    ["read"] = "ConfigClaim.CAN_READ",
                    ["update"] = "ConfigClaim.CAN_UPDATE"
                }
            };
            var context = new CommandContext(adminName, CommandTypes.Create, changeRequest, store);

            IRunConfigOperation operation = Factory.CreateOperation(context);
            Result result = await operation.ExecuteAsync();
            
            return result;
        }

        private class InMemoryStore : IStorageModel
        {
            public readonly List<AdministratorEntity> _admins = new List<AdministratorEntity>
            {
                new AdministratorEntity 
                { 
                    Name = "AdminName",
                    Claims = new List<ConfigClaim>
                    {
                        new ConfigClaim 
                        {
                            Name =  ConfigClaim.CAN_CREATE,
                            Value = ConfigClaim.CAN_CREATE
                        },
                        new ConfigClaim 
                        {
                            Name = "ConfigClaim.CAN_READ",
                            Value = "ConfigClaim.CAN_READ"
                        },
                    }
                }
            };
            public readonly List<ConfigEntity> _store = new List<ConfigEntity>();

            public Task<IEnumerable<T>> AllEntitesAsync<T>() 
                where T : StorageEntity
            {
                IEnumerable<ConfigEntity> asEnumerable = _store;
                return Task.FromResult(asEnumerable.Cast<T>());
            }

            public Task<T> GetEntityByNameAsync<T>(string byName) where T : StorageEntity
            {
                if (typeof(T) == typeof(AdministratorEntity))
                {
                    StorageEntity ergebnis = _admins.FirstOrDefault(x => x.Name == byName);
                    return Task.FromResult((T)ergebnis);
                }

                StorageEntity config = _store.FirstOrDefault(x => x.Name == byName);
                return Task.FromResult<T>((T)config);
            }

            public Task<bool> StoreEntityAsync<T>(T entity) where T : StorageEntity
            {
                if (entity.Id == Guid.Empty || _store.Any(x => x.Id == entity.Id))
                    return Task.FromResult(false);

                bool stored = false;
                if (typeof(T) == typeof(AdministratorEntity))
                {
                    _admins.Add(entity as AdministratorEntity);
                    stored = true;
                }
                else
                {
                    System.Console.WriteLine("storing config entity");
                    _store.Add(entity as ConfigEntity);
                    System.Console.WriteLine(_store.Count);
                    stored = true;
                }
                
                return Task.FromResult(stored);
            }
        }
    }
}
