using System.Threading.Tasks;

namespace heitech.configXt
{
    ///<summary>
    /// Actual Configuration Service for the CRUD Interface
    ///</summary>
    public interface IService
    {
        Task<ConfigResult> CreateAsync(ConfigModel model);
        Task<ConfigResult> RetrieveAsync(string key);
        Task<ConfigResult> UpdateAsync(ConfigModel model);
        Task<ConfigResult> DeleteAsync(string key);
    }
}