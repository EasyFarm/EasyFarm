namespace EasyFarm.Classes
{
    public static class Serialization
    {
        public static IPersister Instance { get; set; } = new Persister();

        public static void Serialize<T>(string fileName, T value)
        {
            Instance.Serialize(fileName, value);
        }

        public static T Deserialize<T>(string fileName)
        {
            return Instance.Deserialize<T>(fileName);
        }
    }
}
