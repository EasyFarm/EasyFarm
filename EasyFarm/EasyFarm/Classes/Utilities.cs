using FFACETools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFarm.UtilityTools
{
    /// <summary>
    /// This class is essential the code graveyard. Any code that I would rather not dispose
    /// of because I found use for it in the past ends up here.
    /// </summary>
    static class Utilities
    {
        public static void Serialize<T>(string filename, T value)
        {
            using (Stream fStream = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fStream, value);
            }
        }

        public static T Deserialize<T>(string filename, T value)
        {
                if (System.IO.File.Exists(filename))
                {
                    using (Stream fStream = new FileStream(filename,
                        FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        XmlSerializer xmlDeserializer = new XmlSerializer(typeof(T));
                        return (T) xmlDeserializer.Deserialize(fStream);
                    }
                }

            return value;
        }
    }
}
