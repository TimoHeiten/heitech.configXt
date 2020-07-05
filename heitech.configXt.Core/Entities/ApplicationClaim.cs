using System;
using System.Collections.Generic;
using System.Linq;
using heitech.configXt.Models;

namespace heitech.configXt.Core.Entities
{
    public class ApplicationClaim
    {
        public Guid Id { get; set; }
        ///<summary>
        /// Corresponds to the applicationName
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        /// Allows for Read
        ///</summary>
        public bool CanRead { get; set; }

        ///<summary>
        /// Allows for write
        ///</summary>
        public bool CanWrite { get; set; }

        ///<summary>
        /// The Associated ConfigEntities
        ///</summary>
        public ConfigEntity ConfigEntity { get; set; }
        private string _configName;
        public string ConfigEntityName 
        {
            get => ConfigEntity != null ? ConfigEntity.Name : _configName;
            set => _configName = value;
        }
        public UserEntity User { get; set; }

        public static ApplicationClaim MapFromAppClaimModel(AppClaimModel model)
        {
            return new ApplicationClaim
            {
                Id = Guid.NewGuid(),
                CanRead = model.CanRead,
                CanWrite = model.CanWrite,
                Name = model.ApplicationName,
                _configName = model.ConfigEntitiyKey
            };
        }
    }
}