using System;

namespace heitech.configXt.Core
{
    ///<summary>
    /// Base class for the Context for the CRUD Access.
    ///</summary>
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
        ///<summary>
        /// Access to the StorageEngine
        ///</summary>
        public IStorageModel StorageEngine { get; }
        protected Action<object> Check = o => SanityChecks.CheckNull(o, $"ctor + [{nameof(ConfigurationContext)}]");
        protected ConfigurationContext(IStorageModel model)
        {
            Check(model);;
            StorageEngine = model;
        }
    }
}