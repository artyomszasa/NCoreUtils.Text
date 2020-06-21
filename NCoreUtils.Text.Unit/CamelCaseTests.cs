using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
    public class CamelCaseTests
    {
        [Theory]
        [InlineData("my-string-property", "myStringProperty")]
        [InlineData("MyStringProperty", "myStringProperty")]
        [InlineData("my_string_property", "myStringProperty")]
        [InlineData("myStringProperty", "myStringProperty")]
        [InlineData("_my_string_property", "myStringProperty")]
        [InlineData("my_string_property_", "myStringProperty")]
        [InlineData("_my_string_property_", "myStringProperty")]
        public void NormalCases(string input, string expected)
        {
            var actual = NamingConvention.CamelCase.Apply(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SimpleVPN", "simpleVPN")]
        public void Acronyms(string input, string expected)
        {
            var actual = NamingConvention.CamelCase.Apply(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExceptionCases()
        {
            Assert.Throws<ArgumentNullException>(() => NamingConvention.CamelCase.Apply(null!));
            Assert.Throws<ArgumentException>(() =>
            {
                Span<char> input = stackalloc char[20];
                for (var i = 0; i < input.Length; ++i)
                {
                    input[i] = 'a';
                }
                Span<char> buffer = stackalloc char[10];
                NamingConvention.CamelCase.Apply(input, buffer);
            });
        }

        [Fact]
        public void InvalidCases()
        {
            Assert.Equal(string.Empty, NamingConvention.CamelCase.Apply(string.Empty));
        }

        [Fact]
        public void LargeString()
        {
            var source = new string('a', 200000);
            Assert.Equal(source, NamingConvention.CamelCase.Apply(source));
        }
    }
}