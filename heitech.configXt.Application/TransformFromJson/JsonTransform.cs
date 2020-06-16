using System.Collections.Generic;
using System.IO;
using System.Linq;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public class JsonTransform : ITransform
    {
        private string _json;
        public JsonTransform()
        { }

        public OperationResult Format(IEnumerable<ConfigEntity> entities)
        {
            var obj =  BackTransform.Transform(entities);
            return OperationResult.Success(obj);
        }

        public OperationResult Parse(string inputString)
        {
            _json = inputString;
            var result = ReadDocument();


            var _stack = new Stack<Combi>();
            result.Select(x => new Combi(x.Item1, x.Item2))
                  .Reverse()
                  .ToList()
                  .ForEach(x => _stack.Push(x));

            var entities = JsRoot.Run(_stack);

            var collection = new ConfigCollection();
            collection.WrappedConfigEntities = entities;


            return OperationResult.Success(collection);
        }

        private List<(JsonToken token, object value)> ReadDocument()
        {
            var reader = new JsonTextReader(new StringReader(_json));
            var list = new List<(JsonToken, object)>();
            while (reader.Read())
            {
                // reader.TokenType, reader.Value
                list.Add((reader.TokenType, reader.Value));
            }
            return list;
        }
    }

    public class Combi 
    {
        public string Value { get; }
        public JsonToken Token { get; }
        public Combi(JsonToken token, object o)
        {
            Token = token;
            Value = o == null ? null : o.ToString();
        }
    }
}


