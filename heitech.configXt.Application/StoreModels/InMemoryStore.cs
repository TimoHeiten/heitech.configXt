using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class InMemoryStore : IStorageModel
    {
        protected readonly Dictionary<string, ConfigEntity> _entities = new Dictionary<string, ConfigEntity>();

        public Task<IEnumerable<ConfigEntity>> AllEntitesAsync()
        {
            return Task.FromResult<IEnumerable<ConfigEntity>>
            (
                _entities.Select(x => x.Value).ToList()
            );
        }

        public Task<ConfigEntity> GetEntityByNameAsync(string byName)
        {
            if (_entities.TryGetValue(byName, out ConfigEntity entity))
                return Task.FromResult(entity);
            else
                return Task.FromResult<ConfigEntity>(null);
        }

        public Task<bool> StoreEntityAsync(ConfigEntity entity)
        {
            Func<bool, Task<bool>> result = b => Task.FromResult(b);
            switch (entity.CrudOperationName)
            {
                case CommandTypes.Create:
                    if (!_entities.ContainsKey(entity.Name))
                    {
                        _entities.Add(entity.Name, entity);
                        return result(true);
                    }
                    else
                    {
                        return result(false);
                    }
                case CommandTypes.UpdateValue:
                    if (_entities.TryGetValue(entity.Name, out ConfigEntity e))
                    {
                        e.Value = entity.Value;
                        return result(true);
                    }
                    else
                        return result(false);
                case CommandTypes.Delete:
                    if (_entities.TryGetValue(entity.Name, out ConfigEntity eDelete))
                    {
                        _entities.Remove(entity.Name);
                        return result(true);
                    }
                    else
                        return result(false);
                default:
                    throw new NotSupportedException(entity.CrudOperationName + " was not found...");
            }
        }
    }
}