using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public class ConfigurationContext
    {
        private string _configName;
        public string ConfigName 
        { 
            get
            {
                return _configName;
            }
            set
            {
                Check(value);
                _configName = value;
            } 
        }
        public IStorageModel StorageEngine { get; }
        protected Action<object> Check = o => SanityChecks.CheckNull(o, $"ctor + [{nameof(ConfigurationContext)}]");
        protected ConfigurationContext(IStorageModel model)
        {
            Check(model);;
            StorageEngine = model;
        }

        internal async Task<ConfigEntity> GetConfigEntityAsync()
        {
            var storage = StorageEngine;
            return await storage.GetEntityByNameAsync(ConfigName);
        }
    }
}