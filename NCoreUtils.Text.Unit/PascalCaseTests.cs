using Xunit;

namespace NCoreUtils.Text.Unit
{
    public class PascalCaseTests
    {
        [Theory]
        [InlineData("my-string-property", "MyStringProperty")]
        [InlineData("MyStringProperty", "MyStringProperty")]
        [InlineData("my_string_property", "MyStringProperty")]
        [InlineData("myStringProperty", "MyStringProperty")]
        [InlineData("_my_string_property", "MyStringProperty")]
        [InlineData("my_string_property_", "MyStringProperty")]
        [InlineData("_my_string_property_", "MyStringProperty")]
        public void NormalCases(string input, string expected)
        {
            var actual = NamingConvention.PascalCase.Apply(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("SimpleVPN", "SimpleVPN")]
        public void Acronyms(string input, string expected)
        {
            var actual = NamingConvention.PascalCase.Apply(input);
            Assert.Equal(expected, actual);
        }
    }
}