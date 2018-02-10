using System.Collections.Generic;
using EasyFarm.Classes;

namespace EasyFarm.Tests.TestTypes
{
    public class TestUnitService : IUnitService
    {
        public bool HasAggro { get; set; }
        public ICollection<IUnit> MobArray { get; set; } = new List<IUnit>();
        public IUnit GetUnitByName(string name) => new FakeUnit();
    }
}
