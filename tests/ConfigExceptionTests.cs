using System;
using FluentAssertions;
using Xunit;

namespace heitech.configXt.tests
{
    public class ConfigExceptionTests
    {
        [Fact]
        public void AllCrudOperations_Return_a_valid_Exception()
        {
            // Arrange
            var allEnums = Enum.GetValues(typeof(Crud));
            var model = ConfigModel.From("any", "a value");
            bool noException = true;

            // Act
            try
            {
                foreach (var item in allEnums)
                {
                    ConfigurationException.Create((Crud)item, model);
                }
            }
            catch
            {
                noException = false;
            }

            // Assert
            noException.Should().BeTrue();
        }
    }
}