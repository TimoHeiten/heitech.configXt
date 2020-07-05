using System;
using heitech.configXt.Core.Commands;

namespace heitech.configXt.Core.Entities
{
    public class ConfigEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ApplicationClaim AppClaim { get; set; }
        public Guid ApplicationClaimId { get; set; }
        public CommandTypes CrudOperationName { get; internal set; }
    }
}

