using System;

namespace MemoryAPI
{
    public abstract class AbstractTimerTools : ITimerTools
    {
        public abstract int GetAbilityRecast(AbilityList abil);
        public abstract short GetSpellRecast(SpellList spell);
    }
}