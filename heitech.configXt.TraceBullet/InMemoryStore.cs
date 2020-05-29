using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.TraceBullet
{
    public class InMemoryStore : IStorageModel
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
                        Name = ConfigClaim.CAN_READ,
                        Value = ConfigClaim.CAN_READ
                    },
                    new ConfigClaim 
                    {
                        Name = ConfigClaim.CAN_UPDATE,
                        Value = ConfigClaim.CAN_UPDATE
                    },
                    new ConfigClaim 
                    {
                        Name = ConfigClaim.CAN_DELETE,
                        Value = ConfigClaim.CAN_DELETE
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
            if (entity.CrudOperationName == CommandTypes.UpdateValue && typeof(T) == typeof(ConfigEntity))
            {
                var result = _store.SingleOrDefault(x => x.Id == entity.Id);
                result.Value = (entity as ConfigEntity).Value;
                return Task.FromResult(true);
            }
            else if (entity.CrudOperationName == CommandTypes.Delete && typeof(T) == typeof(ConfigEntity))
            {
                System.Console.WriteLine("remove it pls");
                _store.Remove(entity as ConfigEntity);
                System.Console.WriteLine("store count after delete: -" + _store.Count);
                return Task.FromResult(true);
            }

            if (entity.Id == Guid.Empty || (_store.Any(x => x.Id == entity.Id)))
            {
                return Task.FromResult(false);
            }

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