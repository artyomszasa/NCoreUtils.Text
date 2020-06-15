using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
    [Obsolete]
    public class SimplifierTests
    {
        [Theory]
        [InlineData("mosogatógép", "mosogatogep")]
        [InlineData("mosogatógép!!", "mosogatogep")]
        [InlineData("!!mosogatógép", "mosogatogep")]
        [InlineData("valamikor [] októberben", "valamikor-oktoberben")]
        [InlineData("valamikor | októberben", "valamikor-oktoberben")]
        [InlineData("русские буквы", "russkie-bukvy")]
        public void Default(string input, string expected)
        {
            var actual = Simplifier.Default.Simplify(input);
            Assert.Equal(expected, actual);
        }
    }
}
