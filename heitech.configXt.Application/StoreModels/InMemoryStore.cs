using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;

namespace heitech.configXt.Application
{
    public class InMemoryStore : IStorageModel, IAuthStorageModel
    {
        protected readonly Dictionary<string, ConfigEntity> _entities = new Dictionary<string, ConfigEntity>();
        protected readonly List<UserEntity> _models = new List<UserEntity>();
        public Task<IEnumerable<ConfigEntity>> AllEntitesAsync()
        {
            return Task.FromResult<IEnumerable<ConfigEntity>>
            (
                _entities.Select(x => x.Value).ToList()
            );
        }

        public InMemoryStore(string name, string passwordHash, string appName)
        {
            _models = new List<UserEntity>
            {
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    PasswordHash = passwordHash,
                    Claims = new List<ApplicationClaim>
                    {
                        new ApplicationClaim
                        {
                            Id = Guid.NewGuid(),
                            Name = appName,
                            CanRead = true,
                            CanWrite = true
                        }
                    }
                }
            };
        }

        public Task<ConfigEntity> GetEntityByNameAsync(string byName)
        {
            if (_entities.TryGetValue(byName, out ConfigEntity entity))
                return Task.FromResult(entity);
            else
                return Task.FromResult<ConfigEntity>(null);
        }

        public Task<bool> IsAllowedReadAsync(AuthModel authModel, string appName)
            => Task.FromResult
            (
                CheckAppClaimFromUser
                (
                    authModel, 
                    appName, 
                    claim => claim.CanRead
                )
            );
        public Task<bool> IsAllowedWriteAsync(AuthModel authModel, string appName)
            => Task.FromResult
            (
                CheckAppClaimFromUser
                (
                    authModel, 
                    appName, 
                    claim => claim.CanWrite
                )
            );

        private bool CheckAppClaimFromUser(AuthModel authModel, string appName, Predicate<ApplicationClaim> claim)
        {
            bool result = false;
            bool hasAuthModel = _models.Any
            (
                x => x.Name == authModel.Name 
                    && x.PasswordHash == authModel.PasswordHash
            );
            if (hasAuthModel)
            {
                result = _models.First(x => x.Name == authModel.Name)
                                .Claims
                                .Any(x => x.Name == appName && claim(x));
            }
            return result;
        }

        public Task<bool> StoreEntityAsync(ConfigEntity entity)
        {
            Func<bool, Task<bool>> result = b => Task.FromResult(b);
            switch (entity.CrudOperationName)
            {
                case CommandTypes.Create:
                    if (!_entities.ContainsKey(entity.Name))
                    {
                        _entities.Add(entity.Name, entity);
                        return result(true);
                    }
                    else
                    {
                        return result(false);
                    }
                case CommandTypes.UpdateValue:
                    if (_entities.TryGetValue(entity.Name, out ConfigEntity e))
                    {
                        e.Value = entity.Value;
                        return result(true);
                    }
                    else
                    {
                        return result(false);
                    }
                case CommandTypes.Delete:
                    if (_entities.TryGetValue(entity.Name, out ConfigEntity eDelete))
                    {
                        _entities.Remove(entity.Name);
                        return result(true);
                    }
                    else
                    {
                        return result(false);
                    }
                default:
                    throw new NotSupportedException(entity.CrudOperationName + " was not found...");
            }
        }

        public Task StoreUserAsync(AuthModel model, ApplicationClaim[] claims)
        {
            var entity = _models.FirstOrDefault(x => x.Name == model.Name);
            if (entity == null)
            {
                _models.Add
                (
                    new UserEntity
                    {
                        Name = model.Name,
                        Id = Guid.NewGuid(),
                        Claims = claims.ToList(),
                        PasswordHash = model.PasswordHash
                    }
                );
            }
            else 
            {
                entity.PasswordHash = model.PasswordHash;
                entity.Claims.AddRange(claims);
            }
            return Task.CompletedTask;
        }

        public Task<bool> UserExistsAsync(AuthModel model)
        {
            bool result = _models.Any(x => x.Name == model.Name && model.PasswordHash == x.PasswordHash);
            return Task.FromResult(result);
        }

        public Task<UserEntity> GetUserAsync(AuthModel model)
        {
            var user = _models.FirstOrDefault(x => x.Name == model.Name && x.PasswordHash == model.PasswordHash);

            return Task.FromResult(user);
        }

        public async Task<bool> DeleteUserAsync(AuthModel model)
        {
            var user = await GetUserAsync(model);
            if (user != null)
            {
                _models.RemoveAll(x => x.Name == model.Name);
                return true;
            }
            else return false;
        }
    }
}