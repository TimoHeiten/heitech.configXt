using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public static class Tokens
    {
        public static bool IsStartObject(this JsonToken token) => token == JsonToken.StartObject;
        public static bool IsStartArray(this JsonToken token) => token == JsonToken.StartArray;
        public static bool IsEndObject(this JsonToken token) => token == JsonToken.EndObject;
        public static bool IsEndArray(this JsonToken token) => token == JsonToken.EndArray;

        public static bool IsPropertyName(this JsonToken token) => token == JsonToken.PropertyName;
        public static bool IsNone(this JsonToken token) => token == JsonToken.None;
        public static bool IsOther(this JsonToken token) 
        {
            return !(token.IsNone() || token.IsStartArray() || token.IsEndObject() || token.IsEndArray() || token.IsEndObject() || token.IsPropertyName());
        }
    }
}