using System;
using heitech.configXt.Core.Operation;

namespace heitech.configXt.Core
{
    internal class Factory
    {
        public static IRunConfigOperation CreateQueryOperation(ConfigurationOperationContext context)
        {
            string methName = nameof(CreateQueryOperation);
            SanityChecks.CheckNull(context, methName);
            if (context is QueryContext queryContext)
            {
                var operation = CreateAQuery(queryContext);

                return new RunQueryConfigOperation(operation, queryContext);
            }
            else if (context is CommandContext commandContext)
            {
                var operation = CreateACommand(commandContext);
                return new RunCommandConfigOperation(operation, commandContext);
            }
            else
            {
                SanityChecks.NotSupported(context.GetType().FullName, methName);
                // never called cause not supported throws
                return null;
            } 

        }

        internal static IConfigureCommand CreateACommand(CommandContext context)
        {
            Action<object> check = o =>  SanityChecks.CheckNull(o, nameof(CreateACommand));
            check(context.Admin);
            check(context.ChangeRequest);

            switch (context.CommandType)
            {
                case CommandTypes.Create:
                    break;
                case CommandTypes.UpdateRights:
                    break;
                case CommandTypes.UpdateValue:
                    break;
                case CommandTypes.Delete:
                    break;
                default:
                    throw new NotSupportedException($"{nameof(CommandTypes)} is not supported: {context.CommandType}");
            }

            return null;
        }

        internal static IConfigureQuery CreateAQuery(QueryContext context)
        {
            Action<object> check = o =>  SanityChecks.CheckNull(o, nameof(CreateAQuery));
            check(context.Admin);
            check(context.Request);

            switch (context.QueryType)
            {
                case QueryTypes.AdminExistsRequest:
                    break;
                case QueryTypes.ValueExistsRequest:
                    break;
                case QueryTypes.ValueRequest:
                    break;
                case QueryTypes.IsAllowedRequest:
                    break;
                default:
                    throw new NotSupportedException($"{nameof(QueryTypes)} is not supported: {context.QueryType}");
            }

            return null;
        }
    }
}