using EasyFarm.Memory;
using EasyFarm.Classes;

namespace EasyFarm.Tests.Mocks
{
    public class MockMemorySource : IMemorySource
    {
        public Position GetPlayerPosition()
        {
            return new Position()
            {
                H = 1,
                X = 1,
                Y = 1,
                Z = 1,
            };
        }
    }
}
