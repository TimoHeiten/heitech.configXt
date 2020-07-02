using System;

namespace heitech.configXt.Core
{
    ///<summary>
    /// Base class for the Context for the CRUD Access.
    ///</summary>
    public class ConfigurationContext
    {
        private string _configName;
        ///<summary>
        /// The Key of the ConfigurationEntry in question
        ///</summary>
        public string ConfigurationEntryKey 
        { 
            get
            {
                return _configName;
            }
            set
            {
                Check(value, "configurationEntry");
                _configName = value;
            } 
        }
        ///<summary>
        /// Access to the StorageEngine
        ///</summary>
        public IStorageModel StorageEngine { get; }
        protected ConfigurationContext(IStorageModel model)
        {
            Check(model, "model");
            StorageEngine = model;
        }

        protected void Check<T>(T item, string paramName)
            where T : class
        {
            SanityChecks.CheckNull(item, $"ctor + [{nameof(ConfigurationContext)}]", paramName);
        }
    }
}