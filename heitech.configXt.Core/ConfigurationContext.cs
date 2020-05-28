using System;

namespace heitech.configXt.Core
{
    public class ConfigurationContext
    {
        public string AdminName { get; }
        public IStorageModel StorageEngine { get; }
        protected Action<object> Check = o => SanityChecks.CheckNull(o, $"ctor + [{nameof(ConfigurationContext)}]");
        protected ConfigurationContext(string admin, IStorageModel model)
        {
            Check(admin);
            Check(model);
            AdminName = admin;
            StorageEngine = model;
        }
    }
}