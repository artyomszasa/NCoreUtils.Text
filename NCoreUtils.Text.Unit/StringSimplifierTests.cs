using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
    [Obsolete]
    public class StringSimplifierTests
    {
        [Theory]
        [InlineData("mosogatógép", "mosogatogep")]
        [InlineData("mosogatógép!!", "mosogatogep")]
        [InlineData("!!mosogatógép", "mosogatogep")]
        [InlineData("valamikor [] októberben", "valamikor-oktoberben")]
        [InlineData("valamikor | októberben", "valamikor-oktoberben")]
        [InlineData("levée en masse", "levee-en-masse")]
        [InlineData("lev\u0065\u0301e en masse", "levee-en-masse")]
        [InlineData("русские буквы", "russkie-bukvy")]
        [InlineData("welcome\uD83D\uDE00here", "welcome-here")]
        public void Default(string input, string expected)
        {
            var actual = StringSimplifier.Default.Simplify(input);
            Assert.Equal(expected, actual);
        }
    }
}
