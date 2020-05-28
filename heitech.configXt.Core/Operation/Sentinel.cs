using System;
using System.Collections.Generic;

namespace heitech.configXt.Core.Operation
{
    internal static class Sentinel
    {
        internal static bool IsAllowed(CommandTypes types, IEnumerable<ConfigClaim> hasClaims, IEnumerable<ConfigClaim> requiredClaims)
        {
            switch (types)
            {
                case CommandTypes.Create:
                    // todo
                    return true;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}