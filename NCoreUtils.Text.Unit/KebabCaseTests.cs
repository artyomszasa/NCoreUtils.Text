using Xunit;

namespace NCoreUtils.Text.Unit
{
    public class KebabCaseTests
    {
        [Theory]
        [InlineData("my-string-property", "my-string-property")]
        [InlineData("MyStringProperty", "my-string-property")]
        [InlineData("my_string_property", "my-string-property")]
        [InlineData("myStringProperty", "my-string-property")]
        [InlineData("_my_string_property", "my-string-property")]
        [InlineData("my_string_property_", "my-string-property")]
        [InlineData("_my_string_property_", "my-string-property")]
        public void NormalCases(string input, string expected)
        {
            var actual = NamingConvention.KebabCase.Apply(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SimpleVPN", "simple-vpn")]
        public void Acronyms(string input, string expected)
        {
            var actual = NamingConvention.KebabCase.Apply(input);
            Assert.Equal(expected, actual);
        }
    }
}