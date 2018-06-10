using System;
using EliteMMO.API;

namespace MemoryAPI.Resources
{
    public interface IResourcesTools
    {
        EliteAPI.ISpell GetSpell(Int32 index);
        EliteAPI.IAbility GetAbility(Int32 index);
        EliteAPI.IItem GetItem(Int32 index);
    }
}