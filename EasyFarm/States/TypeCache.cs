using System;
using System.Collections.Generic;

namespace EasyFarm.States
{
    public class TypeCache<T>
    {
        private readonly Dictionary<Type, T> _cache = new Dictionary<Type, T>();

        public T this[object @object]
        {
            get
            {
                if (@object == null) return default(T);

                if (_cache.ContainsKey(@object.GetType()))
                {
                    return _cache[@object.GetType()];
                }

                return default(T);
            }
            set
            {
                if (@object == null) return;

                if (_cache.ContainsKey(@object.GetType()))
                {
                    _cache[@object.GetType()] = value;
                }
                else
                {
                    _cache.Add(@object.GetType(), value);
                }
            }
        }
    }
}