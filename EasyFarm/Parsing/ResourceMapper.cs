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