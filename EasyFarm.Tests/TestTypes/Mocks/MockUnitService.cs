using System.Collections.Generic;
using EasyFarm.Classes;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockUnitService : IUnitService
    {
        public bool HasAggro { get; }
        public ICollection<IUnit> MobArray { get; } = new List<IUnit>();
        public IUnit GetUnitByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}