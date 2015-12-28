using System;

namespace MemoryAPI
{
    public interface ITimerTools
    {
        int GetAbilityRecast(int index);
        int GetSpellRecast(int index);
    }
}