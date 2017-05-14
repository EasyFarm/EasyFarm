using System;
using System.Runtime.Caching;

namespace MemoryAPI
{
    internal static class RuntimeCache
    {
        public static T Get<T>(string key)
        {
            if (!MemoryCache.Default.Contains(key)) return default(T);
            var entry = MemoryCache.Default.Get(key);
            return (T)entry;
        }

        public static void Set(string key, object value, DateTimeOffset expiration)
        {
            MemoryCache.Default.Set(key, value, expiration);
        }
    }
}