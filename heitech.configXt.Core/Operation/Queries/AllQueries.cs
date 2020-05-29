using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Queries
{
    internal static class AllQueries
    {
        internal static async Task<OperationResult> QueryConfigEntityAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            // check if exists
            var config = await context.StorageEngine.GetEntityByNameAsync(context.ConfigName);
            if (config == null)
            {
                return SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            return OperationResult.Success(config);
        }

        internal static async Task<OperationResult> QueryAllConfigEntityValuesAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            // check if exists
            var configs = await context.StorageEngine.AllEntitesAsync();
            if (configs.Any() == false)
            {
                return SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            var collection = new ConfigCollection { WrappedConfigEntities = configs };
            return OperationResult.Success(collection);
        }
    }
    
}