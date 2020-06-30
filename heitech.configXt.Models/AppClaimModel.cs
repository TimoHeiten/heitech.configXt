using System;
using System.Linq;

namespace heitech.configXt.Models
{
    ///<summary>
    /// DTO for the Application Claim
    ///</summary>
    public class AppClaimModel
    {
        public string ApplicationName { get; set; }
        public string ConfigEntitiyKey { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
}