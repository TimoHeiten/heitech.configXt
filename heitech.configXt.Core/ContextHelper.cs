using System.Linq;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;
using heitech.configXt.Models;

namespace heitech.configXt.Core
{
    public static class ContextHelper
    {
        ///<summary>
        /// Crud.Create Context
        ///</summary>
        public static ConfigurationContext CreateContext(this IStorageModel model,string _for, string _with)
            =>  _Create(CommandTypes.Create, model, (_for, _with));

        ///<summary>
        /// Crud.Update Context
        ///</summary>
        public static ConfigurationContext UpdateContext(this IStorageModel model, string _for, string _with)
            =>  _Create(CommandTypes.UpdateValue, model, (_for, _with));

        ///<summary>
        /// Crud.Delete Context
        ///</summary>
        public static ConfigurationContext DeleteContext(this IStorageModel model, string _for, string _with = "")
            =>  _Create(CommandTypes.Delete, model, (_for, _with));

        ///<summary>
        /// Crud.Delete Context
        ///</summary>
        public static ConfigurationContext QueryOne(this IStorageModel model, string _for)
        {
            return new QueryContext(_for, QueryTypes.ValueRequest, model);
        }

        ///<summary>
        /// Crud.Delete Context
        ///</summary>
        public static ConfigurationContext QueryAll(this IStorageModel model, string _for)
        {
            return new QueryContext(_for, QueryTypes.AllValues, model);
        }

        private static ConfigurationContext _Create(CommandTypes cType, IStorageModel model, (string n, string v) tuple)
        {
            var rq = new ConfigChangeRequest { Name = tuple.n, Value = tuple.v };
            return new CommandContext(cType, rq, model);
        }

        public static ConfigurationContext CreateUserContext(this IStorageModel model, AuthModel user, IAuthStorageModel authStorage, AppClaimModel[] claims)
        {
            var appClaims = claims.Select(x => ApplicationClaim.MapFromAppClaimModel(x));
            return new UserContext(user, model, CommandTypes.Create, authStorage, appClaims.ToArray());
        }

        public static ConfigurationContext UpdateUserContext(this IStorageModel model, AuthModel user, IAuthStorageModel authStorage, AppClaimModel[] claims)
        {
            var appClaims = claims.Select(x => ApplicationClaim.MapFromAppClaimModel(x));
            return new UserContext(user, model, CommandTypes.UpdateValue, authStorage, appClaims.ToArray());
        }

        public static ConfigurationContext ReadUserContext(this IStorageModel model, AuthModel user, IAuthStorageModel authStorage)
        {
            return new UserContext(user, model, null, authStorage);
        }

        public static ConfigurationContext DeleteUserContext(this IStorageModel model, AuthModel user, IAuthStorageModel authStorage)
        {
            return new UserContext(user, model, CommandTypes.Delete, authStorage);
        }
    }
}