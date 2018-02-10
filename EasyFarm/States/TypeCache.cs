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

                if (_cache.ContainsKey(@object.GetType())) return _cache[@object.GetType()];

                return default(T);
            }
            set
            {
                if (@object == null) return;

                if (_cache.ContainsKey(@object.GetType()))
                    _cache[@object.GetType()] = value;
                else
                    _cache.Add(@object.GetType(), value);
            }
        }
    }
}