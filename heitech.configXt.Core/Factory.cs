using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.Core
{
    public class Factory
    {
        ///<summary>
        /// Run the configEntity Crud operation with respect to the Context type.
        ///  Returns an OperationResult of Type ConfigEntity
        ///</summary>
        public static async Task<OperationResult> RunOperationAsync(ConfigurationContext context)
        {
            string methName = nameof(RunOperationAsync);
            SanityChecks.CheckNull(context, methName);
            if (context is QueryContext queryContext)
            {
                if (_queries.TryGetValue(queryContext.QueryType, out Func<QueryContext, Task<OperationResult>> query))
                    return await query(queryContext);
                else 
                {
                    return OperationResult.Failure
                    (
                        ResultType.BadRequest,
                        $"did not find query type: {queryContext.QueryType}"
                    );
                }
            }
            else if (context is CommandContext commandContext)
            {
                if (_cmds.TryGetValue(commandContext.CommandType, out Func<CommandContext, Task<OperationResult>> command))
                    return await command(commandContext);
                else
                {
                    return OperationResult.Failure
                    (
                        ResultType.BadRequest,
                        $"Did not find CommandType: {commandContext.CommandType}"
                    );
                }
            }
            else
            {
                return OperationResult.Failure
                (
                    ResultType.BadRequest,
                    $"Type {context.GetType()} is not supported"
                );
            } 
        }

        private static readonly Dictionary<CommandTypes, Func<CommandContext, Task<OperationResult>>> _cmds = 
            new Dictionary<CommandTypes, Func<CommandContext, Task<OperationResult>>>
            {
                [CommandTypes.Delete] = async ctx => await AllCommands.DeleteAsync(ctx),
                [CommandTypes.Create] = async ctx => await AllCommands.CreateAsync(ctx),
                [CommandTypes.UpdateValue] = async ctx => await AllCommands.UpdateAsync(ctx),
            };
        
        private static readonly Dictionary<QueryTypes, Func<QueryContext, Task<OperationResult>>> _queries = 
            new Dictionary<QueryTypes, Func<QueryContext, Task<OperationResult>>>
            {
                [QueryTypes.ValueRequest] = async ctx => await AllQueries.QueryConfigEntityAsync(ctx),
                [QueryTypes.AllValues] = async ctx => await AllQueries.QueryAllConfigEntityValuesAsync(ctx),
            };
    }
}