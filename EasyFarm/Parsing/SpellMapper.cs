using System;
using System.Linq;
using EliteMMO.API;

namespace EasyFarm.Parsing
{
    public class SpellMapper
    {
        public Ability Map(EliteAPI.ISpell spell)
        {
            return new Ability
            {
                CastTime = spell.CastTime,
                English = spell.Name?.FirstOrDefault() ?? "",
                Distance = spell.Range,
                Index = spell.Index,
                Id = spell.ID,
                Prefix = "/magic",
                Recast = spell.RecastDelay,
                MpCost = spell.MPCost,
                TargetType = (TargetType) spell.ValidTargets,
                AbilityType = GetAbilityType(spell)
            };
        }

        private static AbilityType GetAbilityType(EliteAPI.ISpell spell)
        {
            var spellType = (MagicType)spell.MagicType;

            switch (spellType)
            {
                case MagicType.None:
                    return AbilityType.Unknown;
                case MagicType.WhiteMagic:
                    return AbilityType.Magic;
                case MagicType.BlackMagic:
                    return AbilityType.Magic;
                case MagicType.Summon:
                    return AbilityType.Magic;
                case MagicType.Ninjutsu:
                    return AbilityType.Ninjutsu;
                case MagicType.Song:
                    return AbilityType.Song;
                case MagicType.BlueMagic:
                    return AbilityType.Magic;
                case MagicType.Geomancy:
                    return AbilityType.Magic;
                case MagicType.Trust:
                    return AbilityType.Trust;
            }

            return AbilityType.Unknown;
        }
    }
}