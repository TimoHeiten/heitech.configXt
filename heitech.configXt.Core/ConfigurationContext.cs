using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public class ConfigurationContext
    {
        public string AdminName { get; }
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
        protected ConfigurationContext(string admin, IStorageModel model)
        {
            Check(admin);
            Check(model);;
            AdminName = admin;
            StorageEngine = model;
        }

        internal async Task<ConfigEntity> GetConfigEntityAsync()
        {
            var storage = StorageEngine;

            return await storage.GetEntityByNameAsync<ConfigEntity>(ConfigName);
        }

        internal async Task<AdministratorEntity> GetAdminEntityAsync()
        {
            var storage = StorageEngine;

            var admin = await storage.GetEntityByNameAsync<AdministratorEntity>(AdminName);
            if (admin == null)
            {
                SanityChecks.NotFound(AdminName, $"{nameof(ConfigurationContext)}.{nameof(GetAdminEntityAsync)}");
            }
            return admin;
        }
    }
}