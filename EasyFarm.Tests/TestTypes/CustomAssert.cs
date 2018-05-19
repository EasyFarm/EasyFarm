using Xunit.Sdk;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class CustomAssert
    {
        public static void Incomplete()
        {
            throw new XunitException("This test is incomplete.");
        }
    }
}