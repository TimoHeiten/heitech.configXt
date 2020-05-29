using System;
using System.Linq;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    ///<summary>
    /// Object that describes the result of an operation.
    ///</summary>
    public class OperationResult
    {
        ///<summary>
        /// The Wrapped Result
        ///</summary>
        public ConfigEntity Result { get; private set; }
        ///<summary>
        /// Tells if the operation was successfull
        ///</summary>
        public bool IsSuccess { get; private set; }
        ///<summary>
        /// If not successfull, contains the exception
        ///</summary>
        public Exception Error { get; private set; }

        ///<summary>
        /// ErrorMessage on Failure if necessary
        ///</summary>
        public string ErrorMessage { get; set; }

        ///<summary>
        /// Marks the Result in an error code style, similar to HttpCodes
        ///</summary>
        public ResultType ResultType { get; private set; }

        private OperationResult()
        { }

        ///<summary>
        /// Static factory method: Indicating Success of operation
        ///</summary>
        public static OperationResult Success(ConfigEntity obj)
            => Create(ResultType.Ok, obj);

        private static OperationResult Create(ResultType t, ConfigEntity o = null, Exception e = null)
            => new OperationResult
            {
                Error = e,
                Result = o,
                ResultType = t
            };

        ///<summary>
        /// Static factory method: Indicating Failure without an exception
        ///</summary>
        public static OperationResult Failure(ResultType type, string message)
            => new OperationResult
            {
                IsSuccess = false,
                ErrorMessage = message,
                ResultType = type
            };

        ///<summary>
        /// Static factory method: Indicating Failure with an exception
        ///</summary>
        public static OperationResult Failure(ResultType type, Exception ex)
            => Create(type, e: ex);

        ///<summary>
        /// Static factory method: Indicating Failure with an exception and a describing object
        ///</summary>
        public static OperationResult Failure(ResultType type, Exception ex, ConfigEntity obj)
            => Create(type, obj, ex);
    }


    public enum ResultType
    {
        BadRequest,
        NoContent,
        NotFound,
        InternalError,
        Created,
        Forbidden,
        Unauthorized,
        Ok
    }
}