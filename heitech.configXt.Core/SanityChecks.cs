using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("heitech.configXt.Tests")]
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

        internal static OperationResult NotSupported(string fullName, string methodName)
        {
            return OperationResult.Failure(ResultType.BadRequest, methodName);
        }

        internal static OperationResult NotFound(string name, string v)
        {
            return OperationResult.Failure(ResultType.NotFound, v);
        }

        internal static OperationResult StorageFailed<T>(string operation, string v)
        {
            return OperationResult.Failure(ResultType.Forbidden, v);
        }

        internal static void IsSameOperationType(string expected, string actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected OperationType [{expected}] did not match [{actual}]");
            }
        }
    }
}