using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Queries
{
    internal static class AllQueries
    {
        internal static async Task<Result> QueryConfigEntityAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            // check if exists
            var config = await context.GetConfigEntityAsync();
            if (config == null)
            {
                SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            return new Result 
            {
                RequestType = context.QueryType.ToString(),
                Current = config,
                Before = config,
                Success = true,
            };
        }

        internal static async Task<Result> QueryAllConfigEntityValuesAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            // check if exists
            var configs = await context.StorageEngine.AllEntitesAsync();
            if (configs.Any() == false)
            {
                SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            var collection = new ConfigCollection { WrappedConfigEntities = configs };
            return new Result 
            {
                RequestType = context.QueryType.ToString(),
                Current = collection,
                Before = collection,
                Success = true,
            };
        }
    }
    
}