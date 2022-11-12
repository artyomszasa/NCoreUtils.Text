using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
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
        [InlineData("Главная", "glavnaja")]
        [InlineData("welcome\uD83D\uDE00here", "welcome-here")]
        [InlineData("groß", "gross")]
        [InlineData("українських студентів", "ukrainskih-studentiv")]
        [InlineData("émosogatógép😍😍😍", "emosogatogep")]
        public void Default(string input, string expected)
        {
            var actual = DynamicStringSimplifier.Simplify(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExceptionCases()
        {
            Assert.Throws<ArgumentNullException>(() => DynamicStringSimplifier.Simplify(null!));
            Assert.Throws<ArgumentException>(() =>
            {
                Span<char> input = stackalloc char[20];
                for (var i = 0; i < input.Length; ++i)
                {
                    input[i] = 'a';
                }
                Span<char> buffer = stackalloc char[10];
                DynamicStringSimplifier.Simplify(input, buffer);
            });
        }

        [Fact]
        public void InvalidCases()
        {
            Assert.Equal(string.Empty, DynamicStringSimplifier.Simplify(string.Empty));
        }

        [Fact]
        public void LargeString()
        {
            var source = new string('a', 200000);
            Assert.Equal(source, DynamicStringSimplifier.Simplify(source));
        }
    }
}
