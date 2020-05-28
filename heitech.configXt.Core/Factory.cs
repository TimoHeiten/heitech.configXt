using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Operation;

namespace heitech.configXt.Core
{
    public class Factory
    {
        public static IRunConfigOperation CreateOperation(ConfigurationContext context)
        {
            string methName = nameof(CreateOperation);
            SanityChecks.CheckNull(context, methName);
            if (context is QueryContext queryContext)
            {
                if (_queries.TryGetValue(queryContext.QueryType, out Func<QueryContext, Task<Result>> query))
                    return new RunQueryConfigOperation(query, queryContext);
                else 
                {
                    SanityChecks.NotFound(queryContext.QueryType.ToString(), methName);
                    return new NullOperation();
                }
            }
            else if (context is CommandContext commandContext)
            {
                if (_cmds.TryGetValue(commandContext.CommandType, out Func<CommandContext, Task<Result>> command))
                    return new RunCommandConfigOperation(command, commandContext);
                else
                {
                    SanityChecks.NotFound(commandContext.CommandType.ToString(), methName);
                    return new NullOperation();
                }
            }
            else
            {
                SanityChecks.NotSupported(context.GetType().FullName, methName);
                // never called cause not supported throws
                return new NullOperation();
            } 
        }

        private static readonly Dictionary<CommandTypes, Func<CommandContext, Task<Result>>> _cmds = 
            new Dictionary<CommandTypes, Func<CommandContext, Task<Result>>>
            {
                [CommandTypes.Delete] = ctx => AllCommands.Delete(ctx),
                [CommandTypes.Create] = ctx => AllCommands.CreateAsync(ctx),
                [CommandTypes.UpdateValue] = ctx => AllCommands.Update(ctx),
                [CommandTypes.UpdateRights] = ctx => AllCommands.UpdateRights(ctx),
            };
        
        private static readonly Dictionary<QueryTypes, Func<QueryContext, Task<Result>>> _queries = 
            new Dictionary<QueryTypes, Func<QueryContext, Task<Result>>>
            {
                [QueryTypes.ValueRequest] = null,
                [QueryTypes.IsAllowedRequest] = null,
                [QueryTypes.ValueExistsRequest] = null,
                [QueryTypes.AdminExistsRequest] = null
            };
    }
}