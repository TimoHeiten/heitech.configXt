using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;
using Microsoft.EntityFrameworkCore;

namespace heitech.configXt.Application
{
    public class EfStore : IStorageModel, IAuthStorageModel
    {
        private readonly string _connection;
        public EfStore(string connection)
        {
            _connection = connection;
        }

        public Task InitAsync()
        {
            using (var c = CreateNewContext())
            {
                return c.Database.MigrateAsync();
            }
        }

        private PersistContext CreateNewContext()
        {
            return new PersistContext()
            {
                Connection = _connection
            };
        }

        #region  Store Model
        public async Task<IEnumerable<ConfigEntity>> AllEntitesAsync()
        {
            var result = new List<ConfigEntity>();
            using (var context = CreateNewContext())
            {
                await context.Set<ConfigEntity>().ToListAsync();
            }
            return result;
        }

        public async Task<bool> StoreEntityAsync(ConfigEntity entity)
        {
            using (var context = CreateNewContext())
            {
                var entities = context.Set<ConfigEntity>();
                switch (entity.CrudOperationName)
                {
                    case CommandTypes.Create:
                        var exists = await entities.FirstOrDefaultAsync(x => x.Name == entity.Name);
                        if (exists == null)
                        {
                            context.Add(entity);
                            await context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                             return  false;
                        }
                    case CommandTypes.UpdateValue:
                        var update = await entities.Include(x => x.AppClaim).FirstOrDefaultAsync(x => x.Name == entity.Name);
                        if (update == null)
                        {
                            return false;
                        }
                        else
                        {
                            update.AppClaim = entity.AppClaim ?? update.AppClaim;
                            update.Value = entity.Name;

                            context.Update(update);
                            await context.SaveChangesAsync();
                            return true;
                        }
                    case CommandTypes.Delete:
                        var delete = await entities.FirstOrDefaultAsync(x => x.Name == entity.Name);
                        if (delete == null)
                        {
                             return  false;
                        }
                        else
                        {
                            context.Remove(delete);
                            await context.SaveChangesAsync();
                            return true;
                        }
                    default:
                        return false;
                }
            }
        }

        public Task<ConfigEntity> GetEntityByNameAsync(string byName)
        {
            using (var c = CreateNewContext())
            {
                return c.Set<ConfigEntity>()
                        .Include(x => x.AppClaim)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(x => x.Name == byName);
            }
        }
        #endregion

        #region Auth Storage
        public async Task<bool> DeleteUserAsync(AuthModel model)
        {
            bool deleted = false;
            using (var context = CreateNewContext())
            {
                var exist = await FindAsync(context, model);
                if (exist != null)
                {
                    context.Remove(exist);
                    await context.SaveChangesAsync();
                }
            }

            return deleted;
        }

        public async Task<UserEntity> GetUserAsync(AuthModel model)
        {
            using (var context = CreateNewContext())
            {
                return await FindAsync(context, model);
            };
        }

        private async Task<UserEntity> FindAsync(PersistContext current, AuthModel model)
        {
            var users = current.Set<UserEntity>();
            var exist = await users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.Name == model.Name && x.PasswordHash == model.PasswordHash);
            return exist;
        }

        public async Task<bool> IsAllowedReadAsync(AuthModel authModel, string appName)
        {
            using (var current = CreateNewContext())
            {
                var exist = await FindAsync(current, authModel);
                if (exist != null)
                {
                    return exist.Claims.Any(x => x.Name == appName && x.CanRead);
                }
            }

            return false;
        }

        public async Task<bool> IsAllowedWriteAsync(AuthModel authModel, string appName)
        {
            using (var current = CreateNewContext())
            {
                var exist = await FindAsync(current, authModel);
                if (exist != null)
                {
                    return exist.Claims.Any(x => x.Name == appName && x.CanWrite);
                }
            }

            return false;
        }

        public async Task<bool> UserExistsAsync(AuthModel model)
        {
            using (var context = CreateNewContext())
            {
                var exist = await FindAsync(context, model);
                return exist != null;
            }
        }

        public async Task StoreUserAsync(AuthModel model, ApplicationClaim[] claims)
        {
            using (var context = CreateNewContext())
            {
                var exist = await context.Set<UserEntity>().Include(x => x.Claims).FirstOrDefaultAsync(x => x.Name == model.Name); // w/o password so it can be changed
                var entities = await context.Set<ConfigEntity>().ToListAsync();
                if (exist != null) // update
                {
                    var existingClaims = claims.Where
                    (
                        x => 
                        {
                            bool doesNotExistYet = false == exist.Claims.Any(ex => ex.ConfigEntityName == x.ConfigEntityName);
                            bool configNameExists = entities.Any(e => e.Name == x.ConfigEntityName);

                            return doesNotExistYet && configNameExists;
                        }
                            
                    );
                    exist.Claims.AddRange(existingClaims);
                    exist.PasswordHash = model.PasswordHash;

                    context.Update(exist);
                    await context.SaveChangesAsync();
                }
                else // create
                {
                    // where claim.configNames exist
                    var existingClaims = claims.Where(x => entities.Any(e => e.Name == x.ConfigEntityName));
                    var claim = new UserEntity
                    {
                        Name = model.Name,
                        PasswordHash = model.PasswordHash,

                        Claims = existingClaims.ToList()
                    };

                    context.Add(claim);
                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion
    }
}