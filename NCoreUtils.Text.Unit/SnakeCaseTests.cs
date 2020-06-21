using Xunit;

namespace NCoreUtils.Text.Unit
{
    public class SnakeCaseTests
    {
        [Theory]
        [InlineData("my-string-property", "my_string_property")]
        [InlineData("MyStringProperty", "my_string_property")]
        [InlineData("my_string_property", "my_string_property")]
        [InlineData("myStringProperty", "my_string_property")]
        [InlineData("_my_string_property", "my_string_property")]
        [InlineData("my_string_property_", "my_string_property")]
        [InlineData("_my_string_property_", "my_string_property")]
        public void NormalCases(string input, string expected)
        {
            var actual = NamingConvention.SnakeCase.Apply(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SimpleVPN", "simple_vpn")]
        public void Acronyms(string input, string expected)
        {
            var actual = NamingConvention.SnakeCase.Apply(input);
            Assert.Equal(expected, actual);
        }
    }
}