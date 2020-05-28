using System;
using System.Collections.Generic;

namespace heitech.configXt.Core.Entities
{
    public class AdministratorEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<ConfigClaim> Claims { get; set; }
    }
}