using heitech.configXt.Core;
using Xunit;
using NSubstitute;
using heitech.configXt.Core.Queries;
using heitech.configXt.Core.Commands;
using System.Threading.Tasks;

namespace heitech.configXt.Tests.Core
{
    public class FactoryTests
    {

        private IStorageModel _model = Substitute.For<IStorageModel>();
        [Fact]
        public async Task ConfigurationContextDerivedFailsToYieldAnOperation()
        {
            var ctx = new FakeContext(_model);
            var result = await Factory.RunOperationAsync(ctx);

            Assert.False(result.IsSuccess);
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