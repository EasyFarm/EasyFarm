using System;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Logging;
using EasyFarm.ViewModels;
using MemoryAPI;

namespace EasyFarm.States
{
    public class SetTargetState : CombatState
    {
        private DateTime _lastTargetCheck = DateTime.Now;

        private readonly UnitService _units;

        public SetTargetState(IMemoryAPI fface) : base(fface)
        {
            _units = new UnitService(fface);
        }

        public override bool Check()
        {
            // Currently fighting, do not change target. 
            if (!UnitFilters.MobFilter(fface, Target))
            {
                // Still not time to update for new target. 
                if (DateTime.Now < _lastTargetCheck.AddSeconds(Constants.UnitArrayCheckRate)) return false;

                // First get the first mob by distance.
                var mobs = _units.MobArray.Where(x => UnitFilters.MobFilter(fface, x))
                    .OrderByDescending(x => x.PartyClaim)
                    .ThenByDescending(x => x.HasAggroed)
                    .ThenBy(x => x.Distance)
                    .ToList();

                // Set our new target at the end so that we don't accidentaly cast on a new target.
                Target = mobs.FirstOrDefault();

                // Update last time target was updated. 
                _lastTargetCheck = DateTime.Now;

                if (Target != null) LogViewModel.Write("Now targeting " + Target.Name + " : " + Target.Id);
            }

            return false;
        }
    }
}