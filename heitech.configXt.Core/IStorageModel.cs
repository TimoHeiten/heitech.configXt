using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public interface IStorageModel
    {
        Task<ConfigEntity> GetEntityByNameAsync(string configEntryName);
        Task<bool> StoreConfigEntityAsync(ConfigEntity entity);
    }
}