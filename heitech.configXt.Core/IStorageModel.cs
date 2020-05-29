using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public interface IStorageModel
    {
        Task<ConfigEntity> GetEntityByNameAsync(string byName);
        
        ///<summary>
        /// Create, Update or Delete at once
        ///</summary>
        Task<bool> StoreEntityAsync(ConfigEntity entity);

        Task<IEnumerable<ConfigEntity>> AllEntitesAsync();
    }
}