using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

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
        internal static async Task<OperationResult> CreateAsync(CommandContext context)
        {
            var entity = GenerateConfigEntityFromChangeRequest(context.ChangeRequest);

            bool success = false;
            entity.CrudOperationName = CommandTypes.Create;
            success = await context.StorageEngine.StoreEntityAsync(entity);

            if (!success)
            {
                return SanityChecks.StorageFailed<ConfigEntity>(context.CommandType.ToString(), $"{nameof(AllCommands)}.{nameof(CreateAsync)}");
            }

            return OperationResult.Success(entity);
        }
        #endregion

        #region Update
        internal static async Task<OperationResult> UpdateAsync(CommandContext context)
        {
            string methName = $"{nameof(AllCommands)}.{nameof(UpdateAsync)}";
            var configEntity = await TryExtractConfigEntityAsync(methName, context, CommandTypes.UpdateValue, c => { c.Value = context.ChangeRequest.Value; return c; });
            if (configEntity.Success == false)
            {
                return configEntity.ThrowError();
            }
            // return result
            return OperationResult.Success(configEntity.Entity);
        }
        #endregion

        internal static async Task<OperationResult> DeleteAsync(CommandContext context)
        {
            string methName = $"{nameof(AllCommands)}.{nameof(DeleteAsync)}";
            var configEntityResult = await TryExtractConfigEntityAsync(methName, context, CommandTypes.Delete, c => c);

            if (configEntityResult.Success == false)
            {
                return configEntityResult.ThrowError();
            }

            // return result
            return OperationResult.Success(configEntityResult.Entity);
        }

        private static async Task<ConfigEntityResult> TryExtractConfigEntityAsync(string initiatingMethod, CommandContext context, CommandTypes storeType, Func<ConfigEntity, ConfigEntity> adjustEntity)
        {
            // get config entity if exists
            var config = await context.StorageEngine.GetEntityByNameAsync(context.ConfigName);
            if (config == null)
            {
                return new ConfigEntityResult
                {
                    Success = false,
                    ThrowError = () => SanityChecks.NotFound(context.ConfigName, initiatingMethod)
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
            public Func<OperationResult> ThrowError { get; set; }
            public ConfigEntity Entity { get; set; }
            public ConfigEntity Before { get; set; }
        }
    }
}