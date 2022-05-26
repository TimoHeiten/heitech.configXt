using System;
using heitech.configXt.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace heitech.configXt
{
    ///<summary>
    /// Abstracts a Configuration Entry with key and Value
    ///</summary>
    public class ConfigModel
    {
        private readonly Type initialType;
        public string Key { get; private set; }
        ///<summary>
        /// the actual object as json string
        ///</summary>
        public string Value { get; private set; } // json string
        public ConfigKind Kind { get; }

        private ConfigModel(string key, object value)
        { 
            Key = key;
            var (success, token) = isValidJson(value);
            Kind = DetermineKind(success, token);
            Value = Kind == ConfigKind.Literal 
                    ? $"{value}" 
                    : token.ToString(Formatting.None);
            if (!success)
                Value = "{}";

            initialType = value.GetType();
        }

        private static ConfigKind DetermineKind(bool success, JToken token)
        {
            if (!success)
                return ConfigKind.None;

            if (token is JArray) return ConfigKind.Array;
            else if (token is JObject) return ConfigKind.Object;
            else if (token is JValue) return ConfigKind.Literal;

            throw new NotSupportedException();
        }

        public static bool IsValidJson(object obj)
        {
            var (success, _) = isValidJson(obj);
            return success;
        }

        static bool isLiteral(object obj)
        {
            Type t = obj.GetType();
            return t == typeof(string)
                   || t == typeof(int)
                   || t == typeof(long)
                   || t == typeof(short)
                   || t == typeof(byte)
                   || t == typeof(bool);
        }

        private static (bool, JToken) isValidJson(object obj)
        {
            try
            {
                var asString = obj.GetType() == typeof(string)
                               ? $"{obj}"
                               : ToNormalizedJson(obj);

                var token = JToken.Parse(asString);
                return (true, token);
            }
            catch
            {
                if (IsLiteralButNotFailedParse(obj))
                    return (true, new JValue($"{obj}"));
                else
                    return (false, new JValue("{}"));
            }
        }

        private static bool IsLiteralButNotFailedParse(object obj)
        {
            bool _isLiteral = isLiteral(obj);
            string asString = $"{obj}";
            bool hasNoBracket = !asString.Contains("{") && !asString.Contains("}");

            return _isLiteral && hasNoBracket;
        }

        public static string ToNormalizedJson(object obj)
            => JsonConvert.SerializeObject(obj, Formatting.None);

        public static ConfigModel From<T>(string key, T t) 
            => new ConfigModel(key, t);

        public static ConfigModel PlaceHolder(string key)
            => new ConfigModel(key, string.Empty);

        public object ToOutput() => new {
            Key,
            Value,
            Kind
        };
    }
}