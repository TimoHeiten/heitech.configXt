using System.Collections.Generic;

namespace heitech.configXt
{
    ///<summary>
    /// Abstracts a Configuration Entry with key and Value
    ///</summary>
    public class ConfigModel
    {
        public string Key { get; private set; }
        public object Value { get; private set; }

        private ConfigModel()
        { }

        ///<summary>
        /// If the captured Value is of type T you can extract it with the out parameter
        ///</summary>
        public bool TryGetAs<T>(out T t)
        {
            bool success = false;
            t = default;
            if (Value?.GetType() == typeof(T))
            {
                t = (T)Value;
                success = true;
            }

            return success;
        }

        public static ConfigModel From<T>(string key, T t) => new ConfigModel { Key = key, Value = t };
        public static ConfigModel From<T>(string key, IEnumerable<T> collection) => new ConfigModel { Key = key, Value = collection };

        public static ConfigModel Empty => new ConfigModel { Key = "", Value = "" };
    }
}