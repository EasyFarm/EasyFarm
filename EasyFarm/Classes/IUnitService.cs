using System.Collections.Generic;

namespace EasyFarm.Classes
{
    public interface IUnitService
    {
        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        bool HasAggro { get; }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        ICollection<IUnit> MobArray { get; }
    }
}