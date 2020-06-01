using System;
using Xunit;
using System.Linq;
using heitech.configXt.Core;

namespace heitech.configXt.Tests.Core
{
    public class SanityCheckTests
    {
        
        [Theory]
        [InlineData("not null", false)]
        [InlineData("", false)]
        [InlineData(null, true)]
        public void CheckNullThrowsArgumentException(string value, bool shouldThrow)
        {
            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(() => SanityChecks.CheckNull(value, "test-method"));
            }
            else
            {
                SanityChecks.CheckNull(value, "test-method");
            }
        }

        [Fact]
        public void NotSupportedReturnsOeprationResultFailure()
        {
            var operationResult = SanityChecks.NotSupported("fullname", "methodname");

            Assert.Equal(ResultType.BadRequest, operationResult.ResultType);
        }

        [Fact]
        public void NotFoundReturnsOperationResultFailure()
        {
            var operationResult = SanityChecks.NotFound("fullname", "methodname");

            Assert.Equal(ResultType.NotFound, operationResult.ResultType);
        }

        [Fact]
        public void StorageFailedThrowsOperationFailure()
        {
            var operationResult = SanityChecks.StorageFailed<string>("fullname", "methodname");

            Assert.Equal(ResultType.Forbidden, operationResult.ResultType);
        }
    }
}