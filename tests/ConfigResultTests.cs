using System;
using FluentAssertions;
using Xunit;

namespace heitech.configXt.tests
{
    public class ConfigResultTests
    {
        [Fact]
        public void ConfigResult_Failure_returns_No_Success_And_Exception()
        {
            // Arrange 
            var ex = new InvalidOperationException("not allowed");

            // Act
            var success = ConfigResult.Failure(ex);

            // Assert
            success.IsSuccess.Should().BeFalse();
            success.Result.Should().BeNull();
            success.Exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void ConfigResult_Success_returns_true_And_Result_and_no_Exception()
        {
            // Arrange & Act
            var success = ArrangeSuccess();

            // Assert
            success.IsSuccess.Should().BeTrue();
            success.Result.Should().NotBeNull();
            success.Exception.Should().BeNull();
        }

        private static ConfigResult ArrangeSuccess()
            => ConfigResult.Success(ConfigModel.From("key-1", 42));

        [Fact]
        public void TryGet_returns_true_on_correct_Type()
        {
            // Arrange
            var result = ArrangeSuccess();

            // Act
            bool isInteger = result.TryGetAs(out int i);

            // Assert
            i.Should().Be(42);
            isInteger.Should().BeTrue();
        }

        [Fact]
        public void TryGet_returns_false_on_incorrect_Type()
        {
            // Arrange
            var result = ArrangeSuccess();

            // Act
            bool onWrongType = result.TryGetAs(out string s);

            // Assert
            s.Should().BeNull();
            onWrongType.Should().BeFalse();
        }

        [Fact]
        public void TryGet_returns_false_on_failed_Result()
        {
            // Arrange
            var result = ConfigResult.Failure(new Exception("any ex"));

            // Act
            bool noTypeOnError = result.TryGetAs<int>(out int i);

            // Assert
            i.Should().Be(0);
            noTypeOnError.Should().BeFalse();
        }
    }
}