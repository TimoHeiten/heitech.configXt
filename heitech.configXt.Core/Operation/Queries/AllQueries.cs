using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

[assembly: InternalsVisibleTo("heitech.configXt.Tests.Core")]
namespace heitech.configXt.Core.Queries
{
    internal static class AllQueries
    {
        ///<summary>
        /// Actual Query to get Data from the StorageModel
        ///</summary>
        internal static async Task<OperationResult> QueryConfigEntityAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            SanityChecks.CheckNull(context, methName);
            SanityChecks.IsSameOperationType(QueryTypes.ValueRequest.ToString(), context.QueryType.ToString());

            // check if exists
            var config = await context.StorageEngine.GetEntityByNameAsync(context.ConfigName);
            if (config == null)
            {
                return SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            return OperationResult.Success(config);
        }

        ///<summary>
        /// Actual Query for all Config Entities using the StorageModel.
        ///</summary>
        internal static async Task<OperationResult> QueryAllConfigEntityValuesAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            SanityChecks.CheckNull(context, methName);
            SanityChecks.IsSameOperationType(QueryTypes.AllValues.ToString(), context.QueryType.ToString());

            // check if exists
            var configs = await context.StorageEngine.AllEntitesAsync();
            if (configs == null || configs.Any() == false)
            {
                return SanityChecks.NotFound(context.ConfigName, methName);
            }

            // return entity
            var collection = new ConfigCollection { WrappedConfigEntities = configs };
            return OperationResult.Success(collection);
        }
    }
    
}