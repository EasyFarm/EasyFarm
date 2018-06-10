using System;
using EliteMMO.API;

namespace MemoryAPI.Resources
{
    public class ResourcesTools : IResourcesTools
    {
        private readonly EliteAPI _api;

        public ResourcesTools(EliteAPI api)
        {
            _api = api;
        }

        public EliteAPI.ISpell GetSpell(Int32 index)
        {
            return _api.Resources.GetSpell((UInt32)index);
        }

        public EliteAPI.IAbility GetAbility(Int32 index)
        {
            return _api.Resources.GetAbility((UInt32)index);
        }

        public EliteAPI.IItem GetItem(Int32 index)
        {
            return _api.Resources.GetItem((UInt32)index);
        }
    }
}
