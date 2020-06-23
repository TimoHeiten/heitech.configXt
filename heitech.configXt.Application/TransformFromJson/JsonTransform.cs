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
            var working = new WorkingTransform();
            working.Parse(new JsonTextReader(new StringReader(_json)));
            var entities = working.Yield();

            var collection = new ConfigCollection();
            collection.WrappedConfigEntities = entities;

            return OperationResult.Success(collection);
        }

    }
}


