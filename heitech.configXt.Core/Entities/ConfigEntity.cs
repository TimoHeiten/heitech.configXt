using System.Collections.Generic;

namespace heitech.configXt.Core.Entities
{
    public class ConfigEntity : StorageEntity
    {
        public string Value { get; set; }
        public IList<ConfigClaim> NecessaryClaims { get; set; }
    }
}

