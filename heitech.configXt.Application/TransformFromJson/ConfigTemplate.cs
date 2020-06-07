using heitech.configXt.Core.Entities;
using Newtonsoft.Json;

namespace heitech.configXt.Application
{
    public class ConfigTemplate
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public JsonToken From { get; set; }
        public ConfigTemplate(JsonToken creator)
        {
            From = creator;
        }

        public void AssignName(string name)
        {
            if (From.IsStartObject())
            {
                if (Name == null)
                    Name = name;
                else
                    Name += $":{name}";
            }
            else
            {
                Name = name;
            }
        }

        public void AssignValue(string value)
        {
            if (From.IsStartArray())
            {
                // name stays, value gets tagged on
                if (Value == null)
                    Value = value;
                else
                    Value += $",{value}";
            }
            else 
            {
                Value = value;
            }
        }

        internal ConfigEntity ToEntity()
        {
            return new ConfigEntity
            {
                Name = Name,
                Value = Value
            };
        }
    }
}