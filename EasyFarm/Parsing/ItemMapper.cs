using System;
using System.Linq;
using EliteMMO.API;

namespace EasyFarm.Parsing
{
    public class ItemMapper
    {
        public Ability Map(EliteAPI.IItem item)
        {
            return new Ability()
            {
                CastTime = item.CastTime,
                English = item.Name?.FirstOrDefault() ?? "",
                Index = (Int32) item.ItemID,
                Prefix = "/item",
                Recast = item.RecastDelay,
                TargetType = (TargetType) item.ValidTargets,
                AbilityType = AbilityType.Item
            };
        }
    }
}