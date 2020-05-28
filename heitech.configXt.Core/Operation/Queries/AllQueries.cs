using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Operation;

namespace heitech.configXt.Core.Queries
{
    internal static class AllQueries
    {
        internal static async Task<Result> QueryConfigEntityAsync(QueryContext context)
        {
            string methName = $"{nameof(AllQueries)}.{nameof(QueryConfigEntityAsync)}";
            // check admin
            var admin = await context.GetAdminEntityAsync();
            // check allowed
            bool isAllowed = admin.IsReadAllowed();
            if (isAllowed == false)
            {
                SanityChecks.NotAllowed(new List<string> { ConfigClaim.CAN_READ }, QueryTypes.ValueRequest.ToString(), methName);
            }
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
    }
}