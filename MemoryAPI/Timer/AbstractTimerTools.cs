using System;

namespace MemoryAPI
{
    public abstract class AbstractTimerTools : ITimerTools
    {
        public abstract AbilityList GetAbilityID(byte index);
        public abstract int GetAbilityRecast(byte index);
        public abstract int GetAbilityRecast(AbilityList abil);
        public abstract short GetSpellRecast(SpellList spell);
        public abstract short GetSpellRecast(short id);
    }
}