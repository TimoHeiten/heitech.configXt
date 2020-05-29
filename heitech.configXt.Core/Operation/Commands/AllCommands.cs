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
            bool isAllowed = context.IsAllowed(null /*todo*/, admin.Claims);

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
            string methName = $"{nameof(AllCommands)}.{nameof(UpdateAsync)}";
            var configEntity = await TryExtractConfigEntityAsync(methName, context, CommandTypes.UpdateValue, c => { c.Value = context.ChangeRequest.Value; return c; });
            if (configEntity.Success == false)
            {
                configEntity.ThrowError();
            }
            // return result
            return new Result
            {
                Before = configEntity.Before,
                Current = configEntity.Entity,
                Success = configEntity.Success,
                RequestType = context.CommandType.ToString()
            };
        }
        #endregion

        internal static  Task<Result> UpdateRights(CommandContext context)
        {
            // not relevant yet.
            return Task.FromResult<Result>(new Result { Before = null, Current = null, RequestType = context.CommandType.ToString()});
        }

        internal static async Task<Result> DeleteAsync(CommandContext context)
        {
            string methName = $"{nameof(AllCommands)}.{nameof(DeleteAsync)}";
            var configEntityResult = await TryExtractConfigEntityAsync(methName, context, CommandTypes.Delete, c => c);

            if (configEntityResult.Success == false)
            {
                configEntityResult.ThrowError();
            }

            // return result
            return new Result
            {
                Before = configEntityResult.Before,
                Current = null,
                RequestType = context.CommandType.ToString(),
                Success = true
            };
        }

         private static async Task<ConfigEntityResult> TryExtractConfigEntityAsync(string initiatingMethod, CommandContext context, CommandTypes storeType, Func<ConfigEntity, ConfigEntity> adjustEntity)
        {
            // call storage with delete operation
            // get config entity if exists
            var config = await context.GetConfigEntityAsync();
            if (config == null)
            {
                return new ConfigEntityResult
                {
                    Success = false,
                    ThrowError = () => SanityChecks.NotFound(context.ConfigName, initiatingMethod)
                };
            }
            // // get admin & check claims
            var admin = await context.GetAdminEntityAsync();
            if (admin == null)
            {
                return new ConfigEntityResult
                {
                    Success = false,
                    ThrowError = () => SanityChecks.NotFound(context.AdminName, initiatingMethod)
                };
            }
            // // check is allowed
            bool isAllowed = context.IsAllowed(config, admin.Claims);
            if (!isAllowed)
            {
                return new ConfigEntityResult
                {
                    Success = false,
                    ThrowError = () => SanityChecks.NotAllowed(null /*todo with new claims implementation*/, context.CommandType.ToString(), initiatingMethod)
                };
            }
            var before = new ConfigEntity
            {
                Id = config.Id,
                Name = new string(config.Name.ToCharArray()),
                Value = new string(config.Value.ToCharArray())
            };

            config = adjustEntity(config);

            config.CrudOperationName = storeType;
            bool isSuccess = await context.StorageEngine.StoreEntityAsync(config);
            if (!isSuccess)
            {
                return new ConfigEntityResult
                {
                    ThrowError = () =>  SanityChecks.StorageFailed<ConfigEntity>(context.CommandType.ToString(), initiatingMethod),
                    Success = false
                };
            }

            return new ConfigEntityResult
            {
                Before = before,
                Entity = config,
                Success = true
            };
        }

        private class ConfigEntityResult
        {
            public bool Success { get; set; }
            public Action ThrowError { get; set; }
            public ConfigEntity Entity { get; set; }
            public ConfigEntity Before { get; set; }
        }
    }
}