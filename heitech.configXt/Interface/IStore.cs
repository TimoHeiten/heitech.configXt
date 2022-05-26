using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.configXt
{
    ///<summary>
    /// Store abstraction for the persistent values of the Configuration data
    ///</summary>
    public interface IStore
    {
        Task FlushAsync();

        Task<IReadOnlyList<ConfigModel>> GetAll();
    }
}