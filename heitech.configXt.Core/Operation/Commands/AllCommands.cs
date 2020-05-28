using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Operation;

namespace heitech.configXt.Core.Commands
{
    internal static class AllCommands
    {
       
        private static ConfigEntity GenerateConfigEntityFromChangeRequest(ConfigChangeRequest change)
        {
            return new ConfigEntity()
            {
                Id = Guid.NewGuid(),
                Name = change.Name,
                Value = change.Value
            };
        }

        #region Create
        internal static async Task<Result> CreateAsync(CommandContext context)
        {
            var admin = await context.GetAdminEntityAsync();
            if (admin == null)
            {
                SanityChecks.NotFound(context.AdminName, $"{nameof(AllCommands)}.{nameof(CreateAsync)}");
            }
            var entity = GenerateConfigEntityFromChangeRequest(context.ChangeRequest);
            bool isAllowed = Sentinel.IsAllowed(context.CommandType, admin.Claims);

            if (isAllowed == false)
            {
                SanityChecks.NotAllowed(new List<string> { ConfigClaim.CAN_CREATE }, CommandTypes.Create.ToString(), $"{nameof(AllCommands)}.{nameof(CreateAsync)}");
            }

            bool success = false;
            entity.CrudOperationName = CommandTypes.Create;
            success = await context.StorageEngine.StoreEntityAsync(entity);
            
            if (!success)
            {
                SanityChecks.StorageFailed<ConfigEntity>(context.CommandType.ToString(), $"{nameof(AllCommands)}.{nameof(CreateAsync)}");
            }

            return new Result
            {
                Before = new ConfigEntity(),
                RequestType = CommandTypes.Create.ToString(),
                Success = success,
                Current = entity
            }; 
        }
        #endregion

        #region Update
        internal static async Task<Result> UpdateAsync(CommandContext context)
        {
            string methName = $"{nameof(AllCommands)}.{nameof(CreateAsync)}";
            // find config entity
            var config = await context.GetConfigEntityAsync();
            if (config == null)
            {
                SanityChecks.NotFound(context.ConfigName, methName);
            }
            // find admin
            var admin = await context.GetAdminEntityAsync();
            if (admin == null)
            {
                SanityChecks.NotFound(context.AdminName, methName);
            }
            // check is allowed
            Sentinel.IsAllowed(context.CommandType, admin.Claims);
            // update
            config.CrudOperationName = CommandTypes.UpdateValue;
            var before = new ConfigEntity
            {
                Id = config.Id,
                Name = new string(config.Name.ToCharArray()),
                Value = new string(config.Value.ToCharArray())
            };
            config.Value = context.ChangeRequest.Value;
            // call storage
            bool success = await context.StorageEngine.StoreEntityAsync(config);
            if (!success)
            {
                SanityChecks.StorageFailed<ConfigEntity>(context.CommandType.ToString(), methName);
            }

            // return result
            return new Result
            {
                Before = before,
                Current = config,
                Success = success,
                RequestType = context.CommandType.ToString()
            };
        }
        #endregion

        internal static  Task<Result> UpdateRights(CommandContext context)
        {
            return Task.FromResult<Result>(null);
        }

        internal static  Task<Result> Delete(CommandContext context)
        {
            return Task.FromResult<Result>(null);
        }
    }
}