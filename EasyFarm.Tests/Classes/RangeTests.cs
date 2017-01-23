using EasyFarm.Classes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class RangeTests
    {
        [Fact]
        public void NotSetWhenBothHighAndLowAreZero()
        {
            var range = new Range(0, 0);
            Assert.True(range.NotSet());
        }
        
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void SetWhenEitherHighOrLowIsNotZero(int low, int high)
        {
            var range = new Range(low, high);
            Assert.False(range.NotSet());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InRangeWhenValueBetweenLowAndHigh(int value)
        {
            var range = new Range(0, 2);
            Assert.True(range.InRange(value));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void NotInRangeWhenValueNotBetweenLowAndHigh(int value)
        {
            var range = new Range(0, 2);
            Assert.False(range.InRange(value));
        }
    }
}
