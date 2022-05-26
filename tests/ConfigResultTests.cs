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
    }
}