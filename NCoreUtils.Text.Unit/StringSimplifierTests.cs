using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
    [Obsolete]
    public class StringSimplifierTests : TestBase
    {
        [Theory]
        [InlineData("émosogatógép", "emosogatogep")]
        [InlineData("émosogatógép!!", "emosogatogep")]
        [InlineData("!!mosogatógép", "mosogatogep")]
        [InlineData("évalamikor [] októberben", "evalamikor-oktoberben")]
        [InlineData("évalamikor | októberben", "evalamikor-oktoberben")]
        [InlineData("levée en masse", "levee-en-masse")]
        [InlineData("lev\u0065\u0301e en masse", "levee-en-masse")]
        [InlineData("русские буквы", "russkie-bukvy")]
        [InlineData("welcome\uD83D\uDE00here", "welcome-here")]
        [InlineData("Straßentheater", "strassentheater")]
        public void Default(string input, string expected)
        {
            var actual = DynamicStringSimplifier.Simplify(input);
            Assert.Equal(expected, actual);
            actual = StaticStringSimplifier.Simplify(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExceptionCases()
        {
            Assert.Throws<ArgumentNullException>(() => StaticStringSimplifier.Simplify(null!));
            Assert.Throws<ArgumentException>(() =>
            {
                Span<char> input = stackalloc char[20];
                for (var i = 0; i < input.Length; ++i)
                {
                    input[i] = 'a';
                }
                Span<char> buffer = stackalloc char[10];
                StaticStringSimplifier.Simplify(input, buffer);
            });
        }

        [Fact]
        public void InvalidCases()
        {
            Assert.Equal(string.Empty, StaticStringSimplifier.Simplify(string.Empty));
        }

        [Fact]
        public void LargeString()
        {
            var source = new string('a', 200000);
            Assert.Equal(source, StaticStringSimplifier.Simplify(source));
        }
    }
}
