// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Caching;

namespace MemoryAPI.Memory
{
    public static class RuntimeCache
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