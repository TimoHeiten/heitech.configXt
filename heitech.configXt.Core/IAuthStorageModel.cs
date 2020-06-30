using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public interface IAuthStorageModel
    {
        ///<summary>
        ///serves as Update or Create
        ///</summary>
        Task StoreUserAsync(AuthModel model, ApplicationClaim[] claims);
        ///<summary>
        ///test if user exists
        ///</summary>
        Task<bool> UserExistsAsync(AuthModel model);
        ///<summary>
        ///Get the user by name and pw hash
        ///</summary>
        Task<UserEntity> GetUserAsync(AuthModel model);
        ///<summary>
        /// Delete a user given by its name and pw hash
        ///</summary>
        Task<bool> DeleteUserAsync(AuthModel model);
    }
}