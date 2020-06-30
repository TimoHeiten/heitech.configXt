using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public class UserContext : ConfigurationContext
    {
        public IAuthStorageModel AuthStorage { get; }
        ///<summary>
        /// The AuthModel for the User
        ///</summary>
        public AuthModel AuthModel { get; set; }
        ///<summary>
        /// The claims associated with the user (only necessary for create or update)
        ///</summary>
        public ApplicationClaim[] Claims { get; set; }
        ///<summary>
        /// Commandtype, or if null query
        ///</summary>
        public CommandTypes? Type { get; set; } // null means Query
        public UserContext(AuthModel user, IStorageModel model, CommandTypes? type, IAuthStorageModel authStorage, params ApplicationClaim[] claims) 
            : base(model)
        {
            string msg = $"ctor of UserContext for [{type.ToString()}]";
            SanityChecks.CheckNull(model, msg);
            SanityChecks.CheckNull(user, msg);
            SanityChecks.CheckNull(authStorage, msg);

            Type = type;
            Claims = claims;
            AuthModel = user;
            AuthStorage = authStorage;
        }
    }
}