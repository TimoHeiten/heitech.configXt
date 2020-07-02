using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("heitech.configXt.Tests")]
namespace heitech.configXt.Core
{
    internal static class SanityChecks
    {
        ///<summary>
        /// Check Null with method name
        ///</summary>
        internal static void CheckNull<T>(T obj, string methodName, string paramName = "not-specified")
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentException($"in method: [{methodName}] the argument of type [{typeof(T)}] and paramName [{paramName}] was unexpectedly null!");
            }
        }

        ///<summary>
        /// OperationResult.Failure BadRequest
        ///</summary>
        internal static OperationResult NotSupported(string fullName, string methodName)
        {
            return OperationResult.Failure(ResultType.BadRequest, methodName);
        }

        ///<summary>
        /// OperationResult.Failure NotFound
        ///</summary>
        internal static OperationResult NotFound(string name, string v)
        {
            return OperationResult.Failure(ResultType.NotFound, v);
        }

        ///<summary>
        /// OperationResult.Failure Forbidden
        ///</summary>
        internal static OperationResult StorageFailed<T>(string operation, string v)
        {
            return OperationResult.Failure(ResultType.Forbidden, v);
        }

        ///<summary>
        /// Throws if expected and actual are not the same string.
        ///</summary>
        internal static void IsSameOperationType(string expected, string actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected OperationType [{expected}] did not match [{actual}]");
            }
        }
    }
}