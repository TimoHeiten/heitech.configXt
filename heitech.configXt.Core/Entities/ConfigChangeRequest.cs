using System.Collections.Generic;

namespace heitech.configXt.Core.Entities
{
    public class ConfigChangeRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public IDictionary<string, string> Claims { get; set; }
    }
}