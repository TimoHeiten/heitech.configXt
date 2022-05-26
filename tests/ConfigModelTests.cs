using System.Collections.Generic;
using FluentAssertions;
using heitech.configXt;
using heitech.configXt.Interface;
using Xunit;

namespace tests
{
    public class ConfigModelTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ConfigModel_IsValid(string key, object value, string repr, ConfigKind kind)
        {
            // Arrange
            // Act
            ConfigModel model = ConfigModel.From(key, value);

            // Assert
            model.Key.Should().Be(key);
            model.Kind.Should().Be(kind);
            model.Value.Should().BeEquivalentTo(repr);
        }

        [Fact]
        public void NonParseableValueResultsInConfigKindNone()
        {
            // Arrange
            string nonParseable = "{'kind':'value'";

            // Act
            ConfigModel m = ConfigModel.From("key", nonParseable);

            // Assert
            m.Kind.Should().Be(ConfigKind.None);
            m.Value.Should().Be("{}");
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { "1", "", "", ConfigKind.Literal };
                yield return new object[] { "3", "literal", "literal", ConfigKind.Literal };

                yield return new object[] { "2", "[1,2,3]", "[1,2,3]", ConfigKind.Array };
                yield return new object[] { "2", new[] { 1, 2, 3 }, "[1,2,3]", ConfigKind.Array };

                yield return new object[] { "4", "{\"Key\":\"Value\"}", "{\"Key\":\"Value\"}", ConfigKind.Object };
                yield return new object[] { "key 5", "{\"Key\":{\"NestedKey\":\"Value\"}}", "{\"Key\":{\"NestedKey\":\"Value\"}}", ConfigKind.Object };

                yield return new object[] { "key 4", new { Key = "abc", Value = "def" }, "{\"Key\":\"abc\",\"Value\":\"def\"}", ConfigKind.Object };
                yield return new object[]
                {
                    "key 4",
                    new { Key = "abc", Value = new { Key2 = 42 } },
                     "{\"Key\":\"abc\",\"Value\":{\"Key2\":42}}",
                    ConfigKind.Object
                };
            }
        }
    }
}