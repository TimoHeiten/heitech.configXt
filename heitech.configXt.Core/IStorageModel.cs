using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public interface IStorageModel
    {
        ///<summary>
        /// GetConfigEntity by name 
        ///</summary>
        Task<ConfigEntity> GetEntityByNameAsync(string byName);
        
        ///<summary>
        /// Create, Update or Delete at once
        ///</summary>
        Task<bool> StoreEntityAsync(ConfigEntity entity);

        ///<summary>
        /// Return IEnumerable of Entities 
        ///</summary>
        Task<IEnumerable<ConfigEntity>> AllEntitesAsync();

        Task<bool> IsAllowedWriteAsync(AuthModel authModel, string appName);
        Task<bool> IsAllowedReadAsync(AuthModel authModel, string appName);
    }
}