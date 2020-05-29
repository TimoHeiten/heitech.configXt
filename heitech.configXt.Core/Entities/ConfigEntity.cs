using System;
using heitech.configXt.Core.Commands;

namespace heitech.configXt.Core.Entities
{
    public class ConfigEntity
    {
        public string Value { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CommandTypes CrudOperationName { get; internal set; }
    }
}

