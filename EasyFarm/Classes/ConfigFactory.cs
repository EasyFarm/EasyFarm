namespace EasyFarm.Classes
{
    public class ConfigFactory : IConfigFactory
    {
        public Config GetConfig()
        {
            return Config.Instance;
        }
    }
}