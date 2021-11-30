using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace heitech.configXt.tests
{
    public class ServiceTests
    {
        private const string KEY = "key";

        private readonly IService _sut;
        private readonly IStore _store;
        private readonly ConfigModel _model1;

        public ServiceTests()
        {
            _model1 = ConfigModel.From(KEY, "value");
            _store = Substitute.For<IStore>();
            _sut = Entry.CreateService(_store);
        }

        #region Create Tests
        [Fact]
        public async Task CreateAsync_once_returns_Success()
        {
            // Act
            var result = await _sut.CreateAsync(_model1);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_adds_calls_Flush()
        {
            // Act
            var result = await _sut.CreateAsync(_model1);

            // Assert
            await _store.Received(1).FlushAsync(
                Arg.Is<Dictionary<string, ConfigModel>>(d => d.ContainsKey(KEY))
            );
        }

        [Fact]
        public async Task CreateWithSameKeyTwice_ReturnsFailure()
        {
            // Arrange
            _ = await _sut.CreateAsync(_model1);

            // Act
            var result = await _sut.CreateAsync(_model1);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.GetType().Name.Should().Be("CreateException");
        }

        [Fact]
        public async Task Create_Actually_puts_a_new_item()
        {
            // Act
            var result = await _sut.CreateAsync(_model1);
            var retrieved = await _sut.RetrieveAsync(_model1.Key);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _ = result.TryGetAs(out string actual);
            actual.Should().Be("value");

            retrieved.IsSuccess.Should().BeTrue();
            retrieved.TryGetAs(out actual);
            actual.Should().Be("value");

        }
        #endregion


        #region Retrieve
        private async Task ArrangeExistingItem()
        {
            await _sut.CreateAsync(_model1);
            _store.ClearReceivedCalls();
        }

        [Fact]
        public async Task Retrieve_not_Existing_returns_false()
        {
            // Arrange
            // nothing

            // Act
            var result = await _sut.RetrieveAsync("no-key");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.GetType().Name.Should().Be("RetrieveException");
        }

        [Fact]
        public async Task Retrieve_existing_returns_true()
        {
            // Arrange
            await ArrangeExistingItem();

            // Act
            var result = await _sut.RetrieveAsync(KEY);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData(KEY)]
        [InlineData("no-key")]
        public async Task Retrieve_does_not_call_Flush(string key)
        {
            // Arrange
            await ArrangeExistingItem();

            // Act
            _ = await _sut.RetrieveAsync(key);

            // Assert
            await _store.DidNotReceive().FlushAsync(Arg.Any<Dictionary<string, ConfigModel>>());
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_on_existing_item_returns_success_Result()
        {
            // Arrange
            await ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "value");

            // Act
            var result = await _sut.UpdateAsync(model);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _store.Received().FlushAsync(Arg.Any<Dictionary<string, ConfigModel>>());
        }

        [Fact]
        public Task Update_on_non_existing_item_returns_failed_Result()
            => RunMutationWithoutExistingKey(() => _sut.UpdateAsync(ConfigModel.From(KEY, "value")), "UpdateException");

        [Fact]
        public async Task Update_Actually_Updates()
        {
            // Arrange
            await ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "updated");
            _store.ClearReceivedCalls();

            // Act
            var result = await _sut.UpdateAsync(model);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _ = result.TryGetAs(out string actual);
            actual.Should().Be("updated");
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_on_existing_item_returns_success_Result()
        {
            // Arrange
            await ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "value");
            _store.ClearReceivedCalls();

            // Act
            var result = await _sut.DeleteAsync(KEY);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _store.Received().FlushAsync(Arg.Any<Dictionary<string, ConfigModel>>());
        }

        [Fact]
        public Task Delete_on_non_existing_item_returns_failed_Result()
            => RunMutationWithoutExistingKey(() => _sut.DeleteAsync(KEY), "DeleteException");

        [Fact]
        public async Task Delete_success_and_retrieve_then_fails()
        {
            // Arrange
            await ArrangeExistingItem();
            _store.ClearReceivedCalls();
            _ = _sut.DeleteAsync(KEY);

            // Act
            var retrieveResult = await _sut.RetrieveAsync(KEY);

            // Assert
            retrieveResult.IsSuccess.Should().BeFalse();
        }
        #endregion


        private async Task RunMutationWithoutExistingKey(Func<Task<ConfigResult>> callback, string excName)
        {
            // Arrange
            // Act
            var result = await callback();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.GetType().Name.Should().Be(excName);
            await _store.DidNotReceive().FlushAsync(Arg.Any<Dictionary<string, ConfigModel>>());
        }
    }
}
