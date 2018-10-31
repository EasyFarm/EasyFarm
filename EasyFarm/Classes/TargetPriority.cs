using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Classes
{
    public static class TargetPriority
    {
        public static IOrderedEnumerable<IUnit> Prioritize(IEnumerable<IUnit> units)
        {
            return units.OrderByDescending(x => x.PartyClaim)
                .ThenByDescending(x => x.HasAggroed)
                .ThenBy(x => x.Distance);
        }
    }
}