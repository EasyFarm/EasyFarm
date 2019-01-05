// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Linq;
using System.Reflection;

namespace EasyFarm.Parsing
{
    public class ResourceMapper
    {
        public static void Map(Object from, Object to)
        {
            Dictionary<String, Object> fromProps = from.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(from));
            PropertyInfo[] toProps = to.GetType().GetProperties();

            foreach (PropertyInfo toProp in toProps)
            {
                if (!fromProps.ContainsKey(toProp.Name)) continue;

                Object value = fromProps[toProp.Name];
                if (value == null) continue;

                SetValueOnObject(@from, to, toProp, value);
            }
        }

        private static void SetValueOnObject(Object @from, Object to, PropertyInfo toProp, Object value)
        {
            try
            {
                toProp.SetValue(to, Convert.ChangeType(value, toProp.PropertyType));
            }
            catch (Exception e)
            {
                throw new Exception($"Error Parsing " +
                                    $"\r\nFROM: [{@from.GetType()}].[{toProp.Name}].[{value.GetType()}]" +
                                    $"\r\nTO: [{to.GetType()}].[{toProp.Name}].[{toProp.PropertyType}]" +
                                    $"\r\nVALUE: [{value}]", e);
            }
        }
    }
}