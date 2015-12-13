using System;

namespace MemoryAPI
{
    public interface ITimerTools
    {
        int GetAbilityRecast(AbilityList abil);
        short GetSpellRecast(SpellList spell);
    }
}