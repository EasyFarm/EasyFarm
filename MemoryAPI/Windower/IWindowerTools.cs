using EliteMMO.API;

namespace MemoryAPI.Windower
{
    public interface IWindowerTools
    {
        void SendString(string stringToSend);
        void SendKeyPress(Keys keys);
    }
}