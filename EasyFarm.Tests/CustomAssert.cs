using Xunit.Sdk;

namespace EasyFarm.Tests
{
    public class CustomAssert
    {
        public static void Incomplete()
        {
            throw new XunitException("This test is incomplete.");
        }
    }
}