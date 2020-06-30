using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public static class UserOperations
    {
        public static async Task<OperationResult> StoreUserAsync(UserContext context)
        {
            AuthModel model = context.AuthModel;
            try
            {
                await context.AuthStorage.StoreUserAsync(model, context.Claims);
                return OperationResult.Success(ResultEntity);
            }
            catch (System.Exception ex)
            {
                return OperationResult.Failure(ResultType.InternalError, ex);
            }
        }

        public static async Task<OperationResult> GetAsync(UserContext context)
        {
            AuthModel model = context.AuthModel;
            var user = await context.AuthStorage.GetUserAsync(model);

            return user == null
                   ? OperationResult.Failure(ResultType.NotFound, $"user: [{model.Name}] not found")
                   : OperationResult.Success(ResultEntity);
        }

        public static async Task<OperationResult> DeleteAsync(UserContext context)
        {
            AuthModel model = context.AuthModel;
            bool exists = await context.AuthStorage.UserExistsAsync(model);
            bool worked = false;
            if (exists)
            {
                worked = await context.AuthStorage.DeleteUserAsync(model);
            }

            return worked 
                   ? OperationResult.Success(ResultEntity)
                   : OperationResult.Failure(ResultType.NotFound, $"User: [{model.Name}] does not exist");
        }

        private static  ConfigEntity ResultEntity => new ConfigEntity { Name = "Just a Result of a user operation"};
    }
}