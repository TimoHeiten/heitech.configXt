using System;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public class ConfigurationOperationContext
    {
        public AdministratorEntity Admin { get; }
        public IStorageModel StorageEngine { get; }
        protected Action<object> Check = o => SanityChecks.CheckNull(o, $"ctor + [{nameof(ConfigurationOperationContext)}]");
        protected ConfigurationOperationContext(AdministratorEntity admin, IStorageModel model)
        {
            Check(admin);
            Check(model);
            Admin = admin;
            StorageEngine = model;
        }
    }
}