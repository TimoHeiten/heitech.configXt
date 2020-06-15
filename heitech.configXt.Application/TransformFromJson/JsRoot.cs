using System;
using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Core.Entities;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public class JsRoot
    {
        private static IJitem _current;
        public static IEnumerable<ConfigEntity> Run(Stack<Combi> stack)
        {
            _current = new Jobj2("", null);
            while (stack.Any())
            {
                Combi combi = stack.Pop();
                JsonToken token = combi.Token;
                if (token.IsPropertyName())
                {
                    var property = new JProperty(combi.Value, _current);
                    _current = property;
                }
                else if (token.IsStartObject())
                {
                    if (_current.Parent != null)
                    {
                        var obj = new Jobj2(_current.Name, _current);
                        _current = obj;
                    }
                }
                else if (token.IsStartArray())
                {
                    var array = new JArray(_current.Name, _current);
                    _current = array;
                }
                else if (token.IsValue() || token == JsonToken.Null)
                {
                    if (_current is JArray)
                    {
                        _current.AddValue(new JArrayElement(combi.Value));
                    }
                    else
                    {
                        _current.AddValue(new JValue(combi.Value, _current));
                        _current = _current.Parent;
                    }
                }
                else if (token.IsEndArray())
                {
                     _current.Parent.AddValue(_current);
                    _current = _current.Parent.Parent;
                }
                else if (token.IsEndObject())
                {
                    if (stack.Any())
                        _current = _current.Parent.Parent;
                }
            }
            var result = _current.Yield();
            return result;
        }
    }
}