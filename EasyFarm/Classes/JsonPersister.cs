using System.IO;
using Newtonsoft.Json;

namespace EasyFarm.Classes
{
    public class JsonPersister : IPersister
    {
        public void Serialize<T>(string fileName, T value)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            using (JsonWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, value);
            }
        }

        public T Deserialize<T>(string fileName)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            using (StreamReader streamReader = new StreamReader(fileName))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                return jsonSerializer.Deserialize<T>(jsonReader);
            }
        }
    }
}