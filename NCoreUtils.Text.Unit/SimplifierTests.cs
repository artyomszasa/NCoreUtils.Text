using System;
using Xunit;

namespace NCoreUtils.Text.Unit
{
    [Obsolete]
    public class SimplifierTests : TestBase
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
            var actual = DynamicSimplifier.Simplify(input);
            Assert.Equal(expected, actual);
            actual = StaticSimplifier.Simplify(input);
            Assert.Equal(expected, actual);
        }
    }
}
