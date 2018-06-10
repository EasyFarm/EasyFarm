using System;
using System.Linq;
using EliteMMO.API;

namespace EasyFarm.Parsing
{
    public class AbilityMapper
    {
        public Ability Map(EliteAPI.IAbility ability)
        {
            return new Ability
            {
                English = ability.Name?.FirstOrDefault() ?? "",
                Distance = ability.Range,
                Index = ability.TimerID,
                Prefix = ability.TimerID == 900 ? "/weaponskill" : "/jobability",
                TpCost = ability.TimerID == 900 ? 1000 : ability.TP,
                TargetType = (TargetType) ability.ValidTargets,
                AbilityType = GetAbilityType(ability)
            };
        }

        private AbilityType GetAbilityType(EliteAPI.IAbility ability)
        {
            EliteMMO.API.AbilityType abilityType = (EliteMMO.API.AbilityType)ability.Type;

            switch (abilityType)
            {
                case EliteMMO.API.AbilityType.Job:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.General:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.BloodPactRage:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Corsair:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.CorsairShot:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.BloodPactWard:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Samba:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Waltz:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Step:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Florish1:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Scholar:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Jig:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Flourish2:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Flourish3:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Pet:
                    return AbilityType.Pet;
                case EliteMMO.API.AbilityType.Monster:
                    return AbilityType.Monsterskill;
                case EliteMMO.API.AbilityType.Weapon:
                    return AbilityType.Weaponskill;
                case EliteMMO.API.AbilityType.Weaponskill:
                    return AbilityType.Weaponskill;
                case EliteMMO.API.AbilityType.Trait:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Rune:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Ward:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Effusion:
                    return AbilityType.Jobability;
            }

            return AbilityType.Unknown;
        }
    }
}