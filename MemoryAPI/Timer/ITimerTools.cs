using System;

namespace MemoryAPI
{
    public interface ITimerTools
    {
        AbilityList GetAbilityID(byte index);
        int GetAbilityRecast(byte index);
        int GetAbilityRecast(AbilityList abil);
        short GetSpellRecast(SpellList spell);
        short GetSpellRecast(short id);
    }
}