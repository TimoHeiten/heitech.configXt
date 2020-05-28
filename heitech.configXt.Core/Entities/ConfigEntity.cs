using System;

namespace heitech.configXt.Core.Entities
{
    public class ConfigEntity
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }

        public string[] AllowedForRead { get; set; }
        public string[] AllowedForAlter { get; set; }
    }
}

