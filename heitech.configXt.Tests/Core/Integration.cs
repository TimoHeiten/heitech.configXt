using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;
using heitech.configXt.Models;
using Xunit;

namespace heitech.configXt.Tests.Core
{
    public class Integration
    {
        private class StoreStub : IStorageModel
        {
            public ConfigEntity _entity;
            public Task<IEnumerable<ConfigEntity>> AllEntitesAsync()
            {
                if (_entity != null)
                {
                    return Task.FromResult<IEnumerable<ConfigEntity>>(new List<ConfigEntity> { _entity }); 
                } 
                else
                {
                    return Task.FromResult(Enumerable.Empty<ConfigEntity>());
                }
            }

            public Task<ConfigEntity> GetEntityByNameAsync(string byName)
            {
                return _entity != null && _entity.Name == byName
                       ? Task.FromResult(_entity)
                       : Task.FromResult<ConfigEntity>(null);
            }

            public Task<bool> IsAllowedReadAsync(AuthModel authModel, string appName)
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsAllowedWriteAsync(AuthModel authModel, string appName)
            {
                throw new NotImplementedException();
            }

            public Task<bool> StoreEntityAsync(ConfigEntity entity)
            {
                if (entity.CrudOperationName == CommandTypes.Create)
                    _entity = entity;
                else if (entity.CrudOperationName == CommandTypes.Delete)
                    _entity = null;
                else if (entity.CrudOperationName == CommandTypes.UpdateValue)
                {
                    _entity.Value = entity.Value;
                }
                return Task.FromResult(true);
            }
        }

        [Fact]
        public async Task IntegrateHappyPathCreateQueryUpdateQueryDeleteQueryAll()
        {
            // stub and initial code
            var model = new StoreStub();
            string name = "config-name";
            string startValue = "starting-value";

            // create it
            var createContext = new CommandContext(CommandTypes.Create, new ConfigChangeRequest { Name = name, Value = startValue}, model);
            Assert.Null(model._entity);
            var result = await Factory.RunOperationAsync(createContext);
            Assert.True(result.IsSuccess);
            Assert.NotNull(model._entity);
            result = null;

            // query it 
            Func<string, Task> queryAsync = async (expected) => 
            {
                var queryContext = new QueryContext(name, QueryTypes.ValueRequest, model);
                result = await Factory.RunOperationAsync(queryContext);
                Assert.True(result.IsSuccess);
                Assert.Equal(name, result.Result.Name);
                Assert.Equal(expected, result.Result.Value);
                result = null;
            };
            await queryAsync(startValue);

            // update it
            string updated = "updated";
            var update = new CommandContext(CommandTypes.UpdateValue, new ConfigChangeRequest { Name = name, Value = updated}, model);
            result = await Factory.RunOperationAsync(update);
            Assert.True(result.IsSuccess);
            Assert.Equal(updated, result.Result.Value);

            // query again
            await queryAsync(updated);

            // delete
            var delete = new CommandContext(CommandTypes.Delete, new ConfigChangeRequest { Name = name, Value = "does not matter" }, model);
            result = await Factory.RunOperationAsync(delete);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            // last query reveals null
            var queryContext2 = new QueryContext(name, QueryTypes.ValueRequest, model);
            result = await Factory.RunOperationAsync(queryContext2);
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultType.NotFound, result.ResultType);
        }
    }
}