using System.Collections.Generic;
using System.IO;
using System.Linq;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public class JsonTransform : ITransform
    {
        private string _json;
        private readonly IStorageModel _model;
        public JsonTransform(IStorageModel model)
        {
            _model = model;
        }

        public OperationResult Transform(string inputString)
        {
            _json = inputString;
            var entities = new List<ConfigEntity>();
            var result = ReadDocument();

            ConfigTemplate current = null;
            var path = new CurrentObjectPath(string.Empty);
            foreach (var (token, value) in result)
            {
                bool decommission = false;
                if (current == null)
                {
                    current = new ConfigTemplate(token);
                    if (path.Exists)
                    {
                        current.From = JsonToken.StartObject;
                        if (value != null && value.ToString() == "Key")
                        {
                            System.Console.WriteLine(path.ToString());
                        }
                        current.AssignName(path.ToString());
                    }
                }
                if (token.IsPropertyName())
                {
                    current.AssignName(value.ToString());
                }
                else if (token.IsOther() && value != null)
                {
                    current.AssignValue(value.ToString());
                    bool isArray = current.From.IsStartArray();
                    if (!isArray)
                        decommission = true;
                }
                else if (token.IsStartArray())
                {
                    current.From = token;
                }
                else if (token.IsStartObject())
                {
                    path.Add(current.Name);
                    current.From = token;
                }
                else if (token.IsEndObject())
                {
                    path.SubtractOne();
                    current = null;
                }
                else if (token.IsEndArray())
                {
                    decommission = true;
                }

                if (decommission)
                {
                    entities.Add(current.ToEntity());
                    current = null;
                }
            }

            var collection = new ConfigCollection();
            collection.WrappedConfigEntities = entities;
            return OperationResult.Success(collection);
        }

        private IList<(JsonToken token, object value)> ReadDocument()
        {
            var reader = new JsonTextReader(new StringReader(_json));
            var list = new List<(JsonToken, object)>();
            while (reader.Read())
            {
                // reader.TokenType, reader.Value
                list.Add((reader.TokenType, reader.Value));
            }
            return list.Skip(1)
                       .Take(list.Count-1)
                       .ToList();
        }

        private bool Starter(JsonToken token)
        {
            return token.IsStartObject() || token.IsStartArray();
        }
    }
}