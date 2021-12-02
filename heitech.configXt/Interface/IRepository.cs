using System.Threading.Tasks;

namespace heitech.configXt
{
    public interface IRepository
    {
        void Add(ConfigModel model);
        Task<ConfigModel> DeleteAsync(string key);
        Task<ConfigModel> UpdateAsync(ConfigModel model);
        Task<ConfigModel> Get(string id);
    }
}