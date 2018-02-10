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

namespace EasyFarm.Persistence
{
    public class Persister : IPersister
    {
        public void Serialize<T>(string fileName, T value)
        {
            var jsonPersister = new JsonPersister();
            jsonPersister.Serialize(fileName, value);
        }

        public T Deserialize<T>(string fileName)
        {
            var jsonPersister = new JsonPersister();

            var exceptions = new Queue<Exception>();

            try
            {
                return jsonPersister.Deserialize<T>(fileName);
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }

            throw new AggregateException("Persister failed deserialization", exceptions);
        }
    }
}