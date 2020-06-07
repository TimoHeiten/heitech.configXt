using System;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Application;
using Xunit;
using NSubstitute;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using System.Collections.Generic;

namespace heitech.configXt.Tests.Application
{
    public class TransformTests
    {
        IStorageModel _model = Substitute.For<IStorageModel>();
        [Fact]
        public void LoadJsonAsyncSimpleOnes()
        {
            string json = @"
            {
                ""Simple"" : ""Value"",
                ""RabbitMQ"" : ""simple-2""
            }
            ";

            var transform = new JsonTransform(_model);

            var result = transform.Transform(json);
            var collection = (result.Result as ConfigCollection).WrappedConfigEntities;

            Assertion(collection, "Simple", "Value", 0);
            Assertion(collection, "RabbitMQ", "simple-2", 1);
            System.Console.WriteLine();
        }

        [Fact]
        public void LoadJsonAsyncWithObject()
        {
            string json = @"
            {
                ""Simple"" : ""Value"",
                ""RabbitMQ"" : {
                    ""Host"" : ""localhost"",
                    ""Port"" : ""5672""
                }
            }
            ";

            var transform = new JsonTransform(_model);

            var result = transform.Transform(json);
            var collection = (result.Result as ConfigCollection).WrappedConfigEntities;

            Assertion(collection, "Simple", "Value", 0);
            Assertion(collection, "RabbitMQ:Host", "localhost", 1);
            Assertion(collection, "RabbitMQ:Port", "5672", 2);
            System.Console.WriteLine();
        }

        [Fact]
        public void LoadJsonAsyncWithObjectAndArray()
        {
            string json = @"
            {
                ""Simple"" : ""Value"",
                 ""RabbitMQ"" : {
                    ""Host"" : ""localhost"",
                    ""Port"" : ""5672""
                },
                ""Connections"" : [
                    1, 2, 3, 4
                ]
            }
            ";

            var transform = new JsonTransform(_model);

            var result = transform.Transform(json);
            var collection = (result.Result as ConfigCollection).WrappedConfigEntities;

            Assertion(collection, "Simple", "Value", 0);
            Assertion(collection, "RabbitMQ:Host", "localhost", 1);
            Assertion(collection, "RabbitMQ:Port", "5672", 2);
            Assertion(collection, "Connections", "1,2,3,4", 3);
        }

        [Fact]
        public void NestedObjectAndArray()
        {
            string json = @"
            {
                ""Object"" : {
                    ""Nested"" : {
                        ""Array"" : [
                            1,2,3,4
                        ],
                        ""Key"" : ""Value""
                    },
                    ""Nested2"" : {
                        ""Nested3"" : {
                            ""N-Key"" : ""N-Value""
                        }
                    }
                }
            }
            ";

            var transform = new JsonTransform(_model);

            var result = transform.Transform(json);
            var collection = (result.Result as ConfigCollection).WrappedConfigEntities;

            Assertion(collection, "Object:Nested:Array", "1,2,3,4", 0);
            Assertion(collection, "Object:Nested:Key", "Value", 1);
            Assertion(collection, "Object:Nested2:Nested3:N-Key", "N-Value", 2);
        }

        private void Assertion(IEnumerable<ConfigEntity> collection, string key, string value, int at)
        {
             Assert.Equal(key, collection.ElementAt(at).Name);
                Assert.Equal(value, collection.ElementAt(at).Value);
        }

    }
}