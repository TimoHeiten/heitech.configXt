using System;

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
            throw new InvalidOperationException($"ConfigEntity by Name: [{name}] not found in {v}");
        }

        internal static void StorageFailed(Result result, string v)
        {
            throw new InvalidOperationException($"Storing the entity by Name: [{result.Before?.Key}] failed in {v}");
        }
    }
}