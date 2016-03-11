using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class CastTimeTest
    {
        [Fact]
        public void IsCasted()
        {
            Assert.True(((CastTime)100).IsCasted);
        }

        [Fact]
        public void IsInterrupted()
        {
            Assert.False(((CastTime)75).IsCasted);
        }
    }
}
