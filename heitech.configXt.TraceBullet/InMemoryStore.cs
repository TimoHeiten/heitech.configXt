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
        public readonly List<ConfigEntity> _store = new List<ConfigEntity>();

        public Task<IEnumerable<ConfigEntity>> AllEntitesAsync() 
        {
            IEnumerable<ConfigEntity> asEnumerable = _store;
            return Task.FromResult(asEnumerable);
        }

        public Task<ConfigEntity> GetEntityByNameAsync(string byName)
        {
            var config = _store.FirstOrDefault(x => x.Name == byName);
            return Task.FromResult(config);
        }

        public Task<bool> StoreEntityAsync(ConfigEntity entity)
        {
            if (entity.CrudOperationName == CommandTypes.UpdateValue)
            {
                var result = _store.SingleOrDefault(x => x.Id == entity.Id);
                result.Value = (entity as ConfigEntity).Value;

                return Task.FromResult(true);
            }
            else if (entity.CrudOperationName == CommandTypes.Delete)
            {
                _store.Remove(entity as ConfigEntity);
                return Task.FromResult(true);
            }

            if (entity.Id == Guid.Empty || (_store.Any(x => x.Id == entity.Id)))
            {
                return Task.FromResult(false);
            }

            _store.Add(entity as ConfigEntity);
            bool stored = true;
            return Task.FromResult(stored);
        }
    }
}