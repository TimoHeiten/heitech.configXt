using System;

namespace heitech.configXt.Core.Entities
{
    public class ApplicationClaim
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public ConfigEntity ConfigEntity { get; set; }
    }
}