using EliteMMO.API;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes
{
    public class FakeWindower : IWindowerTools
    {
        public void SendString(string stringToSend)
        {
            LastCommand = stringToSend;
        }

        public void SendKeyPress(Keys keys)
        {
        }

        public string LastCommand { get; set; }
    }
}