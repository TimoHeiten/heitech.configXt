using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using heitech.configXt.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace heitech.configXt.Application
{
    public class WorkingTransform
    {
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        private readonly List<ConfigEntity> _entities = new List<ConfigEntity>();

        public IEnumerable<ConfigEntity> Yield() => _entities;
        public void Parse(JsonTextReader reader)
        {
            _data.Clear();
            var jsonConfig = JObject.Load(reader);

            VisitJObject(jsonConfig);
        }

        private void VisitJObject(Newtonsoft.Json.Linq.JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitProperty(property);
                ExitContext();
            }
        }

        private void VisitProperty(Newtonsoft.Json.Linq.JProperty property)
        {
            VisitToken(property.Value);
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;

                case JTokenType.Array:
                    VisitArray(token.Value<Newtonsoft.Json.Linq.JArray>());
                    break;

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Bytes:
                case JTokenType.Raw:
                case JTokenType.Null:
                    VisitPrimitive(token.Value<Newtonsoft.Json.Linq.JValue>());
                    break;

                default:
                    throw new FormatException("token not supported");
            }
        }

        private void VisitArray(Newtonsoft.Json.Linq.JArray array)
        {
            for (int index = 0; index < array.Count; index++)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(Newtonsoft.Json.Linq.JValue data)
        {
            var key = _currentPath;

            if (_data.ContainsKey(key))
            {
                throw new FormatException("duplicated key");
            }
            _data[key] = data.ToString(CultureInfo.InvariantCulture);
            _entities.Add(new ConfigEntity() { Name = key, Value = _data[key]});
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}