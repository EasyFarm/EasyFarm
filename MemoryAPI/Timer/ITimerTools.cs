using System;

namespace MemoryAPI
{
    public interface ITimerTools
    {
        int GetVanaUTC { get; }
        DateTime ServerTimeUTC { get; }

        AbilityList GetAbilityID(byte index);
        int GetAbilityRecast(byte index);
        int GetAbilityRecast(AbilityList abil);
        short GetSpellRecast(SpellList spell);
        short GetSpellRecast(short id);
        IVanaTime GetVanaTime();
    }
}