using System;

namespace heitech.configXt
{
    ///<summary>
    /// Encapsulates Configuration Action Result 
    ///</summary>
    public class ConfigResult
    {
        public bool IsSuccess => Result != null;
        public ConfigModel Result { get; }
        public Exception Exception { get; }

        private ConfigResult(ConfigModel model)
            => Result = model;

        private ConfigResult(Exception exception)
            => Exception = exception;

        public bool TryGetAs<T>(out T t)
        {
            t = default;
            return IsSuccess ?  Result.TryGetAs<T>(out t) : false;
        }

        public static ConfigResult Success(ConfigModel model) => new ConfigResult(model);
        public static ConfigResult Failure(Exception e) => new ConfigResult(e);
    }
}