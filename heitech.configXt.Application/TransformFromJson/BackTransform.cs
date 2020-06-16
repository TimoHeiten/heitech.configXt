using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Application
{
    public class BackTransform
    {
        ///<summary>
        /// works only one level deep for now 
        ///
        ///</summary>
        public static ConfigEntityJson Transform(IEnumerable<ConfigEntity> entities)
        {
            System.Console.WriteLine("entities count "+ entities.Count());
            var jObj = new JObject();
            foreach (ConfigEntity e in entities)
            {
                // parse name, allocate in dictionary
                string[] name = e.Name.Split(':');
                if (e.Value.Split(',').Length > 0)
                {
                    Newtonsoft.Json.Linq.JValue[] split = e.Value.Split(',').Select(x => new Newtonsoft.Json.Linq.JValue(e.Value)).ToArray();
                    var array = new Newtonsoft.Json.Linq.JArray();
                    foreach (var item in split)
                    {
                        array.Add(item);
                    }
                    jObj.Add(e.Name, array);
                }
                else if (!name.Contains(":"))
                {
                    var prop = new Newtonsoft.Json.Linq.JProperty(e.Name, e.Value);
                    jObj.Add(prop);
                }
                else 
                {
                    var next = new JObject();
                    string first = name.First();
                    string last = name.Last();
                    if (jObj[e.Name] != null)
                    {
                        jObj.Add(new Newtonsoft.Json.Linq.JProperty(last, e.Value));
                    }
                }
            }
            return new ConfigEntityJson { Json = jObj };
        }
    }
}