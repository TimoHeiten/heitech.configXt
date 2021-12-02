using System;
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
        private readonly IRepository _repository;

        public ServiceTests()
        {
            _model1 = ConfigModel.From(KEY, "value");
            _store = Substitute.For<IStore>();
            _repository = Substitute.For<IRepository>();
            _sut = new DbService(_store, _repository);
        }

        #region Create Tests
        [Fact]
        public async Task CreateAsync_once_returns_Success()
        {
            // Act
            var result = await _sut.CreateAsync(_model1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repository.Received().Add(Arg.Is<ConfigModel>(x => x.Value == _model1.Value));
            await _store.Received().FlushAsync();
        }

        [Fact]
        public async Task Create_Repository_throws_Returns_Failure()
        {
            // Arrange
            _repository.When(x => x.Add(Arg.Any<ConfigModel>())).Do(x => throw new Exception());

            // Act
            var result = await _sut.CreateAsync(_model1);
            
            // Assert
            result.IsSuccess.Should().BeFalse();

        }
        #endregion


        #region Retrieve
        private void ArrangeExistingItem() => _repository.Get("key").Returns(_model1);

        [Fact]
        public async Task Retrieve_not_Existing_returns_false()
        {
            // Arrange
            ArrangeExistingItem();

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
            ArrangeExistingItem();

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
            ArrangeExistingItem();

            // Act
            _ = await _sut.RetrieveAsync(key);

            // Assert
            await _store.DidNotReceive().FlushAsync();
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_on_existing_item_returns_success_Result()
        {
            // Arrange
            ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "value");
            _repository.UpdateAsync(Arg.Is<ConfigModel>(x => x.Key == KEY))
                       .Returns(ConfigModel.From(KEY, "updated"));

            // Act
            var result = await _sut.UpdateAsync(model);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Result.Value.Should().Be("updated");
            await _store.Received().FlushAsync();
        }

        [Fact]
        public async Task Update_Throws_returns_FailureResult()
        {
            // Arrange
            ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "updated");
            _repository.UpdateAsync(Arg.Any<ConfigModel>()).Returns(x => { throw new Exception(); return Task.CompletedTask; });

            // Act
            var result = await _sut.UpdateAsync(model);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.GetType().Name.Should().Be("Exception");
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_on_existing_item_returns_success_Result()
        {
            // Arrange
            var model = ConfigModel.From(KEY, "value");
            _repository.DeleteAsync(KEY).Returns(model);

            // Act
            var result = await _sut.DeleteAsync(KEY);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _store.Received().FlushAsync();
        }

        [Fact]
        public async Task Delete_returns_null_leads_to_failure()
        {
             // Arrange
            var model = ConfigModel.From(KEY, "value");
            _repository.DeleteAsync(KEY).Returns((ConfigModel)null);

            // Act
            var result = await _sut.DeleteAsync(KEY);

            // Assert
            result.IsSuccess.Should().BeFalse();
            await _store.DidNotReceive().FlushAsync();
        }

        [Fact]
        public async Task Delete_Throws_returns_FailureResult()
        {
            // Arrange
            ArrangeExistingItem();
            var model = ConfigModel.From(KEY, "updated");
            _repository.DeleteAsync(KEY).Returns(x => { throw new Exception(); return Task.CompletedTask; });

            // Act
            var result = await _sut.DeleteAsync(KEY);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.GetType().Name.Should().Be("Exception");
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
            await _store.DidNotReceive().FlushAsync();
        }
    }
}
