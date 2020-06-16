using heitech.configXt.Core.Entities;
using Newtonsoft.Json.Linq;

namespace heitech.configXt.Application
{
    public class ConfigEntityJson : ConfigEntity
    {
        public JObject Json { get; set; }
    }
}