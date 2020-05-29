using System;
using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    internal static class Sentinel
    {
        internal static bool IsAllowed(this CommandContext context, ConfigEntity entity, IEnumerable<ConfigClaim> hasClaims)
        {
            Predicate<string> predicate = requiredClaim => hasClaims.Any(x => x.Value.Equals(requiredClaim, StringComparison.InvariantCultureIgnoreCase));

            switch (context.CommandType)
            {
                case CommandTypes.Create:
                    return predicate(ConfigClaim.CAN_CREATE);
                case CommandTypes.UpdateValue:
                    return predicate(ConfigClaim.CAN_UPDATE);
                case CommandTypes.Delete:
                    return predicate(ConfigClaim.CAN_DELETE);
                default:
                    throw new NotSupportedException();
            }
        }

        internal static bool IsReadAllowed(this AdministratorEntity admin)
        {
            return admin.Claims.Any(x => x.Value.Equals(ConfigClaim.CAN_READ, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}