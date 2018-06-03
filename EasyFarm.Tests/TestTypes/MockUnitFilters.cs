using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.Tests.TestTypes
{
    public class MockUnitFilters : IUnitFilters
    {
        public bool Result { get; set; } = true;

        public bool MobFilter(IMemoryAPI fface, IUnit mob, IConfig config)
        {
            return Result;
        }
    }
}