using System;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public static class Tokens
    {
        public static bool IsStartObject(this JsonToken token) => token == JsonToken.StartObject;
        public static bool IsStartArray(this JsonToken token) => token == JsonToken.StartArray;
        public static bool IsEndObject(this JsonToken token) => token == JsonToken.EndObject;
        public static bool IsEndArray(this JsonToken token) => token == JsonToken.EndArray;

        public static bool IsStart(this JsonToken token)
            => token.IsPropertyName() || token.IsStartArray() || token.IsStartObject();
        
        public static bool IsEnd(this JsonToken token)
            => token.IsOther() || token.IsEndArray() || token.IsEndObject();

        public static bool IsPropertyName(this JsonToken token) => token == JsonToken.PropertyName;
        public static bool IsNone(this JsonToken token) => token == JsonToken.None;
        public static bool IsOther(this JsonToken token) 
        {
            return !(token.IsNone() || token.IsStartArray() || token.IsEndObject() || token.IsEndArray() || token.IsEndObject() || token.IsPropertyName());
        }

        public static bool IsValue(this JsonToken token)
        {
            Predicate<JsonToken> p = t => token == t;
            
            return p(JsonToken.String) || p(JsonToken.Integer) || p(JsonToken.Float) || p(JsonToken.Boolean) 
                   || p(JsonToken.Null) || p(JsonToken.Date);
        }

        public static bool IsCounter(this JsonToken token, JsonToken other)
        {
            bool isStart = token.IsStartObject() && other.IsEndObject();
            bool isArray = token.IsStartArray() && other.IsEndArray();
            bool isValue = token.IsPropertyName() && other.IsOther();

            return isStart || isArray || isValue;
        }
    }
}