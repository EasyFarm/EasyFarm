using System.IO;
using System.Xml.Serialization;

namespace EasyFarm.Classes
{
    public class XmlPersister : IPersister
    {
        public void Serialize<T>(string fileName, T value)
        {
            using (Stream fStream = new FileStream(fileName,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fStream, value);
            }
        }

        public T Deserialize<T>(string fileName)
        {
            using (Stream fStream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var xmlDeserializer = new XmlSerializer(typeof(T));
                return (T)xmlDeserializer.Deserialize(fStream);
            }

        }
    }
}