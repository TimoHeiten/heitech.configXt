using System;
using System.Linq;
using heitech.configXt.Application;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using Xunit;

namespace heitech.configXt.Tests.Application
{
    public class TransformBackAndForth
    {
        [Fact]
        public void LoadJsonAndTransformBack()
        {
            string json = @"
            {
                ""Simple"" : ""Value"",
                 ""RabbitMQ"" : {
                    ""Host"" : ""localhost"",
                    ""Port"" : ""5672""
                },
                ""Connections"" : [
                    1, 2, 3, 4]
            }
            ";

            var transform = new JsonTransform();

            var result = transform.Transform(json);
            var collection = (result.Result as ConfigCollection).WrappedConfigEntities;

            OperationResult opResult = transform.ToJson(collection);
            var jsonEntity = opResult.Result as ConfigEntityJson;
            Assert.NotNull(jsonEntity);

            var token = jsonEntity.Json;
            Assert.Equal(token["Simple"], "Value");
            Assert.Equal(token["RabbitMQ:Host"], "localhost");
            Assert.Equal(token["RabbitMQ:Port"], "5672");
            Assert.Equal(token["Connections"], "1,2,3,4");
        }
    }
}