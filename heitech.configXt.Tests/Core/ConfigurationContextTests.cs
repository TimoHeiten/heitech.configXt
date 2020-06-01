using System;
using heitech.configXt.Core;
using heitech.configXt.Core.Commands;
using NSubstitute;
using Xunit;

namespace heitech.configXt.Tests.Core
{
    public class ConfigurationContextTests
    {
        IStorageModel model = Substitute.For<IStorageModel>();
        [Fact]
        public void ConfigurationCtorThrowsArgumentExceptionIfStorageIsNull()
        {
            Assert.Throws<ArgumentException>(() => new ContextTester(null));
            bool doesNotThrow = true;
            try
            {
                var _ = new ContextTester(model);
            }
            catch (System.Exception)
            {
                doesNotThrow = false;
            }
            Assert.True(doesNotThrow);
        }

        [Fact]
        public void CommandContextThrowsOnNameOrValueNull()
        {
            ConfigChangeRequest changeRequest = null;
            Assert.Throws<ArgumentException>(() => new CommandContext(CommandTypes.Create, changeRequest, model));

            changeRequest = new ConfigChangeRequest() { Name = "name"};
            Assert.Throws<ArgumentException>(() => new CommandContext(CommandTypes.Create, changeRequest, model));
            changeRequest = new ConfigChangeRequest() { Value = "name"};
            Assert.Throws<ArgumentException>(() => new CommandContext(CommandTypes.Create, changeRequest, model));
        }

        private class ContextTester : ConfigurationContext
        {
            internal ContextTester(IStorageModel model) : base(model)
            {
            }
        }
    }
}