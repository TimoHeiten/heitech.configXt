using System;
using heitech.configXt.Models;

namespace heitech.configXt.Core.Entities
{
    public class ApplicationClaim
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public ConfigEntity ConfigEntity { get; set; }
        public string ConfigEntityName { get; set; }

        public static ApplicationClaim MapFromAppClaimModel(AppClaimModel model)
        {
            return new ApplicationClaim
            {
                Id = Guid.NewGuid(),
                CanRead = model.CanRead,
                CanWrite = model.CanWrite,
                Name = model.ApplicationName,
                ConfigEntityName = model.ConfigEntitiyKey
            };
        }
    }
}