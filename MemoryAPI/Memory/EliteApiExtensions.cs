using System;
using EliteMMO.API;

namespace MemoryAPI.Memory
{
    public static class EliteApiExtensions
    {
        /// <summary>
        /// Get the cached version of an entity from memory.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Prefering GetStaticEntity over GetEntity since each property on GetEntity calls GetStaticEntity. 
        /// 
        /// This is slowing down memory access dramatically, since accessing Entity.X and Entity.Y 
        /// results in two memory accesses.
        /// 
        /// Instead, we call GetStaticEntity only once and keep the value cached for 1/3 of a second.
        /// </remarks>
        public static EliteAPI.EntityEntry GetCachedEntity(this EliteAPI api, int id)
        {
            var key = $"GetCachedEntity.{id}";

            var result = RuntimeCache.Get<EliteAPI.EntityEntry>(key);
            if (result != null) return result;

            var entity = api.Entity.GetStaticEntity(id);
            RuntimeCache.Set(key, entity, DateTimeOffset.Now.AddMilliseconds(300));

            return entity;
        }
    }
}