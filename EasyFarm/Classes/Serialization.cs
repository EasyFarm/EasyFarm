
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using FFACETools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFarm.Classes
{
    public static class Serialization
    {
        /// <summary>
        /// Serializes an object to file. This function does not 
        /// handle errors and it's the consumer's job to do so. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="value"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        public static void Serialize<T>(string filename, T value)
        {
            using (Stream fStream = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fStream, value);
            }
        }

        /// <summary>
        /// Serializes an object from file. This function does not 
        /// handle errors and it's the consumer's job to do so. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static T Deserialize<T>(string filename)
        {
            using (Stream fStream = new FileStream(filename,
                FileMode.Open, FileAccess.Read, FileShare.None))
            {
                XmlSerializer xmlDeserializer = new XmlSerializer(typeof(T));
                return (T)xmlDeserializer.Deserialize(fStream);
            }
        }
    }
}
