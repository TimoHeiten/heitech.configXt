using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Operation;

namespace heitech.configXt.Core.Commands
{
    internal static class AllCommands
    {
        private static async Task<ConfigEntity> GetConfigEntityAsync(CommandContext context)
        {
            var storage = context.StorageEngine;

            var entity = await storage.GetEntityByNameAsync<ConfigEntity>(context.ChangeRequest.Name);
            if (entity == null)
            {
                SanityChecks.NotFound(context.ChangeRequest.Name, $"{nameof(AllCommands)}.{nameof(GetConfigEntityAsync)}");
            }
            return entity;
        }

        private static async Task<AdministratorEntity> GetAdminEntityAsync(CommandContext context)
        {
            var storage = context.StorageEngine;

            var admin = await storage.GetEntityByNameAsync<AdministratorEntity>(context.AdminName);
            if (admin == null)
            {
                SanityChecks.NotFound(context.AdminName, $"{nameof(AllCommands)}.{nameof(GetAdminEntityAsync)}");
            }
            return admin;
        }

        private static bool IsAllowed(ConfigEntity entity, AdministratorEntity administrator)
        {
            bool guard = entity.NecessaryClaims.Count > administrator.Claims.Count;
            if (guard)
                return false;

            // all necessary claims need to be held by the administrator, but admin can have more
            foreach (var claim in entity.NecessaryClaims)
            {
                if (administrator.Claims.Any(x => string.Equals(x.Value, claim.Value, StringComparison.InvariantCultureIgnoreCase)) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private static ConfigEntity GenerateConfigEntityFromChangeRequest(ConfigChangeRequest change)
        {
            return new ConfigEntity()
            {
                Id = Guid.NewGuid(),
                Name = change.Name,
                Value = change.Value,
                NecessaryClaims = change.Claims
                                        .Select
                                        (
                                            x => new ConfigClaim
                                            {
                                                Name = x.Key,
                                                Value = x.Value,
                                                Id = Guid.NewGuid()
                                            }
                                        )
                                        .ToList()

            };
        }

        internal static async Task<Result> CreateAsync(CommandContext context)
        {
            var admin = await GetAdminEntityAsync(context);
            var entity = GenerateConfigEntityFromChangeRequest(context.ChangeRequest);
            var createClaim = new ConfigClaim[] { ConfigClaim.CanCreate() };
            bool isAllowed = Sentinel.IsAllowed(context.CommandType, admin.Claims, createClaim);

            if (isAllowed == false)
            {
                SanityChecks.NotAllowed(createClaim.Select(x => x.Value).ToList(), CommandTypes.Create, $"{nameof(AllCommands)}.{nameof(CreateAsync)}");
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
                Before = new ConfigEntity() { NecessaryClaims = new List<ConfigClaim>() },
                RequestType = CommandTypes.Create.ToString(),
                Success = success,
                After = entity
            }; 
        }

        internal static  Task<Result> Update(CommandContext context)
        {
            return Task.FromResult<Result>(null);
        }

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