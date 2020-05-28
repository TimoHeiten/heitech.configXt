using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.configXt.Core
{
    public interface IStorageModel
    {
        Task<T> GetEntityByNameAsync<T>(string byName)
            where T : StorageEntity;
        
        ///<summary>
        /// Create, Update or Delete at once
        ///</summary>
        Task<bool> StoreEntityAsync<T>(T entity)
            where T : StorageEntity;

        Task<IEnumerable<T>> AllEntitesAsync<T>()
            where T : StorageEntity;
    }

    public class StorageEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CommandTypes CrudOperationName { get; internal set; }
    }
}