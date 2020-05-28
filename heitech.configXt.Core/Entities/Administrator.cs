using System;
using System.Collections.Generic;

namespace heitech.configXt.Core.Entities
{
    public class AdministratorEntity : StorageEntity
    {
        public IList<ConfigClaim> Claims { get; set; }
    }
}