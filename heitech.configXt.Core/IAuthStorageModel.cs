using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public interface IAuthStorageModel
    {
        Task StoreUserAsync(AuthModel model, ApplicationClaim[] claims);
        Task<bool> UserExistsAsync(AuthModel model);
        Task<UserEntity> GetUserAsync(AuthModel model);

        Task<UserEntity> AddClaims(UserEntity entity, ApplicationClaim[] cliams);
    }
}