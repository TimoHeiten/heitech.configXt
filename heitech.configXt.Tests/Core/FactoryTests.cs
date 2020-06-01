using System;
using System.Linq;
using heitech.configXt.Core;
using Xunit;
using NSubstitute;
using heitech.configXt.Core.Queries;
using heitech.configXt.Core.Operation;
using heitech.configXt.Core.Commands;

namespace heitech.configXt.Tests.Core
{
    public class FactoryTests
    {

        private IStorageModel _model = Substitute.For<IStorageModel>();
        [Fact]
        public void ConfigurationContextDerivedFailsToYieldAnOperation()
        {
            var ctx = new FakeContext(_model);
            var result = Factory.CreateOperation(ctx);

            Assert.IsType<RunConfigNullOperation>(result);
        }

        [Fact]
        public void QueryContextReturnsQueryConfiguration()
        {
            var ctx = new QueryContext("query", QueryTypes.AllValues, _model);
            var result = Factory.CreateOperation(ctx);

            Assert.IsType<RunQueryConfigOperation>(result);
        }

        [Fact]
        public void CommandContextReturnsCommandConfiguration()
        {
             var ctx = new CommandContext(CommandTypes.Create, new ConfigChangeRequest() { Name = "", Value = "" }, _model);
            var result = Factory.CreateOperation(ctx);

            Assert.IsType<RunCommandConfigOperation>(result);
        }

        private class FakeContext : ConfigurationContext
        {
            public FakeContext(IStorageModel model)
                : base(model)
            {
                
            }
        }
    }
}