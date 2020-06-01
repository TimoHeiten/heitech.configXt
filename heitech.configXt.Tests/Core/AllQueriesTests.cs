using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using heitech.configXt.Core.Queries;
using NSubstitute;
using Xunit;

namespace heitech.configXt.Tests.Core
{
    public class AllQueriesTests
    {
        IStorageModel model = Substitute.For<IStorageModel>();

        [Fact]
        public async Task NullContextThrows()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllQueries.QueryConfigEntityAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllQueries.QueryAllConfigEntityValuesAsync(null));
        }


        [Fact]
        public async Task QuerySingleEntityAsyncReturnsNotFoundIfconfigNameDoesNotExist()
        {
            string name = "config-name";
            model.GetEntityByNameAsync(name).Returns((ConfigEntity)null);
            var context = new QueryContext(name, QueryTypes.ValueRequest, model);

            var result = await AllQueries.QueryConfigEntityAsync(context);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.NotFound, result.ResultType);
        }

        [Fact]
        public async Task QuerySingleEntityWithWrongQueryTypeThrowsArgumentException()
        {
            var context = new QueryContext("does-not-matter", QueryTypes.AllValues, model);
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllQueries.QueryConfigEntityAsync(context));
        }

        [Fact]
        public async Task QuerySingleHappyPath()
        {
            string name = "config-name";
            var entity = new ConfigEntity();
            model.GetEntityByNameAsync(name).Returns(entity);

            var context = new QueryContext(name, QueryTypes.ValueRequest, model);
            var result = await AllQueries.QueryConfigEntityAsync(context);

            Assert.Same(entity, result.Result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task QueryAllThrowsOnWrongQueryType()
        {
            var context = new QueryContext("does-not-matter", QueryTypes.ValueRequest, model);
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllQueries.QueryAllConfigEntityValuesAsync(context));
        }

        [Fact]
        public async Task QueryAllNotFoundIfEmpty()
        {
            string name = "does-not-matter";
            model.AllEntitesAsync().Returns((IEnumerable<ConfigEntity>)null);
            var context = new QueryContext(name, QueryTypes.ValueRequest, model);

            var result = await AllQueries.QueryConfigEntityAsync(context);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.NotFound, result.ResultType);
        }

        [Fact]
        public async Task QueryAllHappyPath()
        {
            string name = "config-name";
            var entity = new ConfigEntity();
            var entities = new List<ConfigEntity> { entity };
            model.AllEntitesAsync().Returns((IEnumerable<ConfigEntity>)entities);

            var context = new QueryContext(name, QueryTypes.AllValues, model);
            var result = await AllQueries.QueryAllConfigEntityValuesAsync(context);

            Assert.IsType<ConfigCollection>(result.Result);
            Assert.Same(entity, ((ConfigCollection)(result.Result)).WrappedConfigEntities.First());
            Assert.True(result.IsSuccess);
        }
    }
}