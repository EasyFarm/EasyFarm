using LiteDB;

namespace EasyFarm.Infrastructure
{
    public class DatabaseFactory
    {
        public LiteDatabase Create(string connectionString) =>
            new LiteDatabase(connectionString);
    }
}
