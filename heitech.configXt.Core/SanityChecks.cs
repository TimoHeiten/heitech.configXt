using System;
using System.Collections.Generic;
using heitech.configXt.Core.Exceptions;

namespace heitech.configXt.Core
{
    internal static class SanityChecks
    {
        internal static void CheckNull<T>(T obj, string methodName)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentException($"in method: [{methodName}] the argument of type [{typeof(T)}] was unexpectedly null!");
            }
        }

        internal static void NotSupported(string fullName, string methodName)
        {
            throw new NotSupportedException($"In method: [{methodName}] - {fullName} is not supported");
        }

        internal static void NotFound(string name, string v)
        {
            throw new InvalidOperationException($"Entity by Name: [{name}] not found in {v}");
        }

        internal static void StorageFailed<T>(string operation, string v)
        {
            throw new InvalidOperationException($"Storing the entity of type [{typeof(T)}] with operation: [{operation}] failed at {v}");
        }

        internal static void NotAllowed(List<string> notMet, CommandTypes commandType, string v)
        {
            throw new NotAllowedException(commandType, notMet, $"{nameof(CommandTypes)}.{commandType} was not allowed at - {v}");
        }
    }
}