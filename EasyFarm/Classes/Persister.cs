using System;
using System.Collections.Generic;

namespace EasyFarm.Classes
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
            var xmlPersister = new XmlPersister();
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

            try
            {
                return xmlPersister.Deserialize<T>(fileName);
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }

            throw new AggregateException("Persister failed deserialization", exceptions);
        }
    }
}