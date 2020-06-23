using System;
using System.Collections.Generic;

namespace heitech.configXt.Core.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public List<ApplicationClaim> Claims { get; set; }
    }
}