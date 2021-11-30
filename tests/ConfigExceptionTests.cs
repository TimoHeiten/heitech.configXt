using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
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
            catch (System.Exception _)
            {
                noException = false;
            }

            // Assert
            noException.Should().BeTrue();
        }
    }
}