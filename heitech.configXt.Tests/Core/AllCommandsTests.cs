using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using heitech.configXt.Core.Entities;
using NSubstitute;
using Xunit;

namespace heitech.configXt.Tests.Core
{
    public class AllCommandsTests
    {
        IStorageModel model = Substitute.For<IStorageModel>();

        #region general
        [Theory]
        [InlineData(CommandTypes.Create, CommandTypes.Delete)]
        [InlineData(CommandTypes.Delete, CommandTypes.Create)]
        [InlineData(CommandTypes.UpdateValue, CommandTypes.Delete)]
        public async Task ThrowsOnWrongArgumentType(CommandTypes expected, CommandTypes incoming)
        {
            var context = new CommandContext(incoming, new ConfigChangeRequest(){Name = "name", Value = "value"}, model);
            if (expected == CommandTypes.Create)
            {
               await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.CreateAsync(context));
            }
            else if (expected == CommandTypes.Delete)
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.DeleteAsync(context));
            }
            else if (expected == CommandTypes.UpdateValue)
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.UpdateAsync(context));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        [Fact]
        public async Task ThrowsIfcontextIsNull()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.CreateAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.DeleteAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(async () => await AllCommands.UpdateAsync(null));
        }
        #endregion

        #region Create 
        [Fact]
        public async Task CreateAsyncStorageFailsReturnsStorageFailedOperationResult()
        {
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.Create, request, model);

            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(false);
            var result = await AllCommands.CreateAsync(context);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.Forbidden, result.ResultType);
        }

        [Fact]
        public async Task CreateAsyncHappyPath()
        {
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.Create, request, model);

            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(true);
            var result = await AllCommands.CreateAsync(context);

            Assert.True(result.IsSuccess);
            Assert.Equal("name", result.Result.Name);
            Assert.Equal("value", result.Result.Value);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteAsyncConfigEntityNotFound()
        {
            var request = new ConfigChangeRequest { Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.Delete, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };

            model.GetEntityByNameAsync("name").Returns((ConfigEntity)null);

            var result = await AllCommands.DeleteAsync(context);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.NotFound, result.ResultType);
        }

        [Fact]
        public async Task DeleteAsyncConfigEntityNotStored()
        {
            
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.Delete, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };

            model.GetEntityByNameAsync("name").Returns(expected);
            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(false);

            var result = await AllCommands.DeleteAsync(context);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.Forbidden, result.ResultType);
        }

        [Fact]
        public async Task DeleteAsyncHappyPath()
        {
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.Delete, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };
            model.GetEntityByNameAsync("name").Returns(expected);
            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(true);
            var result = await AllCommands.DeleteAsync(context);

            await model.Received().StoreEntityAsync(Arg.Any<ConfigEntity>());
            Assert.True(result.IsSuccess);
            Assert.Equal("name", result.Result.Name);
            Assert.Equal("old", result.Result.Value);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateAsyncConfigEntityNotFound()
        {
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.UpdateValue, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };

            model.GetEntityByNameAsync("name").Returns((ConfigEntity)null);

            var result = await AllCommands.UpdateAsync(context);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.NotFound, result.ResultType);
        }

        [Fact]
        public async Task UpdateAsyncConfigEntityNotStored()
        {
            
            var request = new ConfigChangeRequest{Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.UpdateValue, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };

            model.GetEntityByNameAsync("name").Returns(expected);
            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(false);

            var result = await AllCommands.UpdateAsync(context);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.Equal(ResultType.Forbidden, result.ResultType);
        }

        [Fact]
        public async Task UpdateAsyncHappyPath()
        {
            var request = new ConfigChangeRequest { Name = "name", Value = "value"};
            var context = new CommandContext(CommandTypes.UpdateValue, request, model);
            var expected = new ConfigEntity { Name = "name", Value = "old" };
            model.GetEntityByNameAsync("name").Returns(expected);
            model.StoreEntityAsync(Arg.Any<ConfigEntity>()).Returns(true);
            var result = await AllCommands.UpdateAsync(context);

            await model.Received().StoreEntityAsync(Arg.Any<ConfigEntity>());
            Assert.True(result.IsSuccess);
            Assert.Equal("name", result.Result.Name);
            Assert.Equal("value", result.Result.Value);
        }
        #endregion
    }
}